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
    public List<Bookmark> Bookmarks { get; set; } = new();
    
}

