using System;
using System.Collections.Generic;

namespace CW1_F21SC;

public class Favourite
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public class Favourites
{
    private List<Favourite> _favorites = new();

    public void AddFavourite(string name, string url)
    {
        _favorites.Add(new Favourite { Name = name, Url = url });
    }
    
    public void DeleteFavourite(string name)
    {
        _favorites.RemoveAll(f => f.Name == name);
    }
    
    

}