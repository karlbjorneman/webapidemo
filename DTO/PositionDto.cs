using MongoDB.Bson.Serialization.Attributes;

namespace webapidemo.DTO
{
    public class PositionDto
    {
        [BsonElement("column")]
        public string Column {get; set;}
    }
}