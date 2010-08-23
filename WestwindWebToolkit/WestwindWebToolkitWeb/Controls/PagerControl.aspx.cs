
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Web.Controls;

namespace Westwind.WebToolkit.Controls
{
    public partial class PagerControl : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {                                    
            
            busCustomer customer = new busCustomer();

            // *** ListView Binding Example **//

            var custList2 = customer.GetCustomerList();

            // Filter the query and assign TotalPagers internally
            custList2 = ListViewPager.FilterIQueryable(custList2);

            // project only fields we need
            var abbrCustList2 = custList2.Select(cust =>
                                        new
                                        {
                                            Name = cust.ContactName,
                                            Company = cust.CompanyName,
                                            Country = cust.Country,
                                            Address = cust.Address,
                                            Entered = cust.Entered,
                                            Pk = cust.CustomerID
                                        });

            CustomerListView.DataSource = abbrCustList2;
            CustomerListView.DataBind();




            // *** Data Grid Binding Example //

            var custList = customer.GetCustomerList();
                                               
            // Filter the customer list to the page used
            custList = Pager.FilterIQueryable(custList);

            var abbrCustList = custList.Select(cust =>
                                        new
                                        {
                                            Name = cust.ContactName,
                                            Company = cust.CompanyName,
                                            Country = cust.Country
                                        });

            gdCustomers.DataSource = abbrCustList;  
            gdCustomers.DataBind();

        }
    }
}
