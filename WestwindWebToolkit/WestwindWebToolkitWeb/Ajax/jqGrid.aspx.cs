using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Web;
using Westwind.WebToolkit.NorthwindCustomers;
using Westwind.Utilities;
using System.IO;
using Westwind.Web.JsonSerializers;
using Westwind.Web.Controls;

namespace Westwind.WebToolkit.Ajax
{
    public partial class jqGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        /// <summary>
        /// Retrieve a customer list for jqGrid.
        /// 
        /// This code sends back only data for the
        /// requested page (ie. 10 recors at a time or so)
        /// </summary>
        /// <returns></returns>
        [CallbackMethod]
        public object GetCustomerList()
        {            
            // jqGrid sends Page (active Page request) and Rows parameters
            int page = 1;
            int.TryParse(Request.QueryString["Page"] ?? "1", out page);

            int pageSize = 15;
            int.TryParse(Request.QueryString["Rows"] ?? "15", out pageSize);            
            
            NorthwindCustomersContext context = new NorthwindCustomersContext();
            var list = context.nw_Customers.OrderBy(cust => cust.CompanyName);
            int totalCount = list.Count();

            list = context.nw_Customers.OrderBy(cust => cust.CompanyName);
            decimal dc = totalCount / pageSize;
            int totalPages = Convert.ToInt32(dc);

            /// grab only requested data
            int offset = (page - 1 ) * pageSize;     
       
            // turn into List here so we can format values 
            // ith toString() (not supported in SQL Expression)
            List<nw_Customer> pagedList;
            if (offset > 0) 
                pagedList= list.Skip(offset).Take(pageSize).ToList();
            else
                pagedList = list.Take(pageSize).ToList();

            var custList = pagedList
                        .Select(cust => new
                        {
                            id = cust.CustomerID,
                            cell = new object[]
                                    { cust.ContactName, 
                                      cust.CompanyName,                                       
                                      cust.Entered.ToString("MMM dd, yyyy")
                                    }
                        }).ToList();
                                
            // format result for jqGrid
            var res = new {

                page= page.ToString(),
                total= totalPages.ToString(),
                records = totalCount.ToString(),
                rows=  custList
            };

            return res;
        }

        [CallbackMethod]
        public bool UpdateCustomerRow()
        {
            string id = Request.Params["id"];
            
            busCustomer customerRepository = new busCustomer();
            
            nw_Customer customer;
            if (string.IsNullOrEmpty(id))
                customer = customerRepository.NewEntity();
            else
            {
                customer = customerRepository.Load( cust=> cust.CustomerID == id );
                if (customer == null)
                    customer = customerRepository.NewEntity();
            }

            customer.ContactName = Request.Params["Name"];
            customer.CompanyName = Request.Params["CompanyName"];
            string qv = Request.Params["Entered"];
            if (!string.IsNullOrEmpty(qv))
                customer.Entered = (DateTime)ReflectionUtils.StringToTypedValue(qv, typeof(DateTime));

            if (!customerRepository.Save())
                throw new InvalidOperationException("Unable to save customer");

            return true;
        }
    }
}
