using System;
using System.Collections.Generic;
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

        private AuthorModel _selectedAuthorToAdd;

        public AuthorModel SelectedAuthorToAdd
        {
            get { return _selectedAuthorToAdd; }
            set
            {
                SetProperty(ref _selectedAuthorToAdd, value);
            }
        }

        private AuthorModel _selectedAuthorToRemove;

        public AuthorModel SelectedAuthorToRemove
        {
            get { return _selectedAuthorToRemove; }
            set
            {
                SetProperty(ref _selectedAuthorToRemove, value);
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

        private ObservableCollection<AuthorModel> _allAuthors = new();

        public ObservableCollection<AuthorModel> AllAuthors
        {
            get { return _allAuthors; }
            set { SetProperty(ref _allAuthors, value); }
        }

        #endregion

        public IRelayCommand SubmitBookCommand { get; }
        public IRelayCommand AddBookCommand { get; }
        public IRelayCommand AddAuthorCommand { get; }
        public IRelayCommand RemoveAuthorCommand { get; }

        public BookViewModel(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;

            SubmitBookCommand = new RelayCommand(SubmitBook);
            AddBookCommand = new RelayCommand(AddBook);
            AddAuthorCommand = new RelayCommand(AddAuthor);
            RemoveAuthorCommand = new RelayCommand(RemoveAuthor);

            UpdateBooks();
            UpdateAllAuthors();
        }

        private void RemoveAuthor()
        {
            if (_selectedAuthorToRemove == null)
            {
                return;
            }

            Authors.Remove(SelectedAuthorToRemove);
        }

        private void AddAuthor()
        {
            if (_selectedAuthorToAdd == null)
            {
                return;
            }

            if (Authors.Any(a => a.ObjectId == _selectedAuthorToAdd.ObjectId))
            {
                return;
            }

            Authors.Add(SelectedAuthorToAdd);
        }

        private void UpdateBooks()
        {
            foreach (var bookModel in _databaseManager.GetAllDocumentsInCollection<BookModel>("books"))
            {
                if (Books.All(b => b.Isbn != bookModel.Isbn))
                {
                    Books.Add(bookModel);
                }
            }
        }
        private void UpdateAllAuthors()
        {
            foreach (var authorModel in _databaseManager.GetAllDocumentsInCollection<AuthorModel>("authors"))
            {
                AllAuthors.Add(authorModel);
            }
        }

        private void AddBook()
        {
            var newBook = new BookModel()
            {
                Title = Title,
                Isbn = Isbn,
                Authors = Authors.Select(a=> new ObjectId(a.ObjectId)).ToList()
            };

            _databaseManager.InsertDocument(newBook, "books");
            UpdateBooks();
            SelectedBook = newBook;
        }

        public void SubmitBook()
        {
            SelectedBook.Title = Title;
            SelectedBook.Isbn = Isbn;
            SelectedBook.Authors = Authors.Select(a => new ObjectId(a.ObjectId)).ToList();
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
