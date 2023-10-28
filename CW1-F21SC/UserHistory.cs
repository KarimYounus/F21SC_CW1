using System;
using System.Collections.Generic;

namespace CW1_F21SC;

[Serializable]
public class Visit
{
    public string Url { get; set; }
    public DateTime Time { get; set; }
}

[Serializable]
public class UserHistory
{
    // Dictionary to store history with Guid as the key
    public Dictionary<DateTime, String> History { get; set; }

    // Default constructor
    public UserHistory()
    {
        History = new Dictionary<DateTime, String>();
    }
    
    // Copy constructor
    public UserHistory(UserHistory history)
    {
        History = new Dictionary<DateTime, String>(history.History);
    }
    
    public void AddVisit(string url)
    {
        History.Add(DateTime.Now, url);
    }
    
}
