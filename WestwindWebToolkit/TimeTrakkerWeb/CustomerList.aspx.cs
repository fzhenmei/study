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

namespace TimeTrakkerWeb
{
    public partial class CustomerList : TimeTrakkerBaseForm
    {
        busCustomer Customer = new busCustomer();

        protected override void OnLoad(EventArgs e)
        {
            this.FocusedControl = this.txtCompanyFilter;

            IQueryable<CustomerEntity> custList = Customer.GetCustomerList();

            // *** Read and apply the filters
            if (!string.IsNullOrEmpty(this.txtCompanyFilter.Text))
                custList = custList.Where(c => c.Company.StartsWith(this.txtCompanyFilter.Text));
            if (!string.IsNullOrEmpty(this.txtLastNameFilter.Text))
                custList = custList.Where(c => c.LastName.StartsWith(this.txtLastNameFilter.Text));


            // *** Now filter the result down to only the fields we need
            var truncCustList = custList.Select(c =>
                                                new
                                                {
                                                    Pk = c.Pk,
                                                    Company = c.Company,
                                                    Lastname = c.LastName,
                                                    Firstname = c.FirstName
                                                });

            // *** Create a concrete List object so we can count and get result in one pass
            var list = truncCustList.ToList();

            this.dgCustomers.DataSource = list;
            this.dgCustomers.DataBind();

            this.lblStatus.Text = list.Count.ToString() + " Customers";


            // *** and clear 'em out - note have to do it AFTER the queries since the strings
            // *** are used in the actual queries otherwise the value will be queried for blank
            this.txtCompanyFilter.Text = "";
            this.txtLastNameFilter.Text = "";

        }
    }
}
