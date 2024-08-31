using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieApi.Models
{
    public class Pelicula
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("year")]
        public int? Year { get; set; } = null;

        [BsonElement("classification")]
        public string Classification { get; set; } = string.Empty;

        [BsonElement("genre")]
        public string Genre { get; set; } = string.Empty;

        [BsonElement("director")]
        public string Director { get; set; } = string.Empty;

        [BsonElement("protagonists")]
        public List<string> Protagonists { get; set; } = new List<string>();

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("image")]
        public string Image { get; set; } = string.Empty;
    }
}
