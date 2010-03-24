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

namespace chapter3
{
    public partial class AutoCompleteBoxDemo : Page
    {
        public AutoCompleteBoxDemo()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AutoCompleteBoxDemo_Loaded);
        }

        void AutoCompleteBoxDemo_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> stateList = new List<string>();
            stateList.Add("Alabama");
            stateList.Add("Alaska");
            stateList.Add("American Somoa");
            stateList.Add("Arizona");
            stateList.Add("Arknasas");
            stateList.Add("Wisconsin");
            stateList.Add("Wyoming");
            stateSelection.ItemsSource = stateList;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
