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

namespace chapter3
{
    public partial class PopUpDemo : UserControl
    {
        public PopUpDemo()
        {
            InitializeComponent();
        }
        void button_Click(object sender, RoutedEventArgs e)
        {
            xamlPopup.IsOpen = false;
        }
        private void showPopup_Click(object sender, RoutedEventArgs e)
        {
            xamlPopup.IsOpen = true;
        }
    }
}
