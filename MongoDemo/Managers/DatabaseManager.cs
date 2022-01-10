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
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://carrey87:uLPk8VMApcYJQ7c@democluster.inlue.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            _client = new MongoClient(settings);
            _database = _client.GetDatabase("Bookstores");
        }

        public void UpdateBook(BookModel book)
        {
            var bookCollection = _database.GetCollection<BookModel>("books");

            var filterDefinition = Builders<BookModel>.Filter.Eq(b => b.ObjectId, book.ObjectId);
            var updateDefinition = Builders<BookModel>.Update
                .Set(b => b.Title, book.Title)
                .Set(b => b.Isbn, book.Isbn)
                .Set(b => b.Authors, book.Authors);

            var result = bookCollection.UpdateOne(filterDefinition, updateDefinition);
        }

        #region GetOperations

        public IEnumerable<T> GetAllDocumentsInCollection<T>(string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);

            var allDocuments = collection.Find(_ => true);

            return allDocuments.ToList();
        }

        public IEnumerable<BookModel> GetAllBooksForAuthor(string selectedAuthorObjectId)
        {
            var bookCollection = _database.GetCollection<BookModel>("books");

            var booksForAuthorId = bookCollection.Find(b => b.Authors.Contains(new(selectedAuthorObjectId))).ToList();

            return booksForAuthorId;
        }

        public IEnumerable<BookModel> GetAllBooksForStore(string objectId)
        {
            var booksInStore = new List<BookModel>();
            var storeCollection = _database.GetCollection<BookstoreModel>("bookstores");

            var store = storeCollection.Find(s => s.ObjectId == objectId).FirstOrDefault();

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

        #region NotInUse

        public List<MovieModel> GetAllMovies()
        {
            var database = _client.GetDatabase("sample_mflix");

            var collection = database.GetCollection<MovieModel>("movies");

            var allDocuments = collection.Find(_ => true).ToList();

            return allDocuments;
        }

        public void PopulateDb()
        {
            var authorCollection = _database.GetCollection<AuthorModel>("authors");

            authorCollection.InsertMany(new List<AuthorModel>
            {
                new (){FirstName = "Astrid", LastName = "Lindgren"},
                new (){FirstName = "William", LastName = "Shakespeare"},
                new (){FirstName = "Douglas", LastName = "Adams"}
            });

            var bookCollection = _database.GetCollection<BookModel>("books");

            var author1 = authorCollection.Find(s => s.FirstName == "Astrid").FirstOrDefault();
            var author2 = authorCollection.Find(s => s.FirstName == "William").FirstOrDefault();
            var author3 = authorCollection.Find(s => s.FirstName == "Douglas").FirstOrDefault();

            bookCollection.InsertMany(new List<BookModel>
            {
                new (){ Title = "Ronja Rövardotter", Authors = new List<ObjectId> {new (author1.ObjectId)} },
                new (){ Title = "Hamlet", Authors = new List<ObjectId> {new (author2.ObjectId) } },
                new (){ Title = "The Hitchhiker's Guide to the Galaxy", Authors = new List<ObjectId> {new (author3.ObjectId) } }
            });

            var storeCollection = _database.GetCollection<BookstoreModel>("bookstores");

            var book1 = bookCollection.Find(s => s.Title.Contains("Ronja")).FirstOrDefault();
            var book2 = bookCollection.Find(s => s.Title.Contains("Hamlet")).FirstOrDefault();
            var book3 = bookCollection.Find(s => s.Title.Contains("Guide")).FirstOrDefault();

            storeCollection.InsertMany(new List<BookstoreModel>
            {
                new (){ Name = "ITHS Bokhandel", Books = new List<ObjectId>
                {
                    new (book1.ObjectId),
                    new (book2.ObjectId),
                    new (book3.ObjectId)
                }}
            });
        }

        #endregion

    }
}
