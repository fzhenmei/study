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
using System.Text;
using System.Collections.Generic;
using Westwind.Utilities;
using Westwind.Web;

namespace TimeTrakkerWeb
{
    public partial class BrowseEntries : TimeTrakkerBaseForm
    {
        
        protected busEntry Entry = TimeTrakkerFactory.GetEntry();
        
        /// <summary>
        /// Flag used alternating background in ListView
        /// </summary>
        protected bool AlternateFlag = true;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Let AjaxMethodCallback create a proxy            
            this.Proxy.ClientProxyTargetType = typeof(Callbacks);

            busCustomer customer = new busCustomer();
            
            IQueryable CustomerList = customer.GetCustomerList();
            
            this.lstCustomer.DataSource = CustomerList;
            this.lstCustomer.DataTextField = "Company";
            this.lstCustomer.DataValueField = "Pk";
            this.lstCustomer.DataBind();

            this.lstCustomer.Items.Insert(0,new ListItem("-- All Customers --", ""));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
              
            // *** Get the base query
            IQueryable<EntryEntity> entryQuery = this.Entry.GetEntries(this.TimeTrakkerMaster.UserPk);

            // *** Apply initial filter string if passed on query string
            string filter = Request.QueryString["Filter"];
            if (string.IsNullOrEmpty(filter))
                filter = this.lstFilter.SelectedValue;
            else
            {
                // *** Try to bind the filter - if invalid use default
                try { this.lstFilter.SelectedValue = filter; }
                catch { filter = this.lstFilter.SelectedValue;  }
            }

            // *** Apply the filter to the query dynamically - 
            //     NOTE: Sort of blurring lines between bus and UI
            if (filter == "OpenEntries")
                entryQuery = entryQuery.Where(en => !en.PunchedOut);
            else if (filter == "RecentEntries")
                entryQuery = entryQuery.Take(10);
            else if (filter == "RecentClosedEntries")
                entryQuery = entryQuery.Where(en => en.PunchedOut).Take(10);

            if (!string.IsNullOrEmpty(this.lstCustomer.SelectedValue))
            {
                int custPk = -1;
                int.TryParse(this.lstCustomer.SelectedValue, out custPk);

                entryQuery = entryQuery.Where(en => en.CustomerPk == custPk);
            }


            // *** Create an instanced list so we can get a count without querying database
            List<EntryEntity> entryList = entryQuery.ToList();

            this.divStatus.InnerHtml = entryList.Count.ToString() + " entries";
            

            // *** Note we'll bind an entity here so binding has access to
            // *** an entity object
            this.lstEntries.DataSource = entryList;  // binding entity List
            this.lstEntries.DataBind();
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
            sb.Append(TimeUtils.ShortDateString( entry.TimeIn,true ) );
                //entry.TimeIn.ToString("MMM. dd, yyyy") + " - " + entry.TimeIn.ToString("t").Replace(" ", "").ToLower());

            if (entry.TimeOut > App.MIN_DATE_VALUE)
            {                
                sb.Append( "<br /><span style='color:steelblue'>" + TimeUtils.FractionalHoursToString(entry.TotalHours,"{0}h:{1}min") + "</span>"  );
            }
            return sb.ToString();
        }


        /// <summary>
        /// Returns the URL for punching out the entry. Function because it's reused a bunch
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        protected string GetPunchOutUrl(int Id)
        {
            return this.ResolveUrl("~/punchout.aspx?id=" + Id.ToString());
        }
    }
}
