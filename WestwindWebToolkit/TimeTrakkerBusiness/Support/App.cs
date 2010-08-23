using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


using Westwind.Utilities;
using System.Web;
using System.Globalization;

namespace TimeTrakker
{
    public class App
    {
        public static string TIMETRAKKER_APPNAME = "";

        /// <summary>
        /// The minimum date value
        /// </summary>
        public static DateTime MIN_DATE_VALUE = DateTime.Parse("01/01/1900",CultureInfo.InvariantCulture);
        
        /// <summary>
        /// Configuration object
        /// </summary>
        public static ApplicationConfiguration Configuration
        {
            get { return _Configuration; }
            set { _Configuration = value; }
        }
        static ApplicationConfiguration _Configuration;

        
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


        /// <summary>
        /// Returns a list of states. This list is cached after first load
        /// </summary>
        public static IList<StateListItem> StateList
        {
            get
            {
                if (_StateList != null)
                    return _StateList;

                busLookup lookup = new busLookup();
                _StateList = lookup.GetStates().ToList();
                
                return _StateList;
            }
                    
        }
        private static IList<StateListItem> _StateList = null;

        /// <summary>
        /// Returns a list of country codes and c
        /// </summary>
        public static IList<CountryListItem> CountryList
        {
            get {
                if (_CountryList != null)
                    return _CountryList;

                busLookup lookup = new busLookup();
                _CountryList = lookup.GetCountries().ToList() ;

                return _CountryList.ToList();            
            }
            
        }
        private static IList<CountryListItem> _CountryList = null;



        static App()
        {
            /// *** Load the properties from the Config file
            Configuration = new ApplicationConfiguration();
            TIMETRAKKER_APPNAME = Configuration.ApplicationTitle;

        }

    }

}


