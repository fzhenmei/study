/// <reference path="~/scripts/jquery.js" />
/// <reference path="~/scripts/ww.jquery.js" />

$(document).ready(function() {

    if (userToken)
        LoadQuotes();

    $("#StockEditWindow").shadow();
});

function GetStockQuote()
{       
    var symbol = $("#txtSymbol").val();            
    if (!symbol)
    {
        showStatus("Please provide a symbol value.",5000);
        return;
    }    
    showStockProgress();

    Proxy.GetStockQuote(symbol,
        function(result) {
            
            if (!result) {
                showStatus("Invalid Stock Symbol", 4000);
                showStockProgress(true);
                return;
            }
            showStockProgress(true);

            $("#StockName").text(result.Company + " (" + result.Symbol + ")");
            $("#LastPrice").text(result.LastPrice.toFixed(2));
            $("#OpenPrice").text(result.OpenPrice.toFixed(2));
            $("#QuoteTime").text(result.LastQuoteTimeString);
            $("#NetChange").text(result.NetChange.toFixed(2));
            if (result.NetChange < 0.00)
                $("#NetChange").css("color", "red");
            else
                $("#NetChange").css("color", "green");

            // if hidden make visible
            var sr = $("#divStockQuoteResult:hidden").slideDown("slow");

            // Also graph it
            var stocks = [];
            stocks.push(result.Symbol);
            var url = GetStockGraphUrl(stocks, result.Company, 350, 150, 2);

            $("#imgStockQuoteGraph").attr("src", url);
        },
        OnPageError);         
}

// Var that tracks Edit State - if non-0 we're editing
var editPk = 0;

// Display the Window 
function ShowStockEditWindow(stockItemElement)
{
    // Make it visible
    var win = $("#StockEditWindow").show().draggable();    
    
    var txtSymbol = $("#txtPortfolioSymbol");
    var txtQty = $("#txtPortfolioQty");

    if(stockItemElement)        
    {        
        // Grab the Item Template
        var jItem = $(stockItemElement);

        // Grab Pk from Id of the item template
        editPk = parseInt(jItem[0].id.replace("_STOCK",""));

        // Pick up values from the selected template item - a little parsing
        var symbol = jItem.find(".itemstockname").text();                    
        symbol = symbol.substr(0,symbol.indexOf("-")-1);
        txtSymbol.val(symbol);            
        txtQty.val( jItem.find("#tdLastQty").text() );                                   

        // Adjust the text of the dialog                
        $("#btnAddStock").val( "Update");
        win.find(".gridheader").text("Update Portfolio Item");        
    }            
    else
    {
        editPk = 0;   // No item editing 
                
        // Clear out
        txtSymbol.val("");
        txtQty.val("");        
        
        // Adjust the text of the dialog                
        $("#btnAddStock").val("Add");
        $(win).find(".gridheader").text("Add new Portfolio Item");        
    }
    txtSymbol[0].focus();            
}
function LoadQuotes(noMsg)
{    
    if (!userToken) {
        if (!noMsg) 
            showStatus("Please sign in before loading quotes.");
        return;   // not logged in
    }

    showProgress();

    Proxy.GetPortfolioItems(userToken, true,
         function(message) {
             showProgress(true);
             $("#lstPortfolioContainer").empty();

             $.each(message.Items, function(i) {
                 var item = this;   // this is the iterated item!

                 // Create a new node from the template by cloning
                 //                 var newEl = $("#StockItemTemplate")
                 //                                .clone()
                 //                                .attr("id", item.Pk + "_STOCK")
                 //                                .fadeIn("slow");

                 // dump the data into it
                 var newEl = UpdatePortfolioItem(item);

             });

             // Update totals    
             $("#spanPortfolioTotal").text(message.TotalValue.formatNumber("c"));
             $("#divPortfolioCount").text(message.TotalItems.formatNumber("f0") + " items");
         },
         function(error) { alert(error.message); });
         //OnPageError);
}
// Add Portfolio Item button click
function UpdatePortfolioQuote() {
    var symbol = $("#txtPortfolioSymbol").val();    
    var qty = $("#txtPortfolioQty").val();
        
    if (symbol=="")
    { showStatus("Please enter a symbol.",3000); return; }
    if (qty == "")
    { showStatus("Please enter a quantity.",3000); return; }

    qty = qty * 1.0;
    
    // Token is user identifier
    var token = "rstrahl@west-wind.com";
    
    showProgress();
    
    if (editPk == 0)
        Proxy.AddPortfolioItem(
                 symbol,qty, userToken,
                 UpdateQuote_Callback,OnPageError);
    else
        Proxy.UpdatePortfolioItem(
                 editPk, symbol, qty, userToken,
                    UpdateQuote_Callback,OnPageError);
    
}

// Callback for add operation adds or redisplays
// the item in the list
function UpdateQuote_Callback(portfolioMessage)
{
    showProgress(true);
    
    if (portfolioMessage == null)
    {
        showStatus("Unable to update portfolio item.",5000);
        return;
    }
    
    // Grab single portfolio item from message
    var item = portfolioMessage.SingleItem;
    
    // Update the item
    UpdatePortfolioItem(item,true);
    
    // Update totals    
    $("#spanPortfolioTotal").text( portfolioMessage.TotalValue.formatNumber("c") );
    $("#divPortfolioCount").text( portfolioMessage.TotalItems.formatNumber("f0") + " items");

    if (editPk == 0) {
        var el = $("#lstPortfolioContainer").get(0);
        el.scrollTop = el.scrollHeight;
    }

    $("#StockEditWindow").hide();
    editPk = 0;    
}
function UpdatePortfolioItem(stock,highlight) {
    
    // Retrieve the Item template
    var template = $("#StockItemTemplate").html();
    
    // Parse the template and merge stock data into it
    var html = parseTemplate(template, stock);

    // Create jQuery object from gen'd html
    var newItem = $(html);

    // See if we have an existing item
    var origItem = $("#" + stock.Pk + "_STOCK");    
    

    if (origItem.length < 1) {    
        // Add a new item
        newItem.appendTo("#lstPortfolioContainer");
    }
    else {
    
        // Insert after original then remove original
        origItem.after(newItem).remove()                   
    }

    newItem
      .attr("id", stock.Pk + "_STOCK")
      .click(function() { ShowStockEditWindow(this) })
      .show();
      

    if (highlight) {
        newItem
            .addClass("pulse")
            .effect("bounce", { distance: 15, times: 3 }, 400);
        setTimeout(function() { newItem.removeClass("pulse") }, 1200);
    }
       

    return newItem;
}
function DeleteQuote(item,ev)    
{           
    var Pk = item.id.replace("_STOCK","");                
    
    ev.cancelBubble = true;

    Proxy.DeletePortfolioItem(Pk * 1.0, userToken,
            function(portfolioMessage) {
                // Update totals
                $("#spanPortfolioTotal").text(portfolioMessage.TotalValue.formatNumber("c"));
                $("#divPortfolioCount").text(portfolioMessage.TotalItems.formatNumber("f0") + " items");

                $(item).fadeOut(function() { $(this).remove() });   // jquery remove from dom

                showStatus("Portfolio Item deleted.", 2000);
            },
            OnPageError);
}   
function ShowHistoryGraph(event)
{
    event.cancelBubble = true;
 
    var jImg = $("#imgPortfolioHistory");
    
    showProgress();

    Proxy.GetPortfolioItems(userToken,false,
                function(portfolioMessage) {                    
                    var items = portfolioMessage.Items;
                    var stocks = [];
                    for (var x = 0; x < items.length; x++) {
                        stocks.push(items[x].Symbol);
                    }
                    var url = GetStockGraphUrl(stocks, "Portfolio History", 620, 400, 2);

                    jImg.attr("src", url);
                    jImg.fadeOut("normal");

                    jImg.one("load", function() {
                        jImg.fadeIn("slow");
                        showProgress(true);
                    });
                },
                OnPageError);        
}
function SymbolLookup(symbol,target)
{
    // create a new instance because of rapid fire requests
    
    Proxy.GetStockQuote(symbol,
        function(result) 
        {                            
            var text = "";
            if (result && result.LastPrice != 0.00)
               text = result.Company + " (" + result.LastPrice.toFixed(2) + ")";
                
            $("#" + target).text(text);
        },
        OnPageError);
}
function GetStockGraphUrl(stocks,title,width,height,years)
{         
	var stockParm = "";
    for(var x=0; x < stocks.length; x++)
        stockParm +=  stocks[x] + ',';

    stockParm = stockParm.replace(/,$/,""); // trim ,

    // Note we can pass parameters by URL to the Ajax Handler
    // The method has stream result which is displayed here
    var url = String.format("JsonStockService.ashx?Method=GetStockHistoryGraphSafe&symbollist={0}&title={1}&width={2}&height={3}&years=2&t=" + new Date().getMilliseconds().toString(),
                             stockParm,title,width,height);                    
    return url;
}

function showStatus(message,timeout)
{        
    if (!message)
       message="Ready"
                
    var jCtl = jQuery("#divStatusBar").text(message).addClass("statusbarhighlight");
    if (timeout)
       setTimeout(function() { jCtl.removeClass("statusbarhighlight").text("Ready"); },timeout);
}
function showProgress(hide)
{
    if (hide)
        $("#divProgress").hide();
    else
        $("#divProgress").show();        
}
function showStockProgress(hide)
{ 
    if (hide)
        $("#imgStockProgress").hide();
    else
        $("#imgStockProgress").show();        
}
function OnPageError(error) {    
    showStatus("Callback Error: " + error.message, 5000);
    showProgress(true);
    showStockProgress(true);
}
function getText(el)
{
    if (!el) return "";
    if (el.innerText != "undefined") return el.innerText;
    return el.textContent;
}
function setText(el,text)
{        
    if(!el)
        return;
    if (el.innerText != "undefined") el.innerText = text;
    el.textContent = text;
}

