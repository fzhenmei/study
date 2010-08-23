using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Utilities.Logging;
using Westwind.Utilities;
using System.Data;
using Westwind.Web;
using Westwind.Web.JsonSerializers;

namespace Westwind.WebToolkit.Admin
{
    public partial class WebRequestLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // embed log data filter object into page
            ScriptVariables scriptVars = new ScriptVariables(this, "scriptVars");
            scriptVars.Add("LogDataFilter",new LogDataFilter());
            
            
            this.lstLogType.DataSource = ReflectionUtils.GetEnumList(typeof(ErrorLevels));
            this.lstLogType.DataTextField = "Key";
            this.lstLogType.DataValueField = "Value";
            this.lstLogType.DataBind();

            this.lstLogType.SelectedValue = ErrorLevels.All.ToString();

            if (LogManager.Current == null)
            {
                this.ErrorDisplay.ShowError("Logging is currently not enabled.<br />Please make sure you configure the LogManager on Application Startup.");
                return;
            }

        }

        protected void btnPurge_Click(object sender, EventArgs e)
        {
            if (LogManager.Current.Clear())
                this.ErrorDisplay.ShowMessage("Log cleared");
            else
                this.ErrorDisplay.ShowError("Log was not cleared");
        }

        
        protected void btnCauseError_Click(object sender, EventArgs e)
        {
            WebLogEntry entry = null;
            entry.IpAddress = "";  // cause exception
        }


        [CallbackMethod]
        public object  GetListData(LogDataFilter filter)
        {           
            ILogAdapter adapter = LogManager.Current.LogAdapter;
            IDataReader reader =  adapter.GetEntries(filter.ErrorLevel, filter.Count,
                                                    filter.StartDate, filter.EndDate,
                                                    "Id,Url,QueryString,RequestDuration,Message,Entered,ErrorLevel,ErrorType");

            return reader;            
        }


        [CallbackMethod]
        public WebLogEntry GetLogItem(int id)
        {           
            ILogAdapter adapter = LogManager.Current.LogAdapter;
            WebLogEntry entry = adapter.GetEntry(id);

            if (entry.ErrorLevel == ErrorLevels.Error)
            {
                entry.Details = "<pre>" + entry.Details + "</pre>";
                //StringUtils.DisplayMemo(entry.Details);
                entry.StackTrace = "<pre>" + entry.StackTrace + "</pre>"; // StringUtils.DisplayMemo(entry.StackTrace);
            }
            
            return entry;
        }

    }

    public class LogDataFilter
    {
        public ErrorLevels ErrorLevel = ErrorLevels.All;
        public int Count = 100;
        public DateTime StartDate = DateTime.UtcNow.AddDays(-30).Date;
        public DateTime EndDate = DateTime.UtcNow.AddDays(1).Date;

        public int Page = 1;
        public int PageCount = 1;
                

    }
}
