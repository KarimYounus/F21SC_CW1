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
    }
    
    public UserSettings(bool defaultSettings)
    {
        // Set default values
        DefaultSettings();
        Console.WriteLine("Default settings loaded");
    }
    
    // Copy constructor
    public UserSettings(UserSettings settings)
    {
        HomePage = settings.HomePage;
        DownloadFile = settings.DownloadFile;
    }
    
    // Set default values
    private void DefaultSettings()
    {
        // Set default home page to university website
        HomePage = "https://www.hw.ac.uk/";
        // Set default download path to bulk.txt
        DownloadFile = "bulk.txt";
    }
}