using System;
using System.Web.UI;

namespace MultiSelect
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ShowMe(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(typeof (Page), "a", string.Format("alert('你选择了：{0}');", multiSelect.Value),
                                               true);
        }
    }
}