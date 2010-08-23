<%@ Page Language="C#" 
         MasterPageFile="~/TimeTrakkerMaster.Master" 
         CodeBehind="Punchin.aspx.cs"
         Inherits="TimeTrakkerWeb.Punchin" Title="New Time Entry - Time Trakker" 
         EnableEventValidation="false"
%>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI.HtmlControls" tagprefix="cc1" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .widecontrol        
        {
            width: 400px;
        }                
        .widedropdown
        {
               width: 370px;
        }
        .morebuttons
        {
            width: 25px;
            padding: 1px;
        }
    </style>
 
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
      <%-- provides script into the page and Intellisense for script --%>    
     <ww:ScriptContainer ID="scripts" runat="server" RenderMode="Header">  
        <Scripts>  
            <Script src="Scripts/jquery.js" Resource="jquery"></Script>  
            <Script src="~/Scripts/ww.jquery.js"  Resource="ww.jquery"></Script>          
            <Script src="~/Scripts/jquery-ui.js" Resource="jqueryui" AllowMinScript="True"></Script>                              
        </Scripts> 
    </ww:ScriptContainer>
    
    <center>
        <div class="dialog" style="text-align: left;width: 450px;" id="mainWindow">
            <div class="dialog-header">
                New Time Entry
            </div>
            <div class="containercontent">
                <ww:ErrorDisplay ID="ErrorDisplay" runat="server" />
                
                <asp:Label runat="server" ID="lblTitle">Title:</asp:Label><br />
                <asp:TextBox runat="server" ID="txtTitle" class="widecontrol"></asp:TextBox>
                <br />
                <br />
                <asp:Label runat="server" ID="lblCustomer">Customer:</asp:Label><br />
                <asp:DropDownList runat="server" ID="lstCustomers" CssClass="widedropdown"
                                   onchange="GetProjectsForCustomer(this.value);">
                </asp:DropDownList>
                <asp:Button runat="server" ID="btnNewCustomer" Text="..." CssClass="morebuttons" />
                <br />
                <br />
                <asp:Label runat="server" ID="lblProject">Project:</asp:Label><br />
                <asp:DropDownList runat="server" ID="lstProjects" CssClass="widedropdown">
                </asp:DropDownList>
                <input type="button" ID="btnNewProject" 
                       value="..." class="morebuttons"
                       onclick="ShowProject(event);" />
                <br />
                <br />
                <div class="widecontrol gridalternate" style="padding: 5px; padding-top: 2px;">
                    <fieldset>
                    <legend> Time in: </legend>
                    <asp:Label runat="server" ID="lblDateIn">Date:</asp:Label>
                    <ww:jQueryDatePicker runat="server" ID="txtDateIn" Width="80px" />
                    &nbsp;
                    <asp:Label runat="server" ID="lblTimeIn">Time:</asp:Label>
                    <asp:TextBox runat="server" ID="txtTimeIn" Width="65px"></asp:TextBox>
                    </fieldset>
                </div>
                <br />
                <asp:Label runat="server" ID="lblNotes">Detailed Description:</asp:Label>
                <br />
                <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" Height="80px" Width="395"></asp:TextBox>
                <hr />
                <asp:Button runat="server" ID="btnPunchIn" Text="Punch In" AccessKey="S" CssClass="submitbutton"
                    OnClick="btnPunchIn_Click" />
            </div>
        </div>
    </center>
  
    <ww:DataBinder ID="DataBinder" runat="server">
        <DataBindingItems>
            <ww:DataBindingItem ID="DataBindingItem1" runat="server" BindingSource="Entry.Entity"
                BindingSourceMember="Title" ControlId="txtTitle">
            </ww:DataBindingItem>
            <ww:DataBindingItem ID="DataBindingItem2" runat="server" BindingMode="OneWay"
                BindingSource="Entry.Entity" BindingSourceMember="Timein" ControlId="txtDateIn" BindingProperty="SelectedDate"
                DisplayFormat="{0:d}">
            </ww:DataBindingItem>
            <ww:DataBindingItem ID="DataBindingItem3" runat="server" BindingSource="Entry.Entity"
                ControlId="lblTimeIn">
            </ww:DataBindingItem>
            <ww:DataBindingItem ID="DataBindingItem4" runat="server" BindingMode="OneWay"
                BindingSource="Entry.Entity" BindingSourceMember="Timein" ControlId="txtTimeIn"  
                DisplayFormat="{0:t}">
            </ww:DataBindingItem>
            <ww:DataBindingItem ID="DataBindingItem5" runat="server" BindingSource="Entry.Entity"
                BindingSourceMember="Description" ControlId="txtNotes">
            </ww:DataBindingItem>
            <ww:DataBindingItem ID="DataBindingItem6" runat="server" ControlId="lstProjects"
                BindingMode="TwoWay" BindingProperty="SelectedValue" BindingSource="Entry.Entity"
                BindingSourceMember="ProjectPk">
            </ww:DataBindingItem>            
        <ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
            BindingSource="Entry.Entity" BindingSourceMember="CustomerPk"  
            ControlId="lstCustomers"></ww:DataBindingItem>   
            <ww:DataBindingItem runat="server" BindingErrorMessage="" 
                ControlId="scripts">
            </ww:DataBindingItem>
</DataBindingItems>    
     </ww:DataBinder>
    
    
    <ww:HoverPanel ID="ProjectPanel" runat='server' class="dialog"              
             IFrameHeight="400px"   
             style="display:none;"         
             Width="550px"   
             ClientCompleteHandler="ProjectPanelComplete"                      
             DragHandleID="PanelHeader"                          
             Draggable="True"
             Closable="True" 
             ShadowOffset="5"                               
             ServerUrl="ShowProject.aspx" 
             EventHandlerMode="ShowIFrameInPanel"
     >
    <div id="PanelHeader" class="dialog-header">
           Project 
    </div>
    </ww:HoverPanel>

    <%-- Callback control used for Ajax Callbacks --%>
    <ww:AjaxMethodCallback  ID="Proxy" runat="server"  
                          PostBackMode="PostNoViewstate" 
                          ServerUrl="~/services/Callbacks.ashx" />   

 
    
<script type="text/javascript">
    $(document).ready(function() {       
        // Load related projects
        GetProjectsForCustomer($("#" + serverVars.lstCustomersId).val());

        // *** Hook up letter 'T' to set current time in timeout
        $("#" + serverVars.txtTimeInId)
                .bind("keydown", function(e) {
                    if (e.keyCode == 84) // t
                    {
                        e.target.value = new Date().formatDate("hh:mm t");
                        return false;
                    }
                });   
    });

function GetProjectsForCustomer(CustPk) {
    Proxy.GetActiveProjectsForCustomer(CustPk * 1,
            function(result) {
                // Databind the result array of data
                $("#" + serverVars.lstProjectsId).
                    listSetData(result, { dataValueField: "Pk", dataTextField: "ProjectName" });
            },
            OnError);
}
function ShowProject(e) {    
    ProjectPanel.startCallback(e, 'View=Small&id=' +
                               $("#" + serverVars.lstProjectsId).val(), null, OnError);
    $("#ProjectPanel").centerInClient();
}
function ProjectPanelComplete() {
    $("#ProjectPanel").centerInClient();            
}



function OnError(error)
{
    alert(error.message);
}
</script>    
</asp:Content>