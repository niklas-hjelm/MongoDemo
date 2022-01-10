using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Bson;
using MongoDemo.Managers;
using MongoDemo.Models;

namespace MongoDemo.ViewModels
{
    public class BookViewModel : ObservableObject
    {
        private readonly DatabaseManager _databaseManager;

        #region Properties

        private ObservableCollection<BookModel> _books = new();

        public ObservableCollection<BookModel> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
        }

        private BookModel _selectedBook;

        public BookModel SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                SetProperty(ref _selectedBook, value);
                UpdateForm();
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _isbn;

        public string Isbn
        {
            get { return _isbn; }
            set { SetProperty(ref _isbn, value); }
        }

        private ObservableCollection<AuthorModel> _authors = new();

        public ObservableCollection<AuthorModel> Authors
        {
            get { return _authors; }
            set { SetProperty(ref _authors, value); }
        }

        #endregion

        public IRelayCommand SubmitBookCommand { get; }

        public BookViewModel(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;

            SubmitBookCommand = new RelayCommand(SubmitBook);

            foreach (var bookModel in _databaseManager.GetAllDocumentsInCollection<BookModel>("books"))
            {
                Books.Add(bookModel);
            }
        }

        public void SubmitBook()
        {
            SelectedBook.Title = Title;
            SelectedBook.Isbn = Isbn;
            _databaseManager.UpdateBook(SelectedBook);
        }

        private void UpdateForm()
        {
            Title = _selectedBook.Title;
            Isbn = _selectedBook.Isbn;
            Authors.Clear();
            foreach (var selectedBookAuthor in _databaseManager.GetAllAuthorsForBook(_selectedBook.ObjectId))
            {
                Authors.Add(selectedBookAuthor);
            }
        }
    }
}
