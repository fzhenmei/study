<%@ Page Language="C#" MasterPageFile="~/TimeTrakkerMaster.Master" AutoEventWireup="true" CodeBehind="ShowCustomer.aspx.cs" Inherits="TimeTrakkerWeb.ShowCustomer" Title="Untitled Page" %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.tabpage
{
    border: solid 1px Navy;
    padding: 10px;
    height: 250px;
    background-color: #b5c7d6;    
}
</style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<center>
<div class="dialog" style="width:475px;">
<div class="dialog-header">Customer Information</div>
<div class="toolbarcontainer">
    <a href="ShowCustomer.aspx" class="hoverbutton"><img src="images/Customer.png" alt="New Customer"/> New</a>
    <asp:LinkButton runat="server" ID="btnDelete" class="hoverbutton" 
        onclick="btnDelete_Click">
    <img src="images/remove.gif"  alt="Delete"/> Delete
    </asp:LinkButton>        
</div>
<div style="padding: 20px;">
    <ww:ErrorDisplay ID="ErrorDisplay" runat='server' />
    
    <asp:Label ID="lblCompany" runat="server" Text="Company:"></asp:Label>
    <br />
    <asp:TextBox ID="txtCompany" runat="server" Width="400px"></asp:TextBox>
    <br />
    <br />    
    <asp:Label ID="lblFirstName" runat="server" Text="First Name:"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblLastName" runat="server" Text="Last Name:"></asp:Label>
    <br />
    <asp:TextBox ID="txtFirstName" runat="server" Width="190px"></asp:TextBox>&nbsp;
    <asp:TextBox ID="txtLastName" runat="server" Width="195px"></asp:TextBox>
    
    <br />
    <br />
    <ww:TabControl ID="Tabs" runat="server" TabHeight="25px" 
        SelectedTab="Address">
        <TabPages>
        <ww:TabPage runat="server" Caption="Address" TabPageClientId="Address" ActionLink="default" 
                ID="tabAddress"></ww:TabPage>
        <ww:TabPage runat="server" Caption="Contacts" TabPageClientId="Contact" ActionLink="default" 
                     ID="tabContactPage1"></ww:TabPage>                
            <ww:TabPage ID="tabBilling" runat="server" Caption="Billing" TabPageClientId="Billing" ActionLink="default">
            </ww:TabPage>         
        </TabPages>
    </ww:TabControl>

<div id="Address" class="tabpage" >
    Address:<br />
    <ww:wwTextBox ID="txtAddress"  TextMode="MultiLine" runat="server" 
                     height="50px" width="400px"></ww:wwTextBox>
    
    <br />
    <br />
    City:<br />
    <asp:TextBox runat="server" ID="txtCity"  Width="400"/>
    <br />
    <br />
    State:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    Zip:<br />
    <ww:wwDropDownList ID="txtState" runat="server" width="252px">
    </ww:wwDropDownList>
    &nbsp;
    <asp:TextBox ID="txtZip" runat="server" Width="129px"></asp:TextBox>
    <br />
    <br />
    Country:<br />
    <ww:wwDropDownList ID="txtCountries" runat="server" width="400">
    </ww:wwDropDownList>    
</div> 
<div id="Contact" class="tabpage">
    Email Address:<br />
    <asp:TextBox runat="server" ID="txtEmail"  Width="400px" /> 
    <br />
    <br />
    Phone:<br />
    <asp:TextBox runat="server" ID="txtPhone" width="400px" />
    <br />
    <br />
    Fax:<br />
    <asp:TextBox runat="server" ID="txtFax"  width="400px"/>
    <br />
    <br />
    Notes:
    <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" width="400px" Height="50px" />        
</div>
<div id="Billing" class="tabpage">
  
    Billing Rate:<br />
    <asp:TextBox runat="server" ID="txtBillingRate"  Width="100px" /> 
  
    <br />
    <br />
    Entered:<br />
    <ww:jQueryDatePicker ID="txtEntered" runat="server" />
    <br />
    <br />
    Last Updated:<br />
    <ww:jQueryDatePicker ID="txtUpdated" runat="server" />
    <br />
    <br />
    Last Order:<br />
    <ww:jQueryDatePicker ID="txtLastOrder" runat="server" />
  
</div>

    <br />
   
    <asp:Button ID="btnSubmit" runat="server" Text="Save Customer"  
                CssClass="submitbutton" onclick="btnSubmit_Click"  AccessKey="S"/>            
    <br />    
</div>
</div>
</center>
<ww:DataBinder ID="DataBinder" runat="server">
    <databindingitems>
<ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
        BindingSourceMember="Company" ControlId="txtCompany" IsRequired="True"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" ControlId="txtFirstName" 
        BindingSource="Customer.Entity" BindingSourceMember="FirstName"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" ControlId="txtLastName" 
        BindingSource="Customer.Entity" BindingSourceMember="LastName"></ww:DataBindingItem>
<ww:DataBindingItem runat="server" ControlId="txtAddress" 
            BindingSource="Customer.Entity" BindingSourceMember="Address"></ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
            BindingSourceMember="City" ControlId="txtCity">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
            BindingSourceMember="BillingRate" ControlId="txtBillingRate">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
            BindingSourceMember="Entered" ControlId="txtEntered" DisplayFormat="{0:d}">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
            BindingSourceMember="Updated" ControlId="txtUpdated" DisplayFormat="{0:d}">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
            BindingSourceMember="LastOrder" ControlId="txtLastOrder" 
            DisplayFormat="{0:d}">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" 
            ControlId="txtNotes" BindingSource="Customer.Entity" 
            BindingSourceMember="Notes">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
            BindingSourceMember="Fax" ControlId="txtFax">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
            BindingSourceMember="Phone" ControlId="txtPhone">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
            BindingSourceMember="Email" ControlId="txtEmail">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
            BindingSource="Customer.Entity" BindingSourceMember="State" 
            ControlId="txtState">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
            BindingSource="Customer.Entity" BindingSourceMember="CountryId" 
            ControlId="txtCountries">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" BindingSource="Customer.Entity" 
            BindingSourceMember="Zip" ControlId="txtZip">
        </ww:DataBindingItem>
        <ww:DataBindingItem runat="server" ControlId="btnDelete">
        </ww:DataBindingItem>
</databindingitems>
</ww:DataBinder>

</asp:Content>
