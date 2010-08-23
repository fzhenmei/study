using System;
using Westwind.Web.JsonSerializers;

namespace Westwind.WebToolkit
{
    public partial class AjaxDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((WestWindWebToolkitMaster)Master).SubTitle = "Ajax Samples Home";

            if (Request.Params["method"] == "DoIt")
                DoIt();
        }

        public void DoIt()
        {
            JSONSerializer json = new JSONSerializer();
            string newVariable = "Hello World. Time is: " + DateTime.Now.ToString();

            newVariable += Request.Params["Data"].ToString();

            Response.Write(json.Serialize(newVariable));
            Response.End();
        }
    }
}
