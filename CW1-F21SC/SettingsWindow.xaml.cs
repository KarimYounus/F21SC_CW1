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

public partial class SettingsWindow : Window
{

    private UserBookmarks _updatedBookmarks;
    private readonly UserBookmarks _originalBookmarks;
    private UserSettings _settings;
    public event BookmarkUpdateEventHandler? BookmarkUpdate;
    public event SettingsUpdateEventHandler? SettingsUpdate;
    private bool _discardChanges = true;


    public SettingsWindow(UserSettings settings, UserBookmarks bookmarks)
    {
        InitializeComponent();
        _settings = settings;
        _originalBookmarks = bookmarks;
        _updatedBookmarks = new UserBookmarks(bookmarks);
        HomePageBar.Text = _settings.HomePage;
        DownloadFileBar.Text = _settings.DownloadFile;
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
            _settings.HomePage = HomePageBar.Text;
            settingsChanged = true;
        }
        //If the download file has changed
        if(_settings.DownloadFile != DownloadFileBar.Text)
        {
            _settings.DownloadFile = DownloadFileBar.Text;
            settingsChanged = true;
        }
        
        //Check if any bookmarks have been changed
        foreach (var bookmark in _updatedBookmarks.Bookmarks)
        {
            // If any bookmarks have been marked for deletion, remove them from the bookmarks dictionary
            if (bookmark.Value.MarkedForDeletion)
            {
                _updatedBookmarks.Bookmarks.Remove(bookmark.Key);
                bookmarksChanged = true;
                continue;
            }
            
            // If any bookmarks details have been changed, update them in the bookmarks dictionary
            foreach (var item in BookmarksItemsControl.Items)
            {
                var (bookmarkGuid, bookmarkName, bookmarkUrl) = GetBookmarkDetailsFromItem(item);

                if (bookmark.Value.Name == bookmarkName && bookmark.Value.Url == bookmarkUrl) continue;
                
                _updatedBookmarks.Bookmarks[bookmarkGuid].Name = bookmarkName;
                _updatedBookmarks.Bookmarks[bookmarkGuid].Url = bookmarkUrl;
                bookmarksChanged = true;
            }
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
    
    private (Guid id, string name, string url) GetBookmarkDetailsFromItem(object item)
    {
        var cp = (ContentPresenter)BookmarksItemsControl.ItemContainerGenerator.ContainerFromItem(item);
        var nameBox = cp.ContentTemplate.FindName("BookmarkNameBar", cp) as TextBox;
        var urlBox = cp.ContentTemplate.FindName("BookmarkURLBar", cp) as TextBox;

        return ((Guid)nameBox.Tag, nameBox.Text, urlBox.Text);
    }

    
    private void OnCloseWOSave(object sender, CancelEventArgs e)
    {
        if (!_discardChanges) return;
        foreach (var bookmark in _updatedBookmarks.Bookmarks)
        {
            bookmark.Value.MarkedForDeletion = false;
        }
    }
    
}