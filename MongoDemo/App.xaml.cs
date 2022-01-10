using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MongoDemo.Managers;
using MongoDemo.ViewModels;
using MongoDemo.Views;

namespace MongoDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;
        private readonly DatabaseManager _databaseManager;

        public App()
        {
            _navigationManager = new NavigationManager();
            _databaseManager = new DatabaseManager();

            //_databaseManager.PopulateDb();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationManager.CurrentViewModel = new BookstoreViewModel(_databaseManager);
            var rootWindow = new MainView() { DataContext = new MainViewModel(_navigationManager, _databaseManager) };
            rootWindow.Show();
            base.OnStartup(e);
        }
    }
}
