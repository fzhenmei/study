using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace chapter7
{
    public partial class LaunchChildWindow : UserControl
    {
        public LaunchChildWindow()
        {
            InitializeComponent();
        }

        private void ShowChildWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var newchiledwindow = new ChildWindow1();
            newchiledwindow.Show();
        }
    }
}
