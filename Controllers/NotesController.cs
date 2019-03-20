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

namespace webapidemo.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private IMongoCollection<NoteDto> _notesCollection;
        private IMapper _mapper;

        public NotesController(IMapper mapper, IConfiguration configuration, IMongoDatabase database)
        {
            _mapper = mapper;

            _notesCollection = database.GetCollection<NoteDto>("Note");

            // Text index
            var indexModel = new CreateIndexModel<NoteDto>(Builders<NoteDto>.IndexKeys.Combine(
                Builders<NoteDto>.IndexKeys.Text(p => p.Header),
                Builders<NoteDto>.IndexKeys.Text(p => p.Body )));
            _notesCollection.Indexes.CreateOne(indexModel);

            // CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=gustaftechnotes;AccountKey=VS3VF6xc4p6pycEqfnSmtA5vvMEQF9zKlU+WtDWQ0GuVOI1gT4OPO5z3LVc5SEC2EJVoohXp/Zmtf2UhYJXwhg==;EndpointSuffix=core.windows.net");
            // CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            // CloudBlobContainer blobContainer = blobClient.GetContainerReference("gustafechnotesblobstorage");
            // CloudBlockBlob blob = blobContainer.GetBlockBlobReference("test.jpg");

            // blob.DownloadToFileAsync(@"C:\Users\Karl.Bjorneman\Downloads\test_copy.jpg", FileMode.Create).GetAwaiter().GetResult();
        }
        
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> Get()
        {
            List<NoteDto> noteDtos = await _notesCollection.Find(FilterDefinition<NoteDto>.Empty).ToListAsync();

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

        // POST api/values
        [HttpPost]
        [AcceptVerbs("POST", "OPTIONS")]
        public async Task Post([FromBody] Note value)
        {
            var note = _mapper.Map<NoteDto>(value);
            await _notesCollection.InsertOneAsync(note);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [AcceptVerbs("PUT", "OPTIONS")]
        public async Task Put(string id, [FromBody] Note value)
        {
            var note = _mapper.Map<NoteDto>(value);

            ObjectId objectId = new ObjectId(id);
            await _notesCollection.ReplaceOneAsync(f => f.Id == objectId, note);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
