using System;
using System.Data;
using Westwind.Web.Controls;
using Amazon;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using Westwind.Web;

namespace Westwind.WebToolkit
{
    public partial class BooksAdmin : Page
    {
        /// <summary>
        /// Cached category list so it doesn't have to be reloaded from server
        /// </summary>
        public static List<string> CategoryList
        {
            get
            {
                if (_categoryList == null)
                {
                    busLookup lookups = new busLookup();
                    _categoryList = lookups.GetCategories();
                }
            	return _categoryList;
            }
        }
        private static List<string> _categoryList;

        busBook books = new busBook();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((WestWindWebToolkitMaster)this.Master).SubTitle = this.Title;

            //Specify the target type if not current page
            this.Proxy.ClientProxyTargetType = typeof(Westwind.WebLog.AdminCallbackHandler);

            // Add Javascript object for ClientIds and emptyBook instance (scriptVars global var)
            ScriptVariables scriptVars = new ScriptVariables(this,"scriptVars");
            AmazonBook tbook = new AmazonBook();
            tbook.Entered = DateTime.Now;
            scriptVars.Add("emptyBook", tbook); // turn into Javascript object on client
            scriptVars.AddClientIds(this.Form,true);

            foreach (string category in CategoryList)
            {
                this.txtBookCategory.Items.Add(category);
            }
        }
    }
}
