<%@ Page Language="C#" MasterPageFile="~/WestWindWebToolkit.master" AutoEventWireup="True" 
         CodeBehind="JsonStockClient.aspx.cs" 
         Inherits="Westwind.WebToolkit.JsonStockClient" 
         Title="Raw Json Stock Service"
         EnableViewState="False" %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>

<asp:Content ID="Header" ContentPlaceHolderID="Header" runat="server">
    <link href="JsonStockClient.css" rel="stylesheet" type="text/css" />         
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">    
    <div class="descriptionheader">
    This page demonstrates a host of Ajax features. It uses AjaxMethodCallback for all data retrieval.
    The Portfolio list is rendered with client side templates using parseTemplate from ww.jquery.js. 
    Demonstrates item editing via popups and using Ajax callbacks to force images to reload on the
    server. 
    <br /><br />
    <b>Please Login and use <i>ww</i> as the password to see/update the Stock Portfolio list</b>
    </div>
    <div class="toolbarcontainer">
        <a id="HomeLink" href="../" class="hoverbutton"><img src="../../css/images/home.gif" alt="Home" /> Home</a> |
        <a id="ReloadLink" href="JsonStockClient.aspx" class="hoverbutton"><img src="../../css/images/refresh.gif" alt="Reload"/> Reload Page</a> | 
        <asp:LinkButton runat="server" id="btnLogout" visible="false" OnClick="btnLogout_Click" class="hoverbutton">Logout</asp:LinkButton>&nbsp; 
       
        <span runat="server" id="LoginGroup">                
            Username: <asp:TextBox runat="server" ID="txtUsername" >rstrahl@west-wind.com</asp:TextBox> | 
            Password: <asp:TextBox runat="server" id="txtPassword" TextMode="Password" ></asp:TextBox>  <small>(ww)</small>
            <asp:Button runat="server" id="btnLogin" Text="Go" />
        </span> 
        <span runat="server" id="LoginName"></span>
    </div>    

    
    <br clear="all" />
    <br clear="all" />

    <div id="divContentContainer">
    
    <%-- Single Stock Quote Box --%>
    <div id="divStockQuote" class="grouping">
    <fieldset>
    <legend>Get a Stock Quote</legend>
        <div id="divStockQuotePadder">
            Symbol: 
            <input id="txtSymbol" type="text" value="" style="width:  80px;"  onkeyup="this.value=this.value.toUpperCase();SymbolLookup(this.value,'lblStockName');"/>            
            <input type="button" id="btnTest" value="Get Quote" onclick="GetStockQuote();" />   
            <img id="imgStockProgress" class="smallprogressright"/>
            <span id="lblStockName"></span>
                        
            <div id="divStockQuoteResult">
                <img id="imgStockQuoteGraph" src="" />
                <table cellpadding="5">
                    <tr>
                        <td>Company:</td>
                        <td id="StockName">n/a</td>
                    </tr>
                    <tr>
                        <td>Last Price:</td>
                        <td id="LastPrice">n/a</td>                    
                    </tr>
                    <tr>
                        <td>Open Price:</td>
                        <td id="OpenPrice">n/a</td>
                    </tr>
                    <tr>
                        <td>Net Change:</td>
                        <td id="NetChange" style="font-weight: bold">n/a</td>
                    </tr>
                    <tr>                    
                        <td>Quote Time:</td>
                        <td id="QuoteTime">n/a</td>
                    </tr>
                </table>                
            </div>      
        </div>        
    </fieldset>
    </div>
    <%-- End Single Stock Quote Box --%>
    <br />    
    
    <div id="divPortfolio" class="grouping">
    <fieldset>
    <legend>Your Stock Portfolio</legend>
    
    <div class="toolbarcontainer">    
            <div id="divProgress"  class="smallprogressright" style="display:none"></div>    
            <a href="" class="hoverbutton"  onclick="ShowStockEditWindow();return false;" ><img src="../../css/images/new.gif" /> New</a> </small>
            <a href="javascript:{}" class="hoverbutton" onclick="LoadQuotes();"><img src="../../css/images/refresh.gif">Refresh</a>
            <a href="javascript:{}" class="hoverbutton" onclick="ShowHistoryGraph(event);return false;"><img src="../../images/graph.gif" />Graph Portfolio</a>
            <%= string.IsNullOrEmpty(this.Username) ? "<hr><small>Please log in first to show or edit your portfolio</small>" : "" %>
     </div>

    <!-- The Stock Portfolio Detail List goes here -->                    
    <div id="lstPortfolioContainer" >   
        <!-- This would be the equivalent ListView Code. Not used Client Templating used instead -->
        <!-- 
        <asp:ListView runat="server" ID="lstPortfolio" ItemPlaceholderID="PortfolioItemTemplate">
            <LayoutTemplate>
             <div id="PortfolioItemTemplate" runat="server" />     
            </LayoutTemplate>  
            <ItemTemplate>
              <div id="<%# Eval("Pk") %>_STOCK" class="itemtemplate" onclick="ShowStockEditWindow(this);">
                <div class="stockicon"></div>
                <div class="itemtools">       
                <a href='javascript:{}' class="hoverbutton" onclick="DeleteQuote(this.parentNode.parentNode,event);return false;">
                <img src="../../css/images/remove.gif" /></a>
                </div>
                <div class="itemstockname"><%# Eval("Symbol") %> - <%# Eval("Company") %></div>
                <div class="itemdetail">
                    <table style="padding: 5px;"><tr>
                        <td>Last Trade:</td>
                        <td id="tdLastPrice" class="stockvaluecolumn"><%# Eval("LastPrice","{0:n2}") %></td>
                        <td>Qty:</td>
                        <td id="tdLastQty" class="stockvaluecolumn"><%# Eval("Qty","{0:n0}") %></td>
                        <td>Holdings:</td>
                        <td id="tdItemValue" class="stockvaluecolumn"><%# Eval("ItemValue","{0:c}") %></td>
                        <td colspan="2">as of: <%# Eval("LastDate","{0:MMM dd, hh:mmtt}").ToString() %></td>
                    </tr></table>
                </div>
              </div>
            </ItemTemplate>      
        </asp:ListView>
        -->            
    </div>
    
    <div id="divPortFolioStatus" class="toolbarcontainer">
      <div id="divPortfolioTotal" style="float:right; padding-right: 25px;">Total Portfolio Value: <span id="spanPortfolioTotal" class="errormessage"><%= this.Portfolio.TotalValue.ToString("c") %></span></div>
      <div id="divPortfolioCount"><%= this.Portfolio.TotalItems %> items</div>
    </div>        

    
    <div id="StockEditWindow" class="dialog gridalternate">    
        <div class="dialog-header">Add new Portfolio Item</div>
        <div style="padding: 15px">
            Symbol: <input type="text" id="txtPortfolioSymbol"  style="width: 80px;" 
                            onkeyup="this.value = this.value.toUpperCase();SymbolLookup(this.value,'StockEditDetail')"                             
                            />&nbsp;&nbsp;&nbsp;            
            Qty: <input type="text" id="txtPortfolioQty" style="width: 50px;" />
        <div id="StockEditDetail" style="margin: 10px 0px 10px 0px;">        
        </div>
            <input type="button" id="btnAddStock" value="Add" style="width: 75px" onclick="UpdatePortfolioQuote();"/>
            <input type="button" id="btnCloseStockWindow" value="Close" style="width: 75px" onclick="$('#StockEditWindow').fadeOut();"  />
        </div>
    </div>
    
    <center>    
    <img id="imgPortfolioHistory" src="" style="display:none;"  />    
    
    <div id="divAcknowledgements" style="padding: 8px; font-size: 8pt;">
    Stock data courtesy of <a href="http://finance.yahoo.com/">finance.yahoo.com</a>. Graphs generated with <a href="http://www.zedgraph.org">ZedGraph</a>
    </div>
    </center> 
    
    </fieldset>
    </div>
    
    </div>  
    <!-- End divContentContainer -->
    
    <div class="toolbarcontainer">
        <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
            displaystate="Button" codefile="JsonStockClient.aspx" />
        <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
            displaystate="Button" codefile="JsonStockClient.aspx.cs" />
        <manoli:viewsourcecontrol id="ViewSourceControl2" runat="server" text="Javascript"
            displaystate="Button" codefile="JsonStockClient.js" />
        <manoli:viewsourcecontrol id="ViewSourceControl3" runat="server" text="Ajax Service Handler"
            displaystate="Button" codefile="JsonStockService.ashx.cs" />            
            
    </div>
    
    <div class="containercontent">
    &nbsp;
    </div>

    <div id="divStatusBar" class="statusbar">
    Ready
    </div>
    
   <!-- Using parseTemplate function in ww.jquery.js based on 
        John Resig's Micro Templating Engine to embed template values/JavaScript 
   --> 
    <script type="text/html" id="StockItemTemplate">
    <div class="itemtemplate" style="display:none">   
        <div class="stockicon"></div>    
        <div class="itemtools">
            <a href="javascript:{}" class="hoverbutton" onclick="DeleteQuote(this.parentNode.parentNode,event);return false;"><img src="../../css/images/remove.gif" /></a>
        </div>
        <div class="itemstockname"><#= Symbol #> - <#= Company #></div>
        <div class="itemdetail">
            <table style="padding: 3px;"><tr>
                <td>Last Trade:</td>
                <td id="tdLastPrice" class="stockvaluecolumn"><#= LastPrice.formatNumber("n2") #></td>
                <td>Qty:</td>
                <td id="tdLastQty" class="stockvaluecolumn"><#= Qty #></td>
                <td>Holdings:</td>
                <td id="tdItemValue" class="stockvaluecolumn"><#= ItemValue.formatNumber("c") #></td>                
                <td id="tdTradeDate" colspan="2"><#= LastDate.formatDate("MMM dd, hh:mmt")#></td>
            </tr></table>
        </div>
    </div>
    </script>    

   <!-- global variables that require script evaluation only -->    
   <script type="text/javascript">
   // Pick up the user token this login in script code
   // and make it globally available to the page and script code
   var userToken = "<%= this.UserToken %>";          
  
   </script>


    <ww:AjaxMethodCallback runat="server" ID="Proxy" 
                           ServerUrl="JsonStockService.ashx" 
                           />
    
    <ww:ScriptContainer ID="ScriptContainer" runat="server" RenderMode="Header">
        <Scripts>
            <script src="~/scripts/jquery.js" type="text/javascript" Resource="jquery" RenderMode="HeaderTop"></script>            
            <script src="~/scripts/jquery-ui.js" type="text/javascript" AllowMinScript="true" ></script>  
            <script src="~/scripts/ww.jquery.js" type="text/javascript" Resource="ww.jquery"></script>
            <script src="JsonStockClient.js" type="text/javascript"></script>
        </Scripts>
    </ww:ScriptContainer>
    

</asp:Content>
