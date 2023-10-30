using System;
using System.Collections.Generic;

namespace CW1_F21SC;

/// <summary>
/// This file contains the classes used to store the user's bookmarks. Both classes are serializable so they can be
/// saved to bookmarks.json.
/// </summary>

// Class to store the bookmark information
[Serializable]
public class Bookmark
{
    public string Name { get; set; } // The name of the bookmark
    public string Url { get; set; } // The URL of the bookmark
    public bool MarkedForDeletion { get; set; } // Whether the bookmark is marked for deletion in the settings window

    // Default constructor
    public Bookmark(string name, string url)
    {
        Name = name;
        Url = url;
        MarkedForDeletion = false;
    }
}

// Class to store the user's bookmarks in a dictionary
[Serializable]
public class UserBookmarks
{
    // Dictionary to store bookmarks with Guid as the key
    public Dictionary<Guid, Bookmark> Bookmarks { get; set; }

    // Default constructor
    public UserBookmarks() => Bookmarks = new Dictionary<Guid, Bookmark>();

    // Copy constructor
    public UserBookmarks(UserBookmarks? bookmarks) => Bookmarks = new Dictionary<Guid, Bookmark>(bookmarks.Bookmarks);
} 


