<%@ Page Title="" Language="C#" MasterPageFile="~/WestWindWebToolkit.master" AutoEventWireup="true" CodeBehind="TabControl.aspx.cs" Inherits="Westwind.WebToolkit.Controls.TabControl" %>
<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <h1>TabControl</h1>

<div class="descriptionheader">
The TabControl class provides a very simple, client side handled
tab manager that is managed through an ASP.NET server control. The
control uses client script code to activate/deactivate HTML elements
on the page by mapping tabs to HTML elements which are hidden or
displayed. No dependencies on any external libraries.
</div>

<div class="toolbarcontainer">
    <a href="../" class="hoverbutton">Home</a> |
    <a href="TabControl.aspx" class="hoverbutton">Reload</a> |
    <ww:HelpControl ID="HelpControl" runat="server" HelpTopic="_2te177hra.htm" CssClass="hoverbutton" />
</div>

<div class="containercontent">

    <ww:TabControl runat="server" ID="TabControls" TabHeight="25" TabWidth="120" 
        TabstripSeparatorHeight="" >
        <TabPages>
            <ww:TabPage runat="server" ID="Page1" TabPageClientId="Page1" Caption="Page 1" />
            <ww:TabPage runat="server" ID="Page2" TabPageClientId="Page2" Caption="Page 2" />
            <ww:TabPage runat="server" ID="Page3" TabPageClientId="Page3" Caption="Page 3" />                            
        </TabPages>    
    </ww:TabControl>

    <div id="Page1" class="tabpage">
        Page 1
    </div>
    <div id="Page2" class="tabpage">
        Page 2
    </div>
    <div id="Page3" class="tabpage">
        Page 3
    </div>
</div>
</asp:Content>
