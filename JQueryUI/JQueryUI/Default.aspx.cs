using System;
using System.Web.UI;

namespace JQueryUI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void HitMe(object sender, EventArgs e)
        {
            Response.Write(string.Format("You hit me at {0}", DateTime.Now));
        }

        protected void KissMe(object sender, EventArgs e)
        {
            Response.Write(string.Format("You kiss me at {0}", DateTime.Now));
        }
    }
}