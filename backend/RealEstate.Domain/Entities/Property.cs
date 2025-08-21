using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Domain.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("codeInternal")]
        public int CodeInternal { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("idOwner")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOwner { get; set; } = string.Empty;

        [BsonElement("image")]
        public string Image { get; set; } = string.Empty;

        // no se almacena en DB AAB (20 08 2025)
        [BsonIgnore]
        public Owner? Owner { get; set; }
    }
}