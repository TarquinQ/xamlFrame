using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace xamlFrame.Lib
{
    class NavigationBar : ListBox
    {
        static NavigationBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationBar), new FrameworkPropertyMetadata(typeof(NavigationBar)));
        }
    }

    class NavigationBarItem : ListBoxItem
    {
    }
}

