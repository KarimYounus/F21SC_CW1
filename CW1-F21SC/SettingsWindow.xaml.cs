using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace CW1_F21SC;

public delegate void BookmarkUpdateEventHandler(object sender, EventArgs e);
public delegate void SettingsUpdateEventHandler(object sender, EventArgs e);

public partial class SettingsWindow : Window
{

    private UserBookmarks _updatedBookmarks;
    private UserSettings _settings;
    public event BookmarkUpdateEventHandler? BookmarkUpdate;
    public event SettingsUpdateEventHandler? SettingsUpdate;
    private bool _discardChanges = true;

    public SettingsWindow(UserSettings settings, UserBookmarks bookmarks)
    {
        InitializeComponent();
        _settings = settings;
        _updatedBookmarks = new UserBookmarks(bookmarks);
        HomePageBar.Text = _settings.HomePage;
        Closing += OnCloseWOSave;
        
        // Assign the bookmarks to the ItemsControl
        BookmarksItemsControl.ItemsSource = _updatedBookmarks.Bookmarks;
    }

    private void OnSaveButtonClick(object sender, RoutedEventArgs e)
    {
        _discardChanges = false;
        
        // Create a new user settings object to store the updated settings
        var bookmarksChanged = false;
        string? jsonString;
        var settingsChanged = false;
        
        //If the homepage has changed, 
        if (_settings.HomePage != HomePageBar.Text)
        {
            //Create a new user clone of the user settings . . .
            _settings.HomePage = HomePageBar.Text;
            settingsChanged = true;
        }
        
        
        // If any bookmarks have been marked for deletion, remove them
        foreach (var bookmark in _updatedBookmarks.Bookmarks.Where(bookmark => bookmark.Value.MarkedForDeletion))
        {
            _updatedBookmarks.Bookmarks.Remove(bookmark.Key);
            bookmarksChanged = true;
        }
        
        // Trigger event so that the main window reloads settings
        if (settingsChanged)
        {
            jsonString = JsonSerializer.Serialize(_settings);
            File.WriteAllText("appsettings.json", jsonString);
            SettingsUpdate?.Invoke(this, EventArgs.Empty);
        }
        
        // Trigger event so that the main window reloads bookmarks
        if (bookmarksChanged)
        {
            jsonString = JsonSerializer.Serialize(_updatedBookmarks);
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
        var bookmark = (KeyValuePair<Guid, Bookmark>)clickedButton.DataContext;
        
        ToggleMarkForDeletion(bookmark.Key); // Toggle the bookmark's MarkedForDeletion property
        // Update the button's style
        clickedButton.Style = (Style)FindResource(bookmark.Value.MarkedForDeletion ? "DeleteButtonSelectedStyle" : "DeleteButtonUnselectedStyle");
    }

    // Toggle the bookmark's MarkedForDeletion property
    private void ToggleMarkForDeletion(Guid bookmarkToToggle)
    {
        // Find the bookmark in the bookmarks list and toggle its MarkedForDeletion property
        _updatedBookmarks.Bookmarks[bookmarkToToggle].MarkedForDeletion = !_updatedBookmarks.Bookmarks[bookmarkToToggle].MarkedForDeletion;
    }
    
    private void OnCloseWOSave(object sender, CancelEventArgs e)
    {
        if (_discardChanges)
        {
            foreach (var bookmark in _updatedBookmarks.Bookmarks)
            {
                bookmark.Value.MarkedForDeletion = false;
            }
        }
    }
    
}