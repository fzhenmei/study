using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Web;

namespace Westwind.WebToolkit.Ajax
{
    public partial class ModalDialogs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptVariables scriptVars = new ScriptVariables(this, "scriptVars");
            scriptVars.AddClientIds(this.Form, true);
            
                                      
            
        }
    }
}
