using System;
using System.Data;
using System.Web;
using System.Linq;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.Linq;
using TimeTrakker;
using Westwind.Web.Controls;
using Westwind.Utilities;
using System.Collections.Generic;
using Westwind.Web;
using System.Security;

namespace TimeTrakkerWeb
{
    /// <summary>
    /// Class that handles common Ajax Callback requests from pages
    /// </summary>    
    public class Callbacks : Westwind.Web.CallbackHandler
    {

        [CallbackMethod]
        public object GetArray()
        {
            busProject project = TimeTrakkerFactory.GetProject();
            return project.GetProjectsForCustomer(1);
        }

        /// <summary>
        /// Returns a list of active projects for a given customer.
        /// Returned as a list of entities with Pk, ProjectName.
        /// </summary>
        /// <param name="CustomerPk"></param>
        [CallbackMethod]
        public object GetActiveProjectsForCustomer(int CustomerPk)
        {
            busProject project = TimeTrakkerFactory.GetProject();

            var list = project.GetProjectsForCustomer(CustomerPk)
                           .Select( proj => new 
                           {
                               ProjectName = proj.ProjectName,
                               Pk = proj.Pk
                           });
                   
            return list.ToList();                   
        }


        [CallbackMethod]
        public string ShowProjectEntries(string filter, int projectPk)
        {
            this.Authenticate();

            // *** Load data and bind list control            
            //this.LoadChildEntries();
            Hashtable data = new Hashtable();
            data["Filter"] = filter;
            data["ProjectPk"] = projectPk;
            data["RenderMode"] = "ProjectEntries";

            return Westwind.Utilities.WebUtils.RenderUserControl("~/UserControls/EntryList.ascx", true,data);
        }

        [CallbackMethod]
        public bool DeleteEntry(int pk)
        {
            this.Authenticate();

            busEntry entry = new busEntry();
            if (!entry.Delete(pk))
                throw new InvalidOperationException("Couldn't delete entry: " + entry.ErrorMessage);

            return true;
        }

        ///// test methods
        //[CallbackMethod]
        //public string GetUserControl()
        //{
        //    return WebUtils.RenderUserControl("~/UserControls/EntryList.ascx", true);
        //}


        /// <summary>
        /// Checks if a user is logged into the application. If not an exception
        /// is thrown and the service call is aborted.
        /// </summary>
        private void Authenticate()
        {
            if (HttpContext.Current.User ==  null || 
                !HttpContext.Current.User.Identity.IsAuthenticated)
                throw new SecurityException("You have to be logged in to access this functionality");

        }
    }


}
