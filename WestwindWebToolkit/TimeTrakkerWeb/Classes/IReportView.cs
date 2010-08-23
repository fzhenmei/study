using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace TimeTrakkerWeb
{
    /// <summary>
    /// Interface used to describe report views
    /// </summary>
    public interface IReportView
    {        
        /// <summary>
        /// Used to pass data in IQueryable format to the 
        /// report. Parameters is an optional parameter
        /// that can be passed that is report specific
        /// </summary>
        /// <param name="query"></param>
        /// <param name="Parameters"></param>
        void BindData(IQueryable query, object parameters);
    }
}
