﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using AutoMapper;
using webapidemo.DTO;
using webapidemo.Entity;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace webapidemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private IMongoDatabase _database;
        private IMongoCollection<NoteDto> _notesCollection;
        private IMapper _mapper;

        public NotesController(IMapper mapper)
        {
            _mapper = mapper;

            MongoClient mongoClient = new MongoClient("mongodb://thebear:KgjFg713Walle@cluster0-shard-00-00-kbgve.azure.mongodb.net:27017,cluster0-shard-00-01-kbgve.azure.mongodb.net:27017,cluster0-shard-00-02-kbgve.azure.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin&retryWrites=true");
            _database = mongoClient.GetDatabase("mongodbdemo");
            _notesCollection = _database.GetCollection<NoteDto>("Note");

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
        public ActionResult<IEnumerable<NoteEntity>> Get()
        {
            List<NoteDto> noteDtos = _notesCollection.Find(FilterDefinition<NoteDto>.Empty).ToList();

            var notes = _mapper.Map<List<NoteEntity>>(noteDtos);
            return notes;
        }

        // GET api/values/5
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<NoteEntity>>> Get(string name)
        {
            var notes = await _notesCollection.Find(Builders<NoteDto>.Filter.Text($"\"{name}\"")).ToListAsync();
            if (notes == null)
                return new NoteEntity[0];

            var noteEntities = _mapper.Map<List<NoteEntity>>(notes);
            return new ActionResult<IEnumerable<NoteEntity>>(noteEntities);
        }

        // POST api/values
        [HttpPost]
        [AcceptVerbs("POST", "OPTIONS")]
        public void Post([FromBody] NoteEntity value)
        {
            var note = _mapper.Map<NoteDto>(value);
            _notesCollection.InsertOne(note);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [AcceptVerbs("PUT", "OPTIONS")]
        public async Task Put(string id, [FromBody] NoteEntity value)
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