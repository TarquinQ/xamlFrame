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

        private void NavigationBarItem_Selected_QuickLinks(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/pageQuickLinks.xaml", UriKind.Relative);
            MainWindowNavFrame.NavigationService.Navigate(uri);
        }

        private void NavigationBarItem_Selected_MachineInfo(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/pageMachineInfo.xaml", UriKind.Relative);
            MainWindowNavFrame.NavigationService.Navigate(uri);
        }

        private void NavigationBarItem_Selected_1(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/Page1.xaml", UriKind.Relative);
            MainWindowNavFrame.NavigationService.Navigate(uri);
        }

        private void NavigationBarItem_Selected_2(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/Page2.xaml", UriKind.Relative);
            MainWindowNavFrame.NavigationService.Navigate(uri);
        }
    }
}
