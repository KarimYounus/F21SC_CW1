using System;
using System.Collections.Generic;

namespace CW1_F21SC;

/// <summary>
/// This file contains the classes to store the user's history
/// </summary>

// Class to store the user's history
[Serializable]
public class UserHistory
{
    // Dictionary to store history with time value as the key
    public Dictionary<DateTime, String> History { get; set; } = new();
    

    // Method to add a visit to the history
    public void AddVisit(string url) => History.Add(DateTime.Now, url);
}
