using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDemo.Models
{
    public class BookModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("isbn")]
        public string Isbn { get; set; }

        [BsonElement("authors")]
        public ICollection<ObjectId> Authors { get; set; }
    }
}
