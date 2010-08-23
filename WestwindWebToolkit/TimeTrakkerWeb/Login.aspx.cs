using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using TimeTrakker;
using Westwind.Web.Controls;

namespace TimeTrakkerWeb
{
    public partial class Login : System.Web.UI.Page
    {
        busUser UserObject = TimeTrakkerFactory.GetUser();

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.IsPostBack)
                // *** Always force a signout when the page loads first
                FormsAuthentication.SignOut();

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = this.Username.Text;
            string password = this.Password.Text;
            bool remember = this.RememberMe.Checked;

            if (UserObject.AuthenticateAndLoad(username,password) != null)
            {
                // *** Created custom ticket so we can attach a custom data
                //     item. In this case we store username|userpk
                //     which is split out in the Master Page to provide Username and UserPk props
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        username, 
                        DateTime.Now, 
                        DateTime.Now.AddDays(10),
                        remember, 
                        UserObject.Entity.Pk + "|" + UserObject.Entity.UserName );

                string ticketString = FormsAuthentication.Encrypt(ticket);

                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketString);
                
                if (remember)
                    cookie.Expires = DateTime.Now.AddDays(10);
                
                Response.Cookies.Add(cookie);
                
                // *** Use explict redirect so we get our cookie written
                Response.Redirect(FormsAuthentication.GetRedirectUrl(username, false));

                //FormsAuthentication.RedirectFromLoginPage(username,remember);
            }
            else
            {
                this.ErrorDisplay.ShowError("Invalid Login. Please make sure you fill in username and password.");
            }
        }
        
    }
}
