using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

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
        private UserBookmarks _userBookmarks;
        private readonly FileSystemWatcher _watcher;
        private DateTime _lastRead = DateTime.MinValue;

        
        public MainWindow()
        {
            InitializeComponent();
            _httpFunctions = new HttpFunctions(_viewModel);
            DataContext = _viewModel;
            LoadUserSettings();
            LoadUserBookmarks();
            DisplayHtml(_homepage);
            
            // Initialize FileSystemWatcher to monitor for setting changes during runtime
            _watcher = new FileSystemWatcher
            {
                Path = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "appsettings.json",
                NotifyFilter = NotifyFilters.LastWrite
            };
            
            _watcher.Changed += OnSettingsChanged;
            _watcher.EnableRaisingEvents = true;

        }
        
        //Homepage button
        private async void OnHomeButtonClick(object sender, RoutedEventArgs e)
        {
            DisplayHtml(_homepage); //Display the HTML of the current homepage
        }
        
        //Go button for URL bar
        private async void OnGoButtonClick(object sender, RoutedEventArgs e)
        {
            DisplayHtml(UrlBar.Text); //Display the HTML of the URL
        }

        //Add bookmark button
        private void OnAddBookmarkButtonClick(object sender, RoutedEventArgs e)
        {
            var addBookmarkWindow = new AddBookmarkWindow(UrlBar.Text); //Create a new add bookmark window
            var result = addBookmarkWindow.ShowDialog();

            if (result != true) return;
            _userBookmarks.Bookmarks.Add(addBookmarkWindow.NewBookmark);
            
            // Convert the updated UserBookmarks instance to JSON
            string json = JsonSerializer.Serialize(_userBookmarks);

            // Write the JSON to the file
            File.WriteAllText("bookmarks.json", json);
            Console.WriteLine("Bookmark added");
            
        }
        
        //Favorite button
        private void OnBookmarkButtonClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Favorite button clicked");
            ContextMenu contextMenu = new ContextMenu();
            Style style = FindResource("BookmarkMenuStyle") as Style;
            
            foreach (var bookmark in _userBookmarks.Bookmarks)
            {
                MenuItem item = new MenuItem { Header = bookmark.Name, Style = style};
                item.Click += (_, args) => DisplayHtml(bookmark.Url);
                contextMenu.Items.Add(item);
            }
            contextMenu.IsOpen = true;
        }
        
        //Settings button
        private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(_homepage, _userBookmarks); //Create a new settings window
            settingsWindow.ShowDialog(); 
        }
        
        //Display the HTML of the specified URL
        private async void DisplayHtml(string url)
        {
            var response = await _httpFunctions.SendGetRequest(url); //Send a GET request to the URL
            HtmlDisplayBox.Text = response.content; //If the response is OK, display the response in the HTML display box
            UrlBar.Text = url; //Set the URL bar to the requested URL 
        }
        
        //Load the user settings from the appsettings.json file
        private void LoadUserSettings()
        {
            if (!File.Exists("appsettings.json")) //Check if the file exists
            {
                UserSettings defaultSettings = new UserSettings(); //Create a new UserSettings object
                string jsonStringDefault = JsonSerializer.Serialize(defaultSettings); //Serialize the UserSettings object into a JSON string
                File.WriteAllText("appsettings.json", jsonStringDefault); //Write the JSON string to the appsettings.json file
            } 
            var jsonString = File.ReadAllText("appsettings.json"); //Read the file
            var settings = JsonSerializer.Deserialize<UserSettings>(jsonString); //Deserialize the JSON into a UserSettings object
            //Debug.Assert(settings != null, nameof(settings) + " != null"); //Check if the settings object is null
            _homepage = settings.HomePage; //Set the homepage to the homepage specified in the settings
        }

        //Load the user bookmarks from the bookmarks.json file
        private void LoadUserBookmarks()
        {
            if (!File.Exists("bookmarks.json")) //Check if the file exists
            {
                UserBookmarks userBookmarks = new UserBookmarks(); //Create a new UserBookmarks object
                string jsonStringBookmarks = JsonSerializer.Serialize(userBookmarks); //Serialize the UserSettings object into a JSON string
                File.WriteAllText("bookmarks.json", jsonStringBookmarks); //Write the JSON string to the bookmarks.json file
                Console.WriteLine("Bookmark file created");
            } 
            var jsonString = File.ReadAllText("bookmarks.json");
            var bookmarks = JsonSerializer.Deserialize<UserBookmarks>(jsonString);
            _userBookmarks = bookmarks;
        }
        
        //Event handler for when the appsettings.json file is changed
        private void OnSettingsChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return; //Check if the event is a change event
            
            //FileWatcher raises multiple events when a file is changed, we eliminate duplicate reloads with this check
            
            DateTime lastWriteTime = File.GetLastWriteTime("appsettings.json"); //Get the last write time of the file
            
            //If the last write time is the same as the last read time, then the event is a duplicate and we can ignore it
            if (lastWriteTime == _lastRead) return;
            
            //Else we reload the user settings
            LoadUserSettings();
            Console.WriteLine(File.ReadAllText("appsettings.json"));
            _lastRead = lastWriteTime;
        }
        
    }
}