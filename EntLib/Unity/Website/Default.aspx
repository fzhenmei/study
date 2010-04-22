<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
  <title>Unity Dependency Injection in ASP.NET</title>
  <link href="stylesheets/style.css" rel="stylesheet" type="text/css" />
</head>
<body topmargin="0" leftmargin="0">
  <table width="100%" border="0" cellpadding="0" cellspacing="0">
   <tr>
    <td style="background-color: #0066cc">
     <a href="http://www.codeplex.com/" target="_blank"><img src="stylesheets/pplogo.gif" border="0" /></a>
    </td>
   </tr>
  </table>
  <table width="100%" border="0" cellpadding="20">
   <tr>
    <td>
      <div class="heading">Unity Dependency Injection in ASP.NET<br /></div>
      <hr />
      <form id="form1" runat="server">
      <div>
  
      <p><asp:Button ID="btn_GetInterface" runat="server" Text="&nbsp;&nbsp; &nbsp;" OnClick="btn_GetInterface_Click" />
      Get an object that implements IMyInterface.</p>
      
      <p><asp:Button ID="btn_RegisterNew" runat="server" Text="&nbsp;&nbsp; &nbsp;" OnClick="btn_RegisterNew_Click" />
      Register a different object that implements IMyInterface.</p>

      <p><asp:Button ID="btn_GetLogger" runat="server" Text="&nbsp;&nbsp; &nbsp;" OnClick="btn_GetLogger_Click" />
      Get an object that uses this type of logger:
      <asp:DropDownList ID="lst_LoggerType" runat="server"> 
      <asp:ListItem Text="StandardLogger" />
      <asp:ListItem Text="SuperFastLogger" />
      </asp:DropDownList></p>
      
      <p><asp:Button ID="btn_ConstructorInjection" runat="server" Text="&nbsp;&nbsp; &nbsp;" OnClick="btn_ConstructorInjection_Click" />
      Get an object that uses Constructor Injection.</p>

      <p><asp:Button ID="btn_ReloadContainer" runat="server" Text="&nbsp;&nbsp; &nbsp;" OnClick="btn_ReloadContainer_Click" />
      Reload the original Unity Container configuration.</p>
      
      <hr />
      <p><asp:Label ID="lbl_Output" EnableViewState="false" runat="server" />&nbsp;</p>
      <hr />
      
      </div>
      </form>
    </td>
   </tr>
  </table>
</body>
</html>
