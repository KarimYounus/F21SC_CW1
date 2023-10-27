using System;

namespace CW1_F21SC;

[Serializable]
public class UserSettings
{
    public string? HomePage { get; set; } // The user's homepage
    
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
    }
    
    // Set default values
    private void DefaultSettings()
    {
        // Set default home page to university website
        if (string.IsNullOrEmpty(HomePage)) HomePage = "https://www.hw.ac.uk/";
    }
}