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
using System.Threading;
using System.Globalization;
using Westwind.Web.Controls;

namespace Westwind.GlobalizationWeb
{

    public partial class Cultures : System.Web.UI.Page
    {
        private string Lang = null;
        private string Mode = "Default";


        public string TestProperty
        {
            get { return _TestProperty; }
            set { _TestProperty = value; }
        }
        private string _TestProperty = "";


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // *** Now we can read the 'real' value with ViewState Post applied
            this.Mode = this.radMapping.Text;

        }

        protected override void OnPreRender(EventArgs e)
        {
            // *** Handle display settings after event fired
            if (Mode != "DropDown")
            {
                this.lstLanguage.Enabled = false;
            }
            else
            {
                this.lstLanguage.Enabled = true;
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// To override culture and culture related settings
        /// override the InitializeCulture method which fires
        /// before any Page level events fire.
        /// 
        /// You can do this later in the page cycle, but keep
        /// in mind that all localization related features ASP.NET
        /// natively provides occur during page initialization, hence
        /// this method is the recommended place for this.
        /// </summary>
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            // *** Note this happens way early in the page cycle
            // *** so controls are not even been created yet. 
            //        
            // *** Have to use Request object directly                                
            string Mode = Request.Form["radMapping"];

            if (Mode == "Browser")
            {
                //if (!string.IsNullOrEmpty(Request.Form["chkSetLocale"]))
                //{
                // *** Set the Locale with helper
                this.SetUserLocale("", true);  // SetUserLocale("$",false);

                // *** Use this code to support only very specific locales
                //this.SetUserLocale(this.GetSupportedLocale("en,de,fr", "en"), "$", true);
                //}
            }
            else if (Mode == "DropDown")
            {
                // Explicit User Language Selection (DropDown)
                this.Lang = Session["Lang"] as string;

                if (!string.IsNullOrEmpty(Lang))
                {
                    this.SetUserLocale(this.Lang, "", true);
                }
                else
                {
                    this.SetUserLocale("", true);
                }

            }
        }

        /// <summary>
        /// Allow language selection. Note this request must redirect because by the
        /// time this event fires we can't apply thread localization properly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // *** Damn ListBox events fire because of culture assignments
            //     So check explicitly for mode and limit.
            if (Mode != "DropDown")
                return;

            // *** Read the current value again now
            this.Lang = this.lstLanguage.SelectedValue;
            if (!string.IsNullOrEmpty(Lang))
            {
                Session["Lang"] = this.Lang;

                // *** THIS WON'T WORK for UI Culture but it will update
                //     our culture because the settings are displayed in the page
                this.SetUserLocale(Lang, "", true);
            }
            else
                Session["Lang"] = "";

            // *** NOTE: we can't update the locale at this point in the page
            //           because all the localized code has already rendered
            //           so we post the page and immediately force it to refresh
            //           itself. A redirect wouldn't work because of the presettign
            //           of page items.
            //
            //           There's no real clean way to do this *IF* you need to 
            //           maintain the current page state, but in normal scenarios
            //           you can just redirect to the same or another page and
            //           that will work fine. 


            // *** Immediately cause the page to refresh
            //Response.AppendHeader("Refresh", "2");

            //// *** Must redirect to show the change in 
            //// *** this page immediately
            //Response.Redirect("Cultures.aspx");

            // *** Load wwScriptLibrary for DOM support
            //ControlResources.LoadwwScriptLibrary(this);
            //this.panelLanguageRefresh.Visible = true;
            //this.Page.ClientScript.RegisterStartupScript(typeof(ControlResources),"LanguagePopup","new wwControl('panelLanguageRefresh').centerInClient();");

            this.panelLanguageRefresh.Show(null,null,true);
        }


        /// <summary>
        /// Sets a user's Locale based on the browser's Locale setting. If no setting
        /// is provided the default Locale is used.
        /// </summary>
        public void SetUserLocale(string CurrencySymbol, bool SetUiCulture)
        {
            HttpRequest Request = HttpContext.Current.Request;
            if (Request.UserLanguages == null)
                return;

            string Lang = Request.UserLanguages[0];
            if (Lang != null)
            {
                // *** Problems with Turkish Locale and upper/lower case
                // *** DataRow/DataTable indexes
                if (Lang.StartsWith("tr"))
                    return;

                if (Lang.Length < 3)
                    Lang = Lang + "-" + Lang.ToUpper();
                try
                {
                    System.Globalization.CultureInfo Culture =
                           new System.Globalization.CultureInfo(Lang);
                    System.Threading.Thread.CurrentThread.CurrentCulture = Culture;

                    if (!string.IsNullOrEmpty(CurrencySymbol))
                        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol =
                          CurrencySymbol;

                    if (SetUiCulture)
                        System.Threading.Thread.CurrentThread.CurrentUICulture = Culture;
                }
                catch
                { ;}
            }
        }

        /// <summary>
        /// Typical Auto Locale switching routine. Leaves currency
        /// symbol in locale default and also sets UICulture.
        /// </summary>
        public void SetUserLocale()
        {
            this.SetUserLocale(null, true);
        }

        /// <summary>
        /// Explicitly set a user's locale
        /// </summary>
        /// <param name="LCID"></param>
        /// <param name="CurrencySymbol"></param>
        /// <param name="SetUiCulture"></param>
        public void SetUserLocale(string LCID, string CurrencySymbol, bool SetUiCulture)
        {
            try
            {
                if (LCID.IndexOf("-") < 0)
                    LCID += "-" + LCID;

                System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(LCID);
                System.Threading.Thread.CurrentThread.CurrentCulture = Culture;

                if (!string.IsNullOrEmpty(CurrencySymbol))
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol =
                      CurrencySymbol;

                if (SetUiCulture)
                    System.Threading.Thread.CurrentThread.CurrentUICulture = Culture;
            }
            catch (Exception ex)
            { string t = ex.Message; }
        }





        /// <summary>
        /// Checks user languages for one of the supported languages
        /// </summary>
        /// <param name="SupportedLanguages">Two/three letter language codes separated by commas</param>
        /// <returns></returns>
        private string GetSupportedLocale(string SupportedLanguages, string DefaultLanguage)
        {
            SupportedLanguages += ",";
            foreach (string Lang in HttpContext.Current.Request.UserLanguages)
            {
                string[] LangKeys = Lang.Split('-');
                if (LangKeys.Length > 0)
                {
                    string SelectedLanguage = LangKeys[0].ToLower();

                    if (SupportedLanguages.Contains(SelectedLanguage + ","))
                    {
                        /// *** Check for ;q=0.8  quality key
                        int At = Lang.IndexOf(";");
                        if (At > -1)
                            return Lang.Substring(0, At).ToLower();

                        return Lang.ToLower();
                    }
                }
            }

            return DefaultLanguage.ToLower();
        }

    }

}