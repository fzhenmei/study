<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebRequestLog.aspx.cs" Inherits="Westwind.WebToolkit.Admin.WebRequestLog" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Web Request Log Viewer</title>
    <link href="../css/Westwind.css" rel="stylesheet" type="text/css" />
    <script src="WebRequestLog.js" type="text/javascript"></script>

    <style type="text/css">
    .logitem 
    {
    	margin: 2px;
    	padding: 9px;    	
    	border-bottom: dotted 1px teal;    	
    	font-size: .825em;
    }    
    .logitem:hover
    {
    	background: url(../css/images/lightorangegradient.png);
    	background-repeat: repeat-x;
    }
    .busyiconright
    {
    	float:right;
    	width: 20px; 
    	height: 20px;
    	background-image: url(../css/images/loading_small.gif);
    	background-repeat: no-repeat;
    }
    .infoicon,.erroricon,.warningicon,.messageicon
    {
    	background-image: url(../css/images/info.gif);
    	background-position: left center;
    	background-repeat: no-repeat;  
    	width: 20px;
    	height: 20px;
    	float: left;  	
    	margin-right: 10px;    	
    }
    .itemgroup 
    {
    	padding-top: 8px;
    	clear: both;    	
    }
    .itemgroup td { font-size: .8em; vertical-align: top; }
    .itemlabel
    {    	   	
    	width: 80px;    	
    	font-weight: bold;
    	overflow:hidden;        	
    }
    pre { font: normal 8pt verdana; overflow-x:visible; }
    .erroricon { background-image: url(../css/images/warning.gif); }
    .warningicon { background-image: url(../css/images/warning.gif); }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>

     <h1>Web Request Log Viewer</h1>
     <div class="toolbarcontainer">
        <a href="./" class="hoverbutton"><img src="../css/images/home.gif" /> Home</a> |
        <a href="WebRequestLog.aspx" class="hoverbutton"><img src="../css/images/refresh.gif" /> Refresh</a> |        
        <asp:LinkButton runat="server" ID="btnPurge" 
             class="hoverbutton" onclick="btnPurge_Click"><img src="../css/images/remove.gif" /> Purge Log</asp:LinkButton> |
        <asp:LinkButton runat="server" ID="btnCauseError"  OnClick="btnCauseError_Click"
             class="hoverbutton" >Cause an error</asp:LinkButton> |
        <small>Log Mode: <%= Westwind.Utilities.Logging.LogManagerConfiguration.Current.LogAdapter %></small>
        
        
     </div>
      
      <ww:ErrorDisplay runat="server" ID="ErrorDisplay" />
            
      
      <div class="blackborder" style="width: 800px;margin: 30px auto 0;">          
          <div class="gridheader">Log Items</div>
          <div class="toolbarcontainer">
                 <div class="busyiconright" style="display:none" id="divListProgress"></div>
                 
                 Filter: 
                 <asp:DropDownList runat="server" ID="lstLogType" onchange="loadLogEntries(event);">
                 </asp:DropDownList> |
                 Show Last: <asp:TextBox runat="server" ID="txtCount" Text="150"  
                                     ToolTip="Number of last logged items to show"  
                                     style="width: 50px;text-align: right;" /> |
                 From: <ww:jQueryDatePicker runat="server" ID="txtDateFilter" />
                 To:  <ww:jQueryDatePicker runat="server" ID="txtDateFilter2" /> |
                 
                 <input type="button" id="btnShow" value="  Go  " onclick="loadLogEntries(event);" />
          </div>
          <div id="divListWrapper" style="height: 500px;overflow-y:scroll">
          
          </div>
          <div id="divListStatus" class="toolbarcontainer">Ready</div>
      </div>
    </div>
    
    
    <ww:HoverPanel ID="LogItemPanel" runat="server" 
        CssClass="dialog" 
        style="display:none;overflow: hidden;max-width: 650px;min-width: 300px; min-height: 200px;"
        Draggable="true" DragHandleID="LogItemPanelHeader"
        Closable="true"
        ShadowOffset="4"
        AutoCloseHoverWindow="false"
    >
    <div id="LogItemPanelHeader" class="dialog-header">Log Item Detail</div>    
    <div  id="divItemDetailDisplay" class="dialog-content dialog-bottom">       
    </div>
    </ww:HoverPanel>
    
    
    <script id="LogItemTemplate" type="text/html">
    <div id="log_<#= Id #>" class="logitem">
        <div class="logicon"></div>
        <div style="float: right;text-align: right;">
            <#= Entered.formatDate("MMM. dd @ HH:mm:ss") #><br />
            <#= RequestDuration == -1 ? "" : RequestDuration.formatNumber("999,999.999") + "ms" #>
        </div>
        <b><#= Message #></b>
        <# var curUrl = Url + (QueryString ? '?' + QueryString : ''); #>
        <div><a href='<#= curUrl #>' target='_blank'><#= curUrl #></a></div>        
    </div>
    </script>
    
     <!-- template to load data into -->
        <script id="LogDetailDisplayTemplate" type="text/html">
            <div class="itemgroup">
                <h3><#= Message #></h3>
                <a href="<#= Url #>"><#=  Url #></a><br />
                on <#= Entered.formatDate("MMM. dd, yyyy HH:mmy") #>
            <div>
                        
            <div class="itemgroup">
                <#= Details #>
            </div>
                        
            <hr />             
            
            <table class="itemgroup">            
                <tr>
                <td class="itemlabel">IP Address:</td>
                <td><#= IpAddress #><td>
                </tr>
                
                <tr>    
                <td class="itemlabel">Referrer:</td>
                <td><#= Referrer #> </td>
                </tr>
                
                <tr>
                <td class="itemlabel">User Agent:</td>
                <td><#= UserAgent #></td>                
                </tr>
            </table>
            
            
            <# if (PostData) { #>
            <table class="itemgroup" ><tr>
                <td class="itemlabel">Post Data:</td>
                <td><#= PostData #></td>                
            </tr></table>            
            <# } #>
            
            <# if (ErrorLevel == 2) { #>
            <table class="itemgroup">
                <tr>
                <td class="itemlabel">Error Type: </td>
                <td><#= ErrorType #></td>
                <tr>
                <tr>
                <td class="itemlabel">StackTrace:</td>
                <td><#= StackTrace #></div>
                </tr>
            </table>           
             
            <# } #>
        </script>    
    
    <ww:AjaxMethodCallback runat="server" ID="Proxy" PageProcessingMode="PageInit"  JsonDateEncoding="MsAjax">
    </ww:AjaxMethodCallback>
    
    </form>
</body>
</html>
