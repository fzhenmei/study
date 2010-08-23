<%@ Page language="c#" Inherits="$safeprojectname$.MessageDisplay" 
                       CodeBehind="MessageDisplay.aspx.cs"  
                       enableViewState="false"   AutoEventWireup="True"                        
%>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>                       
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="PageHeader" runat="server">
    <title></title>
    <link href="Css/Westwind.css" rel="stylesheet" type="text/css" />
</head>
<body>
<h1>
       <asp:label ID="lblHeader" runat="server" ></asp:label>
</h1>

<div class="containercontent" >
    <asp:label ID="lblMessage" runat="server"></asp:label>
</div>          

<div class="containercontent">
    <small><asp:label ID="lblRedirectHyperLink" runat="server"></asp:label></small>

<hr />
<small>
<asp:Image runat="server"  ImageUrl="~/images/wwtoollogo_text.gif" align="right" />
&copy West Wind Technologies, 2008 - <%= DateTime.Now.Year %>
</small>

</div>

</body>
</html>
