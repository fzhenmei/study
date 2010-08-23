<%@ Page Language="C#" MasterPageFile="~/TimeTrakkerMaster.Master" 
         AutoEventWireup="false" 
         CodeBehind="ShowProject.aspx.cs" Inherits="TimeTrakkerWeb.ShowProject" 
         Title="Project Information - Time Trakker"          
         %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>

<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    #itemtemplate
    {        
        border-bottom: dashed 1px teal;
        padding: 10px;
        padding-left: 30px;
        height: 35px;
    }
    #itemtemplate:hover
    {
        background: transparent url(images/lightorangegradient.png) repeat-x;
        cursor: pointer;
    }
    #itemtemplate b
    {
        color: Navy;
    }
    #itemtemplate a
    {
        text-decoration: none;
    }
    #itemtemplate a:visited
    {
        color: Navy;
    }
    .itemcontrols
    {
    	float:right;
    	text-align: right;
    	font-size: 8pt;
    }
</style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<center>
<div class="dialog" style="width:475px;">
<div class="dialog-header">Project</div>
<div class="toolbarcontainer">
    <a href="ShowProject.aspx" class="hoverbutton"><img src="images/punchin.gif" /> New</a>

    <asp:LinkButton runat="server" ID="btnDelete" class="hoverbutton" 
        onclick="btnDelete_Click">
    <img src="images/remove.gif" /> Delete
    </asp:LinkButton>    
    </span>
</div>
<div style="padding: 20px;">
    <ww:ErrorDisplay ID="ErrorDisplay" runat='server' />
    <asp:Label ID="lblProjectName" runat="server" Text="Project Name:"></asp:Label>
    <br />
    <asp:TextBox ID="txtProjectName" runat="server" Width="400px"></asp:TextBox>
    <br />
    <br />
    <asp:Label ID="lblCustomer" runat="server" Text="Customer:"></asp:Label><br />
    <asp:DropDownList ID="txtCustomerPk" runat="server" Width="400px">
    </asp:DropDownList>
    <br />
    <br />
    <table cellpadding="4px" style="width:400px;" >
    <tr class="gridalternate">
    <td><asp:Label ID="Label1" runat="server" Text="Start Date:" /></td>
    <td><asp:Label ID="lblEndDate" runat="server" Text="End Date:"></asp:Label></td>
    <td><asp:Label ID="Status" runat="server" Text="Status:"></asp:Label></td>
    </tr>
    <tr style="background:cornsilk">
    <td><ww:jQueryDatePicker ID="txtStartDate" runat='server' Width='80px'></ww:jQueryDatePicker></td>
    <td><ww:jQueryDatePicker ID="txtEndDate" runat='server' Width='80px'></ww:jQueryDatePicker></td>
    <td>
        <asp:DropDownList ID="txtStatus" runat="server">
        <asp:ListItem Text="Entered" Value="0"></asp:ListItem>
        <asp:ListItem Text="Started" Value="1"></asp:ListItem>
        <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
    </asp:DropDownList>   
    <br /> 

    </td>
    </tr>    
    </table>    
    
    <br />
   
    <hr />
    <input type="button" class="smallbutton" value="Show Entries" style="float:right" onclick="ShowEntries();"/>
    <asp:Button ID="btnSubmit" runat="server" Text="Save Project"  
        CssClass="submitbutton" onclick="btnSubmit_Click" />        
    
</div>
</div>
</center>

    
    <ww:DragPanel ID="EntryList" runat="server" 
                    class="blackborder" 
                    style="background: white; height:500px; width: 500px;display: none; position: absolute; top: 200px; left: 200px; " 
                    Draggable="true"
                    DragHandleID="EntryListHeader"                    
                    ShadowOffset="5"
                    Closable="True"
                    >
       <div id="EntryListHeader"  class="gridheader">Entry List</div>        
       <div id="EntryListContent">

       <%--Load content from EntryList.ascx User Control into here with AJAX--%>

       </div>
    </ww:DragPanel>
    
    <ww:AjaxMethodCallback runat="server" ID="Proxy" 
                         PostBackMode="PostMethodParametersOnly" 
                         ServerUrl="~/services/callbacks.ashx" >
    </ww:AjaxMethodCallback>

<ww:DataBinder ID="DataBinder" runat="server">
    <databindingitems>
<ww:DataBindingItem runat="server" BindingSource="Project.Entity" 
        BindingSourceMember="ProjectName" ControlId="txtProjectName" IsRequired="True"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
        BindingSource="Project.Entity" BindingSourceMember="CustomerPk" 
        ControlId="txtCustomerPk"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingSource="Project.Entity" 
        BindingSourceMember="StartDate" ControlId="txtStartDate" 
        DisplayFormat="{0:d}"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingSource="Project.Entity" 
        BindingSourceMember="EndDate" ControlId="txtEndDate" DisplayFormat="{0:d}"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
        BindingSource="Project.Entity" BindingSourceMember="Status" 
        ControlId="txtStatus"></ww:DataBindingItem>
</databindingitems>
</ww:DataBinder>


    <script type="text/javascript">
    function ShowEntries()
    {
        var ctl = $("#ctl00_lstFilter");        
        var filter = "RecentEntries";
        if (ctl)
           filter = ctl.val();
                 
        Proxy.ShowProjectEntries(filter,serverVars.projectPk,ShowEntries_Callback,OnPageError);
    }
    function ShowEntries_Callback(result)
    {                   
        $w("EntryListContent").innerHTML = result; 
        EntryList_DragBehavior.show();   
    }
    function OnPageError(error)
    {
        alert(error.message);
    }
    </script>
</asp:Content>
