using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightApplication1
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //private void btnClick_Click(object sender, RoutedEventArgs e)
        //{
        //    var obj = typeof(Colors);
        //    var colorProps = obj.GetProperties(BindingFlags.Public | BindingFlags.Static);
        //    var randIndex = new Random().Next(0, colorProps.Length);
        //    var color = (Color)colorProps[randIndex].GetValue(obj, null);
            
        //    txtDemo.Background = new SolidColorBrush(color);
        //}
    }
}
