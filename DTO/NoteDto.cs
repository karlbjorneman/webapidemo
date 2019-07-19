using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace webapidemo.DTO
{
    public class NoteDto
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        
        [BsonElement("header")]
        public string Header { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("position")]
        public PositionDto Position {get; set;}

        [BsonElement("imagePath")]
        public string ImagePath {get; set;}

        public string ImageUrl {get; set;}

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId {get; set;}
    }
}
