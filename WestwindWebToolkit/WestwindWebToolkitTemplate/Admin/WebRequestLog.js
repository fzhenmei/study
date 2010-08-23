/// <reference path="~/scripts/jquery.js" />
/// <reference path="~/scripts/ww.jquery.js" />

var itemTemplateText = null;
var firstDialogLoad = true;

$(document).ready(function() {
    showStatus({ afterTimeoutText: "Ready" });
    // set initial value
    showStatus("Ready");

    loadLogEntries();

    itemTemplateText = $("#LogItemTemplate").html();


    $("#LogItemPanel").resizable({ start: function() {
        $(this).css("max-width", ""); // allow resizing to any size
    } });


});

function loadLogEntries() {
    $("#divListWrapper").empty();
    showProgress();
    showListStatus("Downloading...");
            
    var logFilter = scriptVars.LogDataFilter;
    
    logFilter.ErrorLevel = $("#lstLogType").val();
    logFilter.Count = $("#txtCount").val() * 1.0;

    Proxy.GetListData(logFilter,
        function(data) {
            
            showListStatus("Updating List...");

            $(data.Rows).each(function() {
                updateLogItem(this);
            });
            showListStatus(data.Rows.length + " Log Items");

            showProgress(true);
        },
        onPageError);
}

// Apply icons to items by adding classes
function updateLogItem(item) {
    var html = parseTemplate(itemTemplateText, item);
    var jItem = $(html);    
    if (item.ErrorLevel == 2)
        jItem.find("div:first-child").addClass("erroricon");
    else if (item.ErrorLevel == 1)
        jItem.find("div:first-child").addClass("warningicon");
    else 
        jItem.find("div:first-child").addClass("infoicon");

    jItem.click(getLogEntry);
            
    $("#divListWrapper").append(jItem);
}

// Retrieves a log entry from the server and displays it
// in a pop up window.
function getLogEntry(e) {
    var jItem = $(this);
    var id = jItem.get(0).id.replace("log_","");

    Proxy.GetLogItem(+id, function(logItem) {
        var html = parseTemplate($("#LogDetailDisplayTemplate").html(), logItem);
        $("#divItemDetailDisplay").html(html);

        if (firstDialogLoad) {
            $("#" + LogItemPanel.controlId).centerInClient();
            firstDialogLoad = false;
        }
        
        LogItemPanel.show();
    }, onPageError);
}

function showListStatus(message) {
    if (message == null)
        message = "Ready";
    $("#divListStatus").text(message);  
}
function showProgress(hide) {
    if (!hide)
        $("#divListProgress").show();
    else
        $("#divListProgress").hide();
}
function onPageError(error) {
    showProgress(true);
    showStatus(error.message, 4000, true);
}
