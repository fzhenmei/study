using System;
using System.Text;
using System.Web;
using Westwind.Utilities;
using Westwind.InternetTools;
using System.Web.UI;
using System.Data;

namespace $safeprojectname$
{
    
    /// <summary>
    /// This class contains a number of helpful routines for Web applications
    /// concerning site information, formatting and a few other useful items.
    /// </summary>
    public class AppUtils
    {
        /// <summary>
        /// Create a new Cookie object that is static and always active in our app.
        /// This is where we assign the name.
        /// </summary>
        public static ApplicationCookie ApplicationCookie = new ApplicationCookie(App.Configuration.ApplicationCookieName);


        

        /// <summary>
        /// Sends email using the WebStoreConfig Email Configuration defaults.
        /// 
        /// <seealso>Class WebUtils</seealso>
        /// </summary>
        /// <param name="Subject">
        /// The subject of the message.
        /// </param>
        /// <param name="Message">
        /// The body of the message.
        /// </param>
        /// <param name="Recipient">
        /// The recipient as an email address.
        /// </param>
        /// <param name="SendName">
        /// Name descriptive of the sender.
        /// </param>
        /// <param name="SendEmail">
        /// The email address of the sender.
        /// </param>
        /// <param name="NoAsync">
        /// Whether this message is send asynchronously or not.
        /// </param>
        /// <param name="Boolean SendAsText">
        /// Determines whether the message is sent as Text or HTML.
        /// </param>
        /// <returns>Boolean</returns>
        public static bool SendEmail(string Subject, string Message, string Recipient, string SendName,
                                     string SendEmail, bool NoAsync, bool SendAsText)
        {
            try
            {
                SmtpClientCustom Email = new SmtpClientCustom();
                Email.MailServer = App.Configuration.MailServer;
                Email.Recipient = Recipient;

                if (SendAsText)
                    Email.ContentType = "text/plain";
                else
                {
                    Email.ContentType = "text/html; charset=utf-8";
                    Email.Encoding = Encoding.UTF8;
                }


                if (SendName == null || SendName == "")
                {
                    Email.SenderEmail = App.Configuration.SenderEmailAddress;
                    Email.SenderName = App.Configuration.SenderEmailName;
                }
                else
                {
                    Email.SenderEmail = SendEmail;
                    Email.SenderName = SendName;
                }

                Email.Subject = Subject;
                Email.Message = Message;

                // Capture any error messages and log them
                Email.SendError += new delSmtpEvent(OnSmtpError);

                if (NoAsync)
                    return Email.SendMail();
                else
                    Email.SendMailAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sends email using the WebStoreConfig Email Configuration defaults.
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Message"></param>
        /// <param name="Recipient"></param>
        /// <returns></returns>
        public static bool SendEmail(string Subject, string Message, string Recipient)
        {
            return AppUtils.SendEmail(Subject, Message, Recipient, null, null, false, false);
        }

        /// <summary>
        /// Sends email using the WebStoreConfig Email Configuration defaults.
        /// <seealso>Class WebUtils</seealso>
        /// </summary>
        /// <param name="Subject">
        /// The subject of the message.
        /// </param>
        /// <param name="Message">
        /// The body of the message.
        /// </param>
        /// <param name="Recipient">
        /// The recipient as an email address.
        /// </param>
        /// <param name="Boolean SendAsText">
        /// Determines whether the message is sent as Text or HTML.
        /// </param>
        /// <returns>Boolean</returns>
        public static bool SendEmail(string Subject, string Message, string Recipient, bool SendAsText)
        {
            return AppUtils.SendEmail(Subject, Message, Recipient, null, null, false, SendAsText);
        }

        /// <summary>
        /// Sends an Admin email with the Admin email name and email address from Web Store Config
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Message"></param>
        /// <param name="Recipient"></param>
        /// <returns></returns>
        public static bool SendAdminEmail(string Subject, string Message, string ccList)
        {
            try
            {
                SmtpClientCustom Email = new SmtpClientCustom();
                Email.MailServer = App.Configuration.MailServer;                
                Email.Recipient = App.Configuration.AdminEmailAddress;

                Email.SenderEmail = App.Configuration.AdminEmailAddress;
                Email.SenderName = App.Configuration.AdminEmailName;
                Email.ContentType = "text/plain";
                                

                Email.Subject = Subject;
                Email.Message = Message;

                // Capture errors so we can log them
                Email.SendError += new delSmtpEvent(OnSmtpError);

                Email.SendMailAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static void OnSmtpError(SmtpClientCustom Smtp)
        {
            string ErrorMessage = "Error sending Email: " + Smtp.ErrorMessage + "\r\n" +
                       Smtp.Subject + "\r\nTo: " +
                       Smtp.Recipient + "\r\nServer: " + Smtp.MailServer;

            // Log into SQL Error Log
            LogMessage(ErrorMessage, WebRequestLogMessageTypes.Error);
        }



        /// <summary>
        /// Small wrapper method to isolate logging features into a self-contained message
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static bool LogMessage(string ErrorMessage)
        {
            //if (App.Configuration.ConnectType == Westwind.BusinessObjects.ServerTypes.SqlServer)
            //    return WebRequestLog.LogCustomMessage(App.Configuration.ConnectionString,
            //                                      WebRequestLogMessageTypes.ApplicationMessage,
            //                                      ErrorMessage);
            return false;
        }

        /// <summary>
        /// Logging works only with Sql Server at the moment
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <param name="MessageType"></param>
        /// <returns></returns>
        public static bool LogMessage(string ErrorMessage, WebRequestLogMessageTypes MessageType)
        {
            //if (App.Configuration.ConnectType == Westwind.BusinessObjects.ServerTypes.SqlServer)
            //    return WebRequestLog.LogCustomMessage(App.Configuration.ConnectionString,
            //                                          MessageType, ErrorMessage);

            return false;
        }



    }
    
    

    public class ApplicationCookie : Westwind.Web.CookieManager
    {
        public ApplicationCookie(string NewCookie) : base(NewCookie) { ; }

        /// <summary>
        /// Retrieves the Customer's user from a Cookie. If the cookie doesn't
        /// exist the cookie is created and written to the Response.
        /// </summary>
        /// <returns>Customers UserId</returns>
        public string GetUserId()
        {
            return this.GetId();
        }
    }

}
