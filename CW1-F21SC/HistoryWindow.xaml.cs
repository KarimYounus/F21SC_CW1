using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CW1_F21SC;

/// <summary>
/// This class is responsible for displaying the user's history in a window.
/// </summary>
public partial class HistoryWindow : Window
{
    public HistoryWindow(UserHistory? history)
    {
        InitializeComponent();

        // Assign the history to the ItemsControl 
        HistoryItemsControl.ItemsSource = history.History;
    }

    // Method to navigate to the URL when the user clicks on it
    private void HistoryLinkNavigate(object sender, MouseButtonEventArgs e)
    {
        // Navigate to the URL in the main window
        ((MainWindow)Application.Current.MainWindow).DisplayHtml(((TextBlock) sender).Text);
        // Close the history window
        Close();
    }
}