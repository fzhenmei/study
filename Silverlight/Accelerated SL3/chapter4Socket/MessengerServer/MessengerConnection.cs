using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//added
using System.Net.Sockets;
using System.IO;

namespace MessengerServer
{
    public class MessengerConnection
    {
        private Socket Client;
        private string ID;
        private MessengerServer MServer;

        public MessengerConnection(Socket Client, string ID, MessengerServer server)
        {
            this.Client = Client;
            this.ID = ID;
            this.MServer = server;
        }

        private byte[] Message = new byte[1024];

        public void Start()
        {
            try
            {
                // Listen for messages.
                Client.BeginReceive(Message, 0, Message.Length, SocketFlags.None, new AsyncCallback(OnMsgReceived), null);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        public void OnMsgReceived(IAsyncResult ar)
        {
            try
            {
                int bytesRead = Client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    //Sending message to all the connected clients.
                    MServer.DeliverMessage(Message, bytesRead);

                    // Listen for next messages.
                    Client.BeginReceive(Message, 0, Message.Length,SocketFlags.None, new AsyncCallback(OnMsgReceived), null);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public void Close()
        {
            try
            {
                Client.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public void ReceiveMessage(byte[] data, int bytesRead)
        {
            Client.Send(data, 0, bytesRead,SocketFlags.None);
        }
    }
}