<%@ Page Language="C#" 
         MasterPageFile="~/TimeTrakkerMaster.Master" 
         AutoEventWireup="false" EnableViewState="false" EnableEventValidation="false"
         CodeBehind="TimeReport.aspx.cs" 
         Inherits="TimeTrakkerWeb.TimeReport" 
         Title="Time Reports - Time Trakker"          
%>

<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>         
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <center>
    
    <ww:ErrorDisplay runat="server" ID="ErrorDisplay" />    
    
    <div id="divDialogOutline" class="dialog" style="width: 620px;">
    <div class="dialog-header">Time Reports</div>
    
    <div class="containercontent">
   
    <div style="float:right;padding: 10px;width: 350px; "> 
        <fieldset style="padding: 10px;">
        <legend>Filters</legend>
        <asp:Label runat="server" ID="lblFrom" Text="From:" ></asp:Label> <ww:jQueryDatePicker runat="server" ID="txtFrom" Width="80px" />&nbsp;&nbsp;
        <asp:Label runat="server">To:</asp:Label><ww:jQueryDatePicker runat="server" ID="txtTo" Width="80px" />
        <br /><br />
        Generate report for:<br />
        <ww:wwDropDownList ID="lstBillType" runat='server' width="250px">
            <asp:ListItem Text="Unbilled entries" Value="Unbilled"></asp:ListItem>
            <asp:ListItem Text="Billed entries" Value="Billed"></asp:ListItem>
            <asp:ListItem Text="Both" Value="All"></asp:ListItem>
        </ww:wwDropDownList>              
        </fieldset>
        
        <br />
        
        <fieldset style="padding: 10px;">
        <legend>Output Options</legend>
            <ww:wwCheckBox ID="chkMarkAsBilled" runat='server' size='30' Text="Mark as billed after run" />&nbsp;
            <ww:wwCheckBox ID="chkUnmarkAsBilled" runat='server' size='30' 
                Text="Unmark" /><br />
            <ww:wwCheckBox ID="chkCopyToXml" runat='server' size='30' Text="Generate as XML"  Enabled="false"/><br />
            <ww:wwCheckBox ID="chkSummaryReport" runat='server' size='30' Text="Summary report" Enabled="false" />
            <br />
            <br />        
            <asp:Label runat="server" ID="lblReportType" Text="Report to run:"></asp:Label><br />
            <ww:wwDropDownList ID="lstReportType" runat='server' Width='250px'>
                <asp:ListItem Text="Time sheet by client" Value="TimeSheetClient"></asp:ListItem>
                <asp:ListItem Text="Time sheet by project" Value="TimeSheetProject"></asp:ListItem>                
            </ww:wwDropDownList>        
        </fieldset>        
    </div>  
    <div>
    <ww:wwCheckBox runat="server" Text="Select all customers" ID="chkSelectAll" /><br />
    <ww:wwListBox ID="lstCustomers" runat='server' Width='200px' Height="275px" 
            SelectionMode="Multiple">    
    </ww:wwListBox>  
    <hr />
    <asp:Button runat="server" ID="btnSubmit" cssClass="submitbutton" Text="Run Report" 
            onclick="btnSubmit_Click"  />    
    </div>
</div>            
</div>
</center>
        
        
        <ww:DataBinder ID="DataBinder" runat="server">
            <databindingitems>
<ww:DataBindingItem runat="server" BindingSource="ReportParameters" 
                BindingSourceMember="ToDate" ControlId="txtTo" DisplayFormat="{0:d}"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingSource="ReportParameters" 
                BindingSourceMember="FromDate" ControlId="txtFrom" DisplayFormat="{0:d}"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
                BindingSource="ReportParameters" BindingSourceMember="BillType" 
                ControlId="lstBillType"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingProperty="Checked" 
                BindingSource="ReportParameters" BindingSourceMember="MarkAsBilled" 
                ControlId="chkMarkAsBilled"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingProperty="Checked" 
                BindingSource="ReportParameters" BindingSourceMember="GenerateXml" 
                ControlId="chkCopyToXml"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingProperty="Checked" 
                BindingSource="ReportParameters" BindingSourceMember="SummaryReport" 
                ControlId="chkSummaryReport"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
                BindingSource="ReportParameters" BindingSourceMember="ReportType" 
                ControlId="lstReportType"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" ControlId="chkUnmarkAsBilled" BindingProperty="Checked" 
                    BindingSource="ReportParameters" BindingSourceMember="UnmarkAsBilled"></ww:DataBindingItem>
</databindingitems>
        </ww:DataBinder>

    <script type="text/javascript">
    $(document).ready( function() {
        
        // *** Code to handle the list batch selection/unselection 
        var jSel = $("#<%= lstCustomers.ClientID %>");        
        var jChkSel = $("#<%= chkSelectAll.ClientID %>");
        
        // *** Mark all as checked or unchecked
        jChkSel.click(function() {                                                            
            var lis = jSel.find("option");            
            var checked = this.checked;                       
            lis.each( function() {                                                 
                this.selected = checked; 
            } );
        });
        
        // *** unmark checkbox on any list item click
        var lis = jSel.find("option");    
        lis.click( function() {              
             jChkSel[0].checked = false;
        });
            
                
    });
    
    
    </script>
</asp:Content>
