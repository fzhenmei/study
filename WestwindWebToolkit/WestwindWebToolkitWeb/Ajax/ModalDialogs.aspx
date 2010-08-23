<%@ Page Title="Modal Dialogs - West Wind Web Toolkit" Language="C#" MasterPageFile="~/WestWindWebToolkit.master" AutoEventWireup="true" 
         CodeBehind="ModalDialogs.aspx.cs" Inherits="Westwind.WebToolkit.Ajax.ModalDialogs" %>

<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>         
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
        #_MBOX input[type=button]
        {
        	width: 90px;
        	height: 25px;
        }        
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Modal Dialogs</h1>
    <div class="toolbarcontainer">
        <img src="../images/loading_small.gif" id="imgLoading" style="display: none; float: right; margin-right: 5px;" alt="loading..."/>
    
        <a href="./" class="hoverbutton"><asp:Image ID="Image1" runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
            <asp:Image  runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
        </asp:LinkButton> |         
        <small>Page created: <%= DateTime.Now.ToString() %></small><br />
    </div>
    
    
    <div class="descriptionheader">
    The ModalDialog class is a client class that shows an element on top of a shaded background giving the
    appearance of a modal dialog. The functionality is all client driven with a server control to
    wrap the client functionality optionally. You can make any element 'modal' including simple elements
    like images (great for 'working' animations) or more complete 'dialogs'.    
    </div>
    
    <div class="containercontent">
    
        <div class="samplebox">
            <h3>Simple Message Display</h3>
            <small>
            The following is a simple pop up message that simply displays a notification dialog that echos back the content
            you've entered in the field below.
            </small>
        
            <p>
                Enter a title for your dialog:<br />
                <asp:TextBox runat="server" id="txtTitle" Width="400">Modal Popup Sample</asp:TextBox>
                <br />
                <br />
                Enter a message to display in the dialog:<br />
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtMessage" 
                              Width="400" Height="50" 
                              Text="If you can't say something nice, say nothin' at all!">
                </asp:TextBox><br />
                <input type="button" id="Starter" value="Show Dialog" 
                       onclick="SimpleMessageDisplay()" />        
            </p>
        </div>
        
        
        <div class="samplebox">
            <h3>Input Dialog Box</h3>
            <small>
            The following pops up a dialog window that allows text entry. You can enter a value into the text field and
            then click the Save or cancel button to exit. A ClientDialogHandler() is called on any clicks that occur
            in the dialog and you can take appropriate action - such as picking up result values - in response to clicks.
            This dialog also uses a custom div to display a different overlay.
            </small>
            
            
            <p>            
                <input type="button" id="btnTextEntry" value="Enter Name" onclick="MessageBox2.show()" />   
                <div id="divNameResult" class="errordisplay" style="display:none;padding:8px;width:500px;"></div> 
             </p>
        </div>             


        <div class="samplebox">                
                <h3>Generic Message Box without explicit HTML Elements</h3>
                <small>
                The following dialog box is generated with pure client script and doesn't require
                any HTML elements on the page to 'attach to'. Rather the static wwModalDialog.createDialog()
                creates a dialog on the fly and uses that for displaying the message box. Not as
                flexible as the full functionality since the display is fixed and generic, but it
                can be done with two lines of code.
                </small>
                
                <p>
                    <input type="button" onclick="GenericButtonClick();" value="Show Generic Dialog" />
                    <div id="divGenericButtonResult" class="errordisplay" style="display:none;margin: 10px;width:500px;"></div>
                </p>        
        </div>
        
        <div class="samplebox">
            <h3>Display a 'Working' Image</h3>
            <small>
            A common task is to lock out a screen while some operation that needs completion
            before you can move on is happening. This is very easy to do with $().modaldialog().
            This example displays a loading image for 4 seconds before returning the page.
            </small>
            
            <div>
                <input type="button" id="btnShowLoading" value="Show Working Image" 
                            onclick="DisplayWorkingImage()" />
                <input type="button" value="Show Working DIV container" 
                            onclick="DisplayWorkingImage(true)" />
            
            </div>
            
            <img id="imgLoadingBlack" src="../css/images/loading_black.gif" alt="Loading..." style="display:none"/>
            
            <div id="imgLoadingBlack2" style="display:none;">
                <center>
                <img src="../css/images/loading_black.gif" alt="Loading..." />
                <div class="containercontent" style="font-size: 20pt; font-family: Arial Black;color:White;">
                    please wait for four seconds...                
                </div>
                </center>
            </div>
        </div>
        
        
        <div class="samplebox">
            <h3>Pure Client Side Modal Dialog Box</h3>
            <small>
            This simple example demonstrates how you can create a 'dialog'
            template as plain HTML and dynamically feed header and content
            text into it using the jQuery ModalDialog plug-in.
            </small>        
        </div>
        
        <p>
            <input type="button" id="btnClientOnly" value="Show Client Dialog" onclick="PureClientDialog()" />        
        </p>
    
    </div>
    
    <div class="toolbarcontainer">
        <manoli:ViewSourceControl ID="ViewSourceControl" runat="server"
            Text="Show ASPX & JS" DisplayState="Button" CodeFile="ModalDialogs.aspx" />
        <manoli:ViewSourceControl ID="ViewSourceControl1" runat="server"
            Text="Show CodeBehind" DisplayState="Button" CodeFile="ModalDialogs.aspx.cs" />
    </div>
    
<div id="divModal" class="dialog" 
     style="width:350px;background:white;display:none;">
    <div class="dialog-header" id="divModal_Header">Header</div>
    <div class="containercontent" id="divModal_Content">
        Content goes here<hr />
    </div>
    <div style="margin: 5px 15px 10px">
        <input type="button" id="CloseButton" value="Close Dialog"/> 
        <input type="button" id="CancelButton" value="Cancel Dialog"/> 
     </div>
</div>

    <ww:ScriptContainer ID="scripts" runat="server" RenderMode="Header">
        <Scripts>
            <script src="~/Scripts/jquery.js"  ></script>
            <script src="~/Scripts/ww.jquery.js"></script>
        </Scripts>
    </ww:ScriptContainer>
    
    <!-- First Message Box -->
<ww:ModalDialog ID="MessageBox" runat="server" 
    BackgroundOpacity=".70"    
    ContentId="MessageBoxContent" 
    HeaderId="MessageBoxHeader"
    ShadowOpacity=".20" ShadowOffset="8"
    Draggable="true" DragHandleId="MessageBoxHeader"  
    Closable="True" 
    FadeinBackground="True"         
    Style="background: white; display:none;width: 400px;" 
    CssClass="dialog" >
    
    <div id='MessageBoxHeader' class='dialog-header' style="padding:4px">Web Connection</div>
    <div style='padding: 10px;'>
        <div id='MessageBoxContent' style="padding:10px;">
        </div>
        <hr />
        <input id='MessageBoxOk' type='button' value='Close' />
    </div>
    
</ww:ModalDialog>    
    


    <!-- Second Message Box used for name entry -->    
    <ww:ModalDialog ID="MessageBox2" runat="server" 
        ContentId="MessageBox2Content" 
        HeaderId="MessageBox2Content" 
        OverlayId="overlay" 
        BackgroundOpacity=".70"            
        ClientDialogHandler="onMessageBox2ClientClick"                                
        FadeinBackground="true"
        Draggable="true" 
        DragHandleId="MessageBox2Header"     
        Closable="true"            
        ShadowOffset="6" ShadowOpacity=".20"
        Style="background: white; display:none;" cssclass="dialog"
        Width="400px"
    >
        <div id='MessageBox2Header' class='dialog-header' style="padding:2px;">The Name Game</div>
        <div style='padding: 10px;'>
            <div id='MessageBox2Content'>
            Enter your name:<br />
            <input type="text" style="width:300px;" id="txtInputName" />
            <div id="MessageBox2Message" class="errormessage"></div>
            </div>
            <hr />
            <input id='MessageBox2Ok' type='button' value='Save' />
            <input id='MessageBox2Cancel' type='button' value='Cancel' />
        </div>
    </ww:ModalDialog>

    <!-- Optional Div tag to use as overlay so you can format the background -->    
    <div id="overlay" style="background-color:steelblue;background-image:url(../images/sailbig.jpg);background-attachment:fixed;background-repeat:no-repeat;"></div>


    <script type="text/javascript">
        // *** Simple Message Display
        // this functionality basically lets you treat the dialog as a 'message box'
        // by passing a header and body message as parameters.  Note that MessageBox
        // is the name of the ModalDialog 'control' created as global var (maps
        // the name of hte server control or if manually bind the plug-in the instance
        // of ModalDialog instance.
        function SimpleMessageDisplay() 
        {
            MessageBox.show($('#' + scriptVars.txtMessageId).val(), $('#' + scriptVars.txtTitleId).val());
        }


        function onMessageBox2ClientClick(evt, inst) {
            var btn = this;  // this == element clicked
            var text = $("#txtInputName").val();
            
            if (btn.id == "MessageBox2Ok") {
                if (text == "")
                {
                    $("#MessageBox2Message").text("C'mon don't be shy - enter something");
                    return false; // don't close
                }
                $("#divNameResult").text("You entered: " + text).show();
                return true;  // close dialog
            }
            if (btn.id == "MessageBox2Cancel") {
                $("#divNameResult").text("You cancelled the dialog").show();
                return true;  // close dialog
            }
            
            // dialog is not closed
            return false;
        }


        // *** Generic Message box with static client modal dialog
        function GenericButtonClick() {            
            $.modalDialog("This dialog is generic, was created on the fly and requires no page elements.",
                           "Generic Modal Dialog ",
                           ["OK", "Cancel", "Otherwise"],                           
                           function() {
                               var div = $("#divGenericButtonResult");
                               var txt = $(this).val();                               
                               if (txt == null)
                                   return false; // don't exit

                               div.text("You clicked on: " + txt).show();
                               
                               return true; // 
                           },false);
        }


        // *** Display an image ontop of opaque background and centered
        function DisplayWorkingImage(showContainer) {
            var id = "imgLoadingBlack"
            if (showContainer)
                id="imgLoadingBlack2"
              
            
            // Modal Dialog can be applied against any elements - image here
            $("#" + id).modalDialog();            
            
            // hide it after 4 seconds
            setTimeout(function() { $("#" + id).modalDialog("hide"); },4000);
        }


        // *** Displaying a purely client side dialog in a div tag
        function PureClientDialog() {

            var headerText = "Time Alert";  //$("#txtClientHeader").val();
            var contentText = "It's time. Get ready. Time is:<br/>" + 
                               new Date().toLocaleTimeString();  //$("#txtClientContent").val();

            $("#divModal").modalDialog({ contentId: "divModal_Content", headerId: "divModal_Header" }, 
                                         contentText, headerText,true);            
        }
        
    </script>
    
</asp:Content>
