using System.Collections.Generic;
using System.Linq;
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
using webapidemo.Repositories;

namespace webapidemo.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private IMapper _mapper;
        private IColumnRepository _columnRepository;

        public ColumnsController(IColumnRepository columnRepository, IMapper mapper)
        {
            _mapper = mapper;
            _columnRepository = columnRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Column>>> Get()
        {
            var userId = HttpContext.User.GetUserId();
            
            var columnDtos = await _columnRepository.Get(userId);

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

            await _columnRepository.Update(userId, columnId, objectIds);
        }

        
        // PUT api/values/5
        [HttpPut("{sourceColumnId}/{destinationColumnId}/notes")]
        [AcceptVerbs("PUT", "OPTIONS")]
        public async Task Put(string sourceColumnId, string destinationColumnId, [FromBody] UpdateColumn update)
        {
            var userId = HttpContext.User.GetUserId();

            List<ObjectId> sourceIds;
            // Move note inside column
            if (sourceColumnId == destinationColumnId)
            {
                sourceIds = _mapper.Map<List<ObjectId>>(update.SourceNotes);
                await _columnRepository.Update(userId, sourceColumnId, sourceIds);
                return;
            }

            // Move to another column
            sourceIds = _mapper.Map<List<ObjectId>>(update.SourceNotes);
            await _columnRepository.Update(userId, sourceColumnId, sourceIds);
            List<ObjectId> destinationIds = _mapper.Map<List<ObjectId>>(update.DestinationNotes);
            await _columnRepository.Update(userId, destinationColumnId, destinationIds);
        }
    }
}