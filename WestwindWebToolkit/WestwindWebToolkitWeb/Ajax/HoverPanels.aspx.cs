using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Westwind.Web;
using Westwind.Utilities;
using Westwind.WebToolkit.NorthwindCustomers;

namespace Westwind.WebToolkit
{
    public partial class HoverPanels : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // In a callback route to a special handler for pipeline events
            // HoverPanels are handled after Load but before PreRender
            if (this.lineItemPanel.IsCallback)
                this.HandleLineItemsCallback();


            // Assign subtitle to the page
            ((WestWindWebToolkitMaster)this.Master).SubTitle = "HoverPanel Samples";


            // ScriptVariables allows you to embed Server variables to JavaScript client code
            // It creates an object with the name specified and you can add properties/values.
            // Here we create an object with all 'normalized' ClientIds accessible in ASPX or .js files 
            // to simplify MasterPage naming (ie. scriptVars.VarNameId)
            ScriptVariables scriptVars = new ScriptVariables(this, "scriptVars");
            scriptVars.AddClientIds(this.Form,true);
            
            // Invoice Gridview Databinding from business object
            busInvoice invoice = new busInvoice();            
            this.gdInvoiceList.DataSource = invoice.GetLastInvoices(45); 
            this.gdInvoiceList.DataBind();
        }


        /// <summary>
        /// Display invoice lineitems by returning an HTML fragment.
        /// This is a code based approach, but you can just as easily
        /// render controls and return rendered HTML.
        /// </summary>
        private void HandleLineItemsCallback()
        {
            int? orderId = WebUtils.GetParamsInt("OrderId");
            if (orderId == null)
                Response.End(); // empty response

            busInvoice invoice = new busInvoice();

            if (invoice.Load(orderId) == null)
            {
                Response.Write("<div class='errordisplay'>Invalid Order Id '" + orderId.Value.ToString("d") + "' - can't display detail data</div>");
                Response.End();
            }

            Response.Write(invoice.HtmlLineItems());
            Response.End();            
        }


        protected void gdInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gdInvoiceList.PageIndex = e.NewPageIndex;
            this.gdInvoiceList.DataBind();
        }

    }
}
