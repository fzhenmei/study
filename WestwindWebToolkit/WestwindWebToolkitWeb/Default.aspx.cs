using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Westwind.Utilities;

namespace Westwind.WebToolkit
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((WestWindWebToolkitMaster)Master).SubTitle = "Web Toolkit Sample Home";
            
        }
    }
}
