using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web;


namespace Westwind.Web.Banners
{
    /// <summary>
    /// Banner Control that embeds a 
    /// </summary>
    public class BannerControl : Control
    {

        
        /// <summary>
        /// The Banner group that is applied. Use blank for all banners
        /// </summary>
        [Description("The Banner group that is applied. Use blank for all banners")]
        [Category("Miscellaneous"), DefaultValue("")]
        public string BannerGroup
        {
            get { return _BannerGroup; }
            set { _BannerGroup = value; }
        }
        private string _BannerGroup = "";

        
        /// <summary>
        /// Width for the control (used only for placeholder rendering)
        /// </summary>
        [Description("Width for the control (used only for placeholder rendering")]
        [Category("Miscellaneous"), DefaultValue(typeof(Unit),"468px")]
        public Unit Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        private Unit _Width = Unit.Parse("468px") ;

        
        /// <summary>
        /// Height of the banner (used only for placeholder rendering)
        /// </summary>
        [Description("Height of the banner (used only for placeholder rendering)")]
        [Category("Miscellaneous"), DefaultValue(typeof(Unit),"60px")]        
        public Unit Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        private Unit _Height = Unit.Parse("60px");


        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if (HttpContext.Current == null)
            {                
                writer.Write(string.Format("<div style='background:silver;border:solid 1px;width:{0};height:{1};text-align:center;vertical-align:middle;font-weight:bold;'>Banner</div>",Width.ToString(),Height.ToString())) ;
                return;
            }

            writer.Write(BannerManager.Current.RenderBannerLink(BannerGroup));
            //writer.Write(BannerManager.Current.RenderNextBanner(BannerGroup));
        }        

    }
}
