using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace TimeTrakkerWeb
{
    /// <summary>
    /// This is a standard error page which is used for application error messages.
    /// Generally this page is called from WebStoreUtils::ErrorMessage.
    /// 'Parameters' are passed to this page via Context items:
    /// ErrorMessage_Header
    /// ErrorMessage_Message
    /// ErrorMessage_Timeout
    /// ErrorMessage_RedirectUrl
    /// </summary>
    public class MessageDisplay : Westwind.Web.Controls.MessageDisplayBase
    {
        protected Label lblHeader;
        protected Label lblMessage;
        protected Label lblRedirectHyperLink;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            
            // *** Set the display properties and pass down
            this.DisplayPage(this.lblHeader, this.lblMessage, this.lblRedirectHyperLink);
        }
    }

}