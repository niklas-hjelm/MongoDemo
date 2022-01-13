using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDemo.Models;

namespace MongoDemo.Managers
{
    public class DatabaseManager
    {

        private readonly MongoClient _client;

        private readonly IMongoDatabase _database;

        public DatabaseManager()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            _client = new MongoClient(settings);
            _database = _client.GetDatabase("Bookstores");
        }

        public void InsertDocument<T>(T newDocument, string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);

            collection.InsertOne(newDocument);
        }

        public void UpdateBook(BookModel book)
        {
            var bookCollection = _database.GetCollection<BookModel>("books");

            var filterDefinition = Builders<BookModel>.Filter.Eq(b => b.ObjectId, book.ObjectId);
            var updateDefinition = Builders<BookModel>.Update
                .Set(b => b.Title, book.Title)
                .Set(b => b.Isbn, book.Isbn)
                .Set(b => b.Authors, book.Authors);

            bookCollection.UpdateOne(filterDefinition, updateDefinition);
        }

        #region GetOperations

        public IEnumerable<T> GetAllDocumentsInCollection<T>(string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);

            var allDocuments = collection.Find(_ => true).ToList();

            return allDocuments;
        }

        public IEnumerable<BookModel> GetAllBooksForAuthor(string selectedAuthorObjectId)
        {
            var bookCollection = _database.GetCollection<BookModel>("books");

            var booksForAuthorId = bookCollection.Find(b => b.Authors.Contains(new (selectedAuthorObjectId))).ToList();

            return booksForAuthorId;
        }

        public IEnumerable<BookModel> GetAllBooksForStore(string selectedStoreObjectId)
        {
            var booksInStore = new List<BookModel>();
            var storeCollection = _database.GetCollection<BookstoreModel>("bookstores");

            var store = storeCollection.Find(s => s.ObjectId == selectedStoreObjectId).FirstOrDefault();

            var bookCollection = _database.GetCollection<BookModel>("books");

            foreach (var bookId in store.Books)
            {
                var book = bookCollection.Find(b => b.ObjectId == bookId.ToString()).FirstOrDefault();
                booksInStore.Add(book);
            }

            return booksInStore;
        }

        public IEnumerable<AuthorModel> GetAllAuthorsForBook(string selectedBookObjectId)
        {
            var authorsForBook = new List<AuthorModel>();

            var bookCollection = _database.GetCollection<BookModel>("books");

            var bookForId = bookCollection.Find(b => b.ObjectId == selectedBookObjectId).FirstOrDefault();

            var authorCollection = _database.GetCollection<AuthorModel>("authors");

            foreach (var objectId in bookForId.Authors)
            {
                var author = authorCollection.Find(a => a.ObjectId == objectId.ToString()).FirstOrDefault();
                authorsForBook.Add(author);
            }

            return authorsForBook;
        }

        #endregion
        
    }
}
