using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Web;
using System.IO;
using System.Text;

namespace Westwind.WebToolkit.Ajax
{
    public partial class AutoComplete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {    
            // assign type so we can generate a proxy
            this.Proxy.ClientProxyTargetType = typeof(AutoCompleteHandler);

            if (!this.IsPostBack)
                this.txtSymbol.Text = Request.QueryString["symbol"];
        }
    }
}
