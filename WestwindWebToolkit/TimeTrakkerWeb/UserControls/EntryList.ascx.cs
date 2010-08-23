using System;
using System.Collections;
using System.Linq;
using TimeTrakker;
using System.Collections.Generic;
using System.Text;
using Westwind.Utilities;

namespace TimeTrakkerWeb
{
    public partial class EntryList : System.Web.UI.UserControl
    {
     
        /// <summary>
        /// View data
        /// </summary>
        public Hashtable Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
        private Hashtable _Data = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            string renderMode = Data["RenderMode"] as string ?? "ProjectEntries";
            
            string filter = Data["Filter"] as string;

            // *** We have to update our filter display
            this.lstFilter.SelectedValue = filter; 

            if (renderMode == "ProjectEntries")
                this.LoadChildEntries(filter, (int) Data["ProjectPk"]);
        }


        protected void LoadChildEntries(string filter, int projectPk)
        {
            busEntry entry = new busEntry();

            // *** Get the base query
            IQueryable<EntryEntity> entryQuery = entry.GetEntriesByProject(50);

            // *** Apply the filter to the query dynamically - 
            //     NOTE: Sort of blurring lines between bus and UI
            if (filter == "OpenEntries")
                entryQuery = entryQuery.Where(en => !en.PunchedOut);
            else if (filter == "RecentEntries")
                entryQuery = entryQuery.Take(10);
            else if (filter == "RecentClosedEntries")
                entryQuery = entryQuery.Where(en => en.PunchedOut).Take(10);
            else if (filter == "Unbilled")
                entryQuery = entryQuery.Where(en => !en.Billed).Take(10);

            // *** Create an instanced list so we can get a count without re-querying database
            List<EntryEntity> entryList = entry.Converter.ToList(entryQuery);

            this.divListStatus.InnerHtml = entryList.Count.ToString() + " entries";

            // *** Note we'll bind an entity here so binding has access to
            // *** an entity object
            this.lstEntries.DataSource = entryList;  // binding entity List
            this.lstEntries.DataBind();
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
    }
}