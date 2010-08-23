using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Web;
using System.Collections;
using Westwind.Utilities;
using System.Reflection;
using System.Threading;

namespace Westwind.WebToolkit.Ajax
{
    public partial class AjaxMethodCallbacks : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);            

            // Create ScriptVariables container that allows pushing
            // variables into the page from server code.
            // Create client object "serverVars"
            ScriptVariables serverVars = new ScriptVariables(this,"serverVars");
            
            // Make all ClientIds show as serverVars.controlId where Id is the postfix
            serverVars.AddClientIds(this.Form, true);

            // Assign subtitle to the page
            ((WestWindWebToolkitMaster)this.Master).SubTitle = "AjaxMethodCallback Samples";
        }

        [CallbackMethod]
        public string HelloWorld(string name)
        {
            return string.Format("Hello {0}. Time is {1:hh:mm:ss}", name, DateTime.Now);
        }

        [CallbackMethod]
        public decimal AddNumbers(decimal num1, decimal num2)
        {
            return num1 + num2;
        }

        [CallbackMethod]
        public StockQuote GetStockQuote(string symbol)
        {
            StockServer stockServer = new StockServer();
            return stockServer.GetStockQuote(symbol);            
        }

        /// <summary>
        /// This request returns a list of customer objects
        /// </summary>
        /// <returns></returns>
        [CallbackMethod]
        public List<Customer> GetCustomers()
        {
            TimeTrakkerContext context = new TimeTrakkerContext();

            var custList =
                from cust in context.Customers
                orderby cust.Company
                where cust.Company != null || cust.Company != string.Empty
                select cust;

                // you can also return an anonymous type - it will properly encode into JSON
                //select new { Company = cust.Company, Pk = cust.Pk };

            return custList.ToList();
        }

        /// <summary>
        /// Retrieves an individual customer by customer id
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        [CallbackMethod]
        public Customer GetCustomer(int pk)
        {
            TimeTrakkerContext context = new TimeTrakkerContext();
            
            var customer =
                from cust in context.Customers
                where cust.Pk == pk
                select cust;

            return customer.FirstOrDefault();
        }

        [CallbackMethod]
        public bool SaveCustomer(Customer customer)
        {
            TimeTrakkerContext context = new TimeTrakkerContext();
            int pk = customer.Pk;

            Customer existingCustomer = null;
            if (pk < 1)
               existingCustomer = new Customer();
            else
               existingCustomer = (from cust in context.Customers where cust.Pk == pk select cust).FirstOrDefault();

            if (existingCustomer == null)
                throw new ArgumentException("Invalid customer information passed.");

            DataUtils.CopyObjectData(customer,existingCustomer,
                                     "Pk,tversion",
                                     BindingFlags.Public | BindingFlags.Instance);

            context.SubmitChanges();

            // if we made it with exceptions we're good. Otherwise exception is marshalled to client
            return true;
        }

        [CallbackMethod]
        public bool LongRunning(int waitMilliseconds)
        {
            Thread.Sleep(waitMilliseconds);
            return true;
        }
        [CallbackMethod]
        public bool ThrowException()
        {
            throw new InvalidOperationException("This server method fired an exception on purpose!");
        }


    }
}
