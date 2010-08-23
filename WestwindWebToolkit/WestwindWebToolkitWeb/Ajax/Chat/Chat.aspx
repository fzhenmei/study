<%@ Page Language="C#" AutoEventWireup="true" Inherits="Westwind.WebToolkit.ChatClient"
         EnableViewState="true" EnableEventValidation="false" ValidateRequest="false" Codebehind="Chat.aspx.cs" %>
<%@Register Namespace="Westwind.Web.Controls" assembly="Westwind.Web" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ajax MethodCallback Chat</title>
    <link href="../../css/Westwind.css" rel="stylesheet" type="text/css" />
    <link href="../../css/WebToolkitSamples.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        td { font-size: 8pt; font-face: Tahoma sans-serif;}
        input { font-size: 8pt; font-face: Tahoma sans-serif; }
        .ChatList { font-size:8pt; font-face: Tahoma; border: 1px solid Darkblue; background:white; }
        .ControlOpacity { opacity: .70; filter:alpha(opacity='70'); background:white; color: navy; }        
        .BackgroundImage { xopacity:.90;xfilter:alpha(opacity='80');background:url(images/sailbig.jpg); }
        
        .MessageWrapper { padding: 3px; }
        .MessageWrapperMe { padding: 3px; }
        .Message { padding-left: 15px; }
        .MessageAuthor { color: silver; font-weight: bold; }        
                
        .ActiveUser { background-image: url(images/chatlive.gif); background-position: left; background-repeat: no-repeat; padding: 3px; padding-left: 20px;  }
        .InactiveUser { background-image: url(images/chatNotActive.gif); background-position: left; background-repeat: no-repeat; padding: 3px; padding-left: 20px; }
        .OfflineUser { background-image: url(images/chatoff.gif); background-position: left; background-repeat: no-repeat; padding: 3px;  padding-left: 20px; }
    </style>    
    <script src="Chat.js" type="text/javascript"></script>
</head>
<body>
    <form id="ChatForm" runat="server">    
        <div>
            <h1>AjaxMethodCallback Chat</h1>
            <div class="toolbarcontainer">
                <a href="../" class="hoverbutton"><asp:Image ID="Image1" runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
                <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
                    <asp:Image runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
                </asp:LinkButton> |         
                <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
            </div>
                
            <div id="lblErrorMessage" class="errordisplay" 
                 style="width:400px;display:none;padding:8px;padding-left: 20px;">
            </div>
           
            <div class="containercontent">               
                <table class="blackborder BackgroundImage" cellpadding="8" 
                        width="100%" border="0">
                <tr>
                <td valign="top" style="color: White">
                    <div>Messages:</div>
                    <div id="divMessages" 
                        class="ControlOpacity"
                        style="padding:5px;overflow-Y:scroll;border:solid 1px teal;height:300px;left:0px;right:10px;">
                        <asp:Label ID="lblMessages" runat="server" style="left:0px;right:10px;"></asp:Label>
                    </div>                                  
                    <hr />  
                   
                    <div>Enter a Message:</div>
                    <table style="width: 100%">
                        <tr>
                        <td style="border: none; padding: 0px;">                    
                            <asp:TextBox ID="txtMessage" runat="server"  TextMode="MultiLine"
                                         class="ControlOpacity"
                                         style="width:100%;height:50px;left:20px;right:10px;font:normal Tahoma 8pt;overflow-y:scroll;opacity:.80;" />
                        </td>
                        <td style="width: 100px; text-align:right; border: none;vertical-align: top; padding: 0 5px;">
                            <input type="button" id="btnSend" value="Send" accesskey="S" onclick="WriteMessage()"
                                   style="height:50px;width:90px;right:10px;font-weight: bold" />

                        </td>
                        </tr>
                    </table>
                 </td>
                 <td valign="top" style="border-left:solid 1px Navy;width:165px;">
                    <span style="color: white">Your Chat Handle:</span><br />
                    <asp:TextBox ID="txtName" runat="server"
                                 class="ControlOpacity" 
                                 style="width:160px;Font-Size:8pt;margin-bottom:8px;" 
                                 onblur="NameChanged(this.value);"></asp:TextBox>
                                 
                    <div class="gridalternate" style="padding: 5px;border:solid 1px darkblue;">
                    Enter a Chat Id:<br />
                    <asp:TextBox ID="txtChatId" runat="server" style="width: 100px; font-size: 8pt;" onblur="ChatIdChanged(this.value);" Text="TestChat" />
                    <input type="button" id="btnChatList" value="..." onclick="PopupChatList();" style="font-size:8pt;height:20px;width:20px" />
                    <p />
                    Active Users:
                    <div id="divUserList" style="height:230px;overflow-Y:auto;background:white;padding:5px;">     
                    </div>                  

                    </div>
                 </td>
                </tr>
                </table>   
                
    </div>
    <div id="divChatList" class="ChatList" 
         style="position:absolute;border:solid 1px Darkblue;padding:5px;background:cornsilk;display:none;">
    </div>

            <div class="toolbarcontainer">
                <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
                    displaystate="Button" codefile="Chat.aspx" />
                <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
                    displaystate="Button" codefile="Chat.aspx.cs" />
                <manoli:viewsourcecontrol id="ViewSourceControl2" runat="server" text="Javascript"
                    displaystate="Button" codefile="Chat.js" />
                <manoli:viewsourcecontrol id="ViewSourceControl3" runat="server" text="Ajax Service Handler"
                    displaystate="Button" codefile="ChatService.cs" />
                    
            </div>    
    
    <br clear="all" />    

    <ww:AjaxMethodCallback ID="ChatService" runat="server"
                           ServerUrl="ChatService.ashx" />
    

    <div class="descriptionheader">
    This example demonstrates using the AjaxMethodCallback control to an HTTP handler that implements
    the Chat Message service functionality. There's no code in the server page at all except for initial
    assignment of chat id and forum. All other code retrieves data from the Ajax Service Handler.    
    <br />
    <br />
    The majority of the code sits on the client side. This code uses the client proxy to make
    calls to the server and update the controls on the page. 
    <br />
    <br />
    Since AJAX cannot reliably do a PUSH model the only way for the client to get the latest messages is to poll the server for updates frequently. Therefore 
    this is a very chatty interface that fires a lot of requests to the server. Load with many users in this scenario
    could very easily become a scalability problem. However,
    this example uses pure JSON messages which push only updated data back and forth
    so message size generally is very small. You might want to break out Fiddler and
    compare both the ATLAS and Anthem chat which use different approaches, where this
    approach is the more bandwidth friendly of the two.
    </div>
            

<%--  
    <uc1:ShowCode ID="ShowCode1" runat="server" CodeFile="~/chat/Chat.aspx" Text="ASPX" width="130" height="25"/>
    <uc1:ShowCode ID="ShowCode2" runat="server" CodeFile="~/chat/Chat.aspx.cs" Text="CodeBehind" width="130" />
    <uc1:ShowCode ID="ShowCode3" runat="server" CodeFile="~/app_code/ChatService.cs" Text="Service Http Handler" width="130"/>
--%>    

    <script type="text/javascript">   
    //<!--

    // Script Initialization code. The rest of script code is in chat.js

    // global references so we don't get partials
    var Name = $('#txtName').val();
    var ChatId = $('#txtChatId').val();

    // Frequency of updates - 5 seconds
    var PingFrequency = 5000;

    // Check value
    var LastCallbackTime = new Date();

    // -->
    </script>    
    </form>
</body>
</html>
