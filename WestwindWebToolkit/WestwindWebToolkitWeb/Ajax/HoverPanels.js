/// <reference path="~/scripts/jquery.js" />
/// <reference path="~/scripts/ww.jquery.js" />

//
// *** Invoice List script logic
//
// hook up invoice list hover behavior with jQuery
// this is easier and cleaner than embedding onmouseover and onmouseout handlers
// in code: Unobtrusive JavaScript
$(document).ready(function() {
    
    // hook up hover handler for links - find first column <a> tag in grid
    var query = "#" + scriptVars.gdInvoiceListId + " tr>td:first-child>a";
    var jList = $(query)
                      .hover(function(e) {
                          // text is padded with spaces/crs - trim off
                          var orderId = $(this).text().trimEnd().trimStart();                          

                          // *** ACTIVATE THE HOVERPANEL based on orderId
                          lineItemPanel.startCallback(e, "orderId=" + orderId);
                      },
                      function() {
                          lineItemPanel.hide();
                      });
});


var globals = { firstStockDisplay: true }

//
// *** Stock Quote Hover Panel at button example
//
function GetStockQuote(evt) {
    var symbol = $("#" + scriptVars.txtStockSymbolId).val();
    showProgress();
    
    if (globals.firstStockDisplay) 
        globals.firstStockDisplay = false;
    else
        StockDisplayPanel.eventHandlerMode = "ShowHtmlInPanel";  // after first show display at last position

    // get called back when the panel is ready to display
    // use this to update ui - here progress display
    StockDisplayPanel.completed = function(res) {
        showProgress(true);
        return true;  // display the panel  - false = don't autoshow
    };
    StockDisplayPanel.startCallback(evt, "symbol=" + symbol);
}

function showProgress(hide) {
    var img = $("#imgLoading");
    if (hide)
        img.hide();
    else
        img.show();
}  