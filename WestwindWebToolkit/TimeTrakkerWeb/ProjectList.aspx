<%@ Page Language="C#" MasterPageFile="~/TimeTrakkerMaster.Master" 
         AutoEventWireup="true" 
         CodeBehind="ProjectList.aspx.cs" 
         Inherits="TimeTrakkerWeb.ProjectList" 
         Title="Project List" %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<asp:Content ID="Header" ContentPlaceHolderID="head" runat="server">
<style type="text/css">        
    .itemtemplate
    {
        border-bottom: dashed 1px teal;
        padding: 9px;
        padding-left: 30px;
    }
    .itemtemplate:hover
    {
        background-image: url(images/lightorangegradient.png);           
        background-repeat: repeat-x;
        cursor: pointer;        
    }
    .itemtemplate b
    {
        color: Navy;
    }
    .itemtemplate a
    {
        text-decoration: none;
    }
    .itemtemplate a:visited
    {
        color: Navy;
    }
    .companydisplay
    {
    	 float:right;margin-right: 10px;color:Teal;
    }
    .projimg
    {
        width: 16px;
        height: 16px;
        margin-right: 10px;        
        margin-bottom: 5px;
        background-image: url(images/project.png);
        float: left;
    }
</style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <center>
    <ww:ErrorDisplay ID="ErrorDisplay" runat='server' />
    
    <div class="dialog" style="width:600px; ">
    <div class="dialog-header-center">Project List</div>
    <div class="toolbarcontainer">
    <small><a href="ShowProject.aspx" class="hoverbutton"><img src="images/punchin.gif" /> New</a> | Filter: 
        <ww:wwDropDownList ID="lstFilter" runat="server" Width="200" AutoPostBack="true">
            <asp:ListItem Text="Open Projects" Value="OpenProjects" />
            <asp:ListItem Text="Recent Projects" Value="RecentProjects" />     
            <asp:ListItem Text="All Projects" Value="AllProjects" />
        </ww:wwDropDownList>  
        | Project Name: 
        <asp:TextBox runat="server" ID="txtNameFilter" style="width: 75px;"  />        
        <asp:Button runat="server" ID="btnGoFilter" Text="Go" />
    </small>
    </div>
        <div style="height: 410px; overflow: hidden; overflow-y: scroll;">
        <asp:Repeater runat="server" ID="dgProjects">
        <ItemTemplate>
            <div class="itemtemplate" onclick="showProject(<%# Eval("Pk") %>)" >
                <div class="companydisplay"><small><i><%# Eval("Company") %></i></small></div>
                <div class="projimg"></div>
                <b><a href="ShowProject.aspx?id=<%# Eval("Pk") %>"><%# Eval("ProjectName") %></a></b><br />
                <small>
                     <%# TimeUtils.ShortDateString( (DateTime?) Eval("Entered"),false) %> 
                     &nbsp;&nbsp;
                     <%# Enum.GetName(typeof(ProjectStatusTypes), (int) Eval("Status") ) %> 
                </small>
            </div>
        </ItemTemplate>        
        </asp:Repeater>      
        </div>
        <div class="toolbarcontainer">
            <small><asp:Label runat="server" ID="lblStatus" /></small>
        </div>
    </div>
    </center>

<script type="text/javascript">
function showProject(pk)
{
    window.location = "ShowProject.aspx?id=" + pk.toString();
}
</script>    
</asp:Content>
