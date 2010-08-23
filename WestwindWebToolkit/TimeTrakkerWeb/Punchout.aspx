<%@ Page Language="C#" MasterPageFile="~/TimeTrakkerMaster.Master" 
                       CodeBehind="Punchout.aspx.cs"
    Inherits="TimeTrakkerWeb.Punchout" Title="Punch In New Time Entry - Time Trakker" 
    AutoEventWireup="false"    
    EnableViewState="false" EnableEventValidation="false"
%>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <center>                
        <div class="dialog" style="text-align: left;width:700px;">
            <div class="dialog-header">Punch Out Time Entry</div>
            
            <div  class="containercontent">
                
                <ww:ErrorDisplay ID="ErrorDisplay" runat="server" />
                
                <table cellpadding="10px;"><tr>
                <td valign="top">
                    <asp:Label runat="server" ID="lblTitle">Title:</asp:Label><br />
                    <asp:TextBox runat="server" ID="txtTitle" Width="300px"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Label runat="server" ID="lblCustomer">Customer:</asp:Label><br />
                    <asp:DropDownList runat="server" ID="lstCustomers" Width="275px" onchange="GetProjectsForCustomer(this.value);">
                    </asp:DropDownList>
                    <asp:Button runat="server" ID="btnNewCustomer" Text="..." Width="23px" />
                    <br />
                    <br />
                    <asp:Label runat="server" ID="lblProject">Project:</asp:Label><br />
                    <asp:DropDownList runat="server" ID="lstProjects" Width="275px">
                    </asp:DropDownList>
                    <asp:Button runat="server" ID="btnNewProject" Text="..." Width="23px" 
                                UseSubmitBehavior="false" OnClientClick="ShowProject();return false;" />
                </td>
                <td valign="top">
                    <div style="width: 310px; padding: 10px" class="gridalternate">
                        <fieldset>
                        <legend>Time in</legend>
                        <asp:Label runat="server" ID="lblDateIn">Date:</asp:Label>
                        <ww:jQueryDatePicker runat="server" ID="txtDateIn" Width="80px"  onblur="UpdateTotals();"/>
                        &nbsp;&nbsp;
                        
                        <asp:Label runat="server" ID="lblTimeIn">Time:</asp:Label>
                        <asp:TextBox runat="server" ID="txtTimeIn" Width="65px" onblur="UpdateTotals();" Title="press T to set current time"></asp:TextBox>
                        </fieldset>
                        <br />
                        <fieldset style="margin-bottom: 10px;">
                        <legend>Time out</legend>
                        <asp:Label runat="server" ID="lblDateOut">Date:</asp:Label>
                        <ww:jQueryDatePicker runat="server" ID="txtDateOut" Width="80px" onblur="UpdateTotals();" />
                        &nbsp;&nbsp;
                        <asp:Label runat="server" ID="lblTimeOut">Time:</asp:Label>                    
                        <asp:TextBox runat="server" ID="txtTimeOut" Width="65px" onblur="UpdateTotals();" Title="press T to set current time"></asp:TextBox>
                        </fieldset>

                        <div style="text-align: right">                    
                        <asp:Label runat="server" ID="lblTotalHours" CssClass="errormessage"></asp:Label>
                        <asp:Label runat="server" ID="lblLabelForTotalHours" Text=" &nbsp;&nbsp;x&nbsp;&nbsp;"></asp:Label>
                        <asp:Label runat="server" ID="lblRate" Text="Billing Rate:"></asp:Label>
                        <asp:TextBox runat="server" ID="txtRate" Width="75px" style=" width: 75px; text-align:right;" onblur="UpdateTotals();"></asp:TextBox>
                        <hr />
                        <div style="float: left;">
                            <asp:CheckBox runat="server" id="chkPunchedout" Enabled="false" Text="Punched out"   ForeColor="DarkGray"/> 
                            <asp:CheckBox runat="server" id="chkBilled" Text="Billed"/> 
                         </div>
                        <asp:Label runat="server" ID="lbllLabelForItemTotal" Text="Item Total:"></asp:Label>&nbsp;
                        <asp:Label runat="server" ID="lblItemTotal" CssClass="errormessage"  Width="75px"
                                   style=" text-align: right;"></asp:Label>
                        </div>
                    </div>                
                </td>
                </tr></table>
                <asp:Label runat="server" ID="lblNotes">Detailed Description:</asp:Label>
                <br />
                <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" Height="120px" Width="660" ></asp:TextBox>
                <hr />
                <div style="float:right"><asp:Button runat="server" ID="btnDelete" 
                        CssClass="smallbutton" Text="Delete" onclick="btnDelete_Click" /></div>
                <asp:Button runat="server" ID="btnPunchIn" Text="Punch Out Entry" CssClass="submitbutton"
                    OnClick="btnPunchOut_Click" /> 
                    <asp:Button runat="server" ID="btnLeaveOpen" Text="Save and Leave Open" CssClass="submitbutton"
                    OnClick="btnPunchOut_Click" />                    
            </div>
        </div>
    </center>
    
    <asp:HiddenField runat="server" ID="txtEntryId" />
    
    
    <%--Callback to the same page--%>
    <ww:AjaxMethodCallback ID="Proxy" runat="server" PostBackMode="Post" />    
    
    <%--Callback to the generic reusable handler--%>
    <ww:AjaxMethodCallback  ID="ProxyUpdateProjects" runat="server"  
                      PostBackMode="PostNoViewstate" 
                      ServerUrl="~/services/Callbacks.ashx"  />
    
    
    <ww:DataBinder ID="DataBinder" runat="server">
        <DataBindingItems>
            <ww:DataBindingItem ID="DataBindingItem1" runat="server" BindingSource="Entry.Entity"
                BindingSourceMember="Title" ControlId="txtTitle">
            </ww:DataBindingItem>
            <ww:DataBindingItem ID="DataBindingItem2" runat="server" BindingMode="OneWay"
                BindingSource="Entry.Entity" BindingSourceMember="Timein" ControlId="txtDateIn" BindingProperty="SelectedDate"
                DisplayFormat="{0:d}">
            </ww:DataBindingItem>
            
            <ww:DataBindingItem ID="DataBindingItem4" runat="server" BindingMode="OneWay"
                BindingSource="Entry.Entity" BindingSourceMember="Timein" ControlId="txtTimeIn"
                DisplayFormat="{0:hh:mm tt}">
            </ww:DataBindingItem>
            <ww:DataBindingItem ID="DataBindingItem5" runat="server" BindingSource="Entry.Entity"
                BindingSourceMember="Description" ControlId="txtNotes">
            </ww:DataBindingItem>
            <ww:DataBindingItem ID="DataBindingItem6" runat="server" ControlId="lstProjects"
                BindingMode="TwoWay" BindingProperty="SelectedValue" BindingSource="Entry.Entity"
                BindingSourceMember="ProjectPk">
            </ww:DataBindingItem>            
            
        <ww:DataBindingItem ID="DataBindingItem7" runat="server" BindingMode="OneWay"
                BindingSource="Entry.Entity" BindingSourceMember="TimeOut" ControlId="txtTimeOut"
                DisplayFormat="{0:hh:mm tt}"></ww:DataBindingItem>

        <ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
            BindingSource="Entry.Entity" BindingSourceMember="CustomerPk" 
            ControlId="lstCustomers"></ww:DataBindingItem>
        <ww:DataBindingItem runat="server" ControlId="txtDateOut" BindingMode="OneWay" 
                    BindingSource="Entry.Entity" BindingSourceMember="TimeOut" 
                    DisplayFormat="{0:d}" BindingProperty="SelectedDate"></ww:DataBindingItem>
        <ww:DataBindingItem runat="server" ControlId="txtRate" BindingSource="Entry.Entity" 
                    BindingSourceMember="Rate"></ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingMode="OneWay" BindingSource="Entry.Entity" 
                    BindingSourceMember="TotalHours" ControlId="lblTotalHours" DisplayFormat="{0:n2}"></ww:DataBindingItem><ww:DataBindingItem runat="server" ControlId="txtEntryId" BindingMode="OneWay" 
                    BindingProperty="Value" BindingSource="Entry.Entity" BindingSourceMember="Pk"></ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingMode="OneWay" BindingSource="Entry.Entity"  DisplayFormat="{0:n2}"
                    BindingSourceMember="ItemTotal" ControlId="lblItemTotal"></ww:DataBindingItem>
        <ww:DataBindingItem runat="server" ControlId="chkPunchedout" BindingMode="OneWay" 
                    BindingProperty="Checked" BindingSource="Entry.Entity" 
                    BindingSourceMember="PunchedOut"></ww:DataBindingItem>
        <ww:DataBindingItem ID="DataBindingItem3" runat="server" ControlId="chkBilled" 
                    BindingProperty="Checked" BindingSource="Entry.Entity" 
                    BindingSourceMember="Billed"></ww:DataBindingItem>            
        <ww:DataBindingItem runat="server" ControlId="btnDelete"></ww:DataBindingItem>
</DataBindingItems>
</ww:DataBinder>




    <div id="statusbar" 
         style="position: fixed; width: 100%; left: 0px;bottom: 5px; background: black; color: white; padding: 5px;height: 20px; opacity: .70; filter: alpha(opacity='70');display:none;">
    </div>

    <ww:ScriptContainer ID="scripts" runat="server" RenderMode="Header">
        <Scripts>
            <script src="Scripts/jquery.js" resource="jquery"></script>
            <script src="Scripts/ww.jquery.js" resource="ww.jquery"></script>
        </Scripts>
    </ww:ScriptContainer>

<script type="text/javascript">
// *** Initialization code - create controls
    var txtDateIn = $("#" + serverVars.txtDateId);
    var txtDateOut = $("#" + serverVars.txtDateOutId);
    var txtTimeIn = $("#" + serverVars.txtTimeInId);
    var txtTimeOut = $("#" + serverVars.txtTimeOutId);
    var txtRate = $("#" + serverVars.txtRateId);
    var lblTotalHours = $("#" + serverVars.lblTotalHoursId);
    var lblItemTotal = $("#" + serverVars.lblItemTotalId);
    var lstCustomers = $("#" + serverVars.lstCustomersId);
    var lstProjects = $("#" + serverVars.lstProjectsId);

// *** Just get the entry value
var entryPk = $("#" + serverVars.txtEntryIdId).val();

$(document).ready( function() 
{ 
    // *** Load related projects
    GetProjectsForCustomer(   $("#" + serverVars.lstCustomersId).val() );
        
     // *** Hook up letter 'T' to set current time in timeout
    jQuery("#" + serverVars.txtTimeOutId + ",#" + serverVars.txtTimeInId)
            .bind("keydown", function(e) 
                { 
                    if ( e.keyCode == 84 ) // t
                    {
                         e.target.value = new Date().formatDate("hh:mm t");
                         return false;
                    }
                }  
            );            
} );         
// *** Ajax callback to get project dropdown values
function GetProjectsForCustomer(CustPk)
{
    ProxyUpdateProjects.GetActiveProjectsForCustomer(CustPk * 1,
                function(result) {
                    console.dir(result);                    
                    console.dir(lstProjects);
                    var oldSel = lstProjects.val();

                    // *** Databind the result data
                    lstProjects.listSetData(result, { dataTextField: "ProjectName", dataValueField: "Pk" })
                        .listSelectItem(oldSel);
                },
                OnError);
}            
// *** Ajax callback to update pagee totals
function UpdateTotals()
{
    Proxy.UpdateTotals(entryPk,
        function (result)
        {   
           lblItemTotal.text( result.ItemTotal.toFixed(2));
           lblTotalHours.text( result.TotalHours); // *** NOTE: Value is returned as a string   
           txtTimeOut.val( result.TimeOut);
           txtTimeIn.val( result.TimeIn);   
        } );
}        
// *** Open project display in a new window
function ShowProject()
{           
    window.open("ShowProject.aspx?View=Small&id=" + lstProjects.element.value,"_blank","width=500,height=420");
    return; 
}
function statusMessage(message)
{
    jQuery("#statusbar").text(message).Show();
}
function OnError(error)
{
    alert("Callback error: " + error.Message);
}
</script>    
</asp:Content>

