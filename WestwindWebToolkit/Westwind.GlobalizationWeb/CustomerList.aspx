<%@ Page Language="C#" AutoEventWireup="true" Inherits="Westwind.GlobalizationWeb.CustomerList"
                        EnableEventValidation="false" EnableViewState="true"  
                       meta:resourcekey="Page1"                        
                       Culture="auto" 
                       UICulture="auto"
 Codebehind="CustomerList.aspx.cs" %> 

<%@ Register Assembly="Westwind.Globalization" Namespace="Westwind.Globalization" TagPrefix="ww" %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Northwind Customer List Localization Sample</title>
    <link href="Westwind.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        td, table.blackborder > tbody td  { border: none 0px; }    
    </style>
</head> 
<body>   
    <form id="form1" runat="server">
        <h1><asp:Label runat="server" ID="lblPageHeader" meta:resourcekey="lblPageHeader" Text="Northwind Customer List"></asp:Label></h1>
        
        <div class="toolbarcontainer">
            <asp:HyperLink runat="server" ID="lnkHome" class="hoverbutton" NavigateUrl="~/default.aspx" Text="Home" meta:resourcekey="lnkHome" /> | 
            <asp:HyperLink runat="server" id="lnkPageRefresh" class="hoverbutton" NavigateUrl="CustomerList.aspx" Text="Reset Page" meta:resourcekey="lnkPageRefresh"></asp:HyperLink> |        
            <small><asp:Label runat="Server" id="lblPageCreated"  meta:resourcekey="lblPageCreated">Page created:</asp:Label>&nbsp;<%= DateTime.Now %> &nbsp;&nbsp;&nbsp;
            <asp:Label runat="Server" ID="lblClickOnIcons" style="color:red" meta:resourcekey="lblClickOnIcons">Click the red icons to pop up the localization form</asp:Label></small>        
        </div>
        
        <center>  
        <table class="blackborder" style="background: #eeeeee; text-align:left; margin: 30px;"
                cellpadding="0">
          <tr>
              <td colspan="2" class="gridheader" height="24">   
                <asp:Label runat="server" ID="lblDialogTitle" meta:resourcekey="lblDialogTitle" Text="Customer List"></asp:Label>
              </td>
           </tr>
            <tr>
               <td valign="top" style="height: 435px;vertical-align: top">
                    <asp:ListBox ID="lstCustomers" runat="server" Height="450px" Width="300px" 
                                 OnSelectedIndexChanged="lstCustomers_SelectedIndexChanged" 
                                 AutoPostBack="True" EnableViewState="true" meta:resourcekey="lstCustomers" >
                    </asp:ListBox>
                </td>
                <td valign="top" style="padding-left: 20px; padding-top: 10px; padding-right: 10px; height: 435px; vertical-align: top;">                        

                    <asp:Panel runat="server" id="CustomerInfoPanel" meta:resourcekey="CustomerInfoPanel">
                        <ww:ErrorDisplay ID="ErrorDisplay" runat='server' DisplayTimeout="5000" 
                                UseFixedHeightWhenHiding="false" meta:resourcekey="ErrorDisplay" Width="400px">
                        </ww:ErrorDisplay>
                        
                        <asp:HiddenField runat="server" ID="txtPK" Value="<%# this.Customer.Entity.CustomerID %>"/>       
                        
                        <asp:Label ID="lblName" runat="server" Text="Name:" Width="263px" meta:resourcekey="lblName"></asp:Label><br />
                        <asp:TextBox ID="txtContactName" runat="server" Width="350px" Text="<%# this.Customer.Entity.ContactName %>" meta:resourcekey="txtContactName" ></asp:TextBox><br />
                        <br />
                        <asp:Label ID="lblCompany" runat="server" Text="Company:" meta:resourcekey="lblCompany"></asp:Label><br />
                        <asp:TextBox ID="txtCompany" runat="server" Width="350px" Text='<%# this.Customer.Entity.CompanyName %>' meta:resourcekey="txtCompany"></asp:TextBox><br />
                        <br />
                        <asp:Label ID="lblAddress" runat="server" Text="Address:" meta:resourcekey="lblAddress"></asp:Label><br />
                        <asp:TextBox ID="txtAddress" runat="server" Width="350px" Height="35px" TextMode="MultiLine" Text="<%# this.Customer.Entity.Address %>" meta:resourcekey="txtAddress"></asp:TextBox><br />
                        <br />
                        <asp:Label ID="lblCity" runat="server" Text="City:" meta:resourcekey="lblCity"></asp:Label><br />
                        <asp:TextBox ID="txtCity" runat="server" Width="350px" Text="<%# this.Customer.Entity.City %>" meta:resourcekey="txtCity"></asp:TextBox><br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRegion" runat="server" Text="Region:" meta:resourcekey="lblRegion"></asp:Label><br />
                                    <asp:TextBox ID="txtRegion" runat="server" Width="74px" Text="<%# this.Customer.Entity.Region %>" meta:resourcekey="txtRegion"></asp:TextBox></td>
                                <td>
                                    &nbsp;&nbsp;<asp:Label ID="lblPostalCode" runat="server" Text="Postal Code:" meta:resourcekey="lblPostalCode"></asp:Label><br />
                                    &nbsp;&nbsp;<asp:TextBox ID="txtPostalCode" runat="server" Width="192px" Text="<%# this.Customer.Entity.PostalCode %>" meta:resourcekey="txtPostalCode"></asp:TextBox><br />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblCountry" runat="server" Text="Country:" meta:resourcekey="lblCountry"></asp:Label><br />
                        <asp:TextBox ID="txtCountry" runat="server" Width="350px" Text="<%# this.Customer.Entity.Country %>" meta:resourcekey="txtCountry" /><br />
                        <br />
                        <asp:Label ID="lblEntered" runat="server" Text="Entered:" meta:resourcekey="lblEntered"></asp:Label>
                        
                        <ww:jQueryDatePicker ID="txtEntered" runat="server"   SelectedDate="<%# this.Customer.Entity.Entered %>"/>
                        <br />
                        <hr />
                      </asp:Panel>

                      <asp:Button runat="server" ID="btnSave" Text=" Save Customer "  accesskey="S" style="width: 150px" OnClick="btnSave_Click" meta:resourcekey="btnSave" />
                      <asp:Button runat="server" ID="btnRefresh" Text=" Refresh List " style="width:150px;" meta:resourcekey="btnRefresh" />                        
                   </td>
                </tr>
                <tr runat="server" >
                <td colspan="2" style="text-align:center" >
                 <hr />
                 <center>
                 <asp:GridView ID="dgOrders" runat="server" Width="90%" AutoGenerateColumns="False" 
                                         cssclass="blackborder" 
                                         EnableViewState="False"
                                         GridLines="None"
                                         ShowFooter="True"                                          
                                         cellpadding="2" meta:resourcekey="dgOrders">
                                          
                            <AlternatingRowStyle CssClass="gridalternate" />
                            <FooterStyle CssClass="gridheader" />
                            <HeaderStyle CssClass="gridheader" />
                            
                            <Columns>                                
                            <asp:BoundField  DataField="OrderDate" DataFormatString="{0:d}" HeaderText="Date" 
                                             meta:resourcekey="BoundField" HtmlEncode="false">
                                <headerstyle font-italic="False" font-overline="False" font-strikeout="False"
                                    font-underline="False" horizontalalign="Center" width="200px"/>
                            </asp:BoundField>
                                <asp:BoundField DataField="OrderId" HeaderText="Order Number" meta:resourcekey="BoundField1">
                                    <itemstyle  font-italic="False" font-overline="False" font-strikeout="False"
                                        font-underline="False" horizontalalign="Center" />
                                    <headerstyle font-italic="False" font-overline="False" font-strikeout="False"
                                        font-underline="False" horizontalalign="Center" width="200px" />
                                    <footerstyle horizontalalign="Right" />                                        
                                </asp:BoundField>
                                <asp:BoundField DataField="OrderTotal" DataFormatString="{0:n}" HeaderText="Order Total" meta:resourcekey="BoundField2">
                                    <itemstyle  horizontalalign="Right" />
                                    <headerstyle horizontalalign="Right" />
                                    <footerstyle horizontalalign="Right" />
                                </asp:BoundField>
                            </Columns>
                  </asp:GridView>
                  </center>

                </td>
                </tr>
                            
            </table>
        </center>

   <!-- Localization Options Control -->
   <ww:DbResourceControl ID="Localizer" runat="server" 
                           meta:resourcekey="Localizer" 
                           CssClass="errordisplay" >
   </ww:DbResourceControl>    
   
        <div class="toolbarcontainer">
            <manoli:ViewSourceControl runat="server" CodeFile="CustomerList.aspx" Text="ASPX" />                        
            <manoli:ViewSourceControl ID="ViewSourceControl1" runat="server" CodeFile="CustomerList.aspx.cs" Text="CodeBehind" />
        </div>
        
        <div style="margin:25px;">
        <hr />
        This page operates with resources served from database using the wwDbResourceProvider.
        This page is localized into German and the Invariant English Language with the exception of this
        message - too lazy to translate. To check out the localized form switch the browser into any 
        German derivative language.
        <br />
        <br />
        To check out the resource localization features click on any of the red icons next
        to any of the controls which takes you to the Resource Administration form which
        allows browsing and editing of the available resources. If you feel ambitious you
        can try and localize this form into another language. To add a new language simply
        add select any resource key, then click Add to create a new key and add your new
        culture and enter a translation. Save and the new value will show up.
        <br />
        <br />
        The Resource Editing on the form is enabled simply by the wwDbResourceControl on
        the bottom of the form. When the control is enabled and the Show Localization Icons
        box is checked the icons appear. Display of the control can be globally controlled
        via the Resource Provider configuration.
        <hr />
        </div>
    
    <div class="blackborder gridalternate containercontent" ID="_resourcePanel" style="display:none">
    
    </div>
        

    <script type="text/javascript">
        
    </script>
        </form>    
</body>
</html>

