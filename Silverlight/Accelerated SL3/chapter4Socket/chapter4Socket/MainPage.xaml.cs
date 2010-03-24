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
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Xml.Serialization;


namespace chapter4Socket
{
    public partial class MainPage : UserControl
    {
        // The MSocket for the connection.
        private Socket MSocket;

        public MainPage()
        {
            InitializeComponent();
        }

       
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((MSocket != null) && (MSocket.Connected == true)) MSocket.Close();
            }
            catch (Exception err)
            {
                AddMessage("ERROR: " + err.Message);
            }

            DnsEndPoint endPoint = new DnsEndPoint(Application.Current.Host.Source.DnsSafeHost, 4530);
            MSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs SocketArgs = new SocketAsyncEventArgs();
            SocketArgs.UserToken = MSocket;
            SocketArgs.RemoteEndPoint = endPoint;
            SocketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SocketArgs_Completed);
            MSocket.ConnectAsync(SocketArgs);
        }
        private void AddMessage(string message)
        {
            //Separate thread 
            Dispatcher.BeginInvoke(
                    delegate()
                    {
                        Messages.Text += message + "\n";
                        Scroller.ScrollToVerticalOffset(Scroller.ScrollableHeight);

                    });
        }
        void SocketArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (MSocket.Connected==false)
            {
                AddMessage("Connection failed.");
                return;
            }
            AddMessage("Connected to server.");
            // Messages can be a maximum of 1024 bytes.
            byte[] Response = new byte[1024];
            e.SetBuffer(Response, 0, Response.Length);
            e.Completed -= new EventHandler<SocketAsyncEventArgs>(SocketArgs_Completed);
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);

            // Listen for messages.
            MSocket.ReceiveAsync(e);
        }

        void e_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0)
            {
                AddMessage("Server disconnected.");
                try
                {
                    MSocket.Close();
                }
                catch { }
                return;
            }

            try
            {
                // Retrieve and display the message.                
                XmlSerializer serializer = new XmlSerializer(typeof(Message));
                MemoryStream ms = new MemoryStream();
                ms.Write(e.Buffer, 0, e.BytesTransferred);
                ms.Position = 0;
                Message message = (Message)serializer.Deserialize(ms);

                AddMessage("[" + message.Sender + "] " + message.MsgText + " (At " + message.SendTime.ToLocalTime() + ")");
            }
            catch (Exception err)
            {
                AddMessage("ERROR: " + err.Message);
            }

            // Listen for more messages.
            MSocket.ReceiveAsync(e);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SocketAsyncEventArgs Args = new SocketAsyncEventArgs();

            // Prepare the message.
            XmlSerializer serializer = new XmlSerializer(typeof(Message));
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, new Message(txtMessage.Text, txtName.Text));
            byte[] messageData = ms.ToArray();

            List<ArraySegment<byte>> bufferList = new List<ArraySegment<byte>>();
            bufferList.Add(new ArraySegment<byte>(messageData));
            Args.BufferList = bufferList;

            // Send the message.
            MSocket.SendAsync(Args);
            //clear the textbox
            txtMessage.Text = string.Empty;
        }
    }
}
