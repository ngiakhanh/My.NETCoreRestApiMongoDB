using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WebApplication1.Domain.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("Id")]
        public int Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}