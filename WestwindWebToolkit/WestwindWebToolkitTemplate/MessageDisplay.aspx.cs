using System;
using System.Web.UI.WebControls;

namespace $safeprojectname$
{
    /// <summary>
    /// This is a standard error page which is used for application error messages.
    /// Generally this page is called from MessageDisplay.DisplayMessage()
    ///
    /// 'Parameters' are passed to this page via Context items:
    /// ErrorMessage_Header
    /// ErrorMessage_Message
    /// ErrorMessage_Timeout
    /// ErrorMessage_RedirectUrl
    /// 
    /// The base class populates the lblHeader, lblMessage and lblRedirectHyperLink
    /// controls with values.
    /// </summary>
    public partial class MessageDisplay : Westwind.Web.Controls.MessageDisplayBase
    {
        // IMPORTANT: Ensure that these controls exist on the markup page:
        //protected Label lblHeader;
        //protected Label lblMessage;
        //protected Label lblRedirectHyperLink;

        protected void Page_Load(object sender, System.EventArgs e)
        {            
            // Set the display properties and pass down
            this.DisplayPage(this.lblHeader, this.lblMessage, this.lblRedirectHyperLink);
        }
    }

}