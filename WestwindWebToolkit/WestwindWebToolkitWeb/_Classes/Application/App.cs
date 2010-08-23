using System;
using System.Collections.Generic;
using System.Text;

using Westwind.Utilities;
using System.Web;
using Westwind.Web.Banners;
using Westwind.Web.Controls;
using Westwind.Utilities.Logging;
using Amazon;
using Westwind.Utilities.Configuration;
using Westwind.Web;

namespace Westwind.WebToolkit
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



    public static AmazonConfiguration AmazonConfiguration
    {
        get { return _AmazonConfiguration; }
        set { _AmazonConfiguration = value; }
    }
    private static AmazonConfiguration _AmazonConfiguration;



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
        /// Load the general Application config properties from the Config file
        Configuration = new ApplicationConfiguration(null);
        
        /// Load specific settings for Amazon sample
        AmazonConfiguration = new AmazonConfiguration(null);
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

        // Force jQuery to be loaded off Google Content Network
        ControlResources.jQueryLoadMode = JQueryLoadModes.WebResource; // ContentDeliveryNetwork;
        //ControlResources.jQueryCdnUrl = "http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js";

        if (LogManagerConfiguration.Current.LogWebRequests ||
            LogManagerConfiguration.Current.LogErrors)
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


