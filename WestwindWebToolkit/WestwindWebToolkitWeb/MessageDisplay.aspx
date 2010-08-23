<%@ Page language="c#" Inherits="Westwind.WebToolkit.MessageDisplay" 
                       CodeBehind="MessageDisplay.aspx.cs"  
                       enableViewState="false"   AutoEventWireup="True" 
                       MasterPageFile="~/WestWindWebToolkit.master"
%>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>                       

<asp:Content runat="server" ContentPlaceHolderID="MainContent">

<h1><asp:label ID="lblHeader" runat="server" ></asp:label></h1>

<div class="containercontent" style="padding: 30px 90px 30px 50px; text-align: justify" >
   <div style="float: left; margin-right: 25px; margin-bottom: 20px;">
     <asp:Image runat="server" ImageUrl="~/images/WestWindWebToolkit.jpg" />
   </div>
   <asp:label ID="lblMessage" runat="server"></asp:label>
</div>          

<br clear="all" />
<div class="containercontent">
    <small><asp:label ID="lblRedirectHyperLink" runat="server"></asp:label></small>   
<hr />
<small>
<asp:Image runat="server"  ImageUrl="~/images/wwtoollogo_text.gif" align="right" />
&copy West Wind Technologies, 2008 - <%= DateTime.Now.Year %>
</small>

</div>

</asp:Content>
