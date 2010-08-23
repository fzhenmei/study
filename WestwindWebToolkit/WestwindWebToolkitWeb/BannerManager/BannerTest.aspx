<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BannerTest.aspx.cs" Inherits="Westwind.WebLog.BannerTest" %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Banner demonstration</title>
    <link href="../Css/Westwind.css" rel="stylesheet" type="text/css" />    
    <link href="../Css/WebToolkitSamples.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <h1>Banner Test</h1>
    <div class="toolbarcontainer">
        <a href="../" class="hoverbutton"><img src="../css/images/home.gif" /> Home</a> | 
        <a href="bannertest.aspx" class="hoverbutton"><img src="../css/images/refresh.gif" /> Refresh</a> | 
        <a href="../admin/banneradmin.aspx" class="hoverbutton"> Banner Manager</a> |         
    </div>
    
    <ww:ErrorDisplay runat="server" id="ErrorDisplay" />
        
    <div class="containercontent">
        <div class="samplebox">
            <b>Random Banner 1:</b> <small>(script tag with click tracking)</small><br />
            <%= Westwind.Web.Banners.BannerManager.Current.RenderNextBanner() %> 
        </div>

        <div class="samplebox">
            <b>Fixed Banner:</b> <small>(script tag with click tracking</small><br />
            <%= Westwind.Web.Banners.BannerManager.Current.RenderBanner("a416c019")%>            
        </div>        
        
        <div class="samplebox">
            <b>Random Banner 2:</b> <small>(link and image only - no click tracking)</small><br />
            <%= Westwind.Web.Banners.BannerManager.Current.RenderNextBannerLink()%>       
        </div>        
        
        <div class="samplebox">
            <b>Fixed Banner</b>: <small>(embedded as a plain script tag</small><br />
            <script src='/WestWindWebToolkitWeb/wwBanner.ashx?a=s&id=a416c019&t=633903995118140115' type='text/javascript'></script>
        </div>
        
        <div class="samplebox">
            <b>Random Banner</b>: <small>(embedded as a plain script tag (dotnet group)</small><br />
            <script src='/WestWindWebToolkitWeb/wwBanner.ashx?a=s&c=dotnet' type='text/javascript'></script>
        </div>
    </div>
        
    <div class="toolbarcontainer">
        <manoli:ViewSourceControl ID="ViewSourceControl" runat="server" Text="Show ASPX"
            DisplayState="Button" CodeFile="BannerTest.aspx" />
    </div>
    
        </form>
</body>
</html>
    