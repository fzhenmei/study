using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Globalization;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Westwind.GlobalizationWeb.LocalizationAdmin
{
    public partial class ResourceTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ResourceManager resources = new ResourceManager("Westwind.GlobalizationWeb.App_GlobalResources.Resources", Assembly.GetExecutingAssembly());
            JavaScriptResourceHandler.RegisterJavaScriptGlobalResources(this, "globalRes", "resources");
            //JavaScriptResourceHandler.RegisterJavaScriptLocalResources(this, "localRes");

            JavaScriptResourceHandler.RegisterJavaScriptLocalResources(this, "localRes", CultureInfo.CurrentUICulture.IetfLanguageTag, "localizationadmin/localizationadmin.aspx", ResourceProviderTypes.DbResourceProvider, false);
        }
    }
}
