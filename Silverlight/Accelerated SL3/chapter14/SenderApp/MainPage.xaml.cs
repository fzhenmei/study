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
//added
using System.Windows.Messaging;

namespace SenderApp
{
    public partial class MainPage : UserControl
    {
        private const string SenderAppName = "Sender1";
        private const string ReceiverAppName = "Receiver1";
        private string message="Red";

        public MainPage()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            LocalMessageSender msgSender = new LocalMessageSender(ReceiverAppName);

            if(message!=null || message!=string.Empty) 
                msgSender.SendAsync(message);

        }

        private void Color_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbtn= sender as RadioButton;
            message = rbtn.Content.ToString();
        }
    }
}
