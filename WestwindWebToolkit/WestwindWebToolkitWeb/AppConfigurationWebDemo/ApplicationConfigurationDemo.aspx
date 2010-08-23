<%@ Page language="c#" AutoEventWireup="true" 
         Inherits="Westwind.WebToolkit.ApplicationConfigurationDemo" 
         Codebehind="ApplicationConfigurationDemo.aspx.cs"
         MasterPageFile="~/WestWindWebToolkit.master"         
         Title="ApplicationConfiguration Class Demo"         
%>

<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content runat="server"     ID="content" ContentPlaceHolderId="MainContent">
    <div class="toolbarcontainer">
        <div class="hoverbutton rightalign"><ww:HelpControl ID="HelpControl" runat="server" HelpTopic="_2le027umn.htm" /></div>
        <a href="../default.aspx" class="hoverbutton"><asp:Image runat="server" ImageUrl="~/css/images/home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
            <asp:Image runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
        </asp:LinkButton> |         
        <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
    </div>
    <div class="descriptionheader">
    The ApplicationConfiguration class lets you use a class to represent configuration
    settings that can be persisted in a variety of ways. The default mode persists in
    web.config and provide strongly typed object properties for settings. You can also
    save to external files, a database record or a string. Effectively you can access
    App.Configuration.Property from anywhere in your application.
    <br />
    <br />
    Please note that in order to run this demo effectively you need to be able to update
    settings in web.config and external files in the Web folder since it tries to write
    settings. Writing of config settings is optional but quite useful and for this demo
    it's recommended you give full access to NETWORK SERVICE (or whatever your IIS host account is).    
    </div>

<div class="containercontent">
    <blockquote>
    
	<asp:dropdownlist id="txtSource" runat="server" width="288px" autopostback="True">
		<asp:listitem value="Default Web.Config">Default Web.Config</asp:listitem>
		<asp:listitem value="Different Web.Config Section">Different Web.Config Section</asp:listitem>
		<asp:listitem value="Different .Config File">Different .Config File</asp:listitem>
		<asp:listitem value="Simple Xml File">Simple Xml File</asp:listitem>
		<asp:listitem value="String">String</asp:listitem>
		<asp:listitem value="Database">Database</asp:listitem>
	</asp:dropdownlist>
	<hr>
	</blockquote>
	
	
	<div style="MARGIN-LEFT:40px">
        <ww:ErrorDisplay ID="ErrorDisplay" runat="server" />


<%--	<table cellpadding="15"><tr><td valign="top">
	<asp:Image runat="server" ID="imgIcon" ImageUrl="~/css/images/info.gif" /></td>
	<td><asp:Label runat="Server" ID="lblMessage" ></asp:Label></td></tr></table>
	<br />--%>
	
		<%= this.ShowPropertyGrid(this.AppConfig) %>
		<p>
        <asp:button id="btnSaveSettings" runat="server" text="Save Settings"  AccessKey="S"
                    OnClick="btnSaveSettings_Click" class="submitbutton"></asp:button></p>
	</div>
	
	<br />
	<br />
	
    <div class="toolbarcontainer">
        <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
            displaystate="Button" codefile="ApplicationConfigurationDemo.aspx" />
        <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
            displaystate="Button" codefile="ApplicationConfigurationDemo.aspx.cs" />
        <manoli:viewsourcecontrol id="ViewSourceControl2" runat="server" text="Show Configuration Class"
            displaystate="Button" codefile="~/_classes/Configuration/SampleConfiguration.cs" />
    </div>
	
</div>
</asp:Content>