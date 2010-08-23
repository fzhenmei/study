using System;

namespace Westwind.WebToolkit.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((WestWindWebToolkitMaster)Master).SubTitle = "Adminstration Home Page";

        }
    }
}
