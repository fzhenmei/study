<%@Page Language="C#" 
         MasterPageFile="~/TimeTrakkerMaster.Master" 
         AutoEventWireup="false" 
         CodeBehind="OpenEntries.aspx.cs" Inherits="TimeTrakkerWeb.OpenEntries" 
         Title="Open Entries - Time Trakker" 
         EnableViewState="false"         
%>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    #itemtemplate
    {
        border-bottom: dashed 1px teal;
        padding: 9px;
        padding-left: 30px;
    }
    #itemtemplate:hover
    {
        background-image: url(images/lightorangegradient.png);
        background-image: repeat-x;           
        cursor: pointer;
    }
    #itemtemplate b
    {
        color: Navy;
    }
    #itemtemplate a
    {
        text-decoration: none;
    }
    #itemtemplate a:visited
    {
        color: Navy;
    }
    #divItemWrapper
    {
    	height: 350px;  
    	overflow: scroll; 
    	overflow-x: hidden;
    }
    #clockimg
    {
        width: 16px;
        height: 16px;
        margin-right: 15px;
        margin-bottom: 5px;
        background-image: url(images/punchout.gif);
        float: left;
    }
</style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
   <center>
   <ww:ErrorDisplay runat="server" id="ErrorDisplay" />
   
    <div class="dialog" style="width:500px;">        
        <div class="dialog-header">Select one of the Open Entries</div>

        <div id="divItemWrapper">
        <asp:ListView runat="server" ItemPlaceholderID="layoutContainer" ID="lstEntries"  >    
        <LayoutTemplate>
       
            <div id="layoutContainer" runat="server"  />
        </LayoutTemplate>
        
        <ItemTemplate>
        <div id="itemtemplate" onclick="window.location='<%# this.GetPunchOutUrl( (int) Eval("pk")  ) %>';">        
            <div id="clockimg"></div>
            <b><a href='<%# this.GetPunchOutUrl( (int) Eval("pk")  ) %>'><%# Eval("Title") %></a></b><br />
            <small><%# TimeUtils.FriendlyDateString( (DateTime)Eval("Timein"), true)  %></small>    
        </div>
        </ItemTemplate> 
        
        </asp:ListView>    
        </div>
    </div>
    </center>
</asp:Content>
