using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using webapidemo.DTO;
using webapidemo.Extensions;
using webapidemo.Model;

namespace webapidemo.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private IMongoCollection<ColumnDto> _columnsCollection;
        private IMapper _mapper;

        public ColumnsController(IMapper mapper, IMongoDatabase database)
        {
            _mapper = mapper;

            _columnsCollection = database.GetCollection<ColumnDto>("Column");

            // Text index
            var indexModel = new CreateIndexModel<ColumnDto>(Builders<ColumnDto>.IndexKeys.Combine(
                Builders<ColumnDto>.IndexKeys.Text(p => p.ColumnId)));
            _columnsCollection.Indexes.CreateOne(indexModel);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Column>>> Get()
        {
            var userId = HttpContext.User.GetUserId();
            List<ColumnDto> columnDtos = await _columnsCollection.Find(column => column.UserId == userId).ToListAsync();

            var columns = _mapper.Map<List<Column>>(columnDtos);
            return columns;
        }     

        // PUT api/values/5
        [HttpPut("{columnId}/notes")]
        [AcceptVerbs("PUT", "OPTIONS")]
        public async Task Put(string columnId, [FromBody] List<string> noteIds)
        {
            List<ObjectId> objectIds = new List<ObjectId>();
            foreach (string value in noteIds)
            {
                objectIds.Add(new ObjectId(value));
            }

            var userId = HttpContext.User.GetUserId();

            var updateDef = new UpdateDefinitionBuilder<ColumnDto>().Set(x => x.NoteIds, objectIds.ToArray());
            await _columnsCollection.UpdateOneAsync(col => col.ColumnId == columnId && col.UserId == userId, updateDef);
        }
    }
}