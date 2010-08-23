using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;


namespace Westwind.WebToolkit
{
    public partial class JsonStockClient : System.Web.UI.Page
    {
        protected string UserToken = String.Empty;
        protected string Username = String.Empty;

        /// <summary>
        /// PortfolioMessage item we can use in the page to display
        /// info about hte portfolio 
        /// </summary>
        protected PortfolioMessage Portfolio = new PortfolioMessage();
        

        protected busPortfolioItem PortfolioItem = new busPortfolioItem();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Assign subtitle to the page
            ((WestWindWebToolkitMaster)this.Master).SubTitle = "Ajax Stock Client";

            // assign Ajax Handler Service's type so proxy can be generated
            this.Proxy.ClientProxyTargetType = typeof(JsonStockService);
            
            // Login User, create user token and embed into page
            this.LoginCheck();            
        }

        protected override void OnPreRender(EventArgs e)
        {
           if (!string.IsNullOrEmpty(this.Username))
           {
                this.btnLogout.Visible = true;
                this.LoginGroup.Visible = false;
                this.LoginName.Visible = true;
                this.LoginName.InnerText = Username;
            }

            // Removed - do client side via service
            //this.BindPorfolioList();
            base.OnPreRender(e);            
        }

        
        protected void BindPorfolioList()
        {
            if (string.IsNullOrEmpty(this.Username))
                return;

            busUser user = new busUser();
            int userPk = user.GetPkFromEmail(this.Username);

            this.Portfolio.TotalItems = this.PortfolioItem.GetPortfolioItemCount(userPk);
            this.Portfolio.TotalValue = this.PortfolioItem.GetPortfolioTotalValue(userPk);

            IQueryable<PortfolioItem> items = this.PortfolioItem.GetItemsForUser(userPk);

            this.lstPortfolio.DataSource = PortfolioItem.Converter.ToDataReader(items);
            this.lstPortfolio.DataBind();
        }

        /// <summary>
        /// Checks for login - if True sets this.Loggedin
        /// 
        /// Uses LoginUser Session key
        /// </summary>
        protected bool LoginCheck()
        {            
            this.UserToken = Session["UserToken"] as string;

            if (!string.IsNullOrEmpty(this.UserToken))
            {
                this.Username = this.Session["Username"] as string?? "";
                return true;
            }

            busUser user = new busUser();
            if ( user.AuthenticateAndLoad(this.txtUsername.Text, this.txtPassword.Text) != null)
            {
                busUserToken token = new busUserToken();
                string tk = token.GetOrCreateToken(user.Entity.Pk);

                
                token.UpdateTokenExpiration(tk);

                if (string.IsNullOrEmpty(tk))
                    return false;

                this.UserToken = tk;
                Session["UserToken"] = tk;

                this.Username = user.Entity.Email;
                Session["Username"] = this.Username;                
                
                return true;
            }

            return false;
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["UserToken"] = null;
            Session["Username"] = null;
            this.Username = string.Empty;
            this.UserToken = string.Empty;

    
        }

    }    
}
