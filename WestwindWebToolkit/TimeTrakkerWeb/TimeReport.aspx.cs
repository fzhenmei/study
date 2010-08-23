using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using TimeTrakker;
using System.IO;
using System.Collections.Generic;
using Westwind.Web.Controls;
using Westwind.Web;
using Westwind.Utilities;

namespace TimeTrakkerWeb
{
    public partial class TimeReport : System.Web.UI.Page
    {
        protected busCustomer Customer = TimeTrakkerFactory.GetCustomer();
        protected busEntry Entry = TimeTrakkerFactory.GetEntry();

        public TimesheetReportParameters ReportParameters = new TimesheetReportParameters();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ScriptVariables scriptVars = new ScriptVariables();
            scriptVars.AddClientIds(this.Form, true);           


            IQueryable<CustomerEntity> customers = Customer.GetCustomerList();

            // *** Filter the query to only exactly the field names we need
            IQueryable custlist = customers.Select( c=> new { Company = c.Company, Pk = c.Pk } );        

            this.lstCustomers.DataSource = Customer.Converter.ToDataReader(custlist);
            this.lstCustomers.DataTextField = "Company";
            this.lstCustomers.DataValueField = "Pk";
            this.lstCustomers.DataBind();

            if (!this.IsPostBack)
            {
                foreach (ListItem item in this.lstCustomers.Items)
                {
                    item.Selected = true;
                }
                this.DataBinder.DataBind();
                this.chkSelectAll.Checked = true;
            }

            //this.ErrorDisplay.ShowError(Customer.ErrorMessage);            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.ProcessReport();
        }

        protected void ProcessReport()
        {            
            this.ReportParameters.Companies.Clear();

            foreach (ListItem item in this.lstCustomers.Items)
            {
                if (item.Selected)
                    this.ReportParameters.Companies.Add(int.Parse(item.Value));
            }

            this.DataBinder.Unbind();

            if (this.DataBinder.BindingErrors.Count  > 0)
            {
                this.ErrorDisplay.ShowError(this.DataBinder.BindingErrors.ToHtml(), "Please correct the following");
                return;
            }

            IQueryable<EntryEntity> entries = this.Entry.GetTimeSheetByClient(this.ReportParameters);

            //TODO - Under construction. Need to figure out how to present the Report in seperate window

            // *** We'll dynamically load the ReportView User Control and bind it then render
            TimeSheetReport rep = this.LoadControl("~/Reports/TimeSheetReport.ascx") as TimeSheetReport;
            rep.BindData(entries, this.ReportParameters);

            // Render the control into a TextWriter
            StringWriter writer = new StringWriter();
            HtmlTextWriter html = new HtmlTextWriter(writer);
            rep.RenderControl(html);


            if (this.ReportParameters.MarkAsBilled)
            {
                this.Entry.MarkAsBilled( entries ) ;
            }
            if (this.ReportParameters.UnmarkAsBilled)
            {
                this.Entry.UnMarkAsBilled(entries);
            }

            // Dump into the HTTP stream
            Response.Clear();
            Response.Write(writer.ToString());
            Response.End();
        }


    }

}
