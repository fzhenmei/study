using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace chapter4Socket
{
    public class Message
    {
        public string MsgText { get; set; }
        public string Sender { get; set; }
        public DateTime SendTime { get; set; }

        public Message(string text, string sender)
        {
            MsgText = text;
            Sender = sender;
            SendTime = DateTime.Now;
        }
        public Message() 
        { 
        }
    }
}
