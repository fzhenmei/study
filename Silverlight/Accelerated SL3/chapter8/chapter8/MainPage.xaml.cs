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

namespace chapter8
{
    public partial class MainPage : UserControl
    {
        private bool isToggle;
        public MainPage()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (isToggle == false)
            {
                textBlock1.Style = LayoutRoot.Resources["DynamicTitle"] as Style;
            }
            else
            {
                textBlock1.Style = LayoutRoot.
                Resources["MainTitle"] as Style;
            }
            isToggle = !isToggle;
        }
    }
}
