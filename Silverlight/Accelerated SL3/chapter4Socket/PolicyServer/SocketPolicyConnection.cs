using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//added
using System.Net.Sockets;

namespace PolicyServer
{
    class SocketPolicyConnection
    {

        private TcpClient Connection;

            // Buffer to receive client request
            private byte[] Buffer;
            private int Received;
            // The policy to return
            private byte[] Policy;  

            //The request string that is expected from the client
            private static string PolicyRequestString = "<policy-file-request/>";

            public SocketPolicyConnection(TcpClient client, byte[] policy)
            {
                Connection = client;
                Policy = policy;
                Buffer = new byte[PolicyRequestString.Length];
                Received = 0;
                try
                {
                    // receive the request from the client
                    Connection.Client.BeginReceive(Buffer, 0, PolicyRequestString.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
                }
                catch (SocketException)
                {
                    Connection.Close();
                }
            }

            // Called when we receive data from the client
            private void OnReceive(IAsyncResult res)
            {
                try
                {
                    Received += Connection.Client.EndReceive(res);
                    // Make sure that we received full request or try to receieve again
                    if (Received < PolicyRequestString.Length)
                    {
                        Connection.Client.BeginReceive(Buffer, Received, PolicyRequestString.Length - Received,
                        SocketFlags.None, new AsyncCallback(OnReceive), null);
                        return;
                    }
                    // Make sure the request is valid by comparing with PolicyRequestSting
                    string request = System.Text.Encoding.UTF8.GetString(Buffer, 0, Received);
                    if (StringComparer.InvariantCultureIgnoreCase.Compare(request, PolicyRequestString) != 0)
                    {
                        Connection.Close();
                        return;
                    }
                    // Now send the policy
                    Console.Write("Sending the policy...\n");
                    Connection.Client.BeginSend(Policy, 0, Policy.Length, SocketFlags.None,new AsyncCallback(OnSend), null);
                }
                catch (SocketException)
                {
                    Connection.Close();
                }
            }

            // Gets called after sending the policy and close the connection.
            public void OnSend(IAsyncResult ar)
            {
                try
                {
                    Connection.Client.EndSend(ar);
                }
                finally
                {
                    Console.WriteLine("Policy file is served.");
                    Connection.Close();
                }
            }
        }
    }

