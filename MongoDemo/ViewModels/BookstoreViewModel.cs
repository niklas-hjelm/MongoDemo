using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDemo.Managers;
using MongoDemo.Models;

namespace MongoDemo.ViewModels
{
    public class BookstoreViewModel: ObservableObject
    {
        private readonly DatabaseManager _databaseManager;

        private ObservableCollection<BookstoreModel> _bookstores = new ();

        public ObservableCollection<BookstoreModel> Bookstores
        {
            get { return _bookstores; }
            set { SetProperty(ref _bookstores, value); }
        }

        private ObservableCollection<BookModel> _books = new();

        public ObservableCollection<BookModel> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
        }

        private BookstoreModel _selectedBookstore;

        public BookstoreModel SelectedBookstore
        {
            get { return _selectedBookstore; }
            set
            {
                SetProperty(ref _selectedBookstore, value);
                UpdateBooks();
            }
        }

        public BookstoreViewModel(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;

            foreach (var bookstoreModel in _databaseManager.GetAllDocumentsInCollection<BookstoreModel>("bookstores"))
            {
                Bookstores.Add(bookstoreModel);
            }
        }
        private void UpdateBooks()
        {
            Books.Clear();

            foreach (var bookModel in _databaseManager.GetAllBooksForStore(SelectedBookstore.ObjectId))
            {
                Books.Add(bookModel);
            }
        }
    }
}
