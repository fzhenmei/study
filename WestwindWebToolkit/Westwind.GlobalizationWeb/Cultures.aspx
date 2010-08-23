<%@ Page Language="C#" AutoEventWireup="True" 
            Codebehind="Cultures.aspx.cs" 
            Inherits="Westwind.GlobalizationWeb.Cultures"                       
            meta:resourceKey="Page1"                                       
 %>        
<%-- <%@ OutputCache VaryByCustom="Culture" Duration="60" VaryByParam="none" %> --%>
<%@ Register Assembly="Westwind.Globalization" Namespace="Westwind.Globalization" TagPrefix="ww" %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cultures</title>
    <link href="Westwind.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1><asp:Label runat="server" ID="lblPageHeader" Text="Thread Culture Mapping" meta:resourcekey="lblPageHeader"></asp:Label></h1>
    
    <div class="toolbarcontainer">
         <asp:HyperLink runat='server' ID="lnkHome"  class="hoverbutton" Text="Back to Home Page" NavigateUrl="default.aspx" meta:resourcekey="lnkHome"></asp:HyperLink> |
        <asp:LinkButton ID="btnRefresh" runat="server" class="hoverbutton"  meta:resourcekey="btnRefresh" Text="Refresh Page"  ></asp:LinkButton> |     
        <asp:HyperLink ID="btnReload" runat="server" class="hoverbutton"  meta:resourcekey="btnReload" Text="Reload Page"  NavigateUrl="~/Cultures.aspx" ></asp:HyperLink> | 
            
        <small><asp:Label runat="server" ID="lblGeneratedAt" Text="Generated on:" meta:resourcekey="lblGeneratedAt"></asp:Label> <%=DateTime.Now %></small>
    </div>
    <br />
    <br />    
        <ww:ErrorDisplay ID="ErrorDisplay" runat="server" />

        <asp:DropDownList ID="radMapping" runat="server"
                          AutoPostBack="True" Width="220px" meta:resourcekey="radMapping"> 
            <asp:ListItem Selected="True" meta:resourcekey="ListItem" Value="Default">Default</asp:ListItem>
            <asp:ListItem Value="Browser" meta:resourcekey="ListItem1">Map Browser Language</asp:ListItem>
            <asp:ListItem Value="DropDown" meta:resourcekey="ListItem2">Language Selection</asp:ListItem>
        </asp:DropDownList><br />
    <div style="padding:20px;">
        &nbsp;<table cellpadding="7" class="blackborder">

	<tr>
		<td style="WIDTH: 151px"><strong><asp:Label runat="server" ID="lblThreadCulture" meta:resourcekey="lblThreadCulture" Text="Thread Culture:"></asp:Label></strong>
		</td>
		<td>
			<b><%= System.Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag %></b>
		</td>
	</tr>
	<tr>
		<td style="WIDTH: 151px"><strong><asp:Label runat="Server" ID="lblBrowserLanguage" Text="Browser Language:" meta:resourcekey="lblBrowserLanguage"></asp:Label> </strong>
		</td>
		<td>
			<%= Request.UserLanguages[0] %>
		</td>
	</tr>

	<tr>
		<td style="WIDTH: 151px"><strong><asp:Label runat="Server" id="lblDateTime" Text="Date Time:" meta:resourcekey="lblDateTime"></asp:Label></strong>
		</td>
		<td>
			<%= DateTime.Now %>
		</td>
	</tr>
	<tr>
		<td style="WIDTH: 151px" valign="Top"><strong><asp:Label runat="Server" id="lblNumeric" Text="Numeric Value:" meta:resourcekey="lblNumeric"></asp:Label></strong>
		</td>
		<td>
			<%= 5231.22M %>
		</td>
	</tr>
	<tr>
		<td style="WIDTH: 151px" valign="Top"><strong><asp:label runat="server" ID="lblCurrency" Text="Currency Value:" meta:resourcekey="lblCurrency"></asp:label></strong>
		</td>
		<td>
			<%= 5231.22M.ToString("c") %> <br />
		</td>
	</tr>		
</table>
        <br />

<hr />
<asp:Label runat="server" ID="lblSelectLanguage" meta:resourcekey="lblSelectLanguageResource1">Select Language:</asp:Label>
<asp:DropDownList runat="server" ID="lstLanguage" AutoPostBack="True" width="150px"
                  OnSelectedIndexChanged="lstLanguage_SelectedIndexChanged" meta:resourcekey="lstLanguageResource1" >
        <asp:ListItem Text="Default" meta:resourcekey="ListItemResource1"></asp:ListItem>
        <asp:ListItem Value="en-US" Text="English" meta:resourcekey="ListItemResource2"></asp:ListItem>
        <asp:ListItem Value="de-DE" Text="German" meta:resourcekey="ListItemResource3"></asp:ListItem>
        <asp:ListItem Value="fr-FR" Text="French (no UI Translation)" meta:resourcekey="ListItemResource4"></asp:ListItem>
</asp:DropDownList>
<br />
<br />

<!-- Popup Window for the 'Language Change' Selection -->
<ww:ModalDialog ID="panelLanguageRefresh" runat="server" 
           cssClass="blackborder" 
           BackgroundOpacity="0.60"
           ShadowOffset="6"
           Draggable="True" DragHandleID="LanguageNoteHeader"
           style="display:none;width:500px;background:cornsilk;" 
           EnableViewState="False" meta:resourcekey="panelLanguageRefresh" OnClientCloseHandler=""
          >
<div class='gridheader' runat="server" id="LanguageNoteHeader">        
    <asp:Label runat="server" ID="lblLanguageNoteHeader" Text="UI Culture has been changed" meta:resourcekey="lblLanguageNoteHeader"></asp:Label>
</div>
<div style="padding:15px;">
    <asp:Label runat="server" id="lblLanguageNote" Text="" meta:resourcekey="lblLanguageNote"></asp:Label>
    <hr />
    <asp:Button runat="server" ID="btnRefreshLanguage" Text="Refresh now" meta:resourcekey="btnRefreshLanguage" />
</div>    
</ww:ModalDialog>

</div>
    
   <!-- Add this to localize this page with the wwDbResourceProvider -->    
  <ww:DbResourceControl id="DbResourceControl1" runat="server" 
                          cssclass="errordisplay" 
                          Width="250px"
                          ShowIconsInitially="DontShow"                           
                          >
  </ww:DbResourceControl>
</div>
</form>
</body>
</html>
