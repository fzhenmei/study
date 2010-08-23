<%@ Page Language="C#" AutoEventWireup="true" Inherits="Westwind.GlobalizationWeb._Default"  Codebehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Driven Localization</title>
    <link href="WestWind.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        li { margin-bottom: 12px; }
    </style>
</head>
<body style="margin:0px;">
<form id="form1" runat="server">
        <div class="gridheaderbig">
             Data Driven Resource Provider Samples
        </div>
        <div class="toolbarcontainer">
            <a href="../samples/" class="hoverbutton">
                <img src="images/home.gif" alt="Home" /> West Wind Web Toolkit Sample Home
            </a>
        </div>
        
        <div style="background:url(images/newwave.jpg);height:100%;left:0;position:absolute;width:165px;">
        
        </div>
        <div style="margin-left:180px;overflow:hidden;margin-top:15px;padding-right:20px;">
            
            <ul>
                <li>
                    <b><asp:HyperLink ID="lnkCultures" runat="server" NavigateUrl="~/Cultures.aspx">Culture Localization</asp:HyperLink></b><br />
                    <asp:Localize runat="server" ID="lblCultureLocalizations">This page demonstrates various culture localization switching mechanisms.</asp:Localize><br />
                </li>
                <li>
                <a href="CustomerList.aspx"><strong>CustomerList with Resource Editing</strong></a><strong>&nbsp;</strong><br />
                <asp:Localize runat="server" ID="lblCustomerListText">Simple form that demonstrates the data driven ResourceProvider Editing interface.</asp:Localize>
                </li>
                <li>
                    <b><asp:HyperLink runat="server" ID="lnkResourceAdmin" NavigateUrl="~/LocalizationAdmin/default.aspx">Web Resource Administration</asp:HyperLink></b><br />
                    <asp:Localize runat="server" ID="lblResourceAdminText">This is the resource Administration form that is used to administer resources for
                    the data base driven ResourceProvider interactively over the Web.</asp:Localize><br />
                </li>
                <li><a href="LocalizationAdmin/StronglyTypedGlobalResources.aspx">
                        <b>Generate Strongly Typed Global Resources</b>
                    </a>
                    
                    <br />
                    Custom generator that generates a strongly typed resources for global resources
                    in a project. Although ASP.NET has native support for strongly typed global resources,
                    the generated class for it doesn't use the ResourceProvider so it duplicates the
                    resource cache and doesn't work with any custom ResourceProviders. These generated
                    classes however do.
                </li>
            </ul>
            
            
       </div>
</form>

<div style="margin-left: 160px;">
    <hr />
    <div style="float:right;margin-right:5px;">
            <img src="images/help.gif" alt="Configuration Help" />
            <asp:HyperLink runat="server" ID="lnkSampleConfiguration" NavigateUrl="_readme.htm">Sample Configuration</asp:HyperLink><br />
            <img src="images/book.gif" alt="Documentation" />
            <asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl="http://www.west-wind.com/WestwindWebToolkit/docs?page=_1xl057dta.htm">Documentation</asp:HyperLink><br />
            <img src="images/book.gif" alt="Documentation" />
            <asp:HyperLink runat="server" ID="HyperLink3" NavigateUrl="http://www.west-wind.com/presentations/wwDbResourceProvider/">White Paper</asp:HyperLink><br />
            <img src="images/info.gif" alt="Home" />
            <asp:HyperLink runat="server" ID="HyperLink2" NavigateUrl="http://www.west-wind.com/WestwindWebToolkit/">West Wind Web Toolkit</asp:HyperLink></div>
    <a href="http://www.west-wind.com"><img src="images/wwtoollogo_text.gif" alt="West Wind"  border="0" align="left" /></a>
    © Rick Strahl, <a href="http://www.west-wind.com/">West Wind Technologies</a>
</div>    

    
    
</body>
</html>
