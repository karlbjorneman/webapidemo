using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using AutoMapper;
using webapidemo.DTO;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using webapidemo.Model;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using webapidemo.Extensions;
using webapidemo.Repositories;
using Microsoft.AspNetCore.Http;
using webapidemo.Services;

namespace webapidemo.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private IMongoCollection<NoteDto> _notesCollection;
        private IMapper _mapper;
        private IColumnRepository _columnRepository;
        private IPhotoService _photoService;

        public NotesController(IMapper mapper, IMongoDatabase database, IColumnRepository columnRepository, IPhotoService photoService)
        {
            _mapper = mapper;
            _columnRepository = columnRepository;
            _photoService = photoService;

            _notesCollection = database.GetCollection<NoteDto>("Note");

            // Text index
            var indexModel = new CreateIndexModel<NoteDto>(Builders<NoteDto>.IndexKeys.Combine(
                Builders<NoteDto>.IndexKeys.Text(p => p.Header),
                Builders<NoteDto>.IndexKeys.Text(p => p.Body )));
            _notesCollection.Indexes.CreateOne(indexModel);
        }
        
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> Get()
        {
            var userId = HttpContext.User.GetUserId();
            List<NoteDto> noteDtos = await _notesCollection.Find(note => note.UserId == userId).ToListAsync();

            string googleAccessToken = HttpContext.Request.Headers["googleAccessToken"];
            foreach(NoteDto noteDto in noteDtos)
            {
                if (string.IsNullOrEmpty(noteDto.ImagePath))
                    continue;

                GetPhoto mediaItem = await _photoService.GetPhoto(googleAccessToken, noteDto.ImagePath);
                noteDto.ImageUrl = $"{mediaItem.BaseUrl}=w2048-h1024" ;
            }

            var notes = _mapper.Map<List<Note>>(noteDtos);
            return notes;
        }

        // GET api/values/5
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Note>>> Get(string name)
        {
            var notes = await _notesCollection.Find(Builders<NoteDto>.Filter.Text($"\"{name}\"")).ToListAsync();
            if (notes == null)
                return new Note[0];

            var noteEntities = _mapper.Map<List<Note>>(notes);
            return new ActionResult<IEnumerable<Note>>(noteEntities);
        }

        [HttpPost]
        [AcceptVerbs("POST", "OPTIONS")]
        public async Task<ActionResult<Note>> Post(IFormCollection form)
        {
            var userId = HttpContext.User.GetUserId();

            ColumnDto columnToUpdate = await ColumnToUpdate(userId);

            var note = new NoteDto {Header = form["header"], Body = form["body"]};
            string accessToken = form["accessToken"];
            note.Position = new PositionDto() {Column = columnToUpdate.ColumnId};
            note.UserId = userId;

            IFormFile imageFile = form.Files.FirstOrDefault();
            if (imageFile != null)
            {
                NewPhoto uploadedPhoto = await _photoService.AddPhoto(accessToken, userId, imageFile);
                note.ImagePath = uploadedPhoto.Id;
            }

            await _notesCollection.InsertOneAsync(note);

            var noteIds = columnToUpdate.NoteIds.ToList();
            noteIds.Insert(0, note.Id);
            await _columnRepository.Update(userId, columnToUpdate.ColumnId, noteIds);

            return new ActionResult<Note>(_mapper.Map<Note>(note));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [AcceptVerbs("PUT", "OPTIONS")]
        public async Task Put(string id, [FromBody] Note value)
        {
            var note = _mapper.Map<NoteDto>(value);

            ObjectId objectId = new ObjectId(id);

            var updateDef = new UpdateDefinitionBuilder<NoteDto>()
                                .Set(x => x.Body, note.Body)
                                .Set(x => x.Header, note.Header)
                                .Set(x => x.ImagePath, note.ImagePath);
            await _notesCollection.UpdateOneAsync(f => f.Id == objectId, updateDef);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private async Task<ColumnDto> ColumnToUpdate(string userId)
        {
            int minCount = Int32.MaxValue;
            ColumnDto minCountColumn = null;

            IList<ColumnDto> columnDtos = await _columnRepository.Get(userId);
            foreach(ColumnDto columnDto in columnDtos)
            {
                if (columnDto.NoteIds.Length < minCount)
                {
                    minCount = columnDto.NoteIds.Length;
                    minCountColumn = columnDto;
                }
            }

            return minCountColumn;
        }
    }
}
