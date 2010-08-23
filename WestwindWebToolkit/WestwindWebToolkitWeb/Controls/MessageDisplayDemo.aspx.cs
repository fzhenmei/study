using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Westwind.WebToolkit
{
    public partial class MessageDisplayDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((WestWindWebToolkitMaster)Master).SubTitle = "ErrorDisplay and MessageDisplay Page";
        }

        protected void btnErrorDisplay_Click(object sender, EventArgs e)
        {            
            this.ErrorDisplay.ShowError("An error occurred. If this wasn't a demo you'd be angry now!");
        }

        protected void btnErrorDisplayMessage_Click(object sender, EventArgs e)
        {
            this.ErrorDisplay.DisplayTimeout = 5000;
            this.ErrorDisplay.ShowMessage("Thank you for submitting your survey.<br/>Entered at: " + DateTime.Now.ToString());
        }

        protected void btnShowPage_Click(object sender, EventArgs e)
        {
            MessageDisplay.DisplayMessage("An error occurred",
@"
<div class='samplebox'>
<p>
<h3>Ooops. Looks like we've had a little problem in our application.</h3>
To be sure, the  error has been logged and the adminstrator emailed. 
We're on it, and we'll have you up and running  in no time again. This has been a test of the emergency broadcast system. 
Go home. Have a cigar. Nothing to see here...
</p>

<p/>
This page is set up as a template so you can customize the general layout as you see fit with any kind of markup.
The key part of the layout is that you place several controls - header, content and redirectlink - onto the page
with known names. The MessageDisplay class then handles rendering the page with the appropriate content with just
a single line of code.
</p>

<p>
This page will redirect back to the demo page in 15 seconds after it was loaded...
<br/>
<br/>
Thank for your patience,
</p>
</div>
<b>The West Wind Team", "MessageDisplayDemo.aspx", 15);
        }

        protected void btnDynamicMessage_Click(object sender, EventArgs e)
        {
            MessageDisplay.DisplayMessage(this.txtHeader.Text, this.txtMessage.Text, "MessageDisplayDemo.aspx", 8);
        }



    }
}
