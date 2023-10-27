using System;
using System.Windows;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace CW1_F21SC;

public delegate void BookmarkUpdateEventHandler(object sender, EventArgs e);
public delegate void SettingsUpdateEventHandler(object sender, EventArgs e);

public partial class SettingsWindow : Window
{
    private UserBookmarks _bookmarks;
    private UserSettings _settings;
    public event BookmarkUpdateEventHandler? BookmarkUpdate;
    public event SettingsUpdateEventHandler? SettingsUpdate;

    public SettingsWindow(UserSettings settings, UserBookmarks bookmarks)
    {
        InitializeComponent();
        _settings = settings;
        _bookmarks = bookmarks;
        HomePageBar.Text = _settings.HomePage;
        
        // Assign the bookmarks to the ItemsControl
        BookmarksItemsControl.ItemsSource = bookmarks.Bookmarks;
    }

    private void OnSaveButtonClick(object sender, RoutedEventArgs e)
    {

        // Create a new user settings object to store the updated settings
        var bookmarksChanged = false;
        var settingsChanged = false;
        
        //If the homepage has changed, 
        if (_settings.HomePage != HomePageBar.Text)
        {
            //Create a new user clone of the user settings . . .
            _settings.HomePage = HomePageBar.Text;
            settingsChanged = true;
        }
        
        // If any bookmarks have been marked for deletion, remove them
        foreach (var bookmark in _bookmarks.Bookmarks)
        {
            if(!bookmark.MarkedForDeletion) continue;
            _bookmarks.Bookmarks.Remove(bookmark);
            bookmarksChanged = true;
            Console.WriteLine($"Removed {bookmark.Name}");
        }
        
        // Trigger event so that the main window reloads settings
        if (settingsChanged)
        {
            string jsonString = JsonSerializer.Serialize(_settings);
            File.WriteAllText("appsettings.json", jsonString);
            SettingsUpdate?.Invoke(this, EventArgs.Empty);
        }
        
        // Trigger event so that the main window reloads bookmarks
        if (bookmarksChanged)
        {
            string jsonString = JsonSerializer.Serialize(_bookmarks);
            File.WriteAllText("bookmarks.json", jsonString);
            BookmarkUpdate?.Invoke(this, EventArgs.Empty);
        }
        HomePageBar.Clear();
        Close();
    }
    

    // Bookmark Delete Button 
    private void OnDeleteBookmarkButtonClick(object sender, RoutedEventArgs e)
    {
        var clickedButton = (Button)sender;
        var bookmark = (Bookmark)clickedButton.DataContext;
        
        ToggleMarkForDeletion(bookmark); // Toggle the bookmark's MarkedForDeletion property
        // Update the button's style
        clickedButton.Style = (Style)FindResource(bookmark.MarkedForDeletion ? "DeleteButtonSelectedStyle" : "DeleteButtonUnselectedStyle");
    }

    // Toggle the bookmark's MarkedForDeletion property
    private void ToggleMarkForDeletion(Bookmark updatedBookmark)
    {
        // Find the bookmark in the bookmarks list and toggle its MarkedForDeletion property
        _bookmarks.Bookmarks.Find(currentBookmark => currentBookmark.Name == updatedBookmark.Name).MarkedForDeletion = !updatedBookmark.MarkedForDeletion;
    }
    
    
}