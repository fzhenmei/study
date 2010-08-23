<%@ Page Language="C#" MasterPageFile="~/TimeTrakkerMaster.Master" 
         CodeBehind="Default.aspx.cs" Inherits="TimeTrakkerWeb.Default" AutoEventWireup="false"
         Title="Time Trakker" %>

<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
.hoverbutton 
{	
	width: 220px;
	display: block;	
	font-size: 12pt;		
}
.dialog a
{
    font-family: Verdana;
	font-size: 12pt;	
	font-weight: bold;
	color: navy;	    	
}
.hoverbutton img
{	    
    margin-right: 10px;    	
}
</style>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="Content" runat="server">
    <center>
    <ww:ErrorDisplay ID="ErrorDisplay" runat='server' Width="350" />
    <div class="dialog" style="text-align:left; width: 400px;">
       <div runat="server" ID="divMenuHeader" class="dialog-header">Time Trakker Main Menu</div>
       <div style="padding: 40px;padding-left: 80px;">       
       <a href="Punchin.aspx" class="hoverbutton"><img src="images/punchin.gif" align="left" />Punch In</a>
       <a href="OpenEntries.aspx" class="hoverbutton"><img src="images/punchout.gif"  align="left"/> Punch Out</a>
       <br />
       <a href="BrowseEntries.aspx" class="hoverbutton"><img src="images/ShowEntries.png" align="left"/>Browse Entries</a>
       <br />       
       <a href="TimeReport.aspx" class="hoverbutton"><img src="images/reports.gif"  align="left"/>Time Report</a>
       <br />
       <br />
       <asp:HiddenField runat="server" ID="txtValue" />
       </div>
   </div>
   </center>
</asp:Content>
