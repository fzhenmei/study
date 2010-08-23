using System;
using System.Net;
using System.Drawing;
using System.IO;
using System.Web;
using Westwind.Utilities;

namespace Westwind.Web.Banners
{

    /// <summary>
    /// Banner Item entity for an individual banner item. This object
    /// handles the configuration and rendering of the banner item
    /// into string output that can be embedded into any page as
    /// an expression.
    /// </summary>
    public class BannerItem
    {
        /// <summary>
        /// The unique ID for this Banner
        /// </summary>
        public string BannerId
        {
            get { return _BannerId; }
            set { _BannerId = value; }
        }
        private string _BannerId = "";

        public string BannerGroup
        {
            get { return _Group; }
            set { _Group = value; }
        }
        private string _Group = "";

        /// <summary>
        /// The URL where images are found
        /// </summary>
        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }
        private string _ImageUrl = "";

        /// <summary>
        /// The explicit navigation Url that is used when
        /// the banner is clicked
        /// </summary>
        public string NavigateUrl
        {
            get { return _NavigateUrl; }
            set { _NavigateUrl = value; }
        }
        private string _NavigateUrl = "";


        /// <summary>
        /// The original time when this banner was enterd
        /// </summary>
        public DateTime Entered
        {
            get { return _Entered; }
            set { _Entered = value; }
        }
        private DateTime _Entered = DateTime.Now;

        /// <summary>
        /// The last time this banner was updated
        /// </summary>
        public DateTime Updated
        {
            get { return _Updated; }
            set { _Updated = value; }
        }
        private DateTime _Updated = DateTime.Now;

        /// <summary>
        /// An optional Sort order value - the higher will sort to the top of the
        /// list and be shown slightly more frequently.
        /// </summary>
        public int SortOrder
        {
            get { return _SortOrder; }
            set { _SortOrder = value; }
        }
        private int _SortOrder = 0;


       
        /// <summary>
        /// The number of times the item was accessed. This
        /// counter is a total all time count.
        /// </summary>
        public int Hits
        {
            get { return _hits; }
            set { _hits = value; }
        }
        private int _hits = 0;

        /// <summary>
        /// A hit counter that can be reset to track specific time periods
        /// </summary>
        public int ResetHits
        {
            get { return _ResetHits; }
            set { _ResetHits = value; }
        }
        private int _ResetHits = 0;

        /// <summary>
        /// Total number of clicks on this banner if handled through
        /// the banner manager
        /// </summary>
        public int Clicks
        {
            get { return _Clicks; }
            set { _Clicks = value; }
        }
        private int _Clicks = 0;

        /// <summary>
        /// Number of clicks on this banner that is resettable for
        /// specific campaign tracking
        /// </summary>
        public int ResetClicks
        {
            get { return _ResetClicks; }
            set { _ResetClicks = value; }
        }
        private int _ResetClicks = 0;


        /// <summary>
        /// Maximum number of ResetHits before this item
        /// stops being displayed.
        /// </summary>
        public int MaxHits
        {
            get { return _MaxHits; }
            set { _MaxHits = value; }
        }
        private int _MaxHits = 0;


        /// <summary>
        /// Determines whether this item is active
        /// </summary>
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        private bool _Active = true;

        /// <summary>
        /// The pixel width of the banner.
        /// </summary>
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        private int _Width = 460;

        /// <summary>
        /// The pixel height of the banner.
        /// </summary>
        public int  Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        private int  _Height = 60;


        /// <summary>
        /// The type of banner to serve. Image, Swf, SilverLight etc.
        /// </summary>
        public BannerTypes BannerType
        {
            get { return _BannerType; }
            set { _BannerType = value; }
        }
        private BannerTypes _BannerType = BannerTypes.Image;


        /// <summary>
        /// Creates a new instance of BannerItem that has a new GUID set
        /// </summary>
        /// <returns></returns>
        public static BannerItem New()
        {
            BannerItem Item = new BannerItem();
            Item.BannerId = StringUtils.NewStringId();
            return Item;
        }

        /// <summary>
        /// Updates the image size by loading the image from the
        /// Web
        /// </summary>
        public bool UpdateImageSize()
        {            
            Bitmap bmp = null;
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(ImageUrl);
                bmp = new Bitmap(stream);
                stream.Close();                
            }
            catch 
            {
                return false;
            }

            Width = bmp.Width;
            Height = bmp.Height;
            bmp.Dispose();

            return true;
        }


        /// <summary>
        /// Get the clickable Image link for this control. If TrackClicks is true
        /// the output links to the HttpHandler which serves the image and track
        /// the clicks.
        /// </summary>
        /// <returns></returns>
        public string RenderLink()
        {
            if (BannerType == BannerTypes.Swf)
                return RenderSwf();

            string Image = Image = "<img src='" + ImageUrl + "' border='0'>";

            if (BannerConfiguration.Current.TrackStatistics)
                return "<a href='" +  GetTrackedUrl() + "' target='_top'>" + Image + "</a>";
            
            // Otherwise just go straight to navigation
            return "<a href='" + WebUtils.ResolveServerUrl(NavigateUrl) + "' >" + Image + "</a>";
        }


        public string RenderSwf()
        {
            string swf = @"
 <object classid=""clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"" codebase=""http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0"" width=""{1}"" height=""{2}"" id=""wwBanner_ad"" align=""middle"">
<param name=""allowScriptAccess"" value=""sameDomain"" />
<param name=""movie"" value=""{0}"" />
<param name=""quality"" value=""high"" />
<param name=""bgcolor"" value=""#000000"" />
<embed src= ""{0}"" quality=""high"" bgcolor=""#00000"" width=""{1}"" height=""{2}"" name=""wwBanner_ad"" align=""middle"" allowScriptAccess=""sameDomain"" type=""application/x-shockwave-flash"" pluginspage=""http://www.macromedia.com/go/getflashplayer"" />
</object>";

            swf = string.Format(swf, ImageUrl, Width, Height);
            
            if (BannerManager.Current.TrackBannerStatistics)
                return "<a href='" + WebUtils.ResolveServerUrl(GetTrackedUrl()) + "' >" + swf + "</a>";
            else
                return "<a href='" + WebUtils.ResolveServerUrl(NavigateUrl) + "' >" + swf + "</a>";
        }

        /// <summary>
        /// Renders a script tag that calls on the HTTP Handler to load a block
        /// of script code that embeds the banner image into the page.
        /// 
        /// This should be done to avoid hits by robots and other non-visual 
        /// clients from discovering the banner Url and hitting/clicking on
        /// the banner links.
        /// </summary>
        /// <returns></returns>
        public string RenderScriptInclude()
        {
            string Url = WebUtils.ResolveUrl(BannerConfiguration.Current.BannerManagerHandlerUrl) + "?a=s&id=" + BannerId +
                "&t=" + DateTime.UtcNow.Ticks.ToString();
            
            return "<script src='" + Url + "' type='text/javascript'></script>";
            //return "<iframe src=\"" + Url + "\" frameborder=\"0\" scrolling=\"no\" width=\"" + Width.ToString() + "\" height=\"" + Height.ToString() + "\" marginwidth=\"0\" marginheight=\"0\"></iframe>";
        }

        /// <summary>
        /// The returned URL will retrieve a script include to pull the next
        /// banner from the specified group.
        /// </summary>
        /// <param name="bannerGroup">Group or null/"" for all</param>
        /// <returns>Script include string ready to embed into page</returns>
        public static string RenderGroupScriptInclude(string bannerGroup)
        {            
             return WebUtils.ResolveUrl(BannerConfiguration.Current.BannerManagerHandlerUrl) + "?" +
                                          "&a=s" +
                                          "&c=" + HttpUtility.UrlEncode(bannerGroup ?? "") +
                                          "&t=" + DateTime.UtcNow.Ticks.ToString();                                                
        }

        /// <summary>
        /// Returns the Url that will be used for tracking
        /// </summary>
        /// <returns></returns>
        public string GetTrackedUrl()
        {
            string url = WebUtils.ResolveServerUrl(BannerConfiguration.Current.BannerManagerHandlerUrl) +
                   "?a=c" +
                   "&id=" + BannerId + 
                   "&t=" + DateTime.UtcNow.Ticks.ToString();

            if (HttpContext.Current.Request.UrlReferrer != null)
                url += "&u=" + HttpUtility.UrlEncode(HttpContext.Current.Request.UrlReferrer.AbsoluteUri);
            
            return url;                  
        }
        
        /// <summary>
        /// Returns the URL that will be used for tracking but
        /// without a unique timestamp and referring url
        /// 
        /// Use for UI display
        /// </summary>
        /// <returns></returns>
        public string GetBasicTrackedUrl()
        {
            return WebUtils.ResolveServerUrl(BannerConfiguration.Current.BannerManagerHandlerUrl) +
                   "?a=c&id=" + BannerId;
        }

    }

    public enum BannerTypes
    {
        Image = 0,
        Swf = 2,
        Silverlight = 4
    }
}
