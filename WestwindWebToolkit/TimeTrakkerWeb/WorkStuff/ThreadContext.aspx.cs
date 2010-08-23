using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TimeTrakker;
using System.Threading;

namespace TimeTrakkerWeb.WorkStuff
{
    public partial class ThreadContext : System.Web.UI.Page
    {
        TimeTrakkerContext dbContext = new TimeTrakkerContext();
        IQueryable<CustomerEntity> CustomerList;
        CustomerEntity Customer;

        protected void Page_Load(object sender, EventArgs e)
        {
            

            CustomerList =
                from c in dbContext.CustomerEntities
                where c.Company.StartsWith("West")
                select c;

            this.Customer = CustomerList.First();

            Response.Write("Original Values:<br>" + this.Customer.Company + " " + this.Customer.Address + " " + Thread.CurrentThread.ManagedThreadId.ToString());

            this.Customer.Company = "West Wind Technologies " + DateTime.Now.ToLongTimeString(); 


            
            Thread thread = new Thread(new ThreadStart(this.AccessDataContextOnThread));
            thread.Start();

            Thread.Sleep(2000);
        }

        public void AccessDataContextOnThread()
        {
            this.Customer.Address = "33 Kaiea Place " + DateTime.Now.ToLongTimeString();
            this.dbContext.SubmitChanges();

            Response.Write("<hr>From Thread:<br>" + this.Customer.Company + " " + this.Customer.Address + Thread.CurrentThread.ManagedThreadId.ToString());

        }


    }
}
