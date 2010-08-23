<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="TimeSheetReport.ascx.cs" 
            Inherits="TimeTrakkerWeb.TimeSheetReport"             
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href='~/App_Themes/Standard/Standard.css' 
          rel="stylesheet" type="text/css"  runat="server" />   
    <style type="text/css">
     .itemcontainer
     {
     	margin: 10px;
     	border-bottom: dashed 1px teal;
     }
     .groupheader
     {
        font-size: 12pt;
     }
     .itemheader    
     {
     	font-size: 11pt;
     	font-weight: bold;
     	color: Navy;
     	margin-botton: 5px;
     }
    </style>
</head>
<body style="background: gray;">
<center>
<div style="text-align:left; background: white;width: 800px; border: solid 1px silver;padding: 30px;">

<div style="float: right;margin-right: 10px; margin-top: 10px; text-align:right;">
    From: <%= this.Parameters.FromDate.ToString("d") %><br />
    To: <%= this.Parameters.ToDate.ToString("d") %>
</div>
<div class="bigtext">
    <%= App.Configuration.CompanyName  %><br />
    <span style="font-size: 14pt; font-weight: normal;">Time Report</span>
</div>
<hr />

<asp:ListView runat="server" id="lstReport" ItemPlaceholderID="layoutContainer"  >
<LayoutTemplate>    
    <div id="layoutContainer" runat="server" />
</LayoutTemplate>
<ItemTemplate>    
    
    <%# this.RenderProjectGroupHeader(Container.DataItem as ReportEntryItem) %>
    <div class="itemcontainer">    
    <div class="itemheader"><%# Eval("Entry.Title") %></div>        
    <table width="90%" cellpadding="5">
    <tr>
    <td valign="top" style="width:130px;">
        <small><%# TimeUtils.ShortDateString((DateTime) Eval("Entry.TimeIn"),true) %><br />
        <%# TimeUtils.ShortDateString((DateTime) Eval("Entry.TimeOut"),true) %></small>
    </td>
    <td valign="top"><%# Eval("Entry.Description") %></td>
    <td valign="top" style="width: 70px;"><%# TimeUtils.FractionalHoursToString( (decimal) Eval("Entry.TotalHours"), "{0}h {1}min" ) %></td>
    </tr>
    </table>        
    </div>
    <%# this.RenderProjectFooter(Container) %>    
</ItemTemplate>
</asp:ListView>

<hr />
<%= App.Configuration.ReportFooter %>
</div>
</center>
</body>
</html>