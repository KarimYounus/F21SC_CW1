using System;
using System.Collections.Generic;

namespace CW1_F21SC;

/// <summary>
/// This file contains the classes to store the user's history
/// </summary>

// Class to store a visit to a URL
[Serializable]
public class Visit
{
    public string Url { get; set; }
    public DateTime Time { get; set; }
}

// Class to store the user's history
[Serializable]
public class UserHistory
{
    // Dictionary to store history with time value as the key
    public Dictionary<DateTime, String> History { get; set; }

    // Default constructor
    public UserHistory() => History = new Dictionary<DateTime, String>();

    // Copy constructor
    public UserHistory(UserHistory history) => History = new Dictionary<DateTime, String>(history.History);

    // Method to add a visit to the history
    public void AddVisit(string url) => History.Add(DateTime.Now, url);
}
