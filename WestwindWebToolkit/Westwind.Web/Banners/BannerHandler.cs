using System;
using System.Web;
using System.Threading;
using Westwind.Utilities;

namespace Westwind.Web.Banners
{

    /// <summary>
    /// Click handler for the banner.
    /// 
    /// Use either in an ASHX file and inherit from this handler, 
    /// or add into web.config HttpHandlers for a specific URL.
    /// 
    /// Make sure you set the wwBannerConfiguration.wwBannerHandlerUrl 
    /// to point at what ever location you choose.
    /// </summary>
    public class BannerHandler : IHttpHandler
    {
        /// <summary>
        /// This handler has no state and is reusable
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Main banner manager request handler. This 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            BannerManager manager = new BannerManager();
            BannerItem banner = null;

            string Action = context.Request.Params["a"];
            if (string.IsNullOrEmpty(Action))
                Action = "s";
            else
                Action = Action.ToLower();

            string Id = context.Request.Params["id"];
            if (Id == null)
            {
                string Category = context.Request.Params["c"];
                if (Category == null)
                    Category = "";

                banner = manager.GetNextBanner(Category);

            }
            else
            {
                banner = manager.GetBanner(Id);
            }

            if (banner == null)
                ErrorResponse();

            HttpResponse Response = context.Response;

            // No Caching - we want to make sure hits and clicks get counted
            Response.Expires = -1;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // Click action 
            if (Action== "c")
            {
                manager.ClickBanner(banner.BannerId);
                context.Response.Redirect(WebUtils.ResolveUrl(banner.NavigateUrl));
            }
            // display the banner in IFrame
            else if (Action == "s")
            {
                // must have a referrer otherwise don't bother showing it because
                // most likely a bot
                if (context.Request.UrlReferrer == null)
                    return;

                string output = "document.write(" +
                                WebUtils.EncodeJsString(banner.RenderLink()) +
                                ");";
                context.Response.Write(output);

                //Action<object> d = delegate(object val)
                //{
                    // Update the hit counter
                    manager.BannerHit(banner.BannerId);
                //};
                //d.BeginInvoke(null, null, null);

                context.Response.End();           
            }
        }

        protected void ErrorResponse()
        {
            HttpContext.Current.Response.StatusCode = 404;            
            HttpContext.Current.Response.End();
        }
    }
}
