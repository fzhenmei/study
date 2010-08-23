#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          © West Wind Technologies, 2008 - 2009
 *          http://www.west-wind.com/
 * 
 * Created: 09/04/2008
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 **************************************************************  
*/
#endregion


using System;
using System.Web.UI;
using System.IO;
using System.Reflection;
using Westwind.Utilities;

[assembly: TagPrefix("Westwind.Web.Controls", "ww")]

[assembly: WebResource("Westwind.Web.Controls.Resources.jquery.js", "application/x-javascript")]
[assembly: WebResource("Westwind.Web.Controls.Resources.ww.jquery.js", "application/x-javascript")]

                        
[assembly: WebResource("Westwind.Web.Controls.Resources.warning.gif", "image/gif")]
[assembly: WebResource("Westwind.Web.Controls.Resources.info.gif", "image/gif")]
[assembly: WebResource("Westwind.Web.Controls.Resources.loading.gif", "image/gif")]
[assembly: WebResource("Westwind.Web.Controls.Resources.loading_small.gif", "image/gif")]
[assembly: WebResource("Westwind.Web.Controls.Resources.close.gif", "image/gif")]
[assembly: WebResource("Westwind.Web.Controls.Resources.help.gif", "image/gif")]
[assembly: WebResource("Westwind.Web.Controls.Resources.calendar.gif", "image/gif")]



namespace Westwind.Web
{ 
    /// <summary>
    /// Class is used as to consolidate access to resources
    /// </summary>
    public class ControlResources
    {
        public const string JQUERY_SCRIPT_RESOURCE = "Westwind.Web.Controls.Resources.jquery.js";       
        public const string WWJQUERY_SCRIPT_RESOURCE = "Westwind.Web.Controls.Resources.ww.jquery.js";

        // Icon Resource Strings
        public const string INFO_ICON_RESOURCE = "Westwind.Web.Controls.Resources.info.gif";        
        public const string WARNING_ICON_RESOURCE = "Westwind.Web.Controls.Resources.warning.gif";
        public const string CLOSE_ICON_RESOURCE = "Westwind.Web.Controls.Resources.close.gif";
        public const string HELP_ICON_RESOURCE = "Westwind.Web.Controls.Resources.help.gif";
        public const string LOADING_ICON_RESOURCE = "Westwind.Web.Controls.Resources.loading.gif";
        public const string LOADING_SMALL_ICON_RESOURCE = "Westwind.Web.Controls.Resources.loading_small.gif";
        public const string CALENDAR_ICON_RESOURCE = "Westwind.Web.Controls.Resources.calendar.gif";


        public const string STR_JsonContentType = "application/json";
        public const string STR_JavaScriptContentType = "application/x-javascript";
        public const string STR_UrlEncodedContentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// Determines what location jQuery is loaded from
        /// </summary>
        public static JQueryLoadModes jQueryLoadMode = JQueryLoadModes.WebResource;

        /// <summary>
        /// jQuery CDN Url on Google
        /// </summary>
        public static string jQueryCdnUrl = "http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js";

        /// <summary>
        /// jQuery CDN Url on Google
        /// </summary>
        public static string jQueryUiCdnUrl = "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.2/jquery-ui.min.js";

        /// <summary>
        /// jQuery UI fallback Url if CDN is unavailable or WebResource is used
        /// Note: The file needs to exist and hold the minimized version of jQuery ui
        /// </summary>
        public static string jQueryUiLocalFallbackUrl = "~/scripts/jquery-ui.min.js";

        /// <summary>
        /// Loads the appropriate jScript library out of the scripts directory
        /// </summary>
        /// <param name="control"></param>
        /// <param name="jQueryUrl">Optional url to jQuery as a virtual or absolute server path</param>
        public static void LoadjQuery(Control control, string jQueryUrl)
        {            
            ClientScriptProxy p = ClientScriptProxy.Current;

            if (!string.IsNullOrEmpty(jQueryUrl))
                p.RegisterClientScriptInclude(control, typeof(ControlResources), jQueryUrl, ScriptRenderModes.HeaderTop);
            else if (jQueryLoadMode == JQueryLoadModes.WebResource)
                p.RegisterClientScriptResource(control, typeof(ControlResources), ControlResources.JQUERY_SCRIPT_RESOURCE, ScriptRenderModes.HeaderTop);
            else if (jQueryLoadMode == JQueryLoadModes.ContentDeliveryNetwork)
            {
                // Load from CDN Url specified
                p.RegisterClientScriptInclude(control, typeof(ControlResources), jQueryCdnUrl, ScriptRenderModes.HeaderTop);

                // check if jquery loaded - if it didn't we're not online and use WebResource
                string scriptCheck =
                    @"if (typeof(jQuery) == 'undefined')  
        document.write(unescape(""%3Cscript src='{0}' type='text/javascript'%3E%3C/script%3E""));";

                jQueryUrl = p.GetClientScriptResourceUrl(control, typeof(ControlResources),
                                ControlResources.JQUERY_SCRIPT_RESOURCE);                
                p.RegisterClientScriptBlock(control, typeof(ControlResources),
                                "jquery_register", string.Format(scriptCheck, jQueryUrl), true,
                                ScriptRenderModes.HeaderTop);  
            }

            return;
        }

        /// <summary>
        /// Loads the jQuery component uniquely into the page
        /// </summary>
        /// <param name="control"></param>
        /// <param name="jQueryUrl">Optional Url to the jQuery Library. NOTE: Should also have a .min version in place</param>
        public static void LoadjQuery(Control control)
        {
            LoadjQuery(control,null);
        }

        /// <summary>
        /// Loads the ww.jquery.js library from Resources at the end of the Html Header (if available)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="loadjQuery"></param>
        public static void LoadwwjQuery(Control control,bool loadjQuery)
        {
            // jQuery is also required
            if (loadjQuery)
                LoadjQuery(control);

            ClientScriptProxy p = ClientScriptProxy.Current;
            p.RegisterClientScriptResource(control,typeof(ControlResources),ControlResources.WWJQUERY_SCRIPT_RESOURCE,ScriptRenderModes.Header);
        }

        /// <summary>
        /// Loads the ww.jquery.js library from Resources at the end of the Html Header (if available)
        /// </summary>
        /// <param name="control"></param>
        public static void LoadwwjQuery(Control control)
        {
            LoadwwjQuery(control, true);
        }

        /// <summary>
        /// Loads the appropriate jScript library out of the scripts directory
        /// </summary>
        /// <param name="control"></param>
        /// <param name="jQueryUiUrl">Optional url to jQuery as a virtual or absolute server path</param>
        public static void LoadjQueryUi(Control control, string jQueryUiUrl)
        {
            ClientScriptProxy p = ClientScriptProxy.Current;

            // jQuery UI isn't provided as a Web Resource so default to a fixed URL
            if (jQueryLoadMode == JQueryLoadModes.WebResource)
            {
                //throw new InvalidOperationException(Resources.WebResourceNotAvailableForJQueryUI);                
                jQueryUiUrl = WebUtils.ResolveUrl(jQueryUiLocalFallbackUrl);
            }

            if (!string.IsNullOrEmpty(jQueryUiUrl))
                p.RegisterClientScriptInclude(control, typeof(ControlResources), jQueryUiUrl, ScriptRenderModes.Header);                        
            else if (jQueryLoadMode == JQueryLoadModes.ContentDeliveryNetwork)
            {                
                // Load from CDN Url specified
                p.RegisterClientScriptInclude(control, typeof(ControlResources), jQueryUiCdnUrl, ScriptRenderModes.Header);

                // check if jquery loaded - if it didn't we're not online and use WebResource
                string scriptCheck =
                    @"if (typeof(jQuery) == 'undefined')  
        document.write(unescape(""%3Cscript src='{0}' type='text/javascript'%3E%3C/script%3E""));";

                p.RegisterClientScriptBlock(control,
                                            typeof(ControlResources), "jquery_ui",
                                            string.Format(scriptCheck, WebUtils.ResolveUrl(jQueryUiLocalFallbackUrl)),
                                            true, ScriptRenderModes.Header);
            }

            return;
        }



        /// <summary>
        /// Returns a string resource from a given assembly.
        /// </summary>
        /// <param name="assembly">Assembly reference (ie. typeof(ControlResources).Assembly) </param>
        /// <param name="ResourceName">Name of the resource to retrieve</param>
        /// <returns></returns>
        public static string GetStringResource(Assembly assembly, string ResourceName)
        {            
            Stream st = assembly.GetManifestResourceStream(ResourceName);
            StreamReader sr = new StreamReader(st);
            string content = sr.ReadToEnd();            
            st.Close();
            return content;
        }

        /// <summary>
        /// Returns a string resource from the from the ControlResources Assembly
        /// </summary>
        /// <param name="ResourceName"></param>
        /// <returns></returns>
        public static string GetStringResource(string ResourceName)
        {
            return GetStringResource(typeof(ControlResources).Assembly, ResourceName);
        }


    }    

    /// <summary>
    /// The location from which jQuery and jQuery UI are loaded
    /// in Release mode.
    /// </summary>
    public enum JQueryLoadModes
    {        
        ContentDeliveryNetwork,
        WebResource,
        Script,
        None
    }
    

}
