<%@ Page Title="Ajax Samples - West Wind Web Toolkit" Language="C#" 
         CodeBehind="Default.aspx.cs" Inherits="Westwind.WebToolkit.AjaxDefault"
         MasterPageFile="~/WestWindWebToolkit.master" AutoEventWireup="true" 
%>
<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
        
        <div class="toolbarcontainer">
            <a href="../" class="hoverbutton"><asp:Image runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
            <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
                <asp:Image runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
            </asp:LinkButton> |         
            <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
        </div>
         
        <table width="100%">
        <tr>
        <td style="width: 150px; text-align: left; padding: 10px; vertical-align: top; background-image: url(../images/newwave.jpg);background-repeat: repeat-y;">
            <img src="../images/WestwindWebToolkit_100.jpg" />            
        </td>   
        
        <td class="containercontent" style="vertical-align: top;">
            <h3>Functional Examples</h3>
            
            <p>
            The following examples demonstrate the basic features of some of the
            various AJAX based controls. For more involved examples that combine
            components and provide more useful behavior see the examples in the right sidebar.
            </p>
            
            <hr />
            
            <p>
            <a href="AjaxMethodCallbacks.aspx" class="headerlink">Making Ajax Callbacks with AjaxMethodCallback</a>
            The AjaxMethodCallback controls makes it very, very easy to call server
            side page, control or a custom handler method and return data back to the client.
            Using JSON messages raw data can be passed from server to client and back via simple
            method calls.                         
            </p>
            
            <p>
            <a href="HoverPanels.aspx" class="headerlink">Popup Windows with the HoverPanel</a>
            The HoverPanel control makes it real easy to display server generated content in
            the current page with options to provide for popup 'indows' or displaying content
            in fixed locations. Content can be directed at any HTML element and provide
            things like closability, dragging and shadows.            
            </p>
            
            <p>
            <a href="ModalDialogs.aspx" class="headerlink">Modal Popup Windows</a>
            Demonstrates how you can use modal dialogs to display content on
            an opaque background to block access to the underlying page content.
            </p>                                   
            
            <p>
            <a href="jQueryDatePicker.aspx" class="headerlink">jQuery DateTime Picker</a>
            The jQuery date picker is a client side date selection component that provides
            a popup calendar. Based on ui.jQuery's DatePicker wrapped up into an easy
            to use and fully self contained control.
            </p>
            
            <br />
            
            <h3>Working with Third Party jQuery Plug-ins</h3>
            <hr />
            
            <p>
            <a href="AutoComplete.aspx" class="headerlink">jQuery AutoComplete Interaction</a>
            Demonstrates how you can use a Callback Handler to feed data to the jQuery AutoComplete
            plug-in in this case feeding stock symbols to a stock quote query text input box.
            </p>
            
            <a href="jqGrid.aspx" class="headerlink">jqGrid Interaction</a>
            This example demonstrates how you can use the powerful jqGrid plug-in with 
            an AjaxMethodCallback control on the page to [CallbackMethods] on the page.
        </td>
 
        <td class="gridalternate sidebar" style="border-left: solid 1px grey;padding: 10px;">
            <h3>Application Examples</h3>
            
            <p>
            More complex examples - small, self-contained applications if you will - 
            that combine features of the AJAX tools into comprehensive samples.
            </p>
            
            <p>
            <a href="StockPortfolio/JsonStockClient.aspx" class="sidebarheader">Stock Portfolio Manager</a>
            Uses the AjaxMethodCallback control and host of the client plug-ins in a rich stock lookup and
            portfolio viewer application.            
            </p>
            
            <p>
            <a href="Chat/Chat.aspx" class="sidebarheader">Chat Application</a>
            Creates a small browser based chat client using AjaxMethodCallback 
            to provide chat detail information. 
            </p>
            
            <p>
            <a href="AmazonBooks/BooksAdmin.aspx" class="sidebarheader">Amazon Book Lookup</a>
            Provides a book list selection tool that allows picking of books from Amazon via
            it's e-commerce Web service. Books can be edited and viewed.Demonstrates 
            the AjaxMethodCallback control and Client Templating for creating client content.
            </p>
            
            <p>
            <a href="../../Westwind.GlobalizationWeb/customerlist.aspx" class="sidebarheader">Localization Administration</a>
            Provides an interactive Localization Resource Editor for a data driven ASP.NET Resource Provider.
            You can add, edit, delete, manage, translate localization resources interactively in a very
            interactive application that utilizes many AjaxMethodCallbacks and a few HoverPanels. Click on
            any of the red icons to access the localization administration form.
            </p>
        </td>
        
        </tr>
        </table>
        
</asp:Content>
