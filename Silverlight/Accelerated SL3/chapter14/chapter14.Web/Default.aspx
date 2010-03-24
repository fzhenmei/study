<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="chapter14.Web.Default" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <tr><td>
    <div style="float: left;">
    <h2>Sender App...</h2>
    <asp:Silverlight ID="Silverlight2" runat="server" Source="~/ClientBin/SenderApp.xap" MinimumVersion="3.0.40307.0" Width="400" Height="300" />
    </div>
    </td><td>
    <h2>Receiver App...</h2>
    <asp:Silverlight ID="Silverlight1" runat="server" Source="~/ClientBin/ReceiverApp.xap" MinimumVersion="3.0.40307.0" Width="400" Height="300" />    
    </td></tr>
    </table>
    </form>
</body>
</html>
