<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MultiSelect._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>多选下拉框</title>
    <link href="Css/screen.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/multiselect.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <h1>多选下拉框</h1>
        <hr />
        点击选择箭头选择：
        <zen:MultiSelect runat="server" id="multiSelect">
            <asp:ListItem Text="北极熊1" Value="1"></asp:ListItem>
            <asp:ListItem Text="北极熊2" Value="2"></asp:ListItem>
            <asp:ListItem Text="北极熊3" Value="3"></asp:ListItem>
            <asp:ListItem Text="北极熊4" Value="4"></asp:ListItem>
            <asp:ListItem Text="北极熊5" Value="5"></asp:ListItem>
        </zen:MultiSelect>
        <hr />
        <asp:Button Text="看看会发生什么？" runat="server" OnClick="ShowMe" />
    </div>
    </form>
</body>
</html>
