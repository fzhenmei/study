<%@ Page Language="C#" MasterPageFile="~/TimeTrakkerMaster.Master" 
         AutoEventWireup="true" 
         CodeBehind="Login.aspx.cs" 
         Inherits="TimeTrakkerWeb.Login" Title="Time Trakker Login" %>

<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    
    <ww:ErrorDisplay ID="ErrorDisplay" runat="server" style="margin: 15px;" />
        <center> 
        <div class="dialog" style="text-align:left;width: 385px;">
            <div class="dialog-header">Please Log In</div>
            <div style="padding:20px">                
                <table cellpadding="4px">
               <tr>
               <td>Username:</td><td><asp:TextBox runat="server" ID="Username" Width="250px"></asp:TextBox></td>
               </tr>
               <tr>
               <td>Password:</td><td><asp:TextBox runat="server" ID="Password" Width="250px" TextMode="Password"></asp:TextBox></td>
               </tr>
               <tr>
               <td colspan="2"><asp:CheckBox runat="server" ID="RememberMe" 
                       Text="Remember me next time" Checked="True" /></td>
               </tr>
               </table>
               <hr />
               <asp:Button runat="server" ID="LoginButton" CommandName="Login" Text="Log in" OnClick="LoginButton_Click" />
            </div>
        </div>
    </center>
</asp:Content>
