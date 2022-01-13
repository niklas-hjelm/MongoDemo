using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDemo.Managers;
using MongoDemo.Models;

namespace MongoDemo.ViewModels
{
    public class AuthorViewModel : ObservableObject
    {
        private readonly DatabaseManager _databaseManager;

        private ObservableCollection<AuthorModel> _authors = new ();

        #region Properties
        
        private ObservableCollection<BookModel> _books = new();

        public ObservableCollection<BookModel> Books
        {
            get { return _books; }
            set { SetProperty(ref _books, value); }
        }


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

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }


        #endregion

        public IRelayCommand AddAuthorCommand { get; }

        public AuthorViewModel(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;

            AddAuthorCommand = new RelayCommand(AddAuthor);

            UpdateAuthors();
        }

        private void AddAuthor()
        {
            var newAuthor = new AuthorModel()
            {
                FirstName = FirstName,
                LastName = LastName
            };

            _databaseManager.InsertDocument(newAuthor, "authors");

            UpdateAuthors();

            SelectedAuthor = newAuthor;
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        private void UpdateAuthors()
        {
            foreach (var author in _databaseManager.GetAllDocumentsInCollection<AuthorModel>("authors"))
            {
                if (Authors.All(a=>a.LastName != author.LastName || a.FirstName != author.FirstName))
                {
                    Authors.Add(author);
                }
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
    }
}
