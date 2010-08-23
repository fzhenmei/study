<%@ Page Title="Miscellaneous Control Samples - West Wind Web Toolkit" Language="C#" 
         CodeBehind="Default.aspx.cs" Inherits="Westwind.WebToolkit.ControlsDefault"
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
         
        <table width="900px;">
        <tr>
        <td style="width: 150px; text-align: left; padding: 10px; vertical-align: top; background-image: url(../images/newwave.jpg);background-repeat: repeat-y;">
            <img src="../images/WestwindWebToolkit_100.jpg" />            
        </td>   
        
        <td class="containercontent" style="vertical-align: top">
            <h3>Miscellaneous Control Examples</h3>
            
            <p>
            The following examples demonstrate the basic features of some of the
            various Web Controls that come with the West Wind Web Toolkit.
            </p>
            
            <hr />
            
            <p>
            <a href="PagerControl.aspx" class="headerlink">Pager Control</a>            
            The Pager provides a control and data source independent server side paging solution.
            You can manipulate the pager purely through property settings or use one of its
            Filter methods to automatically set up the pager values based on the query and
            and Page query string value.
            
            </p>
            
            <p>
            <a href="TabControl.aspx" class="headerlink">TabControl</a>
            The TabControl provides a 'tabstrip' control to allow client side 
            tab page selection. The control is primarily driven through client
            side code that activates raw HTML elements when tab buttons are 
            clicked.
            </p>
            
            <p>
            <a href="MessageDisplayDemo.aspx" class="headerlink">ErrorDisplay Control and MessageDisplay Pages</a>
            These two controls provide an easy and consistent way to display error and informational
            messages consistently in your applications. ErrorDisplay is a page control to display errors
            or information messages. MessageDisplay is single line mechanism to display a pre-defined
            ASP.NET template page with custom header and content text. Both are ideal for error scenarios.
            </p>    
            
        </td>
        
        </tr>
        </table>
        
</asp:Content>
