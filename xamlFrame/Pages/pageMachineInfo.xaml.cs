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
using xamlFrame.Lib;
using xamlFrame.Lib.System;

namespace xamlFrame.Pages
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class PageMachineInfo : Page
    {
        private NavigationService _nav;

        public PageMachineInfo()
        {
            InitializeComponent();
            _nav = this.NavigationService;
            var basicMachineInfo = new BasicMachineInfo();
            this.DataContext = basicMachineInfo;
        }
    }
}
