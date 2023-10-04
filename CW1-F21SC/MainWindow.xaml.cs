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
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://www.google.com";
            var response = await _httpFunctions.SendGetRequest(url);
            MessageBox.Show(response);
        }
        
    }
}