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

namespace TimeTrakkerWeb
{
    public partial class TimeTrakkerMaster : System.Web.UI.MasterPage
    {

        /// <summary>
        /// Title of the page that assign
        /// </summary>
        public string SubTitle
        {
            get { return _SubTitle; }
            set { _SubTitle = value; }
        }
        private string _SubTitle = "Time Trakker";

        /// <summary>
        /// Gets the authenticated user's name
        /// </summary>
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        private string _Username = "";

        /// <summary>
        /// Gets the authenticated user's Pk
        /// </summary>
        
        public int UserPk
        {
            get { return _UserPk; }
            set { _UserPk = value; }
        }
        private int _UserPk = 0;


        
        protected override void OnInit(EventArgs e)
        {
            if (this.Page.User.Identity.IsAuthenticated)
            {                
                // *** User data contains "pk|Username"
                string userData = ((FormsIdentity)this.Page.User.Identity).Ticket.UserData;

                string[] tokens = userData.Split('|');

                if (tokens.Length < 2)
                    return;

                this.Username = tokens[1];

                int.TryParse(tokens[0], out this._UserPk);

                // *** Change the menu choice 
                this.hypLogin.Text = "Logout";
            }
        }

        public void HideMasterContent()
        {
            this.masterHeader.Style.Add("display", "none");
            this.sidebarColumn.Style.Add("display", "none");
            this.contentTable.Style.Add("margin-top", "0px");
        }
    }
}
