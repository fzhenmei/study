<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" 
        Inherits="Westwind.WebToolkit._Default" 
         MasterPageFile="~/WestWindWebToolkit.master"  
         Title="West Wind Web Toolkit Samples"         
%>
<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
        <table width="100%">
        <tr>
        <td style="width: 150px; text-align: left; padding: 10px; vertical-align: top; background-image: url(images/newwave.jpg);background-repeat: repeat-y;">
            <img src="images/WestwindWebToolkit_100.jpg" />            
        </td>   
        
        
        <td class="containercontent" style="vertical-align: top;">
            <h3>Functional Examples</h3>
            
            <p>
            <a href="Ajax" class="headerlink">Ajax Samples</a>
            This sample group demonstrates the various Ajax features 
            of the toolkit including AjaxMethodCallback, HoverPanel,
            Modal Dialogs and the various features of the jQuery 
            base  client library.            
            </p>
            
            <p>
            <a href="AppConfigurationWebDemo/ApplicationConfigurationDemo.aspx" class="headerlink">
            ApplicationConfiguration Class Sample</a>
            This example demonstrates how to easily create strongly typed 
            configuration values that are persisted in various config sources
            and can be updated at runtime.            
            </p>
            
            <p>
            <a href="DataBinder/NorthwindProductEntityBinding.aspx" class="headerlink">DataBinder Examples</a>
            The DataBinder samples demonstrate how you can bind controls to values
            and explicitly perform databinding and unbinding using the DataBinder
            Extender control. <br />
            <small>Page and Arbitrary Object binding example</small>
            </p>
            
            <p>
            <a href="../Westwind.GlobalizationWeb/" class="headerlink">Localization with the Data Driven Resource Provider</a>
            Demonstrates the Data Driven Localization Resource Provider that uses the database to 
            store localization data. Provides a resource administration form that allows live editing
            of resources, refreshing of resources, the ability to import and export Resx resources as well
            as a control that allows interactive editing of resource data in a context sensitive manner.
            Also demonstrates the Ajax features of the Westwind.Web Ajax tools.
            <div class="gridalternate" style="padding: 10px;font-size: 0.80em;margin: 0 30px;">
            Note that if you're using the VS Web Server this link may not work. To run with the VS Server
            please use <i>View in Browser</i> on Default.aspx in the Westwind.GlobalizationWeb project.            
            </div>
            </p>            
                        <p>
            <a href="Controls/Default.aspx" class="headerlink">Miscellaneous WebControl Demos</a>
            This page contains a conglomeration of Web Control samples from the toolkit including
            Pager, TabControl, MessageDisplay, ErrorDisplay and more.
            </p>    
        
            
            
        </td>
 
        <td class="gridalternate sidebar" style="border-left: solid 1px grey;padding: 10px;">
            <h3>Configuration</h3>
            
            <p>
            <a href="admin/" class="sidebarheader">Site Administration</a>
            Provides links to various site administration tasks such as configuration
            of main settings, reviewing and managing of log files and banner administration.
            </p>
            
            <p>
            <b class="sidebarheader">Web Site Configuration</b>
            The sample is set up to run using the Visual Studio Web Server, but we
            recommend you create a virtual directory for the example project as
            WestWindWebToolkitWeb.
            <br />
            <br />
            The WestWindToolkitWeb project is a Web Application Project (WAP) so any changes
            to code behind require re-compiling. This is not a requirement and the Toolkit
            works fine with stock projects, but we prefer working with WAP projects.
            </p>
            
            <p>
            <b class="sidebarheader">Database Configuration</b>
            A sample SQL Server 2008 database is provided in the APP_DATA folder,
            DevSamples. If you only have SQL 2005 run the DevSamplesDbPackage.exe
            to create a SQL 2005 database and overwrite the DevSamples.mdf/.log files.
            <br /><br />
            It's recommended that you Attach the database, but by default the database
            is set up to use the file based link. If you have problem with the file
            based link try to attach the database first.            
            </p>
            
       
            
        </td>
        
        </tr>
        </table>
</asp:Content>
