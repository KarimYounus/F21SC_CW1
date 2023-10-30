using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Controls;

namespace CW1_F21SC;

public delegate void BookmarkUpdateEventHandler(object sender, EventArgs e);
public delegate void SettingsUpdateEventHandler(object sender, EventArgs e);

/// <summary>
/// This class is responsible for the settings window of the application. It allows the user to change their homepage, download file, and edit their bookmarks.
/// </summary>

public partial class SettingsWindow : Window
{

    private UserBookmarks _bookmarks;
    private UserSettings _settings;
    public event BookmarkUpdateEventHandler? BookmarkUpdate;
    public event SettingsUpdateEventHandler? SettingsUpdate;
    private bool _discardChanges = true;

    
    public SettingsWindow(UserSettings settings, UserBookmarks? bookmarks)
    {
        InitializeComponent();
        _settings = settings;
        _bookmarks = bookmarks;
        HomePageBar.Text = _settings.HomePage; // Set the homepage text box to the current homepage
        DownloadFileBar.Text = _settings.DownloadFile; // Set the download file text box to the current download file
        Closing += OnCloseWOSave;
        
        // Assign the bookmarks to the ItemsControl
        BookmarksItemsControl.ItemsSource = _bookmarks.Bookmarks;
    }

    // Save Settings Button
    private void OnSaveButtonClick(object sender, RoutedEventArgs e)
    {
        _discardChanges = false;
        
        // Create a new user settings object to store the updated settings
        var bookmarksChanged = false;
        var settingsChanged = false;
        
        //If the homepage has changed, save the new homepage 
        if (_settings.HomePage != HomePageBar.Text)
        {
            _settings.HomePage = HomePageBar.Text;
            settingsChanged = true;
        }
        //If the download file has changed, save the new download file
        if(_settings.DownloadFile != DownloadFileBar.Text)
        {
            _settings.DownloadFile = DownloadFileBar.Text;
            settingsChanged = true;
        }
        
        //Check if any bookmarks have been changed
        foreach (var bookmark in _bookmarks.Bookmarks.Where(bookmark => bookmark.Value.MarkedForDeletion))
        {
            _bookmarks.Bookmarks.Remove(bookmark.Key);
            bookmarksChanged = true;
        }
        
        // If any bookmarks details have been changed, update them in the bookmarks dictionary
        foreach (var bookmark in BookmarksItemsControl.Items)
        {
            var (bookmarkGuid, bookmarkName, bookmarkUrl) = GetBookmarkDetailsFromItem(bookmark);

            // If the bookmark name and URL haven't changed, continue
            if (_bookmarks.Bookmarks[bookmarkGuid].Name == bookmarkName && _bookmarks.Bookmarks[bookmarkGuid].Url == bookmarkUrl) continue;
                
            // If the bookmark name has changed, update it in the bookmarks dictionary
            _bookmarks.Bookmarks[bookmarkGuid].Name = bookmarkName;
            _bookmarks.Bookmarks[bookmarkGuid].Url = bookmarkUrl;
            bookmarksChanged = true;
        }
        
        // Trigger event so that the main window reloads settings
        if (settingsChanged)
        {
            Serializer.SerializeFile(_settings, "appsettings.json");
            SettingsUpdate?.Invoke(this, EventArgs.Empty);
        }
        
        // Trigger event so that the main window reloads bookmarks
        if (bookmarksChanged)
        {
            Serializer.SerializeFile(_bookmarks, "bookmarks.json");
            BookmarkUpdate?.Invoke(this, EventArgs.Empty);
            
        }
        Close();
    }
    

    // Bookmark Delete Button 
    private void OnDeleteBookmarkButtonClick(object sender, RoutedEventArgs e)
    {
        var clickedButton = (Button)sender;
        
        // Get the bookmark from the button's data context
        var bookmark = (KeyValuePair<Guid, Bookmark>)clickedButton.DataContext;
        
        // Toggle the bookmark's MarkedForDeletion property
        ToggleMarkForDeletion(bookmark.Key); 
        
        // Update the button's style
        clickedButton.Style = (Style)FindResource(bookmark.Value.MarkedForDeletion ? "DeleteButtonSelectedStyle" : "DeleteButtonUnselectedStyle");
    }

    // Toggle the bookmark's MarkedForDeletion property
    private void ToggleMarkForDeletion(Guid bookmarkToToggle)
    {
        // Find the bookmark in the bookmarks dictionary and toggle its MarkedForDeletion property
        _bookmarks.Bookmarks[bookmarkToToggle].MarkedForDeletion = !_bookmarks.Bookmarks[bookmarkToToggle].MarkedForDeletion;
    }
    
    // Get the bookmark details from an item
    private (Guid id, string name, string url) GetBookmarkDetailsFromItem(object item)
    {
        
        var container = (ContentPresenter)BookmarksItemsControl.ItemContainerGenerator.ContainerFromItem(item);
        var id = item is KeyValuePair<Guid, Bookmark> pair ? pair.Key : default;
        TextBox? nameBox = null;
        TextBox? urlBox = null;
        
        var cp = (ContentPresenter)BookmarksItemsControl.ItemContainerGenerator.ContainerFromItem(item);
        nameBox = cp.ContentTemplate.FindName("BookmarkNameBar", cp) as TextBox;
        urlBox = cp.ContentTemplate.FindName("BookmarkURLBar", cp) as TextBox;
        
        //id = nameBox.Tag as Guid? ?? default;

        return (id, nameBox.Text, urlBox.Text);
    }

    
    // Discard Changes on Close without Saving
    private void OnCloseWOSave(object sender, CancelEventArgs e)
    {
        if (!_discardChanges) return;
        // Reset the MarkedForDeletion property of all bookmarks to false
        foreach (var bookmark in _bookmarks.Bookmarks) bookmark.Value.MarkedForDeletion = false;
    }
    
}