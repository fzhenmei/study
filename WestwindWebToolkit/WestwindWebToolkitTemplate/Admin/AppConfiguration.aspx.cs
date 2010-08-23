using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Utilities.Logging;
using Westwind.Utilities;
using $safeprojectname$;

namespace Westwind.WebToolkit.Admin
{
    public partial class AppConfiguration : System.Web.UI.Page
    {
        // *** NOTE: You'll have to change this type to match your actual application
        //           configuration class in case you use a different name
        public ApplicationConfiguration Configuration;
        public LogManagerConfiguration LogConfiguration;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.txtLogModes.DataSource = ReflectionUtils.GetEnumList(typeof(LogAdapterTypes));
            this.txtLogModes.DataTextField = "Value";
            this.txtLogModes.DataValueField = "Key";
            this.txtLogModes.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Can't bind to a static property so assign to property
            this.Configuration = App.Configuration;
            this.LogConfiguration = LogManagerConfiguration.Current;

           
            if (!this.IsPostBack)
                this.DataBinder.DataBind();

#if (DEMOMODE)            
            this.txtConnectionString.TextMode = TextBoxMode.Password;
            this.txtMailUsername.TextMode = TextBoxMode.Password;
#endif
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

#if DEMOMODE
            this.ErrorDisplay.ShowMessage("Sorry! This application is in Demo Mode.<br/>Changes can't be saved.");
            return;
#endif

            // unbind control values back into config object
            this.DataBinder.Unbind();

            // check for binding errors
            if (this.DataBinder.BindingErrors.Count > 0)
            {
                this.ErrorDisplay.ShowError(this.DataBinder.BindingErrors.ToHtml(),
                                            "Please correct the following:");
                return;
            }


            // Save configuration to .config file 
            // NOTE: if you get a failure make sure you have permissions to write
            //       if no permissions you will have to manually write data into config.
            if (!this.Configuration.Write())
            {
                this.ErrorDisplay.ShowError(this.Configuration.ErrorMessage);
                return;
            }
            if (!this.LogConfiguration.Write())
            {
                this.ErrorDisplay.ShowError("Couldn't write log settings: " + this.LogConfiguration.ErrorMessage);
                return;
            }

            this.ErrorDisplay.ShowMessage("Configuration values have been saved.");
            
        }
        

    }
}
