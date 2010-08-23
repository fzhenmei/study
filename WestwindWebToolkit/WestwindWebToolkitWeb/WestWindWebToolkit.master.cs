using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Westwind.WebToolkit
{
    public partial class WestWindWebToolkitMaster : System.Web.UI.MasterPage
    {
        
        public string SubTitle
        {
            get { return _SubTitle; }
            set { _SubTitle = value; }
        }
        private string _SubTitle = "";


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
