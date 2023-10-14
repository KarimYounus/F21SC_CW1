using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CW1_F21SC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpFunctions _httpFunctions;
        private readonly ViewModel _viewModel = new ViewModel();
        private string _homepage;
        private readonly FileSystemWatcher _watcher;
        private DateTime _lastRead = DateTime.MinValue;

        
        public MainWindow()
        {
            InitializeComponent();
            _httpFunctions = new HttpFunctions(_viewModel);
            DataContext = _viewModel;
            LoadUserSettings();
            DisplayHtml(_homepage);
            
            // Initialize FileSystemWatcher
            _watcher = new FileSystemWatcher
            {
                Path = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "appsettings.json",
                NotifyFilter = NotifyFilters.LastWrite
            };
            
            _watcher.Changed += OnSettingsChanged;
            _watcher.EnableRaisingEvents = true;

        }
        
        //Go button for URL bar
        private async void OnGoButtonClick(object sender, RoutedEventArgs e)
        {
            var url = UrlBar.Text; //Get the URL from the URL bar
            DisplayHtml(url);
        }
        
        //Homepage button
        private async void OnHomeButtonClick(object sender, RoutedEventArgs e)
        {
            DisplayHtml(_homepage);
        }
        
        //Settings button
        private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }
        
        //Display the HTML of the specified URL
        private async void DisplayHtml(string url)
        {
            var response = await _httpFunctions.SendGetRequest(url); //Send a GET request to the URL
            HtmlDisplayBox.Text = response.content; //If the response is OK, display the response in the HTML display box
        }
        
        private void OnFavoriteButtonClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Favorite button clicked");
            ContextMenu contextMenu = new ContextMenu();

            MenuItem item1 = new MenuItem { Header = "Item 1" };
            MenuItem item2 = new MenuItem { Header = "Item 2" };
            MenuItem item3 = new MenuItem { Header = "Item 3" };

            contextMenu.Items.Add(item1);
            contextMenu.Items.Add(item2);
            contextMenu.Items.Add(item3);

            contextMenu.IsOpen = true;
        }

        //Load the user settings from the appsettings.json file
        private void LoadUserSettings()
        {
            if (!File.Exists("appsettings.json")) return;
            var jsonString = File.ReadAllText("appsettings.json");
            var settings = JsonSerializer.Deserialize<UserSettings>(jsonString);
            Debug.Assert(settings != null, nameof(settings) + " != null");
            _homepage = settings.HomePage;
        }
        
        //Event handler for when the appsettings.json file is changed
        private void OnSettingsChanged(object source, FileSystemEventArgs e)
        {
            //Eliminate duplicate events by checking the last write time of the file
            DateTime lastWriteTime = File.GetLastWriteTime("appsettings.json");
            if (lastWriteTime != _lastRead)
            {
                if (e.ChangeType != WatcherChangeTypes.Changed) return;
                LoadUserSettings();
                Console.WriteLine(File.ReadAllText("appsettings.json"));
                _lastRead = lastWriteTime;
            }

        }
        
    }
}