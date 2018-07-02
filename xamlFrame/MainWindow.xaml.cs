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

namespace xamlFrame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectPage1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Uri uri = new Uri("Pages/Page1.xaml", UriKind.Relative);
            MainWindowNavFrame.NavigationService.Navigate(uri);
        }

        private void SelectPage2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Uri uri = new Uri("Pages/Page2.xaml", UriKind.Relative);
            MainWindowNavFrame.NavigationService.Navigate(uri);
        }
    }
}
