using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace webapidemo.DTO
{
    public class ColumnDto
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id {get; set;}

        [BsonElement("columnid")]
        public string ColumnId {get; set;}

        [BsonElement("notes")]
        public ObjectId[] NoteIds {get; set;}
    }
}