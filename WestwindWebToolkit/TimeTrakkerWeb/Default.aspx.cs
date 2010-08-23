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
using Westwind.Web.Controls;
using System.Text;

namespace TimeTrakkerWeb
{
    public partial class Default : TimeTrakkerBaseForm
    {
        protected override void OnLoad(EventArgs e)
        {            
            if (this.User.Identity.IsAuthenticated)
                this.divMenuHeader.InnerText = "Welcome " + this.TimeTrakkerMaster.Username;
        }
    }
}
