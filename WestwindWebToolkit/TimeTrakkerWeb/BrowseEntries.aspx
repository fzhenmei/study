<%@ Page Language="C#" MasterPageFile="~/TimeTrakkerMaster.Master" AutoEventWireup="false" 
          CodeBehind="BrowseEntries.aspx.cs" Inherits="TimeTrakkerWeb.BrowseEntries" 
          Title="Entry Browser - Time Trakker" %>

<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .itemtemplate
    {
        border-bottom: dashed 1px teal;
        padding: 10px;
        padding-left: 30px;
        height: 35px;
    }
    .itemtemplate:hover
    {
        background: transparent url(images/lightorangegradient.png) repeat-x;
        cursor: pointer;
    }
    .itemtemplate b
    {
        color: Navy;
    }
    .itemtemplate a
    {
        text-decoration: none;
    }
    .itemtemplate a:visited
    {
        color: Navy;
    }
    #divItemWrapper
    {
    	height: 460px; 
    	min-height: 460px; 
    	overflow: auto; 
    	overflow-y: scroll; 
    	overflow-x: hidden;
    }
    .gridalternate
    {
        background: cornsilk;
    }
    .openentryimg, .punchedoutimg
    {
        width: 25px;
        height: 18px;
        margin-right: 15px;        
        background-image: url(images/punchout.gif);
        background-repeat: no-repeat;
        float: left;  
        padding-top: 5px;           
    }   
    .punchedoutimg
    {
    	background-image: url(images/entry.png);
    }
    .toolbarcontainer select
    {
    	font-size: 8.25pt;
    	width: 200px;
    }
</style>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <center>
        <ww:ErrorDisplay ID="ErrorDisplay" runat='server' />
       <div class="dialog" style="width:650px;text-align: left;">
        
        <div id="browserHeader" class="dialog-header-center">Entry Browser</div>

        <div class="toolbarcontainer">
        <small><a href="punchin.aspx" class="hoverbutton"><img src="images/punchin.gif" /> New</a> 
        
        &nbsp;&nbsp;&nbsp;
        Filter: 
        <ww:wwDropDownList ID="lstFilter" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Recent Entries" Value="RecentEntries" />
            <asp:ListItem Text="Open Entries" Value="OpenEntries" />     
            <asp:ListItem Text="Recent Closed Entries" Value="RecentClosedEntries" />
            <asp:ListItem Text="All Entries" Value="All Entries" />
        </ww:wwDropDownList> 
        
        
        &nbsp;&nbsp;&nbsp;Customers: 
        <ww:wwDropDownList id="lstCustomer" runat="server" AutoPostBack="true" >            
        </ww:wwDropDownList>   
        </small>
        </div>        
    
    <div id="divItemWrapper" >
    <asp:ListView runat="server" ItemPlaceHolderID="layoutContainer" ID="lstEntries" >
    <LayoutTemplate>            
        <div id="layoutContainer" runat="server" />    
    </LayoutTemplate>
    
    <ItemTemplate>
        <div class="itemtemplate <%#  (AlternateFlag = !AlternateFlag) ? "gridalternate" : "" %> " 
             onclick="window.location='<%# this.GetPunchOutUrl( (int) Eval("pk")  ) %>';">        
            <div style="float: right;text-align: right; font-size: 8pt;">
                    <%# this.GetTimeDetail( Container.DataItem as TimeTrakker.EntryEntity )  %><br />
                    <a href="javascript:{}" onclick='deleteEntry(<%# Eval("Pk") %>,this)' class="hoverbutton"><img src="images/remove.gif" /></a>
            </div>
            <div class="<%# this.GetIconClass( Container.DataItem as EntryEntity ) %>"></div>
            <b><a href='<%# this.GetPunchOutUrl( (int) Eval("pk")  ) %>'><%# Eval("Title") %></a></b><br />        
            <small>
                    <i><%# ((EntryEntity) Container.DataItem).ProjectEntity.ProjectName %></i><br />
            </small>    
        </div>
    </ItemTemplate>    
    
    </asp:ListView> 
    </div>  
    
    <div class="dialog-statusbar" runat="server" id="divStatus">Ready</div>

    </center>

   <ww:ScriptContainer ID="scripts" runat="server" RenderMode="Header">
        <Scripts>
            <script src="~/Scripts/jquery.js" resource="jquery"></script>
            <script src="~/Scripts/ww.jquery.js" resource="ww.jquery"></script>
        </Scripts>
    </ww:ScriptContainer>
    

    <ww:AjaxMethodCallback runat="server" ID="Proxy" ServerUrl="~/Services/Callbacks.ashx" />    
    
    <script type="text/javascript">
        function deleteEntry(pk,ev) {
            Proxy.DeleteEntry(pk, function(result) {                
                var e = $.event.fix(ev);

                $(e.target).fadeOut(1500);
            }, onPageError);
            return false;
        }
        
        
    </script>
</asp:Content>
