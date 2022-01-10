using System.Collections.ObjectModel;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDemo.Managers;
using MongoDemo.Models;

namespace MongoDemo.ViewModels
{
    public class AuthorViewModel : ObservableObject
    {
        private readonly DatabaseManager _databaseManager;

        private ObservableCollection<AuthorModel> _authors = new ();

        public ObservableCollection<AuthorModel> Authors
        {
            get { return _authors; }
            set { SetProperty(ref _authors, value); }
        }

        private AuthorModel _selectedAuthor;

        public AuthorModel SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                SetProperty(ref _selectedAuthor, value);
                UpdateBooks();
            }
        }

        private void UpdateBooks()
        {
            Books.Clear();

            foreach (var bookModel in _databaseManager.GetAllBooksForAuthor(SelectedAuthor.ObjectId))
            {
                Books.Add(bookModel);
            }
        }

        private ObservableCollection<BookModel> _books = new();

        public ObservableCollection<BookModel> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
        }

        public AuthorViewModel(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;

            foreach (var author in _databaseManager.GetAllDocumentsInCollection<AuthorModel>("authors"))
            {
                Authors.Add(author);
            }
        }
    }
}
