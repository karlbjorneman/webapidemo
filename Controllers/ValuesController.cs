using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using AutoMapper;

namespace webapidemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IMongoDatabase _database;
        private IMongoCollection<Fruit> _fruitCollection;
        private IMapper _mapper;

        public ValuesController(IMapper mapper)
        {
            _mapper = mapper;

            MongoClient mongoClient = new MongoClient("mongodb://thebear:KgjFg713Walle@cluster0-shard-00-00-kbgve.azure.mongodb.net:27017,cluster0-shard-00-01-kbgve.azure.mongodb.net:27017,cluster0-shard-00-02-kbgve.azure.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin&retryWrites=true");
            _database = mongoClient.GetDatabase("mongodbdemo");
            _fruitCollection = _database.GetCollection<Fruit>("Fruit");
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Fruit>> Get()
        {
            return _fruitCollection.Find(FilterDefinition<Fruit>.Empty).ToList();
        }

        // GET api/values/5
        [HttpGet("{name}")]
        public ActionResult<Fruit> Get(string name)
        {
            Fruit fruit = _fruitCollection.FindSync(f => f.Name == "Banana").First();
            return fruit;
        }

        // POST api/values
        [HttpPost]
        [AcceptVerbs("POST", "OPTIONS")]
        public void Post([FromBody] Fruit value)
        {
            _fruitCollection.InsertOne(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [AcceptVerbs("PUT", "OPTIONS")]
        public async Task Put(string id, [FromBody] FruitEnity value)
        {
            Fruit fruit = _mapper.Map<Fruit>(value);

            ObjectId objectId = new ObjectId(id);
            await _fruitCollection.ReplaceOneAsync(f => f.Id == objectId, fruit);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
