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
using System.Windows.Navigation;

namespace XAMLTour
{
    public partial class RoutedEventExample : Page
    {
        public RoutedEventExample()
        {
            InitializeComponent();
        }
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            eventOrder.Text += " StackPanel";
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            eventOrder.Text += " Grid";
        }
        private void Canvas_MouseLeftButtonDown
        (object sender, MouseButtonEventArgs e)
        {
            eventOrder.Text += " Canvas";
        }
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
