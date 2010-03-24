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

namespace chapter9
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            btnGrow.SizeChanged += new SizeChangedEventHandler(btnGrow_SizeChanged);
        }

        void btnGrow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (btnGrow.ActualWidth == 300)
            {
                btnGrow.Content = "This button now shrinks";
            }

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Grow.Begin();
        }
    }
}
