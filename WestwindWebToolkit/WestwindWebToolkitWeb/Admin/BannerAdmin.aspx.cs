using System;
using System.Web.UI.WebControls;
using Westwind.Utilities;
using Westwind.Web.Controls;
using Westwind.Web.Banners;
using Westwind.Web;

namespace Westwind.WebToolkit.Admin
{

    public partial class BannerManTest : System.Web.UI.Page
    {
        protected BannerManager bannerManager = null;

        protected override void OnLoad(EventArgs e)
        {
            bannerManager = new BannerManager();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!bannerManager.LoadBanners(""))
            {
                if (string.IsNullOrEmpty(this.ErrorDisplay.Text))
                    this.ErrorDisplay.ShowError("Error loading banner data.<br/>" + bannerManager.ErrorMessage);

                this.btnCreateTable.Visible = true;
            }
            else
            {
                this.BannerAdmin.DataSource = bannerManager.Banners.Values;
                this.BannerAdmin.DataBind();
            }
            base.OnPreRender(e);

        }

        protected void BannerAdmin_OnItem(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                this.bannerManager.DeleteBanner(e.CommandArgument as string);
            }
            else if (e.CommandName == "ResetStats")
            {
                BannerItem banner = this.bannerManager.GetBanner(e.CommandArgument as string);
                if (banner == null)
                {
                    this.ErrorDisplay.ShowError("Invalid Banner Id");
                    return;
                }
                banner.ResetClicks = 0;
                banner.ResetHits = 0;

                this.bannerManager.UpdateBannerItem(banner);
            }
        }




        /// <summary>
        /// Returns the CTR Calculation in a safe manner for databinding expression
        /// </summary> 
        /// <param name="Clicks"></param>
        /// <param name="Hits"></param>
        /// <returns></returns>
        protected decimal Ctr(int Clicks, int Hits)
        {
            if (Hits == 0)
                return 0;
            return Convert.ToDecimal(Clicks) / Convert.ToDecimal(Hits) * 100;
        }

        [CallbackMethod]
        public BannerItem GetBanner(string bannerId)
        {
            BannerItem banner = this.bannerManager.GetBanner(bannerId);
            if (banner == null)
                throw new ApplicationException(this.bannerManager.ErrorMessage);

            return banner;
        }

        [CallbackMethod]
        public string SaveBanner(BannerItem banner)
        {
            if (string.IsNullOrEmpty(banner.BannerId))
            {
                string bannerId = this.bannerManager.InsertBannerItem(banner);
                if (bannerId == null)
                    throw new ApplicationException(this.bannerManager.ErrorMessage);
            }

            if (!this.bannerManager.UpdateBannerItem(banner))
                throw new ApplicationException(this.bannerManager.ErrorMessage);

            return banner.BannerId;
        }

        [CallbackMethod]
        public BannerItem NewBanner()
        {
            return new BannerItem();
        }

        [CallbackMethod]
        public string GetBannerLinks(string bannerId)
        {
            BannerItem banner = BannerManager.Current.GetBanner(bannerId);
            var html = "<div class=\"descriptionheader\">You can copy the appropriate links from below into your pages. The ASP.NET syntax is preferred <br/>as it generates unique keys to avoid caching.</div>";
            html += "<div style=\"padding: 10px;\">";
            html += string.Format("<h3 class=\"linkheader\">Show Specific Banner:</h3><div style=\"margin:10px;\">");
            html += "<b>Script:</b><br/>";
            html += Server.HtmlEncode(string.Format("<script src=\"{0}/wwbanner.ashx?a=s&id={1}\"></script>", Request.ApplicationPath, banner.BannerId));
            html += "<div style='margin-top: 10px;'><b>ASP.NET:</b><br/>";
            html += Server.HtmlEncode("<%= BannerManager.Current.RenderBanner(\"" + bannerId + "\") %>");
            html += "</div></div>";

            html += "<h3 class=\"linkheader\">Show Next Random  Banner:</h3><div style=\"margin:10px;\">";
            html += "<b>Script:</b><br/>";
            html += Server.HtmlEncode(string.Format("<script src=\"{0}/wwbanner.ashx?a=s&c={1}\"></script>",Request.ApplicationPath,banner.BannerGroup));
            html += "<p><b>ASP.NET:</b><br/>";
            html += Server.HtmlEncode("<%= BannerManager.Current.RenderNextBanner(\"" + banner.BannerGroup + "\") %>");
            html += "</p></div></div>";

            return html;
        }

        [CallbackMethod]
        public BannerItem UpdateBannerSize(string BannerId)
        {
            BannerItem banner = this.GetBanner(BannerId);
            if (banner == null)
                return null;

            if (!banner.UpdateImageSize())
                return null;

            this.bannerManager.UpdateBannerItem(banner);

            return banner;
        }

        protected void btnCreateTable_Click(object sender, EventArgs e)
        {
            if (!this.bannerManager.CreateTables(null))
                this.ErrorDisplay.ShowError("An error occurred creating the tables:<div style='padding:10px;'>" + this.bannerManager.ErrorMessage + "</div>");
            else
                this.ErrorDisplay.ShowMessage("Banner tables were successfully created.");
        }
    }

}