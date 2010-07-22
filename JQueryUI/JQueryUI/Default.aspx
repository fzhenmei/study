<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="JQueryUI._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JQuery Dialog 和 ASP.NET Button</title>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.2/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/custom-theme/jquery-ui-1.8.2.custom.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="testDialog">
        <h1>I'm Dialog</h1>
        <asp:Button runat="server" Text="Hit me!" OnClick="HitMe" />
        <asp:LinkButton runat="server" Text="Kiss me!" OnClick="KissMe"></asp:LinkButton>
    </div>
    </form>
    <script type="text/javascript">
        $(function () {
            var dlg = $("#testDialog").dialog();
            dlg.parent().appendTo(jQuery("form:first"));
        });
    </script>
</body>
</html>
