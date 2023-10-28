using System;
using System.Collections.Generic;

namespace CW1_F21SC;

[Serializable]
public class Bookmark
{
    public string Name { get; set; }
    public string Url { get; set; }
    public bool MarkedForDeletion { get; set; }

    public Bookmark(string name, string url)
    {
        Name = name;
        Url = url;
        MarkedForDeletion = false;
    }

}

[Serializable]
public class UserBookmarks
{
    // Dictionary to store bookmarks with Guid as the key
    public Dictionary<Guid, Bookmark> Bookmarks { get; set; }

    // Default constructor
    public UserBookmarks()
    {
        Bookmarks = new Dictionary<Guid, Bookmark>();
    }
    
    // Copy constructor
    public UserBookmarks(UserBookmarks bookmarks)
    {
        Bookmarks = new Dictionary<Guid, Bookmark>(bookmarks.Bookmarks);
    }
    
} 


