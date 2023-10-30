using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CW1_F21SC;

/// <summary>
/// This class is responsible for the main window of the application. It is the entry point of the application and serves as the main hub for the user.
/// It contains the HTML display box, the URL bar, and the buttons for the user to interact with.
/// It loads the user settings, bookmarks and history from their respective JSON files on startup.
/// </summary>
public partial class MainWindow : Window
{
    private readonly HttpFunctions _httpFunctions; 
    private readonly ViewModel _viewModel = new(); 
    private UserBookmarks? _userBookmarks;
    private UserSettings _userSettings;
    private UserHistory? _userHistory;
    public const string BookmarksFileName = "bookmarks.json";
    public const string SettingsFileName = "appsettings.json";
    private const string HistoryFileName = "history.json";
    
    private string _currentUrl;
    
    public MainWindow()
    {
        InitializeComponent();
        _httpFunctions = new HttpFunctions(_viewModel);
        DataContext = _viewModel;
        LoadUserSettings(); //Load the user settings from the appsettings.json file
        LoadUserBookmarks(); //Load the user bookmarks from the bookmarks.json file
        LoadUserHistory(); //Load the user history from the history.json file
        _currentUrl = _userSettings.HomePage; //Set the current URL to the homepage
        DisplayHtml(_currentUrl); //Display the HTML of the homepage
        Closing += OnClose; 
    }
        
    //Homepage button
    private void OnHomeButtonClick(object sender, RoutedEventArgs e) => DisplayHtml(_userSettings.HomePage); //Display the HTML of the homepage

    //Go button for URL bar
    private void OnGoButtonClick(object sender, RoutedEventArgs e) => DisplayHtml(UrlBar.Text); //Display the HTML of the URL in the URL bar

    //Refresh button
    private void OnRefreshButtonClick(object sender, RoutedEventArgs e) => DisplayHtml(_currentUrl); //Display the HTML of the last loaded URL

    //Add bookmark button
    private void OnAddBookmarkButtonClick(object sender, RoutedEventArgs e)
    {
        //Create a new add bookmark window
        var addBookmarkWindow = new AddBookmarkWindow(UrlBar.Text); 
        var result = addBookmarkWindow.ShowDialog();
        
        //If user saves the bookmark, add it to the bookmarks list
        if (result != true) return;
        _userBookmarks.Bookmarks.Add(Guid.NewGuid(), addBookmarkWindow.NewBookmark);
        
        //Serialize the bookmarks list and write it to the bookmarks.json file
        Serializer.SerializeFile(_userBookmarks, BookmarksFileName);
        
        Console.WriteLine("Bookmark added");
    }
        
    //Bookmarks button
    private void OnBookmarkButtonClick(object sender, RoutedEventArgs e)
    {
        var contextMenu = new ContextMenu();
        var style = FindResource("BookmarkMenuStyle") as Style; 
        
        //Create a new menu item for each bookmark
        foreach (var bookmark in _userBookmarks.Bookmarks)
        {
            var item = new MenuItem { Header = bookmark.Value.Name, Style = style};
            item.Click += (_, _) => DisplayHtml(bookmark.Value.Url);
            contextMenu.Items.Add(item);
        }
        //Open the context menu
        contextMenu.IsOpen = true;
    }
            
    //History button
    private void OnHistoryButtonClick(object sender, RoutedEventArgs e)
    {
        var historyWindow = new HistoryWindow(_userHistory); //Create a new history window
        historyWindow.ShowDialog();
    }

    private async void OnDownloadButtonClick(object sender, RoutedEventArgs e)
    {
        //Create a new bulk download object
        var bulkDownload = new BulkDownload(_userSettings.DownloadFile);
        //Download the files
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
        var settingsWindow = new SettingsWindow(_userSettings, _userBookmarks); //Create a new settings window
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
        if (!File.Exists(SettingsFileName)) Serializer.SerializeFile(new UserSettings(), SettingsFileName); 
        //Deserialize the settings from the file
        _userSettings = (UserSettings)Serializer.DeserializeFile(new UserSettings(), SettingsFileName); 
    }

    //Load the user bookmarks from the bookmarks.json file
    private void LoadUserBookmarks()
    {
        //Check if the file exists and create it if it doesn't
        if (!File.Exists(BookmarksFileName)) Serializer.SerializeFile(new UserBookmarks(), BookmarksFileName);
        //Deserialize the bookmarks from the file
        _userBookmarks = (UserBookmarks)Serializer.DeserializeFile(new UserBookmarks(), BookmarksFileName);
    }
        
    //Load the user history from the history.json file
    private void LoadUserHistory()
    {
        //Check if the file exists and create it if it doesn't
        if (!File.Exists(HistoryFileName)) Serializer.SerializeFile(new UserHistory(), HistoryFileName);
        //Deserialize the history from the file
        _userHistory = (UserHistory)Serializer.DeserializeFile(new UserHistory(), HistoryFileName);
    }
        
    //Save the user history to the history.json file on close
    private void OnClose(object? sender, CancelEventArgs cancelEventArgs)
    {
        Serializer.SerializeFile(_userHistory, HistoryFileName);
        Console.WriteLine("History saved");
    }
        
}