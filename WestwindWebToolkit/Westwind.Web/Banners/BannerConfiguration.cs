using System;

using Westwind.Utilities.Configuration;


namespace Westwind.Web.Banners
{
    
    /// <summary>
    /// The configuration object for the Banner object. This object
    /// persists its properties into web.config in the wwBanner
    /// configuration section.    
    /// </summary>
    public class BannerConfiguration : AppConfiguration
    {

        public BannerConfiguration()
        {
            Provider = new ConfigurationFileConfigurationProvider<BannerConfiguration>()
            {
                ConfigurationSection = "BannerConfiguration"
            };
            Read();
        }
            
       /// <summary>
       /// Current instance of the configuration object that's
       /// always available and active
       /// </summary>
        public static BannerConfiguration Current = new BannerConfiguration();


        /// <summary>
        /// Connection String to the Database
        /// </summary>
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }
        private string _ConnectionString = "server=.;database=WestwindAdmin;integrated security=true;";

        /// <summary>
        /// The Url that serves banners for click tracking
        /// </summary>        
        public string BannerManagerHandlerUrl
        {
            get { return _BannerManagerHandlerUrl; }
            set { _BannerManagerHandlerUrl = value; }
        }
        private string _BannerManagerHandlerUrl = "~/wwBanner.ashx";

        /// <summary>
        /// Determines whether hits and clicks are tracked by the manager
        /// </summary>
        public bool TrackStatistics
        {
            get { return _TrackStatistics; }
            set { _TrackStatistics = value; }
        }
        private bool _TrackStatistics = true;

        /// <summary>
        /// Name of the banner table
        /// </summary>
        public string BannerTable
        {
            get { return _BannerTable; }
            set { _BannerTable = value; }
        }
        private string _BannerTable = "wwBanners";

        /// <summary>
        /// Name of the Click Tracking table
        /// </summary>
        public string BannerClicksTable
        {
            get { return _BannerClicksTable; }
            set { _BannerClicksTable = value; }
        }
        private string _BannerClicksTable = "wwBannerClicks";

        /// <summary>
        /// The Url used to go Home off the page
        /// </summary>
        public string HomeUrl
        {
            get { return _HomeUrl; }
            set { _HomeUrl = value; }
        }
        private string _HomeUrl = "~/admin/";

    }
}
