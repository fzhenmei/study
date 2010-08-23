using System;
using System.Web;
using Westwind.Utilities;
using Westwind.Utilities.Logging;

namespace Westwind.WebToolkit
{

    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Route to our own internal handler
            App.OnApplicationStart();
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            // Request Logging
            if (LogManagerConfiguration.Current.LogWebRequests)
            {
                try
                {
                    WebLogEntry entry = new WebLogEntry()
                    {
                        ErrorLevel = ErrorLevels.Log,   
                        Message= this.Context.Request.FilePath,
                        RequestDuration = (decimal) DateTime.Now.Subtract(Context.Timestamp).TotalMilliseconds
                    };
                    entry.UpdateFromRequest(this.Context);                                            
                    LogManager.Current.WriteEntry(entry);
                }
                catch { ;}
            }
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom == "GZIP")
            {
                if (Westwind.Utilities.WebUtils.IsGZipSupported())
                    return "GZip";
                return "";
            }
            
            return base.GetVaryByCustomString(context, custom);
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception serverException = Server.GetLastError();

                WebErrorHandler errorHandler;

                // Try to log the inner Exception since that's what
                // contains the 'real' error.
                if (serverException.InnerException != null)
                    serverException = serverException.InnerException;

                errorHandler = new WebErrorHandler(serverException);


                // Log the error if specified
                if (LogManagerConfiguration.Current.LogErrors)
                {
                    errorHandler.Parse(); 
                    WebLogEntry entry = new WebLogEntry(serverException, this.Context);
                    entry.Details = errorHandler.ToString();

                    LogManager.Current.WriteEntry(entry);
                }

                // Retrieve the detailed String information of the Error
                string ErrorDetail = errorHandler.ToString();

                // Optionally email it to the Admin contacts set up in WebStoreConfig
                if (!string.IsNullOrEmpty(App.Configuration.AdminEmailAddress))
                    AppUtils.SendAdminEmail(App.Configuration.ApplicationTitle + "Error: " + Request.RawUrl, ErrorDetail, "");


                // Debug modes handle different error display mechanisms
                // Default  - default ASP.Net - depends on web.config settings
                // Developer  - display a generic application error message with no error info
                // User  - display a detailed error message with full error info independent of web.config setting
                if (App.Configuration.DebugMode == DebugModes.DeveloperErrorMessage)
                {
                    
                    Server.ClearError();                    
                    Response.TrySkipIisCustomErrors = true;                    
                    MessageDisplay.DisplayMessage("Application Error", "<pre class='body'>" + ErrorDetail + "</pre>");
                    return;
                }

                else if (App.Configuration.DebugMode == DebugModes.ApplicationErrorMessage)
                {                    
                    string StockMessage =
                            "The Server Administrator has been notified and the error logged.<p>" +
                            "Please continue on by either clicking the back button or by returning to the home page.<p>" +
                            "<center><b><a href='" + App.Configuration.ApplicationHomeUrl + "'>Web Log Home Page</a></b></center>";

                    // Handle some stock errors that may require special error pages
                    HttpException httpException = serverException as HttpException;
                    if (httpException != null)
                    {
                        int HttpCode = httpException.GetHttpCode();
                        Server.ClearError();

                        if (HttpCode == 404) // Page Not Found 
                        {
                            Response.StatusCode = 404;
                            MessageDisplay.DisplayMessage("Page not found",
                                "You've accessed an invalid page on this Web server. " +
                                StockMessage);
                            return;
                        }
                        if (HttpCode == 401) // Access Denied 
                        {
                            Response.StatusCode = 401;
                            MessageDisplay.DisplayMessage("Access Denied",
                                "You've accessed a resource that requires a valid login. " +
                                StockMessage);
                            return;
                        }
                    }

                    // Display a generic error message
                    Server.ClearError();
                    Response.StatusCode = 500;
                    
                    Response.TrySkipIisCustomErrors = true; 
                    
                    MessageDisplay.DisplayMessage("Application Error",
                        "We're sorry, but an unhandled error occurred on the server. " +
                        StockMessage);
                     
                    return;
                }

                return;
            }
            catch(Exception ex)
            {                
                // Failure in the attempt to report failure - try to email
                if (!string.IsNullOrEmpty(App.Configuration.AdminEmailAddress))
                {
                    AppUtils.SendAdminEmail(App.Configuration.ApplicationTitle + "Error: " + Request.RawUrl,
                            "Application_Error failed!\r\n\r\n" +
                            ex.ToString(), "");
                }

                // and display an error message
                Server.ClearError();
                Response.StatusCode = 200;
                Response.TrySkipIisCustomErrors = true;
                MessageDisplay.DisplayMessage("Application Error Handler Failed",
                        "The application Error Handler failed with an exception."  + 
                        (App.Configuration.DebugMode == DebugModes.DeveloperErrorMessage ?  "<pre>" + ex.ToString() + "</pre>" : ""));
                    
            }
        }


        protected void Application_End(object sender, EventArgs e)
        {
            
            if (LogManagerConfiguration.Current.LogWebRequests)
                LogManager.Current.WriteEntry(new WebLogEntry() { ErrorLevel = ErrorLevels.Message, 
                                                                     Message = "Application shut down" });
        }



    }
}