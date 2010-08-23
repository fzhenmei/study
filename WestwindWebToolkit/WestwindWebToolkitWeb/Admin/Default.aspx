<%@ Page Title="Administration Home - West Wind Web Toolkit" Language="C#" 
         CodeBehind="Default.aspx.cs" Inherits="Westwind.WebToolkit.Admin.Default"
         MasterPageFile="~/WestWindWebToolkit.master" AutoEventWireup="true" 
%>
<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
        
        <div class="toolbarcontainer">
            <a href="../" class="hoverbutton"><asp:Image runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
            <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
                <asp:Image runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
            </asp:LinkButton> |         
            <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
        </div>
         
        <table style="width:100%;">
        <tr>
        <td style="width: 150px; text-align: left; padding: 10px; vertical-align: top; background-image: url(../images/newwave.jpg);background-repeat: repeat-y;height: 700px;">
            <img src="../images/WestwindWebToolkit_100.jpg" />            
        </td>   
        
        <td class="containercontent" style="vertical-align: top;">
            <h3>Administration Tasks</h3>
            
            <p>
            The following are a few examples that demonstrate the base features of
            the various AJAX based controls.
            </p>
            
            <hr />
            
            <p>
            <a href="AppConfiguration.aspx" class="headerlink">Application Configuration</a>
            Allows you to set configuration settings that are stored in web.config that
            are application specific. These settings are fairly generic and usable for 
            most applications and set the properties on the ApplicationConfiguration class
            that is used to keep application configuration details.
            </p>
            
            <p>
            <a href="WebRequestLog.aspx" class="headerlink">Web Request and Error Log</a>
            This page lets you view and administer the application log.
            </p>
            
            <p>
            <a href="BannerAdmin.aspx" class="headerlink">Banner Administration</a>
            This page lets you manage the banners to display and use on the site.
            </p>
            
        </td>
 
        <td class="gridalternate sidebar" style="border-left: solid 1px grey;padding: 10px;">
        </td>
        
        </tr>
        </table>
</asp:Content>
