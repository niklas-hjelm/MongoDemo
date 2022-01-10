using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDemo.Models
{
    public class MovieModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        [BsonElement("plot")]
        public string Plot { get; set; }

        [BsonElement("genres")]
        public ICollection<string> Genres { get; set; }

        [BsonElement("runtime")]
        public int Runtime { get; set; }

        [BsonElement("cast")]
        public ICollection<string> Cast { get; set; }

        [BsonElement("num_mflix_comments")]
        public int NumComments { get; set; }

        [BsonElement("poster")]
        public string Poster { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("fullplot")]
        public string FullPlot { get; set; }

        [BsonElement("countries")]
        public ICollection<string> Countries { get; set; }

        [BsonElement("languages")]
        public ICollection<string> Languages { get; set; }

        [BsonElement("released")]
        public DateTime Released { get; set; }

        [BsonElement("rated")]
        public string Rated { get; set; }

        [BsonElement("directors")]
        public ICollection<string> Directors { get; set; }

        [BsonElement("writers")]
        public ICollection<string> Writers { get; set; }

        [BsonElement("awards")]
        public Award Awards { get; set; }

        [BsonElement("lastupdated")]
        public string LastUpdated { get; set; }

        [BsonElement("imdb")]
        public Imdb Imdb { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("tomatoes")]
        public Tomato Tomatoes { get; set; }

        [BsonElement("metacritic")]
        public int MetaCritic { get; set; }

        [BsonExtraElements]
        public IDictionary<string, object> ExtraElements { get; set; }

    }

    public class Tomato
    {
        [BsonElement("viewer")]
        public Viewer Viewer { get; set; }

        [BsonElement("critic")]
        public Critic Critic { get; set; }

        [BsonElement("dvd")]
        public DateTime DvDateTime { get; set; }

        [BsonElement("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [BsonElement("rotten")]
        public int Rotten { get; set; }

        [BsonElement("fresh")]
        public int Fresh { get; set; }

        [BsonElement("production")]
        public string Production { get; set; }

        [BsonElement("consensus")]
        public string Consensus { get; set; }

        [BsonElement("website")]
        public string Website { get; set; }

        [BsonElement("boxOffice")]
        public string BoxOffice { get; set; }
    }

    public class Critic
    {
        [BsonElement("rating")]
        [BsonRepresentation(BsonType.Double)]
        public decimal Rating { get; set; }

        [BsonElement("numReviews")]
        public int NumReviews { get; set; }

        [BsonElement("meter")]
        public int Meter { get; set; }
    }

    public class Viewer
    {
        [BsonElement("rating")]
        [BsonRepresentation(BsonType.Double)]
        public decimal Rating { get; set; }

        [BsonElement("numReviews")]
        public int NumReviews { get; set; }

        [BsonElement("meter")]
        public int Meter { get; set; }
    }

    public class Award
    {
        [BsonElement("wins")]
        public int Wins { get; set; }
        [BsonElement("nominations")]
        public int Nominations { get; set; }
        [BsonElement("text")]
        public string Text { get; set; }
    }

    public class Imdb
    {
        [BsonExtraElements]
        public IDictionary<string, object> ExtraElements { get; set; }
    }
}
