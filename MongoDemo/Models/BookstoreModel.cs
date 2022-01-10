using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDemo.Models
{
    public class BookstoreModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("books")]
        public ICollection<ObjectId> Books { get; set; }
    }
}
