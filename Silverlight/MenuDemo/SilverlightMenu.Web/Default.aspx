<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SilverlightMenu.Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>万隆证券</title>
    <link href='/css/main.css' type='text/css' rel='stylesheet' />
    <script src="/Js/base.js" type="text/javascript"></script>
    <script type="text/javascript">
        function switchSysBar() {
            var obj = $("switchPoint");

            if (obj.alt == "关闭左栏") {
                obj.alt = "打开左栏";
                obj.src = "/images/nav_show.gif";
                $("frmTitle").style.display = "none";
            }
            else {
                obj.alt = "关闭左栏";
                obj.src = "/images/nav_hide.gif";
                $("frmTitle").style.display = "";
            }
        }
    </script>
    <style type="text/css">
        .frame_class
        {
            border-right: medium none;
            padding-right: 0px;
            border-top: medium none;
            padding-left: 2px;
            padding-bottom: 0px;
            margin: 0px;
            overflow: hidden;
            border-left: medium none;
            padding-top: 0px;
            border-bottom: medium none;
            height: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="padding-top: 10px;
        background-image: url('/Images/HeadBg.gif')">
        <tr>
            <td align="left" valign="top" style="height: 5%">
                &nbsp;
            </td>
            <td align="left" width="100px">
                <font color="white">姓名：</font><span id="lblName" style="color: white" runat="server"></span>
            </td>
            <td align="left" width="120px">
                <font color="white">职位：</font><span id="lblRole" style="color: white" runat="server"></span>
            </td>
            <td align="right" width="120px">
                <asp:DropDownList ID="DropDownList1" runat="server" Width="120px">
                </asp:DropDownList>
            </td>
            <td align="center" width="50px">
                <asp:Button ID="Button1" runat="server" Text="切换职位" />
            </td>
            <td align="right" width="60px">
                <a class="white" href="/Security/EditPwd.aspx" target="main">修改密码</a>
            </td>
            <td align="right" width="40px">
                <a class="white" href="#" target="main">帮助</a>
            </td>
            <td align="center" width="60px">
                <a class="white" href="/Security/LoginOut.aspx" target="main">退 出</a>
            </td>
        </tr>
    </table>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tbody>
            <tr>
                <td id="frmtitle" valign="center" align="middle">
                    <iframe id="left" style="width:140px; height:600px" name="left" src="/LeftMenu.aspx"
                        frameborder="0" scrolling="yes"></iframe>
                </td>
                <td bgcolor="#000000" onclick="switchSysBar()" align="center">
                    <img id="switchpoint" height="13" alt="关闭左栏" src="/Images/nav_hide.gif" width="10"
                        border="0" />
                </td>
                <td style="width: 100%">
                    <iframe id="main" style="width: 100%; height: 100%" name="main" src="/Main.aspx"
                        frameborder="0" scrolling="auto"></iframe>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
