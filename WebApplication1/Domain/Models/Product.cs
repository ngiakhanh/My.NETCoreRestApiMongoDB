using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Domain.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("Id")]
        public int Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("QuantityInPackage")]
        public short QuantityInPackage { get; set; }

        [BsonElement("UnitOfMeasurement")]
        public EUnitOfMeasurement UnitOfMeasurement { get; set; }

        [BsonElement("CategoryId")]
        public int CategoryId { get; set; }

        public IEnumerable<Category> Category { get; set; } = new List<Category>();
    }
}