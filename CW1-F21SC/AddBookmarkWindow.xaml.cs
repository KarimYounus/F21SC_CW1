using System;
using System.Windows;

namespace CW1_F21SC;

public partial class AddBookmarkWindow : Window
{

    public Bookmark NewBookmark;
    
    public AddBookmarkWindow(String url)
    {
        InitializeComponent();
        UrlTextBox.Text = url;
    }
    
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        NewBookmark = new Bookmark(NameTextBox.Text, UrlTextBox.Text);
        this.DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
    }
}