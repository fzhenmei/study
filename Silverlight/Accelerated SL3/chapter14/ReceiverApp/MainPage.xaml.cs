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

namespace ReceiverApp
{
    public partial class MainPage : UserControl
    {
        private const string SenderAppName = "Receiver1";
        private const string ReceiverAppName = "Sender1";


        public MainPage()
        {
            InitializeComponent();

            LocalMessageReceiver msgReceiver = new LocalMessageReceiver(SenderAppName);
            msgReceiver.MessageReceived += (object sender, MessageReceivedEventArgs e) =>
            {
                switch (e.Message)
                {
                    case "Red":
                        {
                            rect.Fill = new SolidColorBrush(Colors.Red);
                            break;
                        }

                    case "Green":
                        {
                            rect.Fill = new SolidColorBrush(Colors.Green);
                            break;
                        }
                    case "Blue":
                        {
                            rect.Fill = new SolidColorBrush(Colors.Blue);
                            break;
                        }
                        
                }
            };
            msgReceiver.Listen();

        }

    }
}
