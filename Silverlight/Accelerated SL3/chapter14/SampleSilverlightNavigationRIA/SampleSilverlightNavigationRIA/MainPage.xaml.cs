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
using System.Net.NetworkInformation;
using System.Windows.Data;

namespace SampleSilverlightNavigationRIA
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            UpdateNetworkConnectivityStatus();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(NetworkChange_NetworkAddressChanged);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateNetworkConnectivityStatus();
            UpdateApplicationModeStatus(); 
        }

        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            UpdateNetworkConnectivityStatus();
        }

        private void UpdateNetworkConnectivityStatus()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                NWStatus.Text = "Connected";
                NWStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                NWStatus.Text = "Disconnected";
                NWStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void UpdateApplicationModeStatus()
        {
            if (App.Current.RunningOffline)
            {
                AppMode.Text = "Out of Browser";
                AppMode.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else
            {
                AppMode.Text = "In Browser";
                AppMode.Foreground = new SolidColorBrush(Colors.Blue);
            }
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            Button navigationButton = sender as Button;
            String goToPage = navigationButton.Tag.ToString();
            this.Frame.Navigate(new Uri(goToPage, UriKind.Relative));
        }
        
        private void InstallOOBButton_Click(object sender, RoutedEventArgs e)
        {
                if (Application.Current.ExecutionState != ExecutionStates.Detached)
                {
                    Application.Current.Detach();
                }
                else
                {
                    MessageBox.Show("The application is already installed or you are running in the Out of Browser mode.");                       
                }       
         }

    }
}
