
using System;
using System.Data;
using System.Linq;

namespace Westwind.GlobalizationWeb
{

    public partial class CustomerList : System.Web.UI.Page
    {
        protected busCustomer Customer = null;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Customer = new busCustomer();
            if (!this.IsPostBack)
                this.BindCustomerList();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack && this.lstCustomers.Items.Count > 0)
            {
                // *** Bind the first item on first load explicitly
                this.lstCustomers.SelectedValue = this.lstCustomers.Items[0].Value;
                this.BindCustomerInfo(this.lstCustomers.Items[0].Value);
            }
        }



        protected void lstCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CustomerId = this.lstCustomers.SelectedValue;
            this.BindCustomerInfo(this.lstCustomers.SelectedValue);
        }

        protected void btnSave_Click(object szender, EventArgs e)
        {
            if (!this.SaveCustomer())
                return;

            // *** Reselect item
            int Selected = this.lstCustomers.SelectedIndex;
            this.lstCustomers.Items.Clear();

            // *** Reload and bind Customer List
            // *** if the Company has changed
            this.BindCustomerList();

            this.lstCustomers.SelectedIndex = Selected;
        }

        public void BindCustomerInfo(string CustomerId)
        {
            // Load the customer object and Entity member
            if (this.Customer.Load(CustomerId) == null)
                this.Customer.NewEntity();

            // *** Rebind only the Panel! Don't bind whole form
            // *** or there will be problems with the List selectedvalue
            this.CustomerInfoPanel.DataBind();

            this.BindInvoiceList(CustomerId);
        }


        public void BindCustomerList()
        {
            var custList = this.Customer.GetCustomerList().ToList();
            if (custList.Count < 1)
            {
                this.ErrorDisplay.ShowError(this.Customer.ErrorMessage);
                return;
            }

            this.lstCustomers.DataSource = custList;
            this.lstCustomers.DataValueField = "CustomerID";
            this.lstCustomers.DataTextField = "CompanyName";
            this.lstCustomers.DataBind();
        }

        protected void BindInvoiceList(string CustomerId)
        {
            if (!this.dgOrders.Visible)
                return;

            busInvoice Invoice = new busInvoice();
            DataTable invList = Invoice.GetInvoicesForCustomer(CustomerId);
            if (invList == null)
            {
                this.ErrorDisplay.ShowError(Invoice.ErrorMessage);
                return;
            }

            this.dgOrders.DataSource = invList;

            // *** Calculate the total to display in the footer
            decimal Totals = 0.00M;
            foreach (DataRow dr in invList.Rows)
            {
                Totals += (decimal)dr["OrderTotal"];
            }

            this.dgOrders.Columns[1].FooterText = this.GetLocalResourceObject("TotalForInvoices") as string;
            this.dgOrders.Columns[2].FooterText = string.Format("{0:c}", Totals);

            this.dgOrders.DataBind();
        }

        /// <summary>
        /// Demonstrates using POSTback form vars
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool SaveCustomer()
        {
            string CustomerId = this.lstCustomers.SelectedValue;

            this.Customer = new busCustomer();

            if (CustomerId == null)
            {
                this.ErrorDisplay.ShowError(this.GetGlobalResourceObject("resources", "CouldNotLoadCustomer") as string);
                return false;
            }

            if (CustomerId == "")
            {
                if (Customer.NewEntity() == null)
                {
                    this.ErrorDisplay.ShowError(this.GetGlobalResourceObject("resources", "CouldNotCreateNewCustomer") as string);
                    return false;
                }
            }
            else
            {
                if (Customer.Load(CustomerId) == null)
                {
                    this.ErrorDisplay.ShowError(this.GetGlobalResourceObject("resources", "CouldNotLoadCustomer") as string);
                    return false;
                }
            }

            // *** Pick up values that were posted back from the
            // *** the client callback
            Customer.Entity.ContactName = this.txtContactName.Text;
            Customer.Entity.CompanyName = this.txtCompany.Text;
            Customer.Entity.Address = this.txtAddress.Text;
            Customer.Entity.City = this.txtCity.Text;
            //Customer.Entity.Entered = this.txtEntered.SelectedDate;

            if (!Customer.Validate())
            {
                this.ErrorDisplay.ShowError(Customer.ValidationErrors.ToString());
                return false;
            }

            if (!Customer.Save())
            {
                this.ErrorDisplay.ShowError(Customer.ErrorMessage);
                return false;
            }

            this.ErrorDisplay.ShowMessage(this.GetGlobalResourceObject("resources", "CustomerSaved") as string);

            return true;
        }


    }
}