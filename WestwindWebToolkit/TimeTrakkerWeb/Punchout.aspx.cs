using System;
using TimeTrakker;
using Westwind.BusinessFramework;
using Westwind.Web.Controls;
using System.Linq;
using System.Data.Linq;
using Westwind.Web;
using Westwind.Utilities;

namespace TimeTrakkerWeb
{
    public partial class Punchout : TimeTrakkerBaseForm
    {
        /// <summary>
        /// Public entry object (public for data binding in medium trust)
        /// </summary>
        public busEntry Entry = TimeTrakkerFactory.GetEntry();        
        private busProject Project = TimeTrakkerFactory.GetProject();
        private busCustomer Customer = TimeTrakkerFactory.GetCustomer();
        
        private int EntryId = -1;        
        private string Action = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ScriptVariables serverVars = new ScriptVariables(this, "serverVars");
            serverVars.AddClientIds(this.Form,true);
            
            this.Action = Request.QueryString["action"];
            if (!string.IsNullOrEmpty(Action) && Action.ToLower() == "delete")
                // *** Handles deletion and redirection and won't fire past here
                this.DeleteEntry();            
            
            // *** Route AJAX callbacks to Callbacks.ashx handler
            this.ProxyUpdateProjects.ClientProxyTargetType = typeof(Callbacks);
            if (this.Proxy.IsCallback)
                return;

            IQueryable<CustomerEntity> customerQuery = Customer.GetCustomerList();
            
            // *** Example of 'post filtering' in the front end
            // *** Filter the query down to just what we need
            IQueryable query = customerQuery
                              .Select( cust => new 
                                               { 
                                                 Company=cust.Company, 
                                                 Pk=cust.Pk 
                                               } );

            this.lstCustomers.DataSource = Customer.Converter.ToDataReader(query); 
            this.lstCustomers.DataTextField = "Company";
            this.lstCustomers.DataValueField = "Pk";
            this.lstCustomers.DataBind();

            // *** Here we're retrieving a 'focused' list right out of the
            // *** the business layer
            IQueryable<ProjectListResult> projectQuery = this.Project.GetOpenProjects();

            //DataTable table = this.Project.Converter.ToDataTable(projectQuery,"TProjects");
            this.lstProjects.DataSource = Project.Converter.ToDataReader(projectQuery);
            this.lstProjects.DataValueField = "Pk";
            this.lstProjects.DataTextField = "ProjectName";
            this.lstProjects.DataBind();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            

            if (this.Proxy.IsCallback)
                return;

            //if (this.Entry.Load( ent => ent.Pk == this.EntryId ) == null)

            // *** Load the entry specified always
            if (!int.TryParse(Request.QueryString["id"], out this.EntryId) ||                
                this.Entry.Load(this.EntryId) == null)
            {
                MessageDisplay.DisplayMessage("Invalid entry id specified.",
                          "You've selected an invalid entry by its id.",
                          "~/default.aspx",3);
                return;
            }

            // *** Databind initially using DataBinder
            if (!this.IsPostBack)
            {                                   
                 // *** Fix up defaults
                 if (this.Entry.Entity.TimeOut == null || this.Entry.Entity.TimeOut <= App.MIN_DATE_VALUE)
                     this.Entry.Entity.TimeOut = DateTime.Now;

                 // *** Always update time/date totals 
                 this.Entry.RoundTimeValues();                     

                 if (this.Entry.Entity.Rate == 0)
                 {
                     if (Customer.Load(this.Entry.Entity.CustomerPk) != null)                    
                         Entry.Entity.Rate = Customer.Entity.BillingRate;                     
                 }
                 this.Entry.CalculateItemTotals();

                // *** Now bind the data to controls
                 this.DataBinder.DataBind();

                // *** Special case: Manually bind the hours 
                 this.lblTotalHours.Text = TimeUtils.FractionalHoursToString(this.Entry.Entity.TotalHours, "{0}h {1}min");
            }
            else
                this.UpdateTotalsOnEntry();
        }


        protected void btnPunchOut_Click(object sender, EventArgs e)
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

            // *** Remember last settings for Project and Customer for the user            
            Entry.Entity.UserEntity.LastProject = this.Entry.Entity.ProjectPk;
            Entry.Entity.UserEntity.LastCustomer = this.Entry.Entity.CustomerPk;

            bool saveResult = false;
            if (sender == this.btnLeaveOpen)
            {
                this.Entry.Entity.TimeOut = App.MIN_DATE_VALUE;
                this.Entry.Entity.PunchedOut = false;
                saveResult = this.Entry.Save();
            }
            else
                saveResult = this.Entry.PunchOut();
                 
            // *** Finally save the entity
            if (!saveResult)
                this.ErrorDisplay.ShowError("Couldn't save entry:<br/>" +
                                            this.Entry.ErrorMessage);
            else
            {            
                // *** Show confirmation
                this.ErrorDisplay.ShowMessage("Entry saved.");

                // *** Cause the page to move back to default after 2 seconds
                Response.AppendHeader("Refresh", "2; Url=default.aspx" );
            }

            this.chkPunchedout.Checked = this.Entry.Entity.PunchedOut;
        }

        ///// <summary>
        ///// Updates the totals of an order based on the forms field data.
        ///// </summary>
        protected void UpdateTotalsOnEntry()
        {
            this.Entry.Entity.TimeIn = this.Entry.GetTimeFromStringValues(this.txtDateIn.Text, this.txtTimeIn.Text);
            this.Entry.Entity.TimeOut = this.Entry.GetTimeFromStringValues(this.txtDateOut.Text, this.txtTimeOut.Text);

            decimal BillRate = 0;
            decimal.TryParse(this.txtRate.Text, out BillRate);
            this.Entry.Entity.Rate = BillRate;

            this.Entry.CalculateItemTotals();

            // *** Rebind those items explicitly - don't do on callback
            if (!this.Proxy.IsCallback)
            {
                // Manually rebind these items
                this.DataBinder.GetDataBindingItem(this.lblItemTotal).DataBind();
                this.DataBinder.GetDataBindingItem(this.lblTotalHours).DataBind();
            }
        }        

        
        /// <summary>
        /// Deletes an entry based of a query string value passed in
        /// </summary>
        protected void DeleteEntry()
        { 
            string id = Request.QueryString["id"];
            if ( string.IsNullOrEmpty(id) )
               MessageDisplay.DisplayMessage("Unable to delete Entry",
                                             "Entry was not deleted because an invalid Id was passed",
                                             Request.UrlReferrer.ToString(),5);
            
            int pk = -1;
            int.TryParse(id,out pk);

            //this.Entry.Load(pk);
            //if (!this.Entry.Delete()) 

            if (this.Entry.Delete(pk))
                Response.Redirect(Request.UrlReferrer.ToString());

            MessageDisplay.DisplayMessage("Unable to delete Entry",
                                          "Entry was not deleted: " + this.Entry.ErrorMessage,
                                          Request.UrlReferrer.ToString(), 5);                
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int pk = 0;
            int.TryParse(this.txtEntryId.Value, out pk);

            if (!this.Entry.Delete(pk))
            {
                this.ErrorDisplay.ShowError("Entry couldn't be deleted");
                return;
            }

            Response.Redirect("~/default.aspx");
        }  

        /// <summary>
        /// Ajax Callback method from the page that handles updating 
        /// the calculation of time totals.
        /// </summary>
        /// <param name="Pk"></param>
        /// <returns></returns>
        [CallbackMethod]
        public object UpdateTotals(string Pk)
        {
            int pk = 0;
            int.TryParse(Pk, out pk);

            // *** Load Entity into Entry.Entity
            this.Entry.Load(pk);

            // *** Use form controls/formvars to read
            // *** this information and update same
            // *** as a regular postback page
            this.UpdateTotalsOnEntry();

            this.Entry.Entity.TimeIn = 
                TimeUtils.RoundDateToMinuteInterval(this.Entry.Entity.TimeIn, 
                                                    App.Configuration.MinimumMinuteInterval, 
                                                    RoundingDirection.RoundUp);
            this.Entry.Entity.TimeOut =
                TimeUtils.RoundDateToMinuteInterval(this.Entry.Entity.TimeOut,
                                                    App.Configuration.MinimumMinuteInterval,
                                                    RoundingDirection.RoundUp);
            this.Entry.CalculateItemTotals();

            // *** Grab the display format from the databinder for timein control
            string timeFormat = this.DataBinder.GetDataBindingItem(this.txtTimeIn).DisplayFormat
                                     .Replace("{0:","")
                                     .Replace("}","");


            // *** Return a projected result object
            return new { TotalHours = TimeUtils.FractionalHoursToString( Entry.Entity.TotalHours,"{0}h {1}min"), 
                         ItemTotal = (decimal) Entry.Entity.ItemTotal,
                         TimeIn = Entry.Entity.TimeIn.ToString(timeFormat).ToLower(),
                         TimeOut = Entry.Entity.TimeOut.ToString(timeFormat).ToLower() };
        }      
    }

}
