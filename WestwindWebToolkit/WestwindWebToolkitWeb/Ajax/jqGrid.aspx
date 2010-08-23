<%@ Page Title="jqGrid with JSON Data from AjaxMethodCallback" Language="C#" MasterPageFile="~/WestWindWebToolkit.master" AutoEventWireup="true" CodeBehind="jqGrid.aspx.cs" Inherits="Westwind.WebToolkit.Ajax.jqGrid" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">
    <link href="../scripts/themes/redmond/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../scripts/themes/redmond/ui.jqgrid.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>jqGrid with JSON Data From AjaxMethodCallback</h1>
    <div class="toolbarcontainer">
        <a href="./" class="hoverbutton">Home</a> |
        <a href="./jqGrid.aspx" class="hoverbutton">Refresh</a> |
        <a href="http://www.trirand.com/blog/" class="hoverbutton">jqGrid Site</a> |
        <a href="http://trirand.com/jqgrid/jqgrid.html" class="hoverbutton">jqGrid Samples</a> |
        <a href="http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options" class="hoverbutton">jqGrid Options Docs</a>
    </div>
    
    <div class="descriptionheader">q
    This example demonstrates how to feed pageable JSON data from the AjaxMethodCallback control and page methods to
    a <a href="http://www.trirand.com/blog/">jqGrid plug-in</a> for customer data and updates. Data is loaded via plain URL requests that route to
    the Proxy control on the page. To edit double click a row and press Enter to save to the server or ESC to abort edits or use the toolbar
    on the left to edit and add rows in an external window.
    </div>
    
    
    <div class="containercontent">
        <table id="gdCustomers" class="scroll" cellpadding="0" cellspacing="0"></table> 
        <div id="gdCustomersPager" class="scroll" style="text-align:center;"></div>         
    </div>
    
        <div class="containercontent">
        <table id="Table1" class="scroll" cellpadding="0" cellspacing="0"></table> 
        <div id="Div1" class="scroll" style="text-align:center;"></div>         
    </div>
    
    
    <div class="toolbarcontainer">
        <manoli:ViewSourceControl ID="ViewSourceControl" runat="server" Text="Show ASPX"
            DisplayState="Button" CodeFile="jqGrid.aspx" />
        <manoli:ViewSourceControl ID="ViewSourceControl1" runat="server" Text="Show CodeBehind"
            DisplayState="Button" CodeFile="jqGrid.aspx.cs" />
    </div>
    
    <ww:AjaxMethodCallback runat="server" ID="Proxy" />
    
 
    

    <script type="text/javascript">
        $(document).ready(function() {
            var gdCustomers = $("#gdCustomers");
            var lastSel;

            gdCustomers.jqGrid({
                url: "jqGrid.aspx?CallbackTarget=Proxy&Method=GetCustomerList",
                editurl: "jqGrid.aspx?CallbackTarget=Proxy&Method=UpdateCustomerRow",
                datatype: "json",
                colNames: ["Name", "Company", "Entered"],
                colModel: [
                             { name: "Name", index: "Name", width: 150, editable: true, edittype: "text" },
                             { name: "CompanyName", index: "CompanyName", width: 200, editable: true },
                             { name: "Entered", index: "Entered", width: 75, editable: true, datefmt: "MM/dd/yyyy" }
                             ],
                sortname: "Company",
                rowNum: 14,
                viewrecords: true,
                sortorder: "asc",
                caption: "Northwind Customer List",
                width: 800,
                height: 308,
                pager: "#gdCustomersPager",
                ondblClickRow: function(id) {
                    gdCustomers.restoreRow(lastSel);
                    gdCustomers.editRow(id, true);
                    lastSel = id;
                }
            }).navGrid("#gdCustomersPager");

            
//            Proxy.GetCustomerList(function(custList) {
//                alert(custList); // object is in right format                
//                gdCustomers.addJSONData(custList);
//            });

//                for (var i = 0; i < custList.total; i++) {
//                     gdCustomers.addRowData(i.toString(), custList.rows[i]);
//                }
//            }, onPageError);
        });
    </script>
    
    <ww:ScriptContainer ID="scripts" runat="server" RenderMode="Header">
        <Scripts>
            <script src="~/Scripts/jquery.js" resource="jquery"></script>
            <script src="~/Scripts/ww.jquery.js" resource="ww.jquery"></script>

            <script src="~/scripts/jquery-ui-custom.min.js" type="text/javascript"></script>
            <script src="~/scripts/ThirdParty/i18n/grid.locale-en.js" type="text/javascript"></script>
            <script src="~/scripts/ThirdParty/jquery.jqGrid.min.js" type="text/javascript"></script>
        </Scripts>
    </ww:ScriptContainer>
</asp:Content>
