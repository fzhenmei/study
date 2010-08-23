#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          © West Wind Technologies, 2008 - 2009
 *          http://www.west-wind.com/
 * 
 * Created: Date
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
using System.Web;
using System.Reflection;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Globalization;
using Westwind.Web.JsonSerializers;
using System.Drawing.Imaging;
using Westwind.Web.Controls.Properties;

namespace Westwind.Utilities
{
    /// <summary>
    /// Summary description for wwWebUtils.
    /// </summary>
    public class WebUtils
    {

        static DateTime DAT_JAVASCRIPT_BASEDATE = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Returns a site relative HTTP path from a partial path starting out with a ~.
        /// Same syntax that ASP.Net internally supports but this method can be used
        /// outside of the Page framework.
        /// 
        /// Works like Control.ResolveUrl including support for ~ syntax
        /// but returns an absolute URL.
        /// </summary>
        /// <param name="originalUrl">Any Url including those starting with ~</param>
        /// <returns>relative url</returns>
        public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
                return null;

            // Absolute path - just return
            if (originalUrl.IndexOf("://") != -1)
                return originalUrl;

            // Fix up image path for ~ root app dir directory
            if (originalUrl.StartsWith("~"))
            {
                //return VirtualPathUtility.ToAbsolute(originalUrl);
                string newUrl = "";
                if (HttpContext.Current != null)
                {
                    newUrl = HttpContext.Current.Request.ApplicationPath +
                          originalUrl.Substring(1);
                    newUrl = newUrl.Replace("//", "/");
                }
                else
                    // Not context: assume current directory is the base directory
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");

                // Just to be sure fix up any double slashes
                return newUrl;
            }

            return originalUrl;
        }


        /// <summary>
        /// This method returns a fully qualified absolute server Url which includes
        /// the protocol, server, port in addition to the server relative Url.
        /// 
        /// Works like Control.ResolveUrl including support for ~ syntax
        /// but returns an absolute URL.
        /// </summary>
        /// <param name="ServerUrl">Any Url, either App relative (~/default.aspx) 
        /// or fully qualified</param>
        /// <param name="forceHttps">if true forces the url to use https</param>
        /// <returns></returns>
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {            
            // Is it already an absolute Url?
            if (serverUrl.IndexOf("://") < 0)
            {
                // Start by fixing up the Url an Application relative Url
                string relPath = ResolveUrl(serverUrl);

                serverUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                serverUrl += relPath;
            }

            if (forceHttps)
                serverUrl = serverUrl.Replace("http://", "https://");

            return serverUrl;
        }

        /// <summary>
        /// This method returns a fully qualified absolute server Url which includes
        /// the protocol, server, port in addition to the server relative Url.
        /// 
        /// It work like Page.ResolveUrl, but adds these to the beginning.
        /// This method is useful for generating Urls for AJAX methods
        /// </summary>
        /// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
        /// <returns></returns>
        public static string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }

        /// <summary>
        /// Returns the Application Path as a full Url with scheme 
        /// </summary>
        /// <returns></returns>
        public static string GetFullApplicationPath()
        {
            var url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);            
            return url + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
        }


        /// <summary>
        /// Returns the executing ASPX, ASCX, MASTER page for a control instance.
        /// Path is returned app relative without a leading slash
        /// </summary>
        /// <param name="Ctl"></param>
        /// <returns></returns>
        public static string GetControlAppRelativePath(Control Ctl)
        {
            return Ctl.TemplateControl.AppRelativeVirtualPath.Replace("~/", "");
        }

        /// <summary>
        /// Converts an ImageFormat value to a Web Content Type
        /// </summary>
        /// <param name="formatGuid"></param>
        /// <returns></returns>
        public static string ImageFormatToContentType(ImageFormat format)
        {
            string ct = null;
            
            if (format.Equals(ImageFormat.Png))
                ct = "image/png";
            else if (format.Equals(ImageFormat.Jpeg))
                ct = "image/jpeg";
            else if (format.Equals(ImageFormat.Gif))
                ct = "image/gif";
            else if (format.Equals(ImageFormat.Tiff))
                ct = "image/tiff";
            else if (format.Equals(ImageFormat.Bmp))
                ct = "image/bmp";
            else if (format.Equals(ImageFormat.Icon))
                ct = "image/x-icon";
            else if (format.Equals(ImageFormat.Wmf))
                ct = "application/x-msmetafile";
            else
                throw new InvalidOperationException(string.Format(Resources.ERROR_UnableToConvertImageFormatToContentType, format.ToString()));

            return ct;
        }

        /// <summary>
        /// Returns an image format from an HTTP content type string
        /// </summary>
        /// <param name="contentType">Content Type like image/jpeg</param>
        /// <returns>Corresponding image format</returns>
        public static ImageFormat ImageFormatFromContentType(string contentType)
        {
            ImageFormat format = ImageFormat.Png;

            contentType = contentType.ToLower();

            if (contentType == "image/png")
                return format;            
            else if (contentType == "image/gif")
                format = ImageFormat.Gif;
            else if(contentType == "image/jpeg")
                format = ImageFormat.Jpeg;
            else if(contentType == "image/tiff")
                format = ImageFormat.Jpeg;
            else if(contentType == "image/bmp")
                format = ImageFormat.Bmp;
            else if(contentType == "image/x-icon")
                format = ImageFormat.Icon;
            else if (contentType == "application/x-msmetafile")
                format = ImageFormat.Wmf;
            else
                throw new InvalidOperationException(string.Format(Resources.ERROR_UnableToConvertContentTypeToImageFormat,contentType));

            return format;
        }



        /// <summary>
        /// Sets the locale based on thr browser's currency setting if possible
        /// </summary>
        /// <param name="currencySymbol">If not null overrides the currency symbol for the culture. 
        /// Use to force a specify currency when multiple currencies are not supported by the application
        /// </param>
        public static void SetUserLocale(string currencySymbol)
        {
            SetUserLocale(currencySymbol, false);
        }

        /// <summary>
        /// Sets a user's Locale based on the browser's Locale setting. If no setting
        /// is provided the default Locale is used.
        /// </summary>
        /// <param name="currencySymbol">If not null overrides the currency symbol for the culture. 
        /// Use to force a specify currency when multiple currencies are not supported by the application
        /// </param>
        /// <param name="setUiCulture">if true sets the UI culture in addition to core culture</param>
        public static void SetUserLocale(string currencySymbol, bool setUiCulture)
        {
            HttpRequest Request = HttpContext.Current.Request;
            if (Request.UserLanguages == null)
                return;

            string Lang = Request.UserLanguages[0];
            if (Lang != null)
            {
                // Problems with Turkish Locale and upper/lower case
                // DataRow/DataTable indexes
                if (Lang.StartsWith("tr"))
                    return;

                if (Lang.Length < 3)
                    Lang = Lang + "-" + Lang.ToUpper();
                try
                {
                    CultureInfo Culture = new CultureInfo(Lang);
                    if (currencySymbol != null && currencySymbol != "")
                        Culture.NumberFormat.CurrencySymbol = currencySymbol;

                    Thread.CurrentThread.CurrentCulture = Culture;

                    if (setUiCulture)
                        Thread.CurrentThread.CurrentUICulture = Culture;
                }
                catch
                { ;}
            }
        }


        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exits
        /// </summary>
        /// <param name="ContainerCtl">The top level container to start searching from</param>
        /// <param name="IdToFind">The ID of the control to find</param>
        /// <returns></returns>
        public static Control FindControlRecursive(Control root, string id)
        {
            return FindControlRecursive(root, id, false);
        }

        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exits
        /// </summary>
        /// <param name="ContainerCtl">The top level container to start searching from</param>
        /// <param name="IdToFind">The ID of the control to find</param>
        /// <param name="alwaysUseFindControl">If true uses FindControl to check for hte primary Id which is slower, but finds dynamically generated control ids inside of INamingContainers</param>
        /// <returns></returns>
        public static Control FindControlRecursive(Control Root, string id, bool alwaysUseFindControl)
        {
            if (alwaysUseFindControl)
            {
                Control ctl = Root.FindControl(id);
                if (ctl != null)
                    return ctl;
            }
            else
            {
                if (Root.ID == id)
                    return Root;
            }

            foreach (Control Ctl in Root.Controls)
            {
                Control foundCtl = FindControlRecursive(Ctl, id, alwaysUseFindControl);
                if (foundCtl != null)
                    return foundCtl;
            }

            return null;
        }

        /// <summary>
        /// Restarts the Web Application
        /// Requires either Full Trust (HttpRuntime.UnloadAppDomain) 
        /// or Write access to web.config.
        /// </summary>
        public static bool RestartWebApplication()
        {
            bool Error = false;
            try
            {
                // This requires full trust so this will fail
                // in many scenarios
                HttpRuntime.UnloadAppDomain();
            }
            catch
            {
                Error = true;
            }

            if (!Error)
                return true;

            // Couldn't unload with Runtime - let's try modifying web.config
            string ConfigPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\web.config";

            try
            {
                File.SetLastWriteTimeUtc(ConfigPath, DateTime.UtcNow);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Routine that can be used read Form Variables into an object if the 
        /// object name and form variable names match either exactly or with a specific
        /// prefix.
        /// 
        /// The method loops through all *public* members of the object and tries to 
        /// find a matching form variable by the same name in Request.Form.
        /// 
        /// The routine returns false if any value failed to parse (ie. invalid
        /// formatting etc.). However parsing is not aborted on errors so all
        /// other convertable values are set on the object.
        /// 
        /// You can pass in a Dictionary<string,string> for the Errors parameter
        /// to optionally retrieve unbinding errors. The dictionary key holds the
        /// simple form varname for the field (ie. txtName), the value the actual
        /// exception error message
        /// </summary>
        /// <remarks>
        /// This method can have unexpected side-effects if multiple naming
        /// containers share common variable names. This routine is not recommended
        /// for those types of pages.
        /// </remarks>
        /// <param name="Target"></param>
        /// <param name="FormVarPrefix">empty or one or more prefixes spearated by |</param>
        /// <param name="errors">Allows passing in a string dictionary that receives error messages. returns key as field name, value as error message</param>
        /// <returns>true or false if an unbinding error occurs</returns>
        public static bool FormVarsToObject(object target, string formvarPrefixes, Dictionary<string, string> errors)
        {
            bool isError = false;
            List<string> ErrorList = new List<string>();

            if (formvarPrefixes == null)
                formvarPrefixes = "";

            if (HttpContext.Current == null)
                throw new InvalidOperationException("FormVarsToObject can only be called from a Web Request");

            HttpRequest Request = HttpContext.Current.Request;

            // try to get a generic reference to a page for recursive find control
            // This value will be null if not dealing with a page (ie. in JSON Web Service)
            Page page = HttpContext.Current.CurrentHandler as Page;

            MemberInfo[] miT = target.GetType().FindMembers(
                MemberTypes.Field | MemberTypes.Property,
                BindingFlags.Public | BindingFlags.Instance,
                null, null);

            // Look through all prefixes separated by |
            string[] prefixes = formvarPrefixes.Split('|');

            foreach (string prefix in prefixes)
            {

                // Loop through all members of the Object
                foreach (MemberInfo Field in miT)
                {
                    string Name = Field.Name;

                    FieldInfo fi = null;
                    PropertyInfo pi = null;
                    Type FieldType = null;

                    if (Field.MemberType == MemberTypes.Field)
                    {
                        fi = (FieldInfo)Field;
                        FieldType = fi.FieldType;
                    }
                    else
                    {
                        pi = (PropertyInfo)Field;
                        FieldType = pi.PropertyType;
                    }

                    // Lookup key will be field plus the prefix
                    string formvarKey = prefix + Name;

                    // Try a simple lookup at the root first
                    string strValue = Request.Form[prefix + Name];


                    // if not found try to find the control and then
                    // use its UniqueID for lookup instead
                    if (strValue == null && page != null)
                    {
                        Control ctl = WebUtils.FindControlRecursive(page, formvarKey);
                        if (ctl != null)
                            strValue = Request.Form[ctl.UniqueID];
                    }

                    // Bool values and checkboxes might require special handling
                    if (strValue == null)
                    {
                        // Must handle checkboxes/radios
                        if (FieldType == typeof(bool))
                            strValue = "false";
                        else
                            continue;
                    }

                    try
                    {
                        // Convert the value to it target type
                        object Value = ReflectionUtils.StringToTypedValue(strValue, FieldType);

                        // Assign it to the object property/field
                        if (Field.MemberType == MemberTypes.Field)
                            fi.SetValue(target, Value);
                        else
                            pi.SetValue(target, Value, null);
                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        if (errors != null)
                            errors.Add(Field.Name, ex.Message);
                    }
                }
            }

            return !isError;
        }

        /// <summary>
        /// Routine that can be used read Form Variables into an object if the 
        /// object name and form variable names match either exactly or with a specific
        /// prefix.
        /// 
        /// The method loops through all *public* members of the object and tries to 
        /// find a matching form variable by the same name in Request.Form.
        /// 
        /// The routine returns false if any value failed to parse (ie. invalid
        /// formatting etc.). However parsing is not aborted on errors so all
        /// other convertable values are set on the object.
        /// 
        /// You can pass in a Dictionary<string,string> for the Errors parameter
        /// to optionally retrieve unbinding errors. The dictionary key holds the
        /// simple form varname for the field (ie. txtName), the value the actual
        /// exception error message
        /// </summary>
        /// <remarks>
        /// This method can have unexpected side-effects if multiple naming
        /// containers share common variable names. This routine is not recommended
        /// for those types of pages.
        /// </remarks>
        /// <param name="Target"></param>
        /// <param name="FormVarPrefix">empty or one or more prefixes spearated by |</param>
        /// <returns>true or false if an unbinding error occurs</returns>
        public static bool FormVarsToObject(object target, string formVarPrefix)
        {
            return FormVarsToObject(target, formVarPrefix, null);
        }

        /// <summary>
        /// Routine that retrieves form variables for each row in a dataset that match
        /// the fieldname or the field name with a prefix.
        /// The routine returns false if any value failed to parse (ie. invalid
        /// formatting etc.). However parsing is not aborted on errors so all
        /// other convertable values are set on the object.
        /// 
        /// You can pass in a Dictionary<string,string> for the Errors parameter
        /// to optionally retrieve unbinding errors. The dictionary key holds the
        /// simple form varname for the field (ie. txtName), the value the actual
        /// exception error message
        /// <seealso>Class wwWebUtils</seealso>
        /// </summary>
        /// <param name="loRow">
        /// A DataRow object to load up with values from the Request.Form[] collection.
        /// </param>
        /// <param name="Prefix">
        /// Optional prefix of form vars. For example, "txtCompany" has a "txt" prefix 
        /// to map to the "Company" field. Specify multiple prefixes and separate with |
        /// Leave blank or null for no prefix.
        /// </param>
        /// <param name="errors">
        /// An optional Dictionary that returns an error list. Dictionary is
        /// has a string key that is the name of the field and a value that describes the error.
        /// Errors are binding errors only.
        /// </param>
        public static bool FormVarsToDataRow(DataRow dataRow, string formvarPrefixes, Dictionary<string, string> errors)
        {
            bool isError = false;

            if (HttpContext.Current == null)
                throw new InvalidOperationException("FormVarsToObject can only be called from a Web Request");

            HttpRequest Request = HttpContext.Current.Request;

            // try to get a generic reference to a page for recursive find control
            // This value will be null if not dealing with a page (ie. in JSON Web Service)
            Page page = HttpContext.Current.CurrentHandler as Page;


            if (formvarPrefixes == null)
                formvarPrefixes = "";

            DataColumnCollection columns = dataRow.Table.Columns;

            // Look through all prefixes separated by |
            string[] prefixes = formvarPrefixes.Split('|');

            foreach (string prefix in prefixes)
            {
                foreach (DataColumn column in columns)
                {
                    string Name = column.ColumnName;

                    // Lookup key will be field plus the prefix
                    string formvarKey = prefix + Name;

                    // Try a simple lookup at the root first
                    string strValue = Request.Form[prefix + Name];

                    // if not found try to find the control and then
                    // use its UniqueID for lookup instead
                    if (strValue == null && page != null)
                    {
                        Control ctl = WebUtils.FindControlRecursive(page, formvarKey);
                        if (ctl != null)
                            strValue = Request.Form[ctl.UniqueID];
                    }

                    // Bool values and checkboxes might require special handling
                    if (strValue == null)
                    {
                        // Must handle checkboxes/radios
                        if (column.DataType == typeof(Boolean))
                            strValue = "false";
                        else
                            continue;
                    }

                    try
                    {
                        object value = ReflectionUtils.StringToTypedValue(strValue, column.DataType);
                        dataRow[Name] = value;
                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        if (errors != null)
                            errors.Add(Name, ex.Message);
                    }

                }
            }

            return !isError;
        }

        /// <summary>
        /// Creates the headers required to force the current request to not go into 
        /// the client side cache, forcing a reload of the page.
        /// 
        /// This method can be called anywhere as part of the Response processing to 
        /// modify the headers. Use this for any non POST pages that should never be 
        /// cached.
        /// <seealso>Class WebUtils</seealso>
        /// </summary>
        /// <param name="Response"></param>
        /// <returns>Void</returns>
        public static void ForceReload()
        {
            HttpResponse Response = HttpContext.Current.Response;
            Response.Expires = 0;
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Cache-Control", "no-cache, mustrevalidate");
        }


        /// <summary>
        /// Returns the result from an ASPX 'template' page in the /templates directory of this application.
        /// This method uses an HTTP client to call into the Web server and retrieve the result as a string.
        /// </summary>
        /// <param name="templatePageAndQueryString">The name of a page (ASPX, HTM etc.) in the Templates directory to retrieve plus the querystring</param>
        /// <param name="errorMessage">If this method returns null this message will contain the error info</param>
        /// <returns>Merged Text or null if an HTTP error occurs - note: could also return an Error page HTML result if the template page has an error.</returns>
        public static string AspTextMerge(string templatePageAndQueryString, ref string errorMessage)
        {
            string MergedText = "";

            // Save the current request information
            HttpContext Context = HttpContext.Current;

            // Fix up the path to point at the templates directory
            templatePageAndQueryString = Context.Request.ApplicationPath +
                "/templates/" + templatePageAndQueryString;

            // Now call the other page and load into StringWriter
            StringWriter sw = new StringWriter();
            try
            {
                // IMPORTANT: Child page's FilePath still points at current page
                //                QueryString provided is mapped into new page and then reset
                Context.Server.Execute(templatePageAndQueryString, sw);
                MergedText = sw.ToString();
            }
            catch (Exception ex)
            {
                MergedText = null;
                errorMessage = ex.Message;
            }

            return MergedText;

        }


        /// <summary>
        /// Renders a control to a string - useful for AJAX return values
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static string RenderControl(Control control)
        {
            StringWriter tw = new StringWriter();

            // Simple rendering - just write the control to the text writer
            // works well for single controls without containers
            Html32TextWriter writer = new Html32TextWriter(tw);
            control.RenderControl(writer);
            writer.Close();

            return tw.ToString();
        }
        
        /// <summary>
        /// Renders a control dynamically by creating a new Page and Form
        /// control and then adding the control to be rendered to it.        
        /// </summary>
        /// <remarks>
        /// This routine works to render most Postback controls but it
        /// has a bit of overhead as it creates a separate Page instance        
        /// </remarks>
        /// <param name="control">The control that is to be rendered</param>
        /// <param name="useDynamicPage">if true forces a Page to be created</param>
        /// <returns>Html or empty</returns>
        public static string RenderControl(Control control, bool useDynamicPage)
        {
            if (!useDynamicPage)
                return RenderControl(control);

            const string STR_BeginRenderControlBlock = "<!-- BEGIN RENDERCONTROL BLOCK -->";
            const string STR_EndRenderControlBlock = "<!-- End RENDERCONTROL BLOCK -->";

            StringWriter tw = new StringWriter();

            // Create a page and form so that postback controls work          
            Page page = new Page();
            page.EnableViewState = false;

            HtmlForm form = new HtmlForm();
            form.ID = "__t";
            page.Controls.Add(form);

            // Add placeholder to strip out so we get just the control rendered
            // and not the <form> tag and viewstate, postback script etc.
            form.Controls.Add(new LiteralControl(STR_BeginRenderControlBlock + "."));
            form.Controls.Add(control);
            form.Controls.Add(new LiteralControl("." + STR_EndRenderControlBlock));

            HttpContext.Current.Server.Execute(page, tw, true);

            string Html = tw.ToString();

            // Strip out form and ViewState, Event Validation etc.
            int at1 = Html.IndexOf(STR_BeginRenderControlBlock);
            int at2 = Html.IndexOf(STR_EndRenderControlBlock);
            if (at1 > -1 && at2 > at1)
            {
                Html = Html.Substring(at1 + STR_BeginRenderControlBlock.Length);
                Html = Html.Substring(0, at2 - at1 - STR_BeginRenderControlBlock.Length);
            }

            return Html;
        }

        /// <summary>
        /// Renders a user control into a string into a string.
        /// </summary>
        /// <param name="page">Instance of the page that is hosting the control</param>
        /// <param name="userControlVirtualPath"></param>
        /// <param name="includePostbackControls">If false renders using RenderControl, otherwise uses Server.Execute() constructing a new form.</param>
        /// <param name="data">Optional Data parameter that can be passed to the User Control IF the user control has a Data property.</param>
        /// <returns></returns>
        public static string RenderUserControl(string userControlVirtualPath,
                                               bool includePostbackControls,
                                               object data)
        {
            const string STR_NoUserControlDataProperty = "Passed a Data parameter to RenderUserControl, but the user control has no public Data property.";
            const string STR_BeginRenderControlBlock = "<!-- BEGIN RENDERCONTROL BLOCK -->";
            const string STR_EndRenderControlBlock = "<!-- End RENDERCONTROL BLOCK -->";

            StringWriter tw = new StringWriter();
            Control control = null;

            if (!includePostbackControls)
            {
                // Simple rendering works if no post back controls are used
                Page curPage = (Page)HttpContext.Current.CurrentHandler;
                control = curPage.LoadControl(userControlVirtualPath);
                if (data != null)
                {
                    try
                    {
                        ReflectionUtils.SetProperty(control, "Data", data);
                    }
                    catch
                    {
                        throw new ArgumentException(STR_NoUserControlDataProperty);
                    }
                }
                return RenderControl(control);
            }

            // Create a page and form so that postback controls work          
            Page page = new Page();
            page.EnableViewState = false;

            // IMPORTANT: Control must be loaded of this NEW page context or call will fail
            control = page.LoadControl(userControlVirtualPath);

            if (data != null)
            {
                try
                {
                    ReflectionUtils.SetProperty(control, "Data", data);
                }
                catch { throw new ArgumentException(STR_NoUserControlDataProperty); }
            }

            HtmlForm form = new HtmlForm();
            form.ID = "__t";
            page.Controls.Add(form);

            form.Controls.Add(new LiteralControl(STR_BeginRenderControlBlock));
            form.Controls.Add(control);
            form.Controls.Add(new LiteralControl(STR_EndRenderControlBlock));

            HttpContext.Current.Server.Execute(page, tw, true);

            string Html = tw.ToString();

            // Strip out form and ViewState, Event Validation etc.
            Html = StringUtils.ExtractString(Html, STR_BeginRenderControlBlock, STR_EndRenderControlBlock);

            return Html;
        }

        /// <summary>
        /// Renders a user control into a string into a string.
        /// </summary>
        /// <param name="userControlVirtualPath">virtual path for the user control</param>
        /// <param name="includePostbackControls">If false renders using RenderControl, otherwise uses Server.Execute() constructing a new form.</param>
        /// <param name="data">Optional Data parameter that can be passed to the User Control IF the user control has a Data property.</param>
        /// <returns></returns>
        public static string RenderUserControl(string userControlVirtualPath,
                                               bool includePostbackControls)
        {
            return RenderUserControl(userControlVirtualPath, includePostbackControls, null);
        }


        /// <summary>
        /// Returns just the Path of a full Url. Strips off the filename and querystring
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrlPath(string url)
        {
            int lnAt = url.LastIndexOf("/");
            if (lnAt > 0)
            {
                return url.Substring(0, lnAt + 1);
            }
            return "/";
        }

        /// <summary>
        /// Translates an ASP.NET path like /myapp/subdir/page.aspx 
        /// into an application relative path: subdir/page.aspx. The
        /// path returned is based of the application base and 
        /// starts either with a subdirectory or page name (ie. no ~)
        /// 
        /// The path is turned into all lower case.
        /// </summary>
        /// <param name="logicalPath">A logical, server root relative path (ie. /myapp/subdir/page.aspx)</param>
        /// <returns>Application relative path (ie. subdir/page.aspx)</returns>
        public static string GetAppRelativePath(string logicalPath)
        {            
            logicalPath = logicalPath.ToLower();

            string appPath = string.Empty;

            if (HttpContext.Current != null)
            {
                appPath = HttpContext.Current.Request.ApplicationPath.ToLower();
                if (appPath != "/")
                    appPath += "/";
                else
                    // Root web relative path is empty - strip off leading slash
                    return logicalPath.TrimStart('/');
            }
            else
            {
                // design time compiler for stock web projects will treat as root web
                return logicalPath.TrimStart('/');
            }
            
            return logicalPath.Replace(appPath, "");            
        }

        /// <summary>
        /// Translates the current ASP.NET path  
        /// into an application relative path: subdir/page.aspx. The
        /// path returned is based of the application base and 
        /// starts either with a subdirectory or page name (ie. no ~)
        /// 
        /// This version uses the current ASP.NET path of the request
        /// that is active and internally uses AppRelativeCurrentExecutionFilePath
        /// </summary>
        /// <returns></returns>
        public static string GetAppRelativePath()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Replace("~/","");            
        }


        /// <summary>
        /// Determines if GZip is supported
        /// </summary>
        /// <returns></returns>
        public static bool IsGZipSupported()
        {
            string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(AcceptEncoding) &&
                 (AcceptEncoding.Contains("gzip") || AcceptEncoding.Contains("deflate")))
                return true;
            return false;
        }

        /// <summary>
        /// Sets up the current page or handler to use GZip through a Response.Filter
        /// IMPORTANT:  
        /// You have to call this method before any output is generated!
        /// </summary>
        public static void GZipEncodePage()
        {
            HttpResponse Response = HttpContext.Current.Response;

            if (IsGZipSupported())
            {
                string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
                if (AcceptEncoding.Contains("deflate"))
                {
                    Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter,
                                               System.IO.Compression.CompressionMode.Compress);
                    Response.AppendHeader("Content-Encoding", "deflate");
                }
                else
                {
                    Response.Filter = new System.IO.Compression.GZipStream(Response.Filter,
                                              System.IO.Compression.CompressionMode.Compress);
                    Response.AppendHeader("Content-Encoding", "gzip");                    
                }
            }

            // Allow proxy servers to cache encoded and unencoded versions separately
            Response.AppendHeader("Vary", "Content-Encoding");
        }

        /// <summary>
        /// Retrieves a value by key from a UrlEncoded string.
        /// </summary>
        /// <param name="urlEncodedString">UrlEncoded String</param>
        /// <param name="key">Key to retrieve value for</param>
        /// <returns>returns the value or "" if the key is not found or the value is blank</returns>
        public static string GetUrlEncodedKey(string urlEncodedString, string key)
        {
            string res = StringUtils.ExtractString("&" + urlEncodedString, "&" + key + "=", "&", false, true);
            return HttpUtility.UrlDecode(res);
        }

        /// <summary>
        /// Returns a query string value parsed into an integer. If the value is not found
        /// or not a number null is returned.
        /// </summary>
        /// <param name="queryStringKey">The query string key to retrieve</param>        
        /// <returns>parsed integer or null on failure</returns>
        public static int? GetParamsInt(string queryStringKey)
        {            
            string val = HttpContext.Current.Request.QueryString[queryStringKey];
            if (val == null)
                return null;                

            int ival = 0;
            if (!int.TryParse(val, out ival))
                return null; 

            return ival;
        }



        /// <summary>
        /// Parses a Carriage Return based into a &lt;ul&gt; style HTML list by 
        /// splitting each carriage return separated line.
        /// <seealso>Class wwWebUtils</seealso>
        /// </summary>
        /// <param name="text">
        /// The carriage return separated text list
        /// </param>
        /// <returns>string</returns>
        public static string TextListToHtmlList(string text)
        {
            string[] TextStrings = text.Split(new char[1] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder();
            foreach (string str in TextStrings)
            {
                if (str == "\n")
                    continue;

                sb.Append("<li>" + str + "</li>\r\n");
            }

            sb.Append("</ul>");
            return "<ul>" + sb.ToString();
        }


        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string that is returned in double quotes.
        /// 
        /// The string returned includes outer quotes: 
        /// "Hello \"Rick\"!\r\nRock on"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeJsString(string s)
        {
            if (s == null)
                return "null";

            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");


            return sb.ToString();
        }
       
        
        /// <summary>
        /// Converts a .NET date to a JavaScript JSON date value. Several
        /// different formats are supported (new Date(), MsAjax style strings
        /// and ISO date strings).
        /// </summary>
        /// <param name="date">.Net Date</param>
        /// <returns></returns>
        public static string EncodeJsDate(DateTime date, JsonDateEncodingModes dateMode)
        {            
            TimeSpan tspan = date.ToUniversalTime().Subtract(DAT_JAVASCRIPT_BASEDATE);
            double milliseconds = Math.Floor(tspan.TotalMilliseconds);

            // raw date expression - new Date(1227578400000)
            if (dateMode == JsonDateEncodingModes.NewDateExpression)
                return "new Date(" + milliseconds.ToString() + ")";

            // MS Ajax style string: "\/Date(1227578400000)\/"
            if (dateMode == JsonDateEncodingModes.MsAjax)
            {
                StringBuilder sb = new StringBuilder(40);
                sb.Append(@"""\/Date(");
                sb.Append(milliseconds);

                // Add Timezone 
                sb.Append((TimeZone.CurrentTimeZone.GetUtcOffset(date).Hours * 100).ToString("0000").PadLeft(4, '0'));

                sb.Append(@")\/""");
                return sb.ToString();
            }

            // ISO 8601 mode string "2009-03-28T21:55:21Z"
            if (dateMode == JsonDateEncodingModes.ISO)                           
                return "\"" + DateTime.UtcNow.ToString("s") + "Z" + "\"";            

            throw new ArgumentException("Date Format not supported.");
        }

        /// <summary>
        /// Converts a .NET date to a JavaScript date value.
        /// 
        /// This version creates a new Date(xxx) expression.
        /// </summary>
        /// <param name="date">.NET Date value</param>
        /// <returns>new Date(xxxx)</returns>
        public static string EncodeJsDate(DateTime date)
        {
            return EncodeJsDate(date,JsonDateEncodingModes.NewDateExpression);
        }
    
        /// <summary>
        /// Returns a resource string. Shortcut for HttpContext.GetGlobalResourceObject.
        /// </summary>
        /// <param name="resourceSet">Resource Set Id (ie. name of the file or 'resource set')</param>
        /// <param name="resourceId">The key in the resource set</param>
        /// <returns></returns>
        public static string GRes(string resourceSet, string resourceId)
        {
            string Value = HttpContext.GetGlobalResourceObject(resourceSet, resourceId) as string;
            if (string.IsNullOrEmpty(Value))
                return resourceId;
            
            return Value;
        }
        
        /// <summary>
        /// Returns a resource string. Shortcut for HttpContext.GetGlobalResourceObject.
        /// 
        /// This version defaults to Resources as the resource set it.
        /// Defaults to "Resources" as the ResourceSet (ie. Resources.xx.resx)
        /// </summary>
        /// <param name="resourceId">Key in the Resources resource set</param>
        /// <returns></returns>
        public static string GRes(string resourceId)
        {
            return GRes("Resources", resourceId);
        }

        /// <summary>
        /// Returns a JavaScript Encoded string from a Global Resource
        /// </summary>
        /// <param name="classKey"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public static string GResJs(string classKey, string resourceId)
        {
            string Value = GRes(classKey, resourceId) as string;
            return EncodeJsString(Value);
        }

        /// <summary>
        /// Returns a local resource from the resource set of the current active request
        /// local resource.
        /// </summary>       
        /// <param name="resourceId">The resourceId of the item in the local resourceSet file to retrieve</param>
        /// <returns></returns>
        public static string LRes(string resourceId)
        {               
            string vPath = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
            string value = HttpContext.GetLocalResourceObject(vPath, resourceId) as string;
                                                                     
            if (value == null)
                return resourceId;
                       
            return value;
        }

        /// <summary>
        /// Returns a local resource for the given resource set that you specify explicitly.
        /// 
        /// Use this method only if you need to retrieve resources from a local resource not
        /// specific to the current request.
        /// </summary>
        /// <param name="resourceSet">The resourceset specified as: subdir/page.aspx or page.aspx or as a virtual path (~/subdir/page.aspx)</param>
        /// <param name="resourceKey">The resource ID to retrieve from the resourceset</param>
        /// <returns></returns>
        public static string LRes(string resourceSet, string resourceKey)
        {
            if (!resourceSet.StartsWith("~/"))
                resourceSet = "~/" + resourceSet;
            
            string Value = HttpContext.GetLocalResourceObject(resourceSet, resourceKey) as string;
            if (Value == null)
                return resourceKey;

            return Value;
        }

        /// <summary>
        /// Returns a local resource properly encoded as a JavaScript string 
        /// including the quote characters.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public static string LResJs(string resourceId)
        {
            return LRes(EncodeJsString(resourceId));
        }

        /// <summary>
        /// Returns a JavaScript Encoded string from a Global Resource
        /// Defaults to the "Resources" resource set.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string GResJs(string resourceKey)
        {
            return GResJs("Resources", resourceKey);
        }

    }

}
