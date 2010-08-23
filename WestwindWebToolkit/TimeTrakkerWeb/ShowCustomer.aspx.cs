using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using TimeTrakker;
using Westwind.BusinessFramework;
using Westwind.Utilities;

namespace TimeTrakkerWeb
{
    public partial class ShowCustomer : System.Web.UI.Page
    {
        public busCustomer Customer = new busCustomer();
        string Id = null;
        int Pk = 0;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.txtState.DataSource = App.StateList;
            this.txtState.DataValueField = "StateCode";
            this.txtState.DataTextField = "State";
            this.txtState.DataBind();

            this.txtCountries.DataSource = App.CountryList;
            this.txtCountries.DataValueField = "CountryCode";
            this.txtCountries.DataTextField = "Country";
            this.txtCountries.DataBind();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Id = Request.QueryString["Id"] ?? string.Empty;
            int.TryParse(Id,out this.Pk);

            if (this.Pk > 0)
            {
                if (this.Customer.Load(this.Id) == null)
                {
                    this.ErrorDisplay.ShowError("Invalid Customer Id");
                    return;
                }
            }
            else
                this.Customer.NewEntity();


            if (!this.IsPostBack)
                this.DataBinder.DataBind();

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.DataBinder.Unbind();

            if (this.DataBinder.BindingErrors.Count > 0)
            {
                this.ErrorDisplay.ShowError(this.DataBinder.BindingErrors.ToHtml(),
                                            "Please correct the following:");
                return;
            }

            if (!this.Customer.Validate())
            {
                foreach (ValidationError error in this.Customer.ValidationErrors)
                {
                    this.DataBinder.AddBindingError(error.Message, error.ControlID);
                }
                this.ErrorDisplay.ShowError(this.DataBinder.BindingErrors.ToHtml(),
                            "Please correct the following:");
                return;
            }


            if (!this.Customer.Save())
            {
                this.ErrorDisplay.ShowError(this.Customer.ErrorMessage);
                return;
            }

            this.ErrorDisplay.ShowMessage("Customer has been saved.");

            // *** New entries have to be redisplayed so we can see the new pk on the query string
            if (string.IsNullOrEmpty(this.Id))
                Response.Redirect(Request.FilePath + "?id=" + Customer.Entity.Pk.ToString());
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string message = string.Format(@"Customer {0} has been deleted.",this.Customer.Entity.Company);
            if (this.Customer.Delete(this.Pk))
                MessageDisplay.DisplayMessage("Customer Deleted", message, "CustomerList.aspx", 2);
            else
                this.ErrorDisplay.ShowError("Couldn't delete customer:<hr/>" + this.Customer.ErrorMessage);
        }
    }
}
