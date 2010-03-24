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
    public partial class ButtonsDemo : UserControl
    {
        public ButtonsDemo()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Put your custom code here
        }
        private int currentValue = 0;
        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            currentValue++;
            repeatButtonValue.Text = currentValue.ToString();
        }
    }
}
