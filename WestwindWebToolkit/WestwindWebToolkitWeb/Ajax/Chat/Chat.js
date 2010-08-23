/// <reference path="~/scripts/jquery.js" />  
/// <reference path="~/scripts/ww.jquery.js" />

// Startup Code ***
$(document).ready(function() {
    // Check active users on a lengthy fixed interval
    window.setInterval(GetActiveUsers, 20000);

    // Run a slow safety loop in case the main message chaining
    //     gets stuck. 
    window.setInterval(function() { GetMessages(true); }, 25000);

    // Initial display of values
    window.setTimeout("GetActiveUsers()", 1000);
    window.setTimeout(function() { GetMessages(true); }, 500);
});

function WriteMessage()
{
    var name = $("#txtName").val();
    if (name == "")
    {
        ShowError("Please enter your name or handle.");
        $('#txtName').focus();
        return;
    }
    
    $('#btnSend').attr("disabled",true);

    ChatService.WriteMessage($('#txtMessage').val(), name, ChatId,
        function (Result) {
            $('#btnSend').attr("disabled",false);
            $('#txtMessage').val('').focus();
            GetMessages(true);
        }, 
        OnError
    );
}
function GetMessages(Force) {    
    // Don't allow requests to fire too frequently
    if (Force != true && new Date().getTime() - PingFrequency < LastCallbackTime.getTime())
        return;

    // Keep track of last callback time     
    LastCallbackTime = new Date();

    // Get a new instance to make sure there's no overlap
    ChatService.GetMessages(ChatId, Name,
        function(Result) {
            // Chain callback - Get next messages in x Seconds
            window.setTimeout(GetMessages, PingFrequency);

            if (Result == null || Result == "")
                return;

            $("#lblMessages").append(Result);
            $("#divMessages").scrollTop(999999);                        
        },
        function(Error) {
            // Chain callback - Get next messages in x Seconds
            window.setTimeout(GetMessages, PingFrequency);
            OnError(Error);
        });
}        
function GetActiveUsers()
{
    ChatService.GetActiveUsers(ChatId,
        function(result) {
            $('#divUserList').html(result);
        });
}
function PopupChatList()
{
    ChatService.GetChatListHtml(
            function(Result) {
                if (Result == null || Result == "")
                    return;

                var Ctl = $('#divChatList');
                Ctl.html(Result +
                                "<hr /><div style='text-align:right'><a href='javascript:SetChatId()'>Close</a></div>");

                Ctl.show();
                Ctl.css("position", "absolute");

                var CtlChatId = $("#txtChatId");
                

                var txtBounds = {
                    x: CtlChatId.position().left,
                    y: CtlChatId.position().top,
                    width: CtlChatId.width(),
                    height: CtlChatId.height()
                };
                CtlChatId.show();

                var Bounds = { x: 0, y: 0, height: 0, width: 0 }
                Bounds.y = txtBounds.y + txtBounds.height + 3;
                Bounds.x = txtBounds.x;
                Bounds.width = txtBounds.width + 12;
                Bounds.height = -1;  // don't set

                Ctl.width(Bounds.width);
                Ctl.height(Bounds.height);
                Ctl.css("left", Bounds.x);
                Ctl.css("top", Bounds.y);
                Ctl.css("zIndex", 99);

                Ctl.show().shadow();
            },
            OnError
    );
}        
function SetChatId(NewChatId)
{
    if (NewChatId != null)
    {  
        $('#txtChatId').val(NewChatId);
        ChatId = NewChatId;
    }
    
    GetActiveUsers();
    
    if (ChatId != "")
        $('#lblMessages').empty();

    $("#divChatList").hide();
}
function NameChanged(NewName)
{
    if (NewName != Name)
    {
        // If we have a new name
        Name = NewName;
        ClearMessages();
    }
}
function ChatIdChanged(NewChatId)
{
    if (NewChatId != ChatId)
    {
        ChatId = NewChatId;
        ClearMessages();       
    }
}
function ClearMessages() {
    $('#lblMessages').empty();        
    $('#divMessages').scrollTop(999999); 
}
function OnError(Result)
{
   ShowError("An error occurred: "  + Result.message);
   $('#btnSend').attr("disabled", false);
}
function ShowError(Message,ShowInfoMessage)
{
    var Ctl = $("#lblErrorMessage");
    if (ShowInfoMessage)
        Ctl.html(" <img src='../../images/info.gif'> " + Message );
    else
        Ctl.html( " <img src='../../images/warning.gif'>  &nbsp;&nbsp;" + Message );
    
    Ctl.css("position","absolute").fadeIn("slow");
    
    window.setTimeout(HideError,5000);
}
function HideError() {
    $("#lblErrorMessage").fadeOut(1200);
}  

