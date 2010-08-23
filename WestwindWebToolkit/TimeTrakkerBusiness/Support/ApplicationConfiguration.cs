using System;
using System.Collections.Generic;
using System.Text;
using Westwind.Utilities;
using Westwind.Utilities.Configuration;


namespace TimeTrakker
{
    public class ApplicationConfiguration : AppConfiguration
    {
        /// <summary>
		/// Custom Constructor that handles Encryption and using a custom Configuration Section
		/// </summary>
		public ApplicationConfiguration()
		{
		}

        public ApplicationConfiguration(IConfigurationProvider provider)
        {
            if (provider == null)
            {
                //this.SetEncryption("ConnectionString,MailPassword,CCMerchantPassword","WebStorePassword");
                this.Provider = new ConfigurationFileConfigurationProvider<ApplicationConfiguration>()
                {
                    //PropertiesToEncrypt = "ConnectionString,MailPassword",                                     
                };
            }
            this.Read();
        }
        /// <summary>
        /// The main title of the Weblog
        /// </summary>
        public string ApplicationTitle
        {
          get { return _ApplicationTitle; }
          set { _ApplicationTitle = value; }
        }
        private string _ApplicationTitle = "Time Trakker";

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
        /// The Home Url of the app - used with the RSS Feed
        /// </summary>
        public string ApplicationHomeUrl
        {
            get { return _ApplicationHomeUrl; }
            set { _ApplicationHomeUrl = value; }
        }
        private string _ApplicationHomeUrl = "http://localhost/TimeTrakker/";

        
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }
        private string _CompanyName = "West Wind Technologies";

        
        public string ReportFooter
        {
            get { return _ReportFooter; }
            set { _ReportFooter = value; }
        }
        private string _ReportFooter = "<center>www.west-wind.com | (503) 914-6335 | sales@west-wind.com</center>";




        /// <summary>
        /// The time interval in minutes to round up or down to 
        /// for time calculations.
        /// 
        /// Use 0 for no rounding.
        /// </summary>
        public int MinimumMinuteInterval
        {
            get { return _MinimumMinuteInterval; }
            set { _MinimumMinuteInterval = value; }
        }
        private int _MinimumMinuteInterval = 0;



        /// <summary>
        /// The cookie used for tracking the user and 
        /// reconnecting to the profile automatically
        /// </summary>
        public string CookieName
        {
            get { return _CookieName; }
            set { _CookieName = value; }
        }
        private string _CookieName = "TimeTrakkerUser";

        /// <summary>
        /// The name of the user
        /// </summary>
        public string AuthCookieName
        {
            get { return _AuthCookieName; }
            set { _AuthCookieName = value; }
        }
        private string _AuthCookieName = "TimeTrakkerUserName";


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

        
        /// <summary>
		/// Determines how errors are displayed:
		/// 0 - Default Behavior (web.config settings)
		/// 1 - DebugMode off - Static Error Message
		/// 2 - DebugMode on  - Request Error Display
		/// </summary>
        public DebugModes DebugMode
        {
            get { return _DebugMode; }
            set { _DebugMode = value; }
        }
        private DebugModes _DebugMode = DebugModes.Default;


        /// <summary>
        /// Determines whether every request is logged
        /// </summary>
        public bool LogWebRequests
        {
            get { return _LogRequests; }
            set { _LogRequests = value; }
        }
        private bool _LogRequests = false;

        /// <summary>
        /// Flag to determine how logs are written
        /// </summary>
        public ErrorLogTypes LogErrors
        {
            get { return _LogErrors; }
            set { _LogErrors = value; }
        }
        private ErrorLogTypes _LogErrors = ErrorLogTypes.None;

        
        public string XmlErrorLogFile
        {
            get { return _XmlErrorLogFile; }
            set { _XmlErrorLogFile = value; }
        }
        private string _XmlErrorLogFile = "ErrorLog.xml";


        /// <summary>
        /// The ID for the blog. The default blog is 0.
        /// </summary>
        public int BlogPk
        {
           get { return _BlogPk; }
           set { _BlogPk = value; }
        }
        private int _BlogPk = 0;

#endregion

#region Email Settings
            
        
        public bool NotifyComments
        {
          get { return _NotifyComments; }
          set { _NotifyComments = value; }
        }
        private bool _NotifyComments = false;

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

        
        public string MailServer
        {
            get { return _MailServer; }
            set { _MailServer = value; }
        }
        private string _MailServer = "";
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

    public enum ErrorLogTypes
    {
        None,
        XmlFile,
        SqlServer
    }
    
}
