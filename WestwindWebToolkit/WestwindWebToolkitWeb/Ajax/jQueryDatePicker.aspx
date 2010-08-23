<%@ Page Language="C#" 
        AutoEventWireup="true" 
        Inherits="Westwind.WebToolkit.jQueryDatePicker" 
        MaintainScrollPositionOnPostback="true" 
        MasterPageFile="~/WestWindWebToolkit.master"                                      
        Codebehind="jQueryDatePicker.aspx.cs"         
        Title="jQuery DatePicker Demo Page"
        trace="false"
%> 
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>

<asp:Content runat="server" ID="head" ContentPlaceHolderID="Header">    
    <style type="text/css">
        .samplebox
        {
        	border-bottom: dashed 1px teal;
        	margin-bottom: 15px;
        	padding: 15px;
        }
    </style>

</asp:Content>
<asp:Content runat="server" id="content" ContentPlaceHolderID="MainContent">
    <ww:ScriptContainer ID="scripts" runat="server" RenderMode="Header">
        <Scripts>            
            <script src="~/Scripts/jquery.js" resource="jquery"></script>
            <script src="~/scripts/jquery-ui.js" resource="jqueryui"></script>
            <script src="~/Scripts/ww.jquery.js" resource="ww.jquery"></script>
        </Scripts>
    </ww:ScriptContainer>           
    
    <div>
    <h1>jQuery UI Datepicker ASP.NET Control</h1>
    <div class="toolbarcontainer">
            <a href="./" class="hoverbutton"><asp:Image  runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
            <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
                <asp:Image runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
            </asp:LinkButton> |         
            <small>Page created: <%= DateTime.Now.ToString() %></small><br />
    </div>

    <div class="descriptionheader">
        This page is a sample of an ASP.NET wrapper control for 
        <a href="http://docs.jquery.com/UI/Datepicker">jQuery.Ui DatePicker</a> control that is part of the jquery.ui library.
        The control relies on jQuery and jQuery UI Datepicker. The control implementation wraps both <a href="http://jquery.com">jQuery</a>
        and jQuery UI Datepicker's resources internally so the control is fully self-contained with overrides
        for external references of all resources.
    </div>
        
    <div style="padding: 25px;">
        <div class="samplebox">
        <h3>Text Box AutoPopup:</h3>
        <hr />
        This version pops up the calendar on field activation:<br /><br />
                
        Enter Date: <ww:jQueryDatePicker runat="server" 
                                         id="txtAutoPopupDate" 
                                         DisplayMode="AutoPopup" 
                                         />
        </div>



        <div class="samplebox">
           <h3>Image Button Popup:</h3>
           <hr />
           This version shows a calendar image button to click on to pop up the calendar:<br /><br />
           
           Enter Date: 
                <ww:jQueryDatePicker runat="server" 
                               id="txtImageButton" 
                               DisplayMode="ImageButton" 
                               DateFormat="MM-dd-yyyy" 
                               ShowButtonPanel="true"
                               SelectedDate="08-10-2009" 
                               Theme="Redmond" />
        </div>



        <div class="samplebox">
            <h3>Plain Button Popup:</h3>
            <hr />
            Basic popup button version of the Calendar.<br /><br />
            Enter Date: 
            <ww:jQueryDatePicker runat="server" 
                                             id="txtPlainButton"  
                                             DisplayMode="Button" />            
        </div>               
        
        
        <div class="samplebox">
            <h3>Date Popup in a Modal Dialog</h3>
            <small>This example demonstrates that the date picker automatically pops up above all other
            content including modal overlays and a 'dialog window'.</small>
            <hr />
            <input type="button" id="" value="Show Modal Dialog" onclick="ModalDialog.show();" />
        </div>
        
        
        <div class="samplebox">
            <h3>Inline Calendar:</h3>
            <hr />
            You can also pop up the calendar inline. This version tracks the calendar SelectedValue
            and reassigns it on PostBack operations. Note this date picker is also restricted to 
                between the last 30 and next 30 days.<br /><br />
            <ww:jQueryDatePicker runat="server" id="txtInline" 
                               DisplayMode="Inline" 
                               SelectedDate="2008-11-20" />
            <br />
            <br clear="all">    
            <br />
            The controls are PostBack aware so they will persist their values across postbacks.<br />
            <asp:Button runat="server" id="btnPostBack" Text="Post Back" />
        </div>
        
            
    </div>
    </div>
    
    <div class="toolbarcontainer">
        <manoli:ViewSourceControl ID="ViewSourceControl" runat="server" Text="Show ASPX"
            DisplayState="Button" CodeFile="jQueryDatePicker.aspx" />
        <manoli:ViewSourceControl ID="ViewSourceControl1" runat="server" Text="Show CodeBehind"
            DisplayState="Button" CodeFile="jQueryDatePicker.aspx.cs" />
    </div>
    
    
    <br clear="all"/>    
    
    <div class="footer">
        <a href="http://www.west-wind.com/"><img alt="" src="../images/wwtoollogo_text.gif" style="float: right" /></a>
        &copy <%= DateTime.UtcNow.Year %>, Rick Strahl - West Wind Technologies
    </div>
    
    <!-- modal popup dialog box -->
    <ww:ModalDialog runat="server" ID="ModalDialog" class="dialog"
                    Closable="true"
                    Draggable="true"
                    DragHandle="ModalDialog_Header"
                    ShadowOffset="5"    
                    width="400px"                       
                    style="display: none"
                    ClientDialogHandler="ModalDialogHandler"             
    >
        <div id="ModalDialog_Header" class="dialog-header">Modal Dialog Date Entry</div>
        <div class="descriptionheader">
            When clicking the date box the jquery date picker should
            pick up ontop of the modal overlay and the dialog automatically
            as the control automatically pops to the top of the zOrder.
        </div>
        <div class="dialog-content dialog-bottom">  
            Please enter a date: <ww:jQueryDatePicker runat="server" ID="txtModalDate" />
            <hr />
            <input type="button" id="btnOk" value="Ok"  style="width: 75px" />
            <input type="button" id="btnCancel" value="Close" style="width: 75px" />
        </div>
    </ww:ModalDialog>
    
    <!-- Modal Popup client handler code -->
    <script type="text/javascript">
        
        $(document).ready(function() {
            showStatus({ afterTimeoutText: "Ready" });
        });        
        function ModalDialogHandler() {            
            if (this.id == "btnOk") {                
                
                var date = $("#<%= this.txtModalDate.ClientID %>").datepicker("getDate");
                
                // datepicker returns null on invalid or empty dates
                if (date == null)
                    showStatus("Please enter a valid date.", 5000, true);
                else {
                    showStatus("You've entered: " + date.formatDate("MMM dd, yyyy"), 5000, true);
                    return true; // close
                }
            }
            else if (this.id == "btnCancel") {
                showStatus("You've cancelled the button click.");
                return true; // close
            }

            // keep dialog open
            return false;
        }

    </script>
</asp:Content>