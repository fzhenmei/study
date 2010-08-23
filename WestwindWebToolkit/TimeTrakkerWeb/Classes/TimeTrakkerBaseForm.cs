using System;
using System.Collections.Generic;
using System.Text;
using Westwind.Web.Controls;
using System.Web.SessionState;

using System.ComponentModel;

namespace TimeTrakkerWeb
{
    
    /// <summary>
    /// All main pages of teh WebLog inherit from this class. 
    /// This class deals with checking for Admin users. It also sets the default
    /// page Master template. Note WebBaseAdminForm inherits from this class and
    /// provides an Admin page specialization.
    /// </summary>
    public class TimeTrakkerBaseForm : wwWebForm   //, IRequiresSessionState
    {
        public bool IsAdmin = false;
        protected string AdminUser = "";

        /// <summary>
        /// Strongly typed master page
        /// </summary>        
        protected TimeTrakkerMaster TimeTrakkerMaster
        {
            get { return base.Master as TimeTrakkerMaster; }
        }        

        protected override void  OnLoad(EventArgs e)
        {

        }
    }

    
}
