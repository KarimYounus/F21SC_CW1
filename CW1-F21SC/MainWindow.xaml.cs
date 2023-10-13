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
        
        public MainWindow()
        {
            InitializeComponent();
            _httpFunctions = new HttpFunctions();
        }
        
        //Go button for URL bar
        private async void OnGoButtonClick(object sender, RoutedEventArgs e)
        {
            string url = UrlBar.Text; //Get the URL from the URL bar
            var response = await _httpFunctions.SendGetRequest(url); //Send a GET request to the URL
            if (response.statusCode != System.Net.HttpStatusCode.OK) //If the response is not OK, show an error message
            {
                MessageBox.Show("Error: " + response.statusCode); 
                return;
            }
            HtmlDisplayBox.Text = response.content; //If the response is OK, display the response in the HTML display box
        }
        
        
        //Test button
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://status.savanttools.com/?code=200%20OK";
            var response = await _httpFunctions.SendGetRequest(url);
            MessageBox.Show(response.content);
        }
        
    }
}