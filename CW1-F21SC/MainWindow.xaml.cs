using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CW1_F21SC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpFunctions _httpFunctions;
        private readonly ViewModel _viewModel = new ViewModel();
        private string _homepage = "https://www.hw.ac.uk/";
        
        public MainWindow()
        {
            InitializeComponent();
            _httpFunctions = new HttpFunctions(_viewModel);
            DataContext = _viewModel;
            DisplayHtml(_homepage);
            
        }
        
        //Go button for URL bar
        private async void OnGoButtonClick(object sender, RoutedEventArgs e)
        {
            var url = UrlBar.Text; //Get the URL from the URL bar
            DisplayHtml(url);
        }
        
        //Homepage button
        private async void OnHomeButtonClick(object sender, RoutedEventArgs e)
        {
            var url = "https://www.hw.ac.uk/"; 
            DisplayHtml(url);
        }
        
        //Display the HTML of the specified URL
        private async void DisplayHtml(string url)
        {
            var response = await _httpFunctions.SendGetRequest(url); //Send a GET request to the URL
            HtmlDisplayBox.Text = response.content; //If the response is OK, display the response in the HTML display box
        }
        
    }
}