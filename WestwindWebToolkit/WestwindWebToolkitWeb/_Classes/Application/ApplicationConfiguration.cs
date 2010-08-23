using System;
using System.Collections.Generic;
using System.Text;
using Westwind.Utilities;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Web;
using Westwind.Utilities.Configuration;
using System.Diagnostics;

namespace Westwind.WebToolkit
{

/// <summary>
/// Application specific config class that holds global configuration values
/// that are read and optionally persisted into a configuration store.
/// </summary>    
public class ApplicationConfiguration : Westwind.Utilities.Configuration.AppConfiguration
{



    /// <summary>
    /// Always implement a default constructor so new instances
    /// can be created by the various de-serialization config schemes.
    /// </summary>
    public ApplicationConfiguration()
    {
    }

    /// <summary>
    /// By convention a second constructor that takes a Config Provider
    /// or null as an optional parameter should be implemented. This
    /// ctor should implement auto-load behavior and create a default
    /// provider for the object. Typically called like this:
    ///
    /// App.Configuration = new ApplicationConfiguration(null);
    /// </summary>
    /// <param name="provider"></param>
    public ApplicationConfiguration(IConfigurationProvider provider)
        : this()
    {

        if (provider == null)
        {
            this.Provider = new ConfigurationFileConfigurationProvider<ApplicationConfiguration>()
            {
                PropertiesToEncrypt = "MailServerPassword,ConnectionString",
                EncryptionKey = "secret",
                ConfigurationSection = "ApplicationConfiguration",
                 ConfigurationFile= HttpContext.Current.Server.MapPath(@"~/Configuration.config")
            };

            // Example of Sql configuration
            //this.Provider = new SqlServerConfigurationProvider<ApplicationConfiguration>()
            //{
            //    FieldsToEncrypt = "MailServerPassword,ConnectionString",
            //    EncryptKey = "secret",
            //    ConnectionString = "DevSampleConnectionString",
            //    Tablename = "Configuration",
            //    Key = 1
            //};
            // Example of external XML configuration - advantage: Supports complex object hierarchies
            //this.Provider = new XmlFileConfigurationProvider<ApplicationConfiguration>()
            //{
            //    FieldsToEncrypt = "MailServerPassword,ConnectionString",
            //    EncryptKey = "secret",
            //    XmlConfigurationFile = HttpContext.Current.Server.MapPath("~/Configuration.xml")
            //};
        }
        else
            this.Provider = provider;

        this.Provider.Read(this);
    }


    #region Generic Application Settings

    /// <summary>
    /// The main title of the Weblog
    /// </summary>
    public string ApplicationTitle
    {
        get { return _ApplicationTitle; }
        set { _ApplicationTitle = value; }
    }
    private string _ApplicationTitle = "West Wind Web Toolkit Sample";

    /// <summary>
    /// The subtitle for the Web Log displayed on the banner
    /// </summary>
    public string ApplicationSubTitle
    {
        get { return _ApplicationSubTitle; }
        set { _ApplicationSubTitle = value; }
    }
    private string _ApplicationSubTitle = "";


    /// <summary>
    /// The Home Url of the Web Log - used with the RSS Feed
    /// </summary>
    public string ApplicationHomeUrl
    {
        get { return _ApplicationHomeUrl; }
        set { _ApplicationHomeUrl = value; }
    }
    private string _ApplicationHomeUrl = "";


    /// <summary>
    /// Application Cookie name used for user tracking
    /// </summary>
    public string ApplicationCookieName
    {
        get { return _ApplicationCookieName; }
        set { _ApplicationCookieName = value; }
    }
    private string _ApplicationCookieName = "_ApplicationId";


    #endregion

    #region System Settings
    /// <summary>
    /// The database connection string for this WebLog instance
    /// </summary>
    public string ConnectionString
    {
        get { return _ConnectionString; }
        set { _ConnectionString = value; }
    }
    private string _ConnectionString = "";

    ///// <summary>
    ///// The type of server the business objects work with.
    ///// </summary>
    //public ServerTypes ConnectType
    //{
    //    get { return _ConnectType; }
    //    set { _ConnectType = value; }
    //}
    //private ServerTypes _ConnectType = ServerTypes.SqlServer;



    /// <summary>
    /// Determines how errors are displayed
    /// Default - ASP.NET Default behavior
    /// ApplicationErrorMessage - Application level error message
    /// DeveloperErrorMessage - StackTrace and full error info
    /// </summary>
    public DebugModes DebugMode
    {
        get { return _DebugMode; }
        set { _DebugMode = value; }
    }
    private DebugModes _DebugMode = DebugModes.ApplicationErrorMessage;

    /// <summary>
    /// Determines how many items are displayed per page in typical list displays
    /// </summary>
    public int MaxPageItems
    {
        get { return _MaxPageItems; }
        set { _MaxPageItems = value; }
    }
    private int _MaxPageItems = 20;

    #endregion

    #region Email Settings

    /// <summary>
    /// The Email Address used to send out emails
    /// </summary>
    public string SenderEmailAddress
    {
        get { return _SenderEmailAddress; }
        set { _SenderEmailAddress = value; }
    }
    private string _SenderEmailAddress = "";

    /// <summary>
    /// The Name of the senders Email 
    /// </summary>
    public string SenderEmailName
    {
        get { return _SenderEmailName; }
        set { _SenderEmailName = value; }
    }
    private string _SenderEmailName = "";

    /// <summary>
    /// Optional CC List for email confirmations to customers.
    /// </summary>
    public string MailCc
    {
        get { return _MailCc; }
        set { _MailCc = value; }
    }
    private string _MailCc = "";


    /// <summary>
    /// Email address use for admin emails
    /// </summary>
    public string AdminEmailAddress
    {
        get { return _AdminEmailAddress; }
        set { _AdminEmailAddress = value; }
    }
    private string _AdminEmailAddress = "";

    /// <summary>
    /// Email name used for Admin emails
    /// </summary>
    public string AdminEmailName
    {
        get { return _AdminEmailName; }
        set { _AdminEmailName = value; }
    }
    private string _AdminEmailName = "";

    /// <summary>
    /// Determines whether administrative emails are sent if
    /// the email addresses are set
    /// </summary>
    public bool AdminSendEmails
    {
        get { return _AdminSendEmails; }
        set { _AdminSendEmails = value; }
    }
    private bool _AdminSendEmails = false;



    /// <summary>
    /// The IP address or domain name of the mail server
    /// used to send email notifications and admin 
    /// alerts through
    /// </summary>
    public string MailServer
    {
        get { return _MailServer; }
        set { _MailServer = value; }
    }
    private string _MailServer = "";


    /// <summary>
    /// Mail Server username if required
    /// </summary>
    [Description("Mail Server username if required")]
    [Category("Miscellaneous"), DefaultValue("")]
    public string MailServerUsername
    {
        get { return _MailServerUsername; }
        set { _MailServerUsername = value; }
    }
    private string _MailServerUsername = "";


    /// <summary>
    /// Mail Server password if required
    /// </summary>
    [Description("Mail Server password if required")]
    [Category("Miscellaneous"), DefaultValue("")]
    public string MailServerPassword
    {
        get { return _MailServerPassword; }
        set { _MailServerPassword = value; }
    }
    private string _MailServerPassword = "";


    #endregion

}

    /// <summary>
    /// Different modes that errors are displayed in the application
    /// </summary>
    public enum DebugModes
    {
        Default,
        ApplicationErrorMessage,
        DeveloperErrorMessage
    }


    //public class MyConfiguration : AppConfiguration
    //{

    //    public MyConfiguration(IConfigurationProvider provider)
    //    {

    //        if (provider == null)
    //        {
    //            this.Provider = new ConfigurationFileConfigurationProvider<ApplicationConfiguration>()
    //            {
    //                PropertiesToEncrypt = "MailServerPassword,ConnectionString",
    //                EncryptionKey = "secret",
    //                ConfigurationSection = "MyConfiguration",
    //                 ConfigurationFile= HttpContext.Current.Server.MapPath(@"~/Configuration.config")
    //            };

    //            // Example of Sql configuration
    //            //this.Provider = new SqlServerConfigurationProvider<ApplicationConfiguration>()
    //            //{
    //            //    FieldsToEncrypt = "MailServerPassword,ConnectionString",
    //            //    EncryptKey = "secret",
    //            //    ConnectionString = "DevSampleConnectionString",
    //            //    Tablename = "Configuration",
    //            //    Key = 1
    //            //};
    //            // Example of external XML configuration - advantage: Supports complex object hierarchies
    //            //this.Provider = new XmlFileConfigurationProvider<ApplicationConfiguration>()
    //            //{
    //            //    FieldsToEncrypt = "MailServerPassword,ConnectionString",
    //            //    EncryptKey = "secret",
    //            //    XmlConfigurationFile = HttpContext.Current.Server.MapPath("~/Configuration.xml")
    //            //};
    //        }
    //        else
    //            this.Provider = provider;

    //        this.Provider.Read(this);
    //    }

        
    //    public string MyProperty
    //    {
    //      get { return _MyProperty; }
    //      set { _MyProperty = value; }
    //    }
    //    private string _MyProperty = "My Value";
    //}


}
