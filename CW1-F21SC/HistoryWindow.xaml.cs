using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CW1_F21SC;

public partial class HistoryWindow : Window
{
    
    private UserHistory _history;
    
    public HistoryWindow(UserHistory history)
    {
        InitializeComponent();
        _history = history;
        
        // Assign the history to the ItemsControl
        HistoryItemsControl.ItemsSource = _history.History;
    }

    private void HistoryLinkNavigate(object sender, MouseButtonEventArgs e)
    {
        var url = ((TextBlock) sender).Text;
        
        // Navigate to the URL
        ((MainWindow)Application.Current.MainWindow).DisplayHtml(url);
        Close();
    }
}