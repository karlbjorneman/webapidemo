using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace webapidemo.DTO
{
    public class UserDto
    {
        [BsonId()]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("email")]
        public string Email {get; set;}

        [BsonElement("familyName")]
        public string FamilyName {get; set;}

        [BsonElement("givenName")]
        public string GivenName {get; set;}

        [BsonElement("picture")]
        public string Picture {get; set;}

        [BsonElement("subject")]
        public string Subject {get; set;}

        [BsonElement("issuer")]
        public string Issuer {get; set;}

        [BsonElement("googlePhotoAlbumId")]
        public string GooglePhotoAlbumId {get; set;}
    }
}