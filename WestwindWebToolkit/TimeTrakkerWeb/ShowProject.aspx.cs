using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using TimeTrakker;
using Westwind.BusinessFramework;
using System.Text;
using System.Collections.Generic;
using Westwind.Web.Controls;
using System.IO;
using Westwind.Utilities;
using Westwind.Web;

namespace TimeTrakkerWeb
{
    public partial class ShowProject : TimeTrakkerBaseForm
    {
        public busProject Project = TimeTrakkerFactory.GetProject();
        public busCustomer Customer = TimeTrakkerFactory.GetCustomer();

        public bool AlternateFlag = false;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            // *** Callbacks.ashx for AJAX callbacks
            this.Proxy.ClientProxyTargetType = typeof(Callbacks);
            
            IQueryable<CustomerEntity> customers = Customer.GetCustomerList();
            
            this.txtCustomerPk.DataSource = this.Customer.Converter.ToDataReader(customers.Select(c => new { c.Pk, c.Company }));
            this.txtCustomerPk.DataTextField = "Company";
            this.txtCustomerPk.DataValueField = "Pk";
            this.txtCustomerPk.DataBind();

        }

        protected override void OnLoad(EventArgs e)
        {            
            base.OnLoad(e);

            if (Request.QueryString["View"] == "Small")
                this.TimeTrakkerMaster.HideMasterContent();

            string id = Request.QueryString["id"];

            if (string.IsNullOrEmpty(id))
            {
                this.Project.NewEntity();
                this.btnDelete.Visible = false;
            }
            else
            {
                int pk = -1;
                int.TryParse(id, out pk);
                if (this.Project.Load(pk) == null)
                {
                    MessageDisplay.DisplayMessage("Invalid project specified.", "Please make sure you select a valid project selection.");
                    return;
                }
            }

            if (!this.IsPostBack)
                this.DataBinder.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // *** Create a client object that embeds these values
            ScriptVariables scriptVars = new ScriptVariables(this,"serverVars");
            
            // *** Variables to embed
            scriptVars.Add("projectPk", this.Project.Entity.Pk);

            // *** Don't render the children on load
            //this.LoadChildEntries(null);
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

            if (!this.Project.Validate())
            {
                foreach (ValidationError error in this.Project.ValidationErrors)
                {
                    this.DataBinder.AddBindingError(error.Message, error.ControlID);                   
                }
                this.ErrorDisplay.ShowError(this.DataBinder.BindingErrors.ToHtml(),
                            "Please correct the following:");
                return;
            }

            if (!this.Project.Save())
            {
                this.ErrorDisplay.ShowError(this.Project.ErrorMessage);
                return;
            }

            this.ErrorDisplay.ShowMessage("Project has been saved.");

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            object Id = Request.QueryString["id"];
            if (Id == null)
                return;

            int Pk = 0;
            int.TryParse(Id as string, out Pk);

            if (!this.Project.Delete(Pk))
                this.ErrorDisplay.ShowError("Failed to delete project");
            else
                // *** Display New Project page
                Response.Redirect("showProject.aspx");
        }

        //protected void LoadChildEntries(string filter)
        //{
        //    busEntry entry = new busEntry();

        //    // *** Get the base query
        //    IQueryable<EntryEntity> entryQuery = entry.GetEntriesByProject(this.Project.Entity.Pk);

        //    // *** Apply initial filter string if passed on query string
        //    if (string.IsNullOrEmpty(filter))
        //    {
        //        filter = Request.QueryString["Filter"];
        //        if (string.IsNullOrEmpty(filter))
        //            filter = this.lstFilter.SelectedValue;
        //        else
        //        {
        //            // *** Try to bind the filter - if invalid use default
        //            try { this.lstFilter.SelectedValue = filter; }
        //            catch { filter = this.lstFilter.SelectedValue; }
        //        }
        //    }

        //    // *** Apply the filter to the query dynamically - 
        //    //     NOTE: Sort of blurring lines between bus and UI
        //    if (filter == "OpenEntries")
        //        entryQuery = entryQuery.Where(en => !en.PunchedOut);
        //    else if (filter == "RecentEntries")
        //        entryQuery = entryQuery.Take(10);
        //    else if (filter == "RecentClosedEntries")
        //        entryQuery = entryQuery.Where(en => en.PunchedOut).Take(10);
        //    else if (filter == "Unbilled")
        //        entryQuery = entryQuery.Where(en => !en.Billed).Take(10);

        //    // *** Create an instanced list so we can get a count without re-querying database
        //    List<EntryEntity> entryList = entry.Converter.ToList(entryQuery);

        //    this.divListStatus.InnerHtml = entryList.Count.ToString() + " entries";

        //    // *** Note we'll bind an entity here so binding has access to
        //    // *** an entity object
        //    this.lstEntries.DataSource = entryList;  // binding entity List
        //    this.lstEntries.DataBind();
        //}

        #region ListView display functions
        /// <summary>
        /// Returns the URL for punching out the entry. Function because it's reused a bunch
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        protected string GetPunchOutUrl(int Id)
        {
            return this.ResolveUrl("~/punchout.aspx?id=" + Id.ToString());
        }


        /// <summary>
        /// Returns the right side of the list
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected string GetTimeDetail(EntryEntity entry)
        {
            if (entry == null)
                return "n/a";

            StringBuilder sb = new StringBuilder(200);
            sb.Append(entry.TimeIn.ToString("MMM. dd, yyyy") + " - " + entry.TimeIn.ToString("t").Replace(" ", "").ToLower());

            if (entry.TimeOut > App.MIN_DATE_VALUE)
            {
                sb.Append("<br /><span style='color:steelblue'>" + TimeUtils.FractionalHoursToString(entry.TotalHours, "{0}h:{1}min") + "</span>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Depending on open or closed returns the appropriate icon for list img
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected string GetIconClass(EntryEntity entry)
        {
            if (entry.PunchedOut)
                return "punchedoutimg";

            return "openentryimg";
        }
#endregion

        

        #region Page Callbacks
        //[CallbackMethod]
        //public object ShowEntries()
        //{
        //    // *** Load data and bind list control
        //    string filter = this.lstFilter.SelectedValue;
        //    this.LoadChildEntries(filter);

        //    // *** Render the list view
        //    string html = wwWebUtils.RenderControl(this.lstEntries);
        //    string statusHtml = this.divListStatus.InnerText;

        //    return new { listHtml=html,  statusHtml=statusHtml };
        //}
        #endregion

    }
}
