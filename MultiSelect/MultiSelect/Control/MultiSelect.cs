using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MultiSelect.Control
{
    /// <summary>
    /// 可多选的DropDownList
    /// </summary>
    [ToolboxData("<{0}:MultiSelect runat=\"server\"></{0}:MultiSelect>"),
     ParseChildren(true, "Items")]
    public class MultiSelect : WebControl, INamingContainer
    {
        private const string ClientScriptInstanceFlag = "MS$Instance";
        private CheckBoxList checkBoxList;
        private TextBox textBox;

        public string Text
        {
            get
            {
                EnsureChildControls();
                return textBox.Text;
            }
        }

        public string Value
        {
            get
            {
                EnsureChildControls();
                return string.Join(",",
                                   checkBoxList.Items.Cast<ListItem>().Where(t => t.Selected).Select(t => t.Value).
                                       ToArray());
            }
        }

        public string DataTextField
        {
            get
            {
                EnsureChildControls();
                return checkBoxList.DataTextField;
            }
            set
            {
                EnsureChildControls();
                checkBoxList.DataTextField = value;
            }
        }

        public string DataValueField
        {
            get
            {
                EnsureChildControls();
                return checkBoxList.DataValueField;
            }
            set
            {
                EnsureChildControls();
                checkBoxList.DataValueField = value;
            }
        }

        public object DataSource
        {
            get
            {
                EnsureChildControls();
                return checkBoxList.DataSource;
            }
            set
            {
                EnsureChildControls();
                checkBoxList.DataSource = value;
            }
        }

        public ListItemCollection Items
        {
            get
            {
                EnsureChildControls();
                return checkBoxList.Items;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(typeof(MultiSelect),
                                                    string.Format("multi_init_{0}", UniqueID),
                                                    string.Format(
                                                        "var {2}{1} = new multiSelector(); {2}{1}.init('{0}');",
                                                        GetJqFriendElementID(), UniqueID, ClientScriptInstanceFlag),
                                                    true);
        }

        private string GetJqFriendElementID()
        {
            return UniqueID.Replace("$", "\\\\$");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderBeginTag(writer);
            RenderContents(writer);
            RenderEndTag(writer);
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.Write("<div id=\"");
            writer.Write(UniqueID);
            writer.Write("\" onmouseover=\"clearTimeout({0}{1}.timoutID);\" onmouseout=\"{2}{3}.hide('",
                         ClientScriptInstanceFlag, UniqueID, ClientScriptInstanceFlag, UniqueID);
            writer.Write(GetJqFriendElementID());
            writer.Write("');\" class=\"");
            writer.Write(string.IsNullOrEmpty(CssClass) ? "multiSelector" : CssClass);
            writer.Write("\">");
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.Write("</div>");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td>");
            textBox.RenderControl(writer);
            string checkBoxListContainerID = string.Format("{0}checkboxContainer", UniqueID);
            writer.Write("</td><td>");
            writer.Write("<img src=\"/Images/drop.gif\" class=\"multi_drop_btn\" /></td></tr>");
                /*如果修改“multi_drop_btn”同时要修改脚本上的代码*/
            writer.Write("<tr><td colspan=\"2\">");
            writer.Write("<div class=\"checkBoxList\" id=\"");
            writer.Write(checkBoxListContainerID);
            writer.Write("\">");
            checkBoxList.RenderControl(writer);
            writer.Write("</div>");
            writer.Write("</td></tr></table>");
        }

        protected override void CreateChildControls()
        {
            textBox = new TextBox {CssClass = "textbox"};
            Controls.Add(textBox);

            checkBoxList = new CheckBoxList();
            Controls.Add(checkBoxList);
        }

        public override void DataBind()
        {
            checkBoxList.DataBind();
            base.DataBind();
        }
    }
}