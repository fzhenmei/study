using System;
using System.Collections.Generic;
using System.Text;

using Westwind.Utilities;
using System.Web;
using Westwind.Web.Banners;
using Westwind.Web.Controls;
using Westwind.Utilities.Logging;

namespace $safeprojectname$
{


    /// <summary>
    /// Global Application class that holds various static and
    /// configuration properties and methods. 
    /// 
    /// This class is meant as a one stop application, configuration
    /// constant and management object that unifies various common
    /// applicaiton tasks in one place.
    /// </summary>
    public class App
    {

        public static ApplicationConfiguration Configuration
        {
            get { return _Configuration; }
            set { _Configuration = value; }
        }
        private static ApplicationConfiguration _Configuration;



        public static bool ApplicationOffline
        {
            get { return _ApplicationOffline; }
            set { _ApplicationOffline = value; }
        }
        private static bool _ApplicationOffline = false;


        public static string ApplicationOfflineMessage
        {
            get { return _ApplicationOfflineMessage; }
            set { _ApplicationOfflineMessage = value; }
        }
        private static string _ApplicationOfflineMessage = "The application is currently offline. Please retry in a minute or so.";

        static App()
        {
            /// Load the properties from the Config file
            Configuration = new ApplicationConfiguration(null);
        }

        /// <summary>
        /// Called when the application starts executing
        /// 
        /// This usually routes merely from Application_Start
        /// to provide consolidated configuration management all in one place.
        /// </summary>
        public static void OnApplicationStart()
        {
            HelpControl.HelpBaseUrl = "http://www.west-wind.com/WestwindWebToolkit/docs/index.htm?page=";

            if (LogManagerConfiguration.Current.LogWebRequests || LogManagerConfiguration.Current.LogErrors)
            {
                // create a log manager based on the LogManager config settings
                LogManager.Create();

                WebLogEntry entry = new WebLogEntry()
                {
                    Message = "Application Started",
                    ErrorLevel = ErrorLevels.Message
                };
                LogManager.Current.WriteEntry(entry);
            }
        }

    }

}


