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
            Uri uri = new Uri("Pages/PageQuickLinks.xaml", UriKind.Relative);
            MainWindowNavFrame.NavigationService.Navigate(uri);
        }

        private void NavigationBarItem_Selected_MachineInfo(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/PageMachineInfo.xaml", UriKind.Relative);
            MainWindowNavFrame.NavigationService.Navigate(uri);
        }

        private void NavigationBarItem_Selected_Health(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Pages/PageMachineHealth.xaml", UriKind.Relative);
            MainWindowNavFrame.NavigationService.Navigate(uri);
        }
    }
}
