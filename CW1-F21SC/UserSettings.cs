using System;

namespace CW1_F21SC;

[Serializable]
public class UserSettings
{
    public string? HomePage { get; set; } // The user's homepage
    public string? DownloadFile { get; set; } // The user's download path
    
    // Default constructor
    public UserSettings()
    {
        // Set default home page to university website
        HomePage = "https://www.hw.ac.uk/";
        // Set default download path to bulk.txt
        DownloadFile = "bulk.txt";
    }
    
    // Constructor with parameters
    public UserSettings(string homePage, string downloadFile)
    {
        HomePage = homePage;
        DownloadFile = downloadFile;
    }
}