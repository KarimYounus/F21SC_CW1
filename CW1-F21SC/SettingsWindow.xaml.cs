using System;
using System.Windows;
using System.IO;
using System.Text.Json;

namespace CW1_F21SC;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
    }
    
    private void OnSaveButtonClick(object sender, RoutedEventArgs e)
    {
            var settings = new UserSettings
            {
                HomePage = HomePageBar.Text
            };

            string jsonString = JsonSerializer.Serialize(settings);
            File.WriteAllText("appsettings.json", jsonString);
            HomePageBar.Clear();
    }
}