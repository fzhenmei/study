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
using Westwind.Utilities;

namespace TimeTrakkerWeb
{

    /// <summary>
    /// QueryString Options:
    /// AutoRedirect=True   -   forces to PunchIn screen if no entries are open 
    /// </summary>
    public partial class OpenEntries : TimeTrakkerBaseForm
    {
        busEntry entry = TimeTrakkerFactory.GetEntry();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            // *** Make sure this page doesn't cache
            Westwind.Utilities.WebUtils.ForceReload();
            
            // *** Set up the base query
            IQueryable<EntryEntity> entries = this.entry.GetOpenEntries(this.TimeTrakkerMaster.UserPk);

            int count = entries.Count();
            if (count == 1)
            {                
                int? Pk = entries.Select(en => en.Pk).FirstOrDefault();
                if (Pk == null)
                    Response.Redirect("~/Default.aspx");
                
                Response.Redirect("~/punchout.aspx?id=" + Pk.ToString());
            }
            if (count == 0)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["AutoRedirect"]))
                    Response.Redirect("~/punchin.aspx");
                     
                this.ErrorDisplay.ShowMessage("There are no open entries to punch out.");                
            }


            // *** More than one open entry - show the list of items
            // *** Assign the data source - note we can filter the data here!
            this.lstEntries.DataSource = this.entry.Converter.ToDataReader(entries.Select(en => new { en.Pk, en.Title, en.TimeIn }));
            this.lstEntries.DataBind();
        }

        protected string GetPunchOutUrl(int Id)
        {
            return this.ResolveUrl("~/punchout.aspx?id=" + Id.ToString());
        }

    }
}
