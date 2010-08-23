using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Westwind.Utilities;
using System.Globalization;

namespace Westwind.WebToolkit
{

    public partial class ChatClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Must assign the target type for the callback control
            // otherwise it can't create the proxy
            this.ChatService.ClientProxyTargetType = typeof(ChatService);

            if (!this.IsPostBack)
            {
                string ChatId = Request.QueryString["ChatId"];
                if (!string.IsNullOrEmpty(ChatId))
                    this.txtChatId.Text = ChatId.Trim();
                string Handle = Request.QueryString["Handle"];
                if (!string.IsNullOrEmpty(Handle))
                    this.txtName.Text = Handle.Trim();
            }
        }

    }
}