<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppConfiguration.aspx.cs" Inherits="Westwind.WebToolkit.Admin.AppConfiguration" %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Application Configuration</title>
    <link href="../css/Westwind.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .tabpage
    {
    	height: 570px;
    	width: 650px;
    	background: #eeeeee;
    	padding: 20px;
    	border: solid 2px navy;
    }        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
     <h1>Application System Configuration</h1>
     <div class="toolbarcontainer">
        <a href="./" class="hoverbutton"><img src="../css/images/home.gif" /> Home</a> |
        <a href="AppConfiguration.aspx" class="hoverbutton"><img src="../css/images/refresh.gif" /> Refresh</a> |
     </div>
      
      <ww:ErrorDisplay runat="server" ID="ErrorDisplay" />           
      
      <div style="margin:20px 40px;">

      <ww:TabControl runat="server" ID="Tabs"  TabHeight="30" TabWidth="120" >
        <TabPages>
            <ww:TabPage TabPageClientId="tabApplication" ID="TabPage1" 
                Caption="Application">                
            </ww:TabPage>

            <ww:TabPage TabPageClientId="tabEmail" ID="EmailTab" 
                Caption="Email">
            </ww:TabPage>
        </TabPages>
      </ww:TabControl>
    
    
        
     <div id="tabApplication" class="tabpage">
        <div class="gridheader">Application Information</div>
        <p>
        <b>Application Title:</b><br />
            <asp:TextBox runat="server" ID="txtApplicationTitle" Width="480" />
        </p>

        <p>
        <b>Application Subtitle:</b><br />
            <asp:TextBox runat="server" ID="txtApplicationSubtitle" Width="480" />
        </p>

        
        <p>
        <b>Application Home Url:</b><br />
            <asp:TextBox runat="server" ID="txtHomeUrl" Width="480" />
        </p>
        
        
        <p>
        <b>Application Cookie Name:</b><br />
            <asp:TextBox runat="server" ID="txtCookieName" Width="480" />
        </p>
        <br />
        <br />
        <div class="gridheader">
                            Database Configuration</div>
                        <p>                                                        
                            <b>Database Connection String:</b><br />
                            <ww:wwTextBox ID="txtConnectionString" runat="server" Width="480px" size="90"></ww:wwTextBox>
                        </p>
                        <div class="gridheader">
                            Error Display and Logging</div>
                            <br />
                            <b>Error Display Mode: </b>
                            <asp:RadioButtonList ID="radDebugMode" 
                                runat="server" Width="608px" size="20" 
                                 RepeatDirection="Horizontal">
                                <asp:listitem value="Default">Asp.Net Default</asp:listitem>
                                <asp:listitem value="ApplicationErrorMessage">Application Error Message</asp:listitem>
                                <asp:listitem value="DeveloperErrorMessage">Detailed Developer Error Message</asp:listitem>
                            </asp:RadioButtonList>
                            <br />     
                            
                            <b>Logging:</b><br />
                            <p>
                            <asp:CheckBox runat="server" ID="chkLogErrors" Text="Enable Error Logging " />&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox runat="server" ID="chkLogWebRequests" Text="Enable Request Loggging" />&nbsp;&nbsp;&nbsp;
                            Log to: 
                                <asp:DropDownList runat="server" id="txtLogModes">
                                </asp:DropDownList>
                   
                            
                            <p>
                            <b>Log Connection String:</b><br />
                            <asp:TextBox runat="server" ID="txtLogConnectionString" Width="480" />
                            </p>
                            
                            <p>
                            <b>Log Table Name:</b><br />
                            <asp:TextBox runat="server" ID="txtLogFilename" Width="480" />                            
                            </p>                       

     </div>    
    

     
     
     
     <div id="tabEmail" class="tabpage">
                        <div class="gridheader">Email Server Information</div>
                        <p>
                            <b>Mail Server:</b>&nbsp; <span class="smallbody">(SMTP Server accessible
                                from Web Server)</span>
                            <br />
                            <asp:TextBox ID="txtMailServer" runat="server" Width="480px" size="20" UserFieldName="Mail Server"></asp:TextBox><br />
                            <b>
                                <br />
                                Username and Password for Mail Server (optional):</b><br />
                            Username:
                            <ww:wwTextBox ID="txtMailUsername" runat="server" Width="192px" size="20" UserFieldName="Mail Server Username"
                                          BindingSource="Config" BindingSourceMember="MailUsername" ErrorMessageLocation="RedTextAndIconBelow"/>&nbsp;
                            Password:
                            <ww:wwTextBox ID="txtMailPassword" runat="server" Width="240px" size="20" UserFieldName="Mail Server Password"
                                          TextMode="Password" />
                        </p>
                        <div class="gridheader">
                            Email Confirmation Details</div>
                        <p>
                            This information is used to send confirmation emails for 
                            customer notifications.
                            <br />
                            If these values are blank no confirmation emails are sent.
                        </p>
                        <p>
                            <b>Sender Name:</b>&nbsp; <small>(descriptive name of the
                                sender, ie. John Doe)</small>
                            <br /> 
                            <asp:TextBox ID="txtMailSenderName" runat="server" size="50"></asp:TextBox><br />
                            <b>
                                <br />
                                Sender Email Address:</b>&nbsp;&nbsp;<small>(becomes reply to email address)</small><br />
                            <asp:TextBox ID="txtMailSenderEmail" runat="server" size="50" ></asp:TextBox><br />
                            <b>
                                <br />
                                Mail CC List:</b>&nbsp; <small>(CCs sent on confirmation.
                                Can be comma delimited list.)</small><br />
                            <asp:TextBox ID="txtMailCCList" runat="server" size="50"></asp:TextBox></p>
                        <div class="gridheader">
                            Administrative Email Confirmation Details</div>
                        <p>
                            Administrative emails are sent when errors occur on the server. These error emails
                            contain extensive error information that is useful for administrators to either
                            debug applications or pass on the information to the developers to fix.<br />
                            <br />
                            <asp:CheckBox ID="txtSendAdminEmail" runat="server" Font-Bold="True" Text="Send Administrative Emails"
                                size="20" UserFieldName="Admin Email"></asp:CheckBox><br />
                            
                            <p>
                            <b>Admin Recipient Email  Address:</b>&nbsp; <small>(email address to send admin emails to)</small><br />                            
                            <asp:TextBox ID="txtMailAdminEmail" runat="server" Width="336px" size="50" UserFieldName="Mail Server"></asp:TextBox>
                            </p>
                            
                             <p>                            
                            <b>Admin Sender Name:</b>&nbsp; <small>(descriptive name
                                of the Admin Sender)</small><br />                            
                            <asp:TextBox ID="txtMailAdminSenderName" runat="server" Width="336px" size="50" UserFieldName="Mail Server"></asp:TextBox><br />
                            </p>
                            
                            

                    </div>

                </div>
                    
                <div class="toolbarcontainer">
                    <asp:Button runat="server" ID="btnSave" Text="Save" 
                        class="submitbutton" AccessKey="s" 
                        onclick="btnSave_Click" />              

                </div>    

         </div>
        
    
      <ww:DataBinder runat="server" id="DataBinder">
          <DataBindingItems>
              <ww:DataBindingItem runat="server" 
                  ControlId="txtMailServer" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="MailServer">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="ConnectionString" 
                  ControlId="txtConnectionString">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingProperty="SelectedValue" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="DebugMode" ControlId="radDebugMode">
              </ww:DataBindingItem>

              <ww:DataBindingItem runat="server" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="MailServerUsername" 
                  ControlId="txtMailUsername">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="MailServerPassword" 
                  ControlId="txtMailPassword">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="SenderEmailName" 
                  ControlId="txtMailSenderName">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="SenderEmailAddress" 
                  ControlId="txtMailSenderEmail">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="MailCc" ControlId="txtMailCCList">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingProperty="Checked" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="AdminSendEmails" 
                  ControlId="txtSendAdminEmail">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="AdminEmailName" 
                  ControlId="txtMailAdminSenderName">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  ControlId="txtMailAdminEmail" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="AdminEmailAddress">
              </ww:DataBindingItem>
              

              <ww:DataBindingItem runat="server" 
                  ControlId="txtApplicationTitle" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="ApplicationTitle">
              </ww:DataBindingItem>

              <ww:DataBindingItem runat="server" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="ApplicationHomeUrl" 
                  ControlId="txtHomeUrl">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="ApplicationCookieName" 
                  ControlId="txtCookieName">
              </ww:DataBindingItem>

              <ww:DataBindingItem runat="server" 
                  ControlId="txtApplicationSubtitle" 
                  BindingSource="this.Configuration" 
                  BindingSourceMember="ApplicationSubtitle">
              </ww:DataBindingItem>            

              <ww:DataBindingItem runat="server" 
                  BindingProperty="Checked" 
                  BindingSource="this.LogConfiguration" 
                  BindingSourceMember="LogErrors" ControlId="chkLogErrors">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingProperty="Checked" 
                  BindingSource="this.LogConfiguration" 
                  BindingSourceMember="LogWebRequests" 
                  ControlId="chkLogWebRequests">
              </ww:DataBindingItem>
              <ww:DataBindingItem runat="server" 
                  BindingProperty="SelectedValue" 
                  BindingSource="this.LogConfiguration" 
                  BindingSourceMember="LogAdapter" ControlId="txtLogModes">
              </ww:DataBindingItem>
              
              <ww:DataBindingItem runat="server" 
                  ControlId="txtLogConnectionString" 
                  BindingSource="this.LogConfiguration" 
                  BindingSourceMember="ConnectionString">
              </ww:DataBindingItem>            
              <ww:DataBindingItem ID="DataBindingItem1" runat="server" 
                  ControlId="txtLogFilename" 
                  BindingSource="this.LogConfiguration" 
                  BindingSourceMember="LogFilename">
              </ww:DataBindingItem>            

          </DataBindingItems>
      
      </ww:DataBinder>
    </div>
    </form>
</body>
</html>
