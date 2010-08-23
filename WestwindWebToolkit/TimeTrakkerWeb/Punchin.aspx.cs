using System;
using TimeTrakker;
using Westwind.BusinessFramework;
using Westwind.Utilities;
using Westwind.Web;

namespace TimeTrakkerWeb
{
    public partial class Punchin : TimeTrakkerBaseForm
    {
        /// <summary>
        /// Public entry object (public for data binding)
        /// </summary>
        public busEntry Entry = TimeTrakkerFactory.GetEntry();

        private busProject Project = TimeTrakkerFactory.GetProject();
        private busCustomer Customer = TimeTrakkerFactory.GetCustomer();      

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);            
            
            Westwind.Utilities.WebUtils.ForceReload();

            // *** Assign the AJAX callback handler (Handler) so 
            // *** a proxy can be generated
            this.Proxy.ClientProxyTargetType = typeof(Callbacks);            
            if (this.Proxy.IsCallback)
                return;

            object projectQuery = this.Project.GetOpenProjects();
            
            //DataTable table = this.Project.Converter.ToDataTable(projectQuery,"TProjects");

            // *** Load this list here for first time load and working without AJAX
            // *** The list will be updated from the client with custom data specific to customer
            this.lstProjects.DataSource = Project.Converter.ToDataReader(projectQuery);
            this.lstProjects.DataValueField = "Pk";
            this.lstProjects.DataTextField = "ProjectName";
            this.lstProjects.DataBind();

            object customerQuery = Customer.GetCustomerList();
            
            this.lstCustomers.DataSource = Customer.Converter.ToDataReader(customerQuery); 
            this.lstCustomers.DataTextField = "Company";
            this.lstCustomers.DataValueField = "Pk";
            this.lstCustomers.DataBind();

            // Embed client ids as serverVars.controlId values
            ScriptVariables scriptVars = new ScriptVariables(this, "serverVars");
            scriptVars.AddClientIds(this.Form,true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            if (this.Proxy.IsCallback)
                return;

            this.TimeTrakkerMaster.SubTitle = "Punch In New Entry";

            if (this.Entry.NewEntity() == null)
            {
                this.ErrorDisplay.ShowError("Unable to load new Entry:<br/>" + this.Entry.ErrorMessage);
                return;
            }

            if (!this.IsPostBack)
            {
                // *** Get the User's last settings
                busUser user = TimeTrakkerFactory.GetUser();

                //if (user.Load( usr => usr.Pk == this.TimeTrakkerMaster.UserPk) != null)
                if (user.Load(this.TimeTrakkerMaster.UserPk) != null)
                {
                    if (user.Entity.LastCustomer > 0)
                        this.Entry.Entity.CustomerPk = user.Entity.LastCustomer;
                    if (user.Entity.LastProject > 0)
                        this.Entry.Entity.ProjectPk = user.Entity.LastProject;
                }

                // *** Now bind it
                this.DataBinder.DataBind();
            }
        }


        protected void btnPunchIn_Click(object sender, EventArgs e)
        {
            // *** Start by unbinding the data from controls into Entity
            this.DataBinder.Unbind();

            // *** Manual fixup for the split date field
            DateTime PunchinTime = Entry.GetTimeFromStringValues(this.txtDateIn.Text, this.txtTimeIn.Text);
            if (PunchinTime <= App.MIN_DATE_VALUE)
            {
                this.DataBinder.BindingErrors.Add( new Westwind.Web.Controls.BindingError("Invalid date or time value", this.txtDateIn.ClientID));
                Entry.ValidationErrors.Add("Invalid date or time value", this.txtTimeIn.ClientID);
            }
            Entry.Entity.TimeIn = PunchinTime;

            // *** Validate for binding errors - and error out if we have any
            if (this.DataBinder.BindingErrors.Count > 0)
            {
                this.ErrorDisplay.ShowError(this.DataBinder.BindingErrors.ToHtml(), "Please correct the following:");
                return;
            }

            // *** Have to make sure we associate a user with this entry from list
            Entry.Entity.UserPk = this.TimeTrakkerMaster.UserPk;


            // *** Validate business rules
            if (!this.Entry.Validate())
            {
                foreach (ValidationError error in this.Entry.ValidationErrors)
                {                    
                    this.DataBinder.AddBindingError(error.Message,error.ControlID);
                }
                this.ErrorDisplay.ShowError(this.DataBinder.BindingErrors.ToHtml(), "Please correct the following:");
                return;
            }

      
            // *** Finally save the entity
            if (!this.Entry.Save())
                this.ErrorDisplay.ShowError("Couldn't save entry:<br/>" +
                                            this.Entry.ErrorMessage);
            else
            {
                this.ErrorDisplay.ShowMessage("Entry saved.");
                Response.AppendHeader("Refresh", "1;Url=default.aspx");

                // *** Remember last settings for Project and Customer for the user
                // *** NOTE: Entry.Entity.User is not available here because it's a NEW record
                //           so we explicitly load and save settings
                busUser User = TimeTrakkerFactory.GetUser();
                User.SaveUserPreferences(Entry.Entity.UserPk, Entry.Entity.CustomerPk, Entry.Entity.ProjectPk);
            }
        }
    }
}
