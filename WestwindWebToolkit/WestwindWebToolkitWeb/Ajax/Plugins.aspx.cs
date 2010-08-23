using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Westwind.Web;

namespace Westwind.WebToolkit.Ajax
{
    public partial class Plugins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Assign subtitle to the page
            ((WestWindWebToolkitMaster)this.Master).SubTitle = "jQuery Plug-in Samples";

        }



        [CallbackMethod]
        public CommentItem[] GetComments()
        {
            List<CommentItem> comments = new List<CommentItem>();
            for (int i = 0; i < 20; i++)
            {
                var comment = new CommentItem
                {
                    Id = i,
                    Title = "Comment " + i.ToString(),
                    Body = "This is the content Body # " + i.ToString(),
                    Entered = DateTime.Now.AddDays(i * -1)
                };
                comments.Add(comment);
            }

                return comments.ToArray();
        }

    }

    public class CommentItem
    {
        public int Id = 0;
        public string Title = "";
        public string Body = "";
        public DateTime Entered = DateTime.Now;
    }

}
