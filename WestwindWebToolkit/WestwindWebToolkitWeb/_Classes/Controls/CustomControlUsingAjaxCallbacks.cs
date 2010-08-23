using System;
using System.Collections.Generic;
using System.Text;

using Westwind.Utilities;
using System.Web;
using Westwind.Web.Banners;
using System.Web.UI;
using Westwind.Web.Controls;
using Westwind.Web;

namespace Westwind.WebToolkit
{

    /// <summary>
    /// This class demonstrates how you can use the AjaxMethodCallback control
    /// from within a custom server control and route callbacks to methods
    /// of the control (or any other object).
    /// </summary>
    public class CustomControlUsingAjaxCallbacks : Control
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Create an instance of the Callback control (adds to Controls collection of the passed control)
            AjaxMethodCallback callback = AjaxMethodCallback.CreateControlInstanceOnPage(this);
            callback.PageProcessingMode = CallbackProcessingModes.PageLoad;
            
            // point at the control/object that has [CallbackMethod] attributes to handle callbacks
            callback.TargetInstance = this;
                        
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write("<div class='errordisplay'>dummy control placeholder</div>");
        }

        /// <summary>
        /// Implement any callback methods you want to have called on your control
        /// (or some other object accessible from your control).
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        [CallbackMethod]
        public int Add(int val1, int val2)
        {
            return val1 + val2;
        }

    }
}