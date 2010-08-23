<%@ Page Title="ErrorDisplay and MessageDisplay Page" Language="C#" MasterPageFile="~/WestWindWebToolkit.master"
         AutoEventWireup="true" CodeBehind="MessageDisplayDemo.aspx.cs" 
         Inherits="Westwind.WebToolkit.MessageDisplayDemo" 
         EnableViewState="false"%>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content  ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content  ContentPlaceHolderID="MainContent" runat="server">
    <div class="toolbarcontainer">
        <div class="rightalign">
            <ww:HelpControl ID="HelpControl" runat="server" HelpTopic="_2tf01g9is.htm" Text="ErrorDisplay" CssClass="hoverbutton" />
            <ww:HelpControl ID="HelpControl1" runat="server" HelpTopic="_2tf01f7qc.htm" Text="MessageDisplay" CssClass="hoverbutton" />
        </div>
        <a href="./" class="hoverbutton"><asp:Image runat="server" ImageUrl="~/css/images/home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
            <asp:Image runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
        </asp:LinkButton> |         
        <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
    </div>
    
    <div class="descriptionheader">
        The ErrorDisplay control provides an in-page control that can be dropped anywhere on a form to 
        provide a simple message display box. Predefined formats exist for error and informational
        displays. The MessageDisplay class provides an easy way to create application specific 'canned'
        pages - like error pages that can be invoked from anywhere and render basic messages in
        common format. You create one more MessageDisplay Template pages that contain a few
        known page elements. You can then call the static MessageDisplay function with custom
        headers, content text and optional redirection URLs to display a full page with a single
        line of code.    
    </div>

    <div class="containercontent">
    
    <ww:ErrorDisplay runat="server" ID="ErrorDisplay" width="500px" />
    
    <div class="samplebox">
    
        <h3>Error and Message Displays</h3>
        <p>
        Just about any page needs some sort of informational display, whether it's to display errors or informational
        messages. The <i>ErrorDisplay</i> control provides a simple way to display an informational display box anywhere in
        your page and display errors or messages with a single line of code easily.        
        </p>
                
        <asp:Button runat="server" ID="btnErrorDisplay" 
            Text="Show Error Message" onclick="btnErrorDisplay_Click" />
        <asp:Button runat="server" ID="btnErrorDisplayMessage"
           Text="Show Information Message" onclick="btnErrorDisplayMessage_Click" />
            
    </div>
    
    <div class="samplebox">
        <h3>Simple Page Display</h3>       
        <p>
        Sometimes inline messages aren't possible because you simply don't have enough data to display the page
        or the context of the page has disappeared.For example, when you click a delete button on
        an item and that item no longer exists. Often you'd want to display just a message that the delete worked
        or that an error occured. This example displays a page and then returns to this page after 8 seconds.
        </p>             
        
        <asp:Button runat="server" ID="btnShowPage" 
            Text="Display Error Page" onclick="btnShowPage_Click" />    
    </div>


    <div class="samplebox">    
    <p>
    Enter a Header:<br />
    <asp:TextBox runat="server" ID="txtHeader" Text="MessageDisplay Test Page" style="width:480px" />
    </p>
    
    <p>
    Enter a Message to display:<br />
    <asp:TextBox runat="server" ID="txtMessage" Text="This message text becomes the body of the page that is displayed." 
                 TextMode="MultiLine"  style="width:480px"/>
    </p>
    
        <asp:Button runat="server" ID="btnDynamicMessage" 
            Text="Show Page" onclick="btnDynamicMessage_Click" />
    
    </div>

</div>

    <div class="toolbarcontainer">
        <manoli:ViewSourceControl ID="ViewSourceControl" runat="server"
            Text="Show ASPX" DisplayState="Button" CodeFile="MessageDisplayDemo.aspx" />
        <manoli:ViewSourceControl ID="ViewSourceControl1" runat="server"
            Text="Show CodeBehind" DisplayState="Button" CodeFile="MessageDisplayDemo.aspx.cs" />
    </div>
</asp:Content>
