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
    public partial class ToolTipServiceDemo : UserControl
    {
        public ToolTipServiceDemo()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Border b = new Border();
            b.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            b.BorderThickness = new Thickness(5);
            TextBlock t = new TextBlock();
            t.Margin = new Thickness(5);
            t.Text = "I am another tool tip";
            b.Child = t;
            ToolTipService.SetToolTip(secondButton, b);
        }
    }
}
