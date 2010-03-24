using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace MessengerServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            MessengerServer MServer= new MessengerServer();
            MServer.Start();
            Console.WriteLine("Messenger server is started.");

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            MServer.Close();
            Console.WriteLine("Messenger server shut down.");
        }
    }
}
