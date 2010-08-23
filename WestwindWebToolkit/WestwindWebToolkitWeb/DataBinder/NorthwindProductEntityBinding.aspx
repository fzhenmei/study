<%@ Page Title="Northwind Product List" Language="C#" MasterPageFile="~/WestWindWebToolkit.master" 
         AutoEventWireup="true" 
         CodeBehind="NorthwindProductEntityBinding.aspx.cs" 
         Inherits="Westwind.WebToolkit.DataBinder.NorthwindProductEntityBinding" 
         EnableViewState="false"             
%>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register assembly="CSharpFormat" namespace="Manoli.Utils.CSharpFormat" tagprefix="manoli" %>

<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
.itemlabel
{
    font-weight: bold;
    margin-top: 10px;
}
.spanlabel
{
	width: 105px;
	margin-left: 10px;
	display: inline-table;	
}
.itemgroup
{
	padding-top: 7px;
}
.itemgroup input
{
	width: 75px;
}
.longtext
{
	width: 400px;
}
.groupheader
{
    background-color: lightsteelblue;
    color: navy;
    margin-top: 15px;
    margin-bottom: 0px;
    -moz-border-radius: 5px;
	-webkit-border-radius: 5px;
}
.groupcontent
{
     background: #eeeeee;
     margin-bottom: 10px;
     padding: 5px;
}
</style>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <div class="toolbarcontainer">
        <div style="float: right" class="hoverbutton">
            <ww:HelpControl ID="HelpControl" runat="server" 
                            HelpControlType="HelpLinkAndF1Handler"
                            HelpTopic="_1v51f7d88.htm"
            />            
        </div>
        <a href="../default.aspx" class="hoverbutton"><asp:Image runat="server" ImageUrl="~/css/images/home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
            <asp:Image  runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
        </asp:LinkButton> |         
        <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
        

    </div>    
    
    <div class="descriptionheader">
    This page demonstrates simple control binding using a business object to a Linq To Sql Entity in 
    a standard Postback interface. Fields are bound to product entity which are bound on first load 
    or product change. Data is then automatically unbound into the entity when the Save button is 
    clicked. Although binding here binds against a single entity you can bind multiple entities or 
    data sources with a single DataBinder.
    </div>
    <div class="toolbarcontainer">
        Select a Product: 
        <asp:DropDownList runat="server" ID="txtProductID" class="longtext" AutoPostBack="true"></asp:DropDownList>
        <asp:LinkButton runat="server" class="hoverbutton" ID="btnNewProduct" 
                        onclick="btnNewProduct_Click">
            <asp:Image runat="server" ImageUrl="~/css/images/new.gif" /> New
        </asp:LinkButton>            
    </div>
    
    <ww:ErrorDisplay ID="ErrorDisplay" runat="server" />    
    
    <div class="containercontent" style="padding: 10px 25x 20px">
        
        
        
        <div class="groupheader">Product Information</div>
        <div class="groupcontent">
            
            <div style="float:right;margin-right: 30px;">
                Pk: <asp:TextBox runat="server" ID="txtPk" style="width: 25px;"  ReadOnly="true"/>
            </div>
            
            <div class="itemlabel">Product Name:</div>
            <asp:TextBox runat="server" ID="txtProductName" class="longtext"></asp:TextBox>          
            
            
            <div class="itemlabel">Category:</div>
            <asp:DropDownList runat="server" ID="txtCategoryID" class="longtext"></asp:DropDownList>
     
            
            <div class="itemlabel">Supplier:</div>
            <asp:DropDownList runat="server" ID="txtSupplierID" class="longtext"></asp:DropDownList>
            
        </div>
                
        <div class="groupheader">Unit Information</div>
        
        <div class="groupcontent">
            <table>
            <tr class="itemgroup">
                <td class="spanlabel">Unit Price:</td>        
                <td><asp:TextBox runat="server" ID="txtUnitPrice" style="text-align:right"></asp:TextBox>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ControlToValidate="txtUnitPrice"
                    ErrorMessage="Unit Proice is required" />
                </td>
            
                <td class="spanlabel">Units in Stock:</td>
                <td><asp:TextBox runat="server" ID="txtUnitsInStock" style="text-align:right"></asp:TextBox></td>            
                <td></td>
            </tr>
            
            <tr class="itemgroup">            
                <td class="spanlabel">Reorder Level:</td>
                <td><asp:TextBox runat="server" ID="txtReorderLevel" style="text-align:right"></asp:TextBox></td>
                
                <td class="spanlabel">Units on Order:</td>
                <td><asp:TextBox runat="server" ID="txtUnitsOnOrder" style="text-align:right"></asp:TextBox></td>
                
                <td class="spanlabel">Expected on:</td>
                <td><ww:jQueryDatePicker runat="server" ID="txtExpectedDate" /></td>
            </tr>            
            </table>            
        </div>                    
        
        <asp:CheckBox runat="server" ID="chkDiscontinued" Text="Discontinued" />
        
        <div class="groupheader">Binding to arbitrary Page Objects  
            <small><asp:CheckBox runat="server" ID="chkShowValues" 
                                 Text="Show these values in Save message" checked="false"/></small>
        </div>            
        
        <div class="groupcontent">
            <table>
            <tr>
                <td>
                    <div class="itemlabel">StringVar:</div>
                    <asp:TextBox runat="server" ID="txtStringVar"></asp:TextBox>
                </td>
                <td>
                    <div class="itemlabel">DecimalVar:</div>
                    <asp:TextBox runat="server" ID="txtDecimalVar"></asp:TextBox>
                </td>
                <td>
                    <div class="itemlabel">DateVar:</div>
                    <ww:jQueryDatePicker runat="server" ID="txtDateVar" />
                </td>
                <td>
                    <div class="itemlabel">BoolVar:</div>
                    <asp:CheckBox runat="server" ID="chkBoolVar" Text="Is it true?" />
                </td>
            </tr>
            </table>
            
            <div class="itemlabel">Title of this Page:</div>
            <asp:TextBox runat="server" ID="txtPageTitle" class="longtext"></asp:TextBox>
        </div>
         
        <asp:Button runat="server" ID="btnSave" Text="Save Changes" 
                    AccessKey="S"
                    class="submitbutton" OnClick="btnSave_Click" />
        
    </div>
        
    <%--    
            Databinder stores the Control -> DataSource associations in the control.
            The Extender Control allows making interactive changes on each control
            in the designer.
            
            Note a default binding source can be set on the DataBinder, but individual
            control bindings can override the BindindSource property to bind to any
            object reachable through the Page (this or me) reference.
    --%>
    <ww:DataBinder ID="DataBinder" runat="server" 
        DefaultBindingSource="this.Product.Entity">
        <DataBindingItems>
            <ww:DataBindingItem runat="server" BindingSource="this.Product.Entity" 
                BindingSourceMember="ProductName" ControlId="txtProductName" 
                IsRequired="True">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
                BindingSourceMember="CategoryID" ControlId="txtCategoryID" 
                BindingSource="this.Product.Entity">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingProperty="SelectedValue" 
                BindingSourceMember="SupplierID" ControlId="txtSupplierID">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" 
                ControlId="txtUnitPrice" BindingSourceMember="UnitPrice" DisplayFormat="{0:c}" >
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingSourceMember="UnitsInStock" 
                ControlId="txtUnitsInStock" UserFieldName="Units in Stock">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingSourceMember="UnitsOnOrder" 
                ControlId="txtUnitsOnOrder">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" ControlId="txtExpectedDate" 
                BindingSourceMember="ExpectedDate" DisplayFormat="{0:d}">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingProperty="Checked" 
                BindingSourceMember="Discontinued" ControlId="chkDiscontinued">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingSourceMember="ReorderLevel" 
                ControlId="txtReorderLevel">
            </ww:DataBindingItem>
            <ww:DataBindingItem  runat="server" BindingSourceMember="ProductId" 
                ControlId="txtPk" BindingMode="OneWay">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingSource="this.CustomType" 
                BindingSourceMember="StringVar" ControlId="txtStringVar">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingSource="this.CustomType" 
                BindingSourceMember="FloatVar" ControlId="txtDecimalVar">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingSource="this.CustomType" 
                BindingSourceMember="DateVar" ControlId="txtDateVar" DisplayFormat="{0:d}" 
                UserFieldName="Date Input Variable">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" ControlId="txtPageTitle"
                BindingSource="this" BindingSourceMember="Title">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" BindingProperty="Checked" 
                BindingSource="this.CustomType" BindingSourceMember="BoolVar" 
                ControlId="chkBoolVar">
            </ww:DataBindingItem>
            <ww:DataBindingItem runat="server" 
                ControlId="txtProductID" BindingProperty="SelectedValue">
            </ww:DataBindingItem>
        </DataBindingItems>
    </ww:DataBinder>

   
    
    <div class="descriptionheader">
    <h3>Things to try on this page:</h3>
    <ul>
        <li>
            Leave the product name blank<br />
            you should see a business object rule validation binding error
        </li>
        <li>
            Put in invalid numeric or date data<br />
            you should see binding errors in the appropriate controls.        
        </li>
        <li>
            Set the Page Title field<br />
            This field is bound to this.Title (Page.Title) and should affect
            the HTML document title of the page via binding.
        </li>
        <li>
            Check the Show these Value in Save message<br />
            Then change some of the fields that are bound to an arbitrary standalone
            object. You should see the values echo'd back after a successful save 
            operation. Note that you can bind controls to any object including to Page 
            properties.            
        </li>
    </ul>
    </div>
    
    <div class="toolbarcontainer">
        <manoli:ViewSourceControl ID="ViewSourceControl" runat="server" 
                                  Text="Show ASPX" DisplayState="Button" 
                                  CodeFile="NorthwindProductEntityBinding.aspx"   />
                                  
        <manoli:ViewSourceControl ID="ViewSourceControl1" runat="server" 
                                  Text="Show CodeBehind" DisplayState="Button" 
                                  CodeFile="NorthwindProductEntityBinding.aspx.cs"    />            
    </div> 


                                 
</asp:Content>
