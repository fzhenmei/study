/// <reference path="~/scripts/jquery.js" />
/// <reference path="~/scripts/ww.jquery.js" />
$(document).ready(function() {
    __statusbar = new StatusBar(null, { afterTimeoutText: "Ready" }).hide();
    //showStatus();    
});


function SayHello() {
    // Id is mangled by ASP.NET so use serverVars created with ScriptVariables
    // component which generates a client object with simple client Id names
    var name = $("#" + serverVars.txtNameId).val();

    Proxy.HelloWorld(name,
        // function called with the method result
        function(result) {
            $("#divHelloMessage")
                .text(result)
                .stop()
                .fadeIn("slow");

            setTimeout(function() { $("#divHelloMessage").fadeOut("slow"); }, 8000);
        },
        // error handler
        onError);
}

function AddNumbers() {
    
    // pick up inputs and turn into numeric values that hte server 
    // AddNumbers expects
    var num1 = $("#" + serverVars.txtNum1Id).val() * 1;
    var num2 = $("#" + serverVars.txtNum2Id).val() * 1;
        
    // Use the generated Proxy to call the server
    Proxy.AddNumbers(num1, num2,
        function(result) {
            $("#" + serverVars.lblAddResultId).text(result);
        }, onError);
}

function GetStockQuote() {
    var symbol = $("#" + serverVars.txtStockSymbolId).val();
    Proxy.GetStockQuote(symbol,
        function(quote) {
            if (!quote) {
                showStatus("Invalid symbol supplied.", 4000, true);
                var ctl = $("#" + serverVars.txtStockSymbolId);
                ctl.addClass("fielderror");
                setTimeout(function() { ctl.removeClass("fielderror"); }, 2000);
                return;
            }

            var html = String.format("<b>{0}</b><hr />", quote.Company) +
                       String.format("Last Price: {0}<br />", quote.LastPrice.formatNumber("N")) +
                       String.format("Change: {0}", quote.NetChange.formatNumber("n"));

            $("#divStockQuoteResult")
                .width(375)
                .html(html)
                .fadeIn("slow");
        }, onError);
}

function LoadCustomerList() {
    Proxy.GetCustomers(
        function(custList) {
            var list = $("#" + serverVars.lstCustomerPkId);

            list.listSetData(null); // clear list
            // // Shortcut to data binding - bind single object array
            list.listSetData(custList, { dataTextField: "Company", dataValueField: "Pk" });

            showStatus("List has been loaded.", 2000);
            
            list.trigger("change");

        }, onError);
}

function ClearCustomerList() {
    var list = $("#" + serverVars.lstCustomerPkId);
    list.listSetData(null);
    showStatus("List cleared", 2000, true);
}

var curCustomer = null;
function LoadCustomer() {
    var pk = $("#" + serverVars.lstCustomerPkId).val() * 1;

    Proxy.GetCustomer(pk,
        function(customer) {
            // save so we have an instance on the client
            curCustomer = customer;
            
            $("#" + serverVars.txtCompanyId).val(customer.Company);
            $("#" + serverVars.txtFirstNameId).val(customer.FirstName);
            $("#" + serverVars.txtLastNameId).val(customer.LastName);
            $("#" + serverVars.txtBillRateId).val(customer.BillingRate.formatNumber("n2"));
        }, onError);
}

function SaveCustomer()
{
    curCustomer.Company = $("#" + serverVars.txtCompanyId).val();
    curCustomer.FirstName = $("#" + serverVars.txtFirstNameId).val();
    curCustomer.LastName =  $("#" + serverVars.txtLastNameId).val();
    curCustomer.BillingRate = $("#" + serverVars.txtBillRateId).val() * 1.00;
    
    Proxy.SaveCustomer(curCustomer,
        function(result) {
            showStatus("Customer " + curCustomer.Company + " saved.",5000);
        }, onError);
}

function LongRunning() {
    $("#lnkLongRunning").attr("disabled",true);
    $("#lnkLongRunning_Progress").show();

    function hideProgress() {
        $("#lnkLongRunning").attr("disabled", false);
        $("#lnkLongRunning_Progress").hide();
    }

    Proxy.LongRunning(10000, function(res) {
        showStatus("call completed", 5000);
        hideProgress();
    }, function(msg) {
        showStatus("Timeout callback failed." + msg.message, 4000);
        hideProgress();
    });
}
function ThrowException() {
    Proxy.ThrowException(function(res) {
        showStatus("Exception not thrown - this shouldn't happen!");
    }, onPageError)
}

function onError(error) {
    // show error message for 4 seconds highlighted
    showStatus("Error: " + error.Message,4000,true);       
}            

