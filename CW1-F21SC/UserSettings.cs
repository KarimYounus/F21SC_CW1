using System;

namespace CW1_F21SC;

[Serializable]
public class UserSettings
{
    public string? HomePage { get; set; }
    
    public UserSettings()
    {
        // Set default values
        DefaultSettings();
        Console.WriteLine("Default settings loaded");
    }
    
    private void DefaultSettings()
    {
        // Set default home page to university website
        if (string.IsNullOrEmpty(HomePage)) HomePage = "https://www.hw.ac.uk/";
    }
}