<%@ Page Title="" Language="C#" MasterPageFile="~/WestWindWebToolkit.master" AutoEventWireup="true" CodeBehind="AutoComplete.aspx.cs" Inherits="Westwind.WebToolkit.Ajax.AutoComplete" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">    
    <style type="text/css">        
        .ac_results ul
        {
            width: 100%;
            background: white;
            list-style-position: outside;
            list-style: none;
            padding: 0;
            margin: 0;
            overflow: hidden;
            overflow-y: auto;
            border: solid 1px steelblue;
        }
        .ac_results iframe
        {
            display: none; 
            display: block; 
            position: absolute;
            top: 0;
            left: 0;
            z-index: -1;
            filter: mask();
            width: 3000px;
            height: 3000px;
            overflow: hidden;
        }
        .ac_results li
        {
            margin: 0px;
            border-bottom: solid 1px silver;
            padding: 2px 5px;
            cursor: pointer;
            display: block;
            width: 100%;
            font: menu;
            font-size: 12px;
            overflow: hidden;
        }
        .ac_loading
        {
            background: Window url(../css/images/loading_small.gif) right center no-repeat;
        }
        .ac_over
        {
            background-color: Highlight;
            color: HighlightText;
        }
        
        #ctl00_MainContent_PageContent
        {
            min-height: 150px;
            width: 470px;
            height: 525px;
            margin: 30px auto;
        }        
        #divStockWrapper
        {
            border: solid 1px navy;
            background: cornsilk;
            display: none;    
            margin-top: 10px;
            padding: 10px;
            width: 450px;
        }
        #divStockDetail
        {
            margin: 10px 0px;
        }
        #divStockDetail td
        {    
            padding: 3px;
        }
        #divStockDetail td:first-child
        {
            text-align: right;
            font-weight: bold;
            width: 100px;
            padding-right: 15px;
        }
        #imgStockChart
        {
            display: none;    
            width: 450px;
        }                
</style>
</asp:Content>

<asp:Content ID="MainContent"  ContentPlaceHolderID="MainContent" runat="server">

    <h1>Using the jQuery AutoComplete Plugin</h1>
    
    <div class="toolbarcontainer">
        <a href="./" class="hoverbutton"><asp:Image ID="Image1" runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton"><asp:Image ID="Image2" runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page</asp:LinkButton> |         
        <small>Page created: <%= DateTime.Now.ToString() %></small><br />
    </div>
    
    <div class="descriptionheader">
    This example demonstrates how you can serve non-JSON data from a CallbackHandler. In this
    example plain string data is served to the 
    <a href="http://bassistance.de/jquery-plugins/jquery-plugin-autocomplete/">jQuery AutoComplete plug-in</a>
    and Stream data is used to serve the stock image graph.
    </div>
    
        
    
    <div class="contentcontainer"  runat="server" id="PageContent">
        <h2>Stock Quote Lookup</h2>
        
        <div class="labelheader" style="margin-top: 20px;">Enter a stock symbol:</div>
        <asp:TextBox runat="server" ID="txtSymbol"  />        
        <input type="button" id="btnGetQuote" value="Go" />
        <br />        
        
        <div id="divStockWrapper">
            <div id="divStockDetail">        
            </div>
            <img id="imgStockChart"  />
        </div>
    </div>
    
    
    
    <ww:AjaxMethodCallback runat="server" id="Proxy"  ServerUrl="AutoCompleteHandler.ashx"  />
    
    <script type="text/javascript">
        $(document).ready(function() {

            // set up AutoComplete
            var txtSymbol = $$("txtSymbol");
            txtSymbol
                .focus()
                .autocomplete("autocompletehandler.ashx?Method=GetStockLookup",
                             { width: 220, scrollHeight: 250,
                                 autofill: true,
                                 max: 100,        // max items
                                 cacheLength: 1,  // no cache of plugin - broken
                                 formatItem: function(item) {  // display for an item
                                     return "<div>" + item[0] + "</div>" +
                                            "<div style='margin-left:15px'>" + item[1] + "</div>";
                                 },
                                 formatResult: function(item) {   // the result value put into textbox
                                     return item[0];  // Symbol
                                 }
                             });

            // hook up event handlers for page
            $("#btnGetQuote").click(GetStockQuote);

            // set error display on 'status bar'
            showStatus({ afterTimeoutText: "Ready" });

            if (txtSymbol.val())
                GetStockQuote()

        });

        function GetStockQuote() {
            var symbol = $$("txtSymbol").val();
            if (!symbol) {
                showStatus("Please enter a symbol to lookup.", 5000, true);
                return;
            }

            $("#divStockWrapper").fadeTo(200, .11);

            // get the stock data to display and expand from template
            Proxy.GetStockQuote(symbol,
                    function(quote) {
                        // update the image
                        $("#imgStockChart")
                            .attr("src", "AutoCompleteHandler.ashx?method=GetStockHistoryGraph&symbol=" + symbol)
                            .show();

                        if (quote == null) {
                            showStatus("Invalid ticker symbol.", 5000, true);
                            $("#divStockWrapper").hide();
                            return;
                        }

                        // use client template to render the stock display
                        var stockTemplate = $("#StockItemTemplate").html();
                        var html = parseTemplate(stockTemplate, quote);

                        $("#divStockDetail")
                            .html(html);

                        $("#divStockWrapper").show().stop().fadeTo(1000, 1);

                    }, onPageError);                                        
        }


    </script>

    <script id="StockItemTemplate" type="text/html">
        <table id="tblStockDetail">
            <tr>
                <td>Company:</td>
                <td><#= Company #></td>
            </tr>
            <tr>
                <td>Last Price:</td>
                <td><#= LastPrice.formatNumber("n2") #> &nbsp; &nbsp; <b> <#= NetChange < 0 ? "<span style='color:red'> " + NetChange.formatNumber("n2") + "</span>" : "<span style='color: green'>+  " + NetChange.formatNumber("n2") + "</span>" #></b></td>
            </tr>
            <tr>
                <td>
                  Open Price:
                </td>
                <td>
                    <#= OpenPrice.formatNumber("n2")  #>  
                </td>
            </tr>
            <tr>
                <td>Quote Date:</td>
                <td><#= LastQuoteTime.formatDate("MMM dd, HH:mm ") #></td>
            </tr>
        </table>        
    </script>
   

    <div class="toolbarcontainer">
        <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
            displaystate="Button" codefile="AutoComplete.aspx" />
        <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
            displaystate="Button" codefile="AutoComplete.aspx.cs" />
        <manoli:viewsourcecontrol id="ViewSourceControl2" runat="server" text="Show AutoComplete Handler"
            displaystate="Button" codefile="AutoCompleteHandler.ashx.cs" />
    </div>    
    
    <ww:ScriptContainer ID="scripts" runat="server" RenderMode="Header">
        <Scripts>
            <script src="../Scripts/jquery.js" resource="jquery"></script>
            <script src="../Scripts/ww.jquery.js" resource="ww.jquery"></script>
            <script src="../scripts/ThirdParty/jquery.autocomplete.js" allowminscript="true"></script>
        </Scripts>
    </ww:ScriptContainer>
    
</asp:Content>
