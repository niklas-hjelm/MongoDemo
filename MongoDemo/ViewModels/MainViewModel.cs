using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDemo.Managers;

namespace MongoDemo.ViewModels
{
    class MainViewModel: ObservableObject
    {
        private readonly NavigationManager _navigationManager;
        private readonly DatabaseManager _databaseManager;

        #region Commands

        public IRelayCommand NavigateStoresCommand { get; }
        public IRelayCommand NavigateAuthorsCommand { get; }
        public IRelayCommand NavigateBooksCommand { get; }

        #endregion

        public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;

        public MainViewModel(NavigationManager navigationManager, DatabaseManager databaseManager)
        {
            _navigationManager = navigationManager;
            _databaseManager = databaseManager;
            DatabaseTest();

            NavigateStoresCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new BookstoreViewModel(_databaseManager));
            NavigateAuthorsCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new AuthorViewModel(_databaseManager));
            NavigateBooksCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new BookViewModel(_databaseManager));

            _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
        }

        private void DatabaseTest()
        {
            //var movies = _databaseManager.GetAllMovies();

        }
        
        private void CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }
}
