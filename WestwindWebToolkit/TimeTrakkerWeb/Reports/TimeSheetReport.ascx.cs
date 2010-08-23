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
using System.Collections.Generic;
using Westwind.Utilities;
using System.Text;

namespace TimeTrakkerWeb
{
    public partial class TimeSheetReport : System.Web.UI.UserControl, IReportView
    {

        protected TimesheetReportParameters Parameters;
        List<ReportEntryItem> entryList;
        //List<EntryEntity> entryList;

        private string lastProjectName = "";
        private decimal groupTotalHours = 0.00M;
        private decimal grandtotalHours = 0.00M;


        /// <summary>
        /// Required method that forces the data to be bound before rendering
        /// </summary>
        public void BindData(IQueryable query, object parameters)
        {
            this.Parameters = parameters as TimesheetReportParameters;

            var q = query as IQueryable<EntryEntity>;


            // denormalize the list so we minimize related queries
            IQueryable<ReportEntryItem>  res = from entry in q
                      select new ReportEntryItem
                      {
                          Entry = entry,
                          ProjectName = entry.ProjectEntity.ProjectName,
                          Company = entry.ProjectEntity.Customer.Company
                      };

            // in this case we want a List<T> so we can lazy load more data
            this.entryList = res.ToList(); 

            this.lstReport.DataSource = this.entryList;
            this.lstReport.DataBind();            
        }

                
        protected string RenderProjectGroupHeader(ReportEntryItem  repItem)
        {
            string output = "";
            EntryEntity entry = repItem.Entry;

            // if the project name has changed render a group as a <div> header
            if (repItem.ProjectName != lastProjectName)
            {
                lastProjectName = repItem.ProjectName;
                
                // *** New project add hours for first time
                this.groupTotalHours = entry.TotalHours;

                string html = string.Format(
@"<div class='groupheader'>
    <div style='float:right'><small>{0}</small></div>
    {1}
</div>
", repItem.Company, repItem.ProjectName);

                return html;
            }
            else
                this.groupTotalHours += entry.TotalHours;
            
            this.grandtotalHours += entry.TotalHours;

            return output;
        }

        protected string RenderProjectFooter(object DataContainer)
        {            
            ListViewDataItem lv = DataContainer as ListViewDataItem;
            ReportEntryItem repItem = lv.DataItem as ReportEntryItem;

            // read ahead to the next entity and see if the project has changed
            if (lv.DataItemIndex + 1 < this.entryList.Count)
            {
                int next = lv.DataItemIndex + 1;
                ReportEntryItem nextItem = this.entryList[next];
                if (nextItem.ProjectName == repItem.ProjectName)
                    return "";
            }

            StringBuilder sb = new StringBuilder(300);
            sb.AppendLine("<div style='text-align: right'><b><span style='color: maroon;'>" + repItem.ProjectName + "</span>: ");
            sb.AppendLine(TimeUtils.FractionalHoursToString(this.groupTotalHours, "{0}h {1}min"));
            sb.AppendLine(" (" + this.groupTotalHours.ToString("n2") + ")</b></div>");

            return sb.ToString();
        }

       
    }

    public class ReportEntryItem
    {
        public EntryEntity Entry {get; set;}
        public string Company {get;set;}
        public string ProjectName { get; set; }
    }
}