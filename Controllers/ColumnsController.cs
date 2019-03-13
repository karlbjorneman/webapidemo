using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using webapidemo.DTO;
using webapidemo.Entity;

namespace webapidemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private IMongoDatabase _database;
        private IMongoCollection<ColumnDto> _notesCollection;
        private IMapper _mapper;

        public ColumnsController(IMapper mapper)
        {
            _mapper = mapper;

            MongoClient mongoClient = new MongoClient("mongodb://thebear:KgjFg713Walle@cluster0-shard-00-00-kbgve.azure.mongodb.net:27017,cluster0-shard-00-01-kbgve.azure.mongodb.net:27017,cluster0-shard-00-02-kbgve.azure.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin&retryWrites=true");
            _database = mongoClient.GetDatabase("mongodbdemo");
            _notesCollection = _database.GetCollection<ColumnDto>("Column");

            // Text index
            var indexModel = new CreateIndexModel<ColumnDto>(Builders<ColumnDto>.IndexKeys.Combine(
                Builders<ColumnDto>.IndexKeys.Text(p => p.ColumnId)));
            _notesCollection.Indexes.CreateOne(indexModel);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ColumnEntity>> Get()
        {
            List<ColumnDto> columnDtos = _notesCollection.Find(FilterDefinition<ColumnDto>.Empty).ToList();

            var columns = _mapper.Map<List<ColumnEntity>>(columnDtos);
            return columns;
        }
    }
}