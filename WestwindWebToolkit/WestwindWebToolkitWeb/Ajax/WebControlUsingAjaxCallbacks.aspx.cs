using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Web.Controls;
using Westwind.Web;

namespace Westwind.WebToolkit.Ajax
{
    public partial class WebControlUsingAjaxCallbacks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [CallbackMethod]
        public string HelloWorld(string firstName)
        {
            return "rick";
        }

    }
}
