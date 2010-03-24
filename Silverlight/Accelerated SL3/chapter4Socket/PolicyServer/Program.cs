using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
//added
using System.Configuration;


namespace PolicyServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            SocketPolicyServer SPServer = 
            //Policy file path is defined in App.config of the application
            new SocketPolicyServer(ConfigurationManager.AppSettings["PolicyFilePath"]);

            Console.WriteLine("Policy server is started.");

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            SPServer.Close();
            Console.WriteLine("Policy server shut down.");

        }
    }
}
