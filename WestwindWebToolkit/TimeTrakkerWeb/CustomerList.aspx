<%@ Page Language="C#" MasterPageFile="~/TimeTrakkerMaster.Master" AutoEventWireup="true" 
         CodeBehind="CustomerList.aspx.cs" 
         Inherits="TimeTrakkerWeb.CustomerList" Title="Customer List" %>
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
        background: orange url(images/lightorangegradient.png) repeat-x;           
        cursor: pointer;
    }
    .itemtemplate b
    {
        color: Navy;
    }
    .itemtemplate a
    {
        text-decoration: none;
        font-weight: bold;
    }
    .itemtemplate a:visited
    {
        color: Navy;
    }
    .namedisplay
    {    	    	
    	float: right;
    	font-size: 8pt;
    	color: Teal;
    }
    .custimg
    {
        width: 16px;
        height: 16px;
        margin-right: 10px;        
        background-image: url(images/customer.png);
        float: left;
    }
</style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <center>
    <ww:ErrorDisplay ID="ErrorDisplay" runat='server' />
    
    <div class="dialog" style="width:600px;">
    <div class="dialog-header-center">Customer List</div>
    <div class="toolbarcontainer">
    <small><a href="ShowCustomer.aspx" class="hoverbutton"><img src="images/punchin.gif" /> New</a> | Filter: 
        Company: 
        <asp:TextBox runat="server" ID="txtCompanyFilter" style="width: 75px;"  /> 
        Last Name:
        <asp:TextBox runat="server" ID="txtLastNameFilter" style="width: 75px;"  />         
        <asp:Button runat="server" ID="btnGoFilter" Text="Go" AccessKey="G"  />
    </small>
    </div>
        <div style="height: 410px; overflow: hidden; overflow-y: scroll;">
        <asp:Repeater runat="server" ID="dgCustomers">
        <ItemTemplate>
            <div class="itemtemplate" onclick="showCustomer(<%# Eval("Pk") %>)" >
                <div class="namedisplay" />                                
                <%# (string) Eval("FirstName") + " " + (string) Eval("LastName") %>
                </div>              
                <div class="custimg"></div>
                <a href="ShowCustomer.aspx?id=<%# Eval("Pk") %>"><%# Eval("Company") %></a>
            </div>
        </ItemTemplate>        
        </asp:Repeater>      
        </div>
        <div class="dialog-statusbar">
            <small><asp:Label runat="server" ID="lblStatus" /></small>
        </div>
    </div>
    </center>

<script type="text/javascript">
function showCustomer(pk)
{
    window.location = "ShowCustomer.aspx?id=" + pk.toString();
}
</script>        
</asp:Content>