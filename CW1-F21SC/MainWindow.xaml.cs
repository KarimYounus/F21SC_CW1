using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CW1_F21SC;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly HttpFunctions _httpFunctions;
    private readonly ViewModel _viewModel = new();
    private UserBookmarks _userBookmarks;
    private UserSettings _userSettings;
    private UserHistory _userHistory;
    private string _currentUrl;
    
    public MainWindow()
    {
        InitializeComponent();
        _httpFunctions = new HttpFunctions(_viewModel);
        DataContext = _viewModel;
        LoadUserSettings();
        LoadUserBookmarks();
        LoadUserHistory();
        _currentUrl = _userSettings.HomePage;
        DisplayHtml(_currentUrl);
        Closing += OnClose;
    }
        
    //Homepage button
    private void OnHomeButtonClick(object sender, RoutedEventArgs e)
    {
        DisplayHtml(_userSettings.HomePage); //Display the HTML of the current homepage
    }
        
    //Go button for URL bar
    private void OnGoButtonClick(object sender, RoutedEventArgs e)
    {
        DisplayHtml(UrlBar.Text); //Display the HTML of the URL
    }
        
    private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
    {
        DisplayHtml(_currentUrl); //Display the HTML of the last loaded URL
    }

    //Add bookmark button
    private void OnAddBookmarkButtonClick(object sender, RoutedEventArgs e)
    {
        //Create a new add bookmark window
        var addBookmarkWindow = new AddBookmarkWindow(UrlBar.Text); 
        var result = addBookmarkWindow.ShowDialog();

        if (result != true) return;
        //_userBookmarks.Bookmarks.Add(Guid.NewGuid(), addBookmarkWindow.NewBookmark);
        _userBookmarks.Bookmarks.Add(Guid.NewGuid(), addBookmarkWindow.NewBookmark);
            
        // Convert the updated UserBookmarks instance to JSON
        var json = JsonSerializer.Serialize(_userBookmarks);

        // Write the JSON to the file
        File.WriteAllText("bookmarks.json", json);
        Console.WriteLine("Bookmark added");
            
    }
        
    //Favorite button
    private void OnBookmarkButtonClick(object sender, RoutedEventArgs e)
    {
        var contextMenu = new ContextMenu();
        var style = FindResource("BookmarkMenuStyle") as Style;
            
        foreach (var bookmark in _userBookmarks.Bookmarks)
        {
            MenuItem item = new MenuItem { Header = bookmark.Value.Name, Style = style};
            item.Click += (_, args) => DisplayHtml(bookmark.Value.Url);
            contextMenu.Items.Add(item);
        }
        contextMenu.IsOpen = true;
    }
            
    //History button
    private void OnHistoryButtonClick(object sender, RoutedEventArgs e)
    {
        HistoryWindow historyWindow = new HistoryWindow(_userHistory); //Create a new history window
        historyWindow.ShowDialog();
    }

    private async void OnDownloadButtonClick(object sender, RoutedEventArgs e)
    {
        var bulkDownload = new BulkDownload(_userSettings.DownloadFile);
        var downloads = await bulkDownload.DownloadFilesAsync();
        HtmlDisplayBox.Text = "Initiating bulk download...";
        if (downloads.Count == 0)
        {
            HtmlDisplayBox.Text = "Error Reading File";
            return;
        }
        if (downloads == null)
        {
            HtmlDisplayBox.Text = "Download Failed";
            return;
        };
        HtmlDisplayBox.Text = "Displaying download results:";
        foreach (var download in downloads)
        {
            HtmlDisplayBox.Text += $"\nStatus Code: {download.StatusCode}, File Size: {download.FileSize} bytes, URL: {download.DownloadUrl}";
        }
    }
    
    //Settings button
    private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
    {
        SettingsWindow settingsWindow = new SettingsWindow(_userSettings, _userBookmarks); //Create a new settings window
        settingsWindow.SettingsUpdate += (_, _) => LoadUserSettings(); //Add an event handler for when the settings are updated
        settingsWindow.BookmarkUpdate += (_, _) => LoadUserBookmarks(); //Add an event handler for when the bookmarks are updated
        settingsWindow.ShowDialog(); 
    }
 
    //Display the HTML of the specified URL
    public async void DisplayHtml(string url)
    {
        var response = await _httpFunctions.SendGetRequest(url); //Send a GET request to the URL
        HtmlDisplayBox.Text = response.content; //If the response is OK, display the response in the HTML display box
        UrlBar.Text = url; //Set the URL bar to the requested URL 
        _currentUrl = url; //Set the current URL to the requested URL
        _userHistory.AddVisit(url);
    }
        
    //Load the user settings from the appsettings.json file
    private void LoadUserSettings()
    {
        //Check if the file exists and create it if it doesn't with default settings
        if (!File.Exists("appsettings.json")) 
        {
            var defaultSettings = new UserSettings(true); //Create a new UserSettings object
            var jsonStringDefault = JsonSerializer.Serialize(defaultSettings); //Serialize the UserSettings object into a JSON string
            File.WriteAllText("appsettings.json", jsonStringDefault); //Write the JSON string to the appsettings.json file
        } 
            
        //Read the file and deserialize the JSON into a UserSettings object
        var jsonString = File.ReadAllText("appsettings.json"); //Read the file
        var settings = JsonSerializer.Deserialize<UserSettings>(jsonString); //Deserialize the JSON into a UserSettings object
        //Debug.Assert(settings != null, nameof(settings) + " != null"); //Check if the settings object is null
        _userSettings = settings; //Set the user settings to the deserialized settings object
    }

    //Load the user bookmarks from the bookmarks.json file
    private void LoadUserBookmarks()
    {
        if (!File.Exists("bookmarks.json")) //Check if the file exists
        {
            _userBookmarks = new UserBookmarks(); //Create a new UserBookmarks object
            var jsonStringBookmarks = JsonSerializer.Serialize(_userBookmarks); //Serialize the UserSettings object into a JSON string
            File.WriteAllText("bookmarks.json", jsonStringBookmarks); //Write the JSON string to the bookmarks.json file
            Console.WriteLine("Bookmark file created");
        } 
        var jsonString = File.ReadAllText("bookmarks.json");
        var bookmarks = JsonSerializer.Deserialize<UserBookmarks>(jsonString);
        _userBookmarks = bookmarks;
    }
        
    //Load the user history from the history.json file
    private void LoadUserHistory()
    {
        if (!File.Exists("history.json")) //Check if the file exists
        {
            _userHistory = new UserHistory(); //Create a new UserHistory object
            var jsonStringHistory = JsonSerializer.Serialize(_userHistory); //Serialize the UserSettings object into a JSON string
            File.WriteAllText("history.json", jsonStringHistory); //Write the JSON string to the bookmarks.json file
            Console.WriteLine("History file created");
        }
            
        var jsonString = File.ReadAllText("history.json");
        var history = JsonSerializer.Deserialize<UserHistory>(jsonString);
        _userHistory = history;
    }
        
    private void OnClose(object? sender, CancelEventArgs cancelEventArgs)
    {
        // Convert the updated UserHistory instance to JSON
        var json = JsonSerializer.Serialize(_userHistory);

        // Write the JSON to the file
        File.WriteAllText("history.json", json);
        Console.WriteLine("History saved");
    }
        
}