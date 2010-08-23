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
using Westwind.Utilities;
using Westwind.BusinessFramework;

namespace TimeTrakkerWeb.WorkStuff
{
    public partial class DisconnectedUpdates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            busCustomer customer = new busCustomer();
            
            CustomerEntity cust = customer.Load(1);

            Response.Write(cust.FirstName);
            cust.FirstName = DateTime.Now.ToLocalTime().ToString();
            
            customer.Dispose();

            customer = new busCustomer();
            customer.Options.TrackingMode = TrackingModes.Disconnected;

            
            //string ser;
            //wwUtils.SerializeObject(cust,out ser);
            //cust = wwUtils.DeSerializeObject(ser, typeof(CustomerEntity)) as CustomerEntity;

            cust = new CustomerEntity();
            
            cust.Company = "West Wind";
            cust.LastName = "bogus";
            cust.FirstName = "bob";
            cust.Email = "joe@bob.com";

            if (customer.Validate(cust))
                customer.Save(cust);

            Response.Write("Error: " + customer.ErrorMessage);

            return;
        }
    }
}
