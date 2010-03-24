using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//added
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace MessengerServer
{
    public class MessengerServer
    {
        private Socket Listener;
        private int ClientNo;
        private List<MessengerConnection> Clients = new List<MessengerConnection>();
        private bool isRunning;

        public void Start()
        {
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listener.SetSocketOption(SocketOptionLevel.Tcp, (SocketOptionName)SocketOptionName.NoDelay, 0);
            // The allowed port range in Silverlight is 4502 to 4534.
            Listener.Bind(new IPEndPoint(IPAddress.Any, 4530));
            // Waiting on connection request
            Listener.Listen(10);
            Listener.BeginAccept(new AsyncCallback(OnConnection), null);
            isRunning = true;
        }

        private void OnConnection(IAsyncResult ar)
        {
            if (isRunning==false) 
                        return;

            ClientNo++;
            // Then look for other connections
            Listener.BeginAccept(new AsyncCallback(OnConnection), null);
            Console.WriteLine("Messenger client No: " + ClientNo.ToString() + " is connected.");
            Socket Client = Listener.EndAccept(ar);

            // Handle the current connection.            
            MessengerConnection NewClient = new MessengerConnection(Client, "Client " + ClientNo.ToString(), this);
            NewClient.Start();

            lock (Clients)
            {
                Clients.Add(NewClient);
            }

        }

        public void Close()
        {
            isRunning = false;
            if (Listener != null)
            {
                try
                {
                    Listener.Close();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
            //close connection for each connected clients
            foreach (MessengerConnection client in Clients)
            {
                client.Close();
            }
        }

        public void DeliverMessage(byte[] message, int bytesRead)
        {
            Console.WriteLine("Delivering the message...");
            // Duplication of connection to prevent cross threading issue
            MessengerConnection[] ClientsConnected;
            lock (Clients)
            {
                ClientsConnected = Clients.ToArray();
            }

            foreach (MessengerConnection cnt in ClientsConnected)
            {
                try
                {
                    cnt.ReceiveMessage(message, bytesRead);
                }
                catch
                {
                    // Remove the disconnected client
                    lock (Clients)
                    {
                        Clients.Remove(cnt);
                    }

                    cnt.Close();
                }
            }
        }

    }
}
