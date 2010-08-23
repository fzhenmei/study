using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Westwind.WebToolkit
{

    public partial class jQueryDatePicker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Assign subtitle to the page
            ((WestWindWebToolkitMaster)this.Master).SubTitle = "jQuery Date Picker";

            if (!this.IsPostBack)
            {
                // Set initial date values
                this.txtAutoPopupDate.SelectedDate = DateTime.Now;
                this.txtImageButton.SelectedDate = DateTime.Now.AddDays(-30);
                this.txtPlainButton.SelectedDate = DateTime.Now.AddYears(-1);
            }

            // Set date range for the inline control 
            this.txtInline.MaxDate = DateTime.Now.AddDays(30);
            this.txtInline.MinDate = DateTime.Now.AddDays(-30);

        }



    }
}