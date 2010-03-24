using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//added
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PolicyServer
{
    class SocketPolicyServer
    {

        private TcpListener Listener;
        private byte[] Policy;
       
        // Path to an XML file containing the socket policy as parameter
        public SocketPolicyServer(string PathToPolicyFile)
        {
            // Load the policy file in FileStream object
            FileStream PStream = new FileStream(PathToPolicyFile, FileMode.Open);
            Policy = new byte[PStream.Length];
            PStream.Read(Policy, 0, Policy.Length);
            PStream.Close();
            // Port 943 is the default listener port in Silverlight
            Listener = new TcpListener(IPAddress.Any,943);
            Listener.Start();
            Listener.BeginAcceptTcpClient(new AsyncCallback(OnConnection), null);

        }
        // This method gets called when we receive a connection from a client
        public void OnConnection(IAsyncResult ar)
        {
            TcpClient Client = null;
            try
            {
                Client = Listener.EndAcceptTcpClient(ar);
            }
            catch (SocketException)
            {
                return;
            }
            // handle this policy request with a SocketPolicyConnection
            SocketPolicyConnection PCon = new SocketPolicyConnection(Client, Policy);

            // Then look for other connections
            Listener.BeginAcceptTcpClient(new AsyncCallback(OnConnection), null);
        }
        //This method gets called upong shutting down the policy server
        public void Close()
        {
            Listener.Stop();
        }
    }
}



    

 


