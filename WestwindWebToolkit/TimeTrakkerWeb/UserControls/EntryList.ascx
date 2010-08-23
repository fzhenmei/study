<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntryList.ascx.cs" Inherits="TimeTrakkerWeb.EntryList" %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>

<div class="toolbarcontainer">
<small><a href="punchin.aspx" class="hoverbutton"><img src="images/punchin.gif" /> New</a> |
Filter: 
<ww:wwDropDownList ID="lstFilter" runat="server" Width="200" onchange="ShowEntries();">
    <asp:ListItem Text="Recent Entries" Value="RecentEntries"  Selected="True"/>
    <asp:ListItem Text="Open Entries" Value="OpenEntries" />     
    <asp:ListItem Text="Recent Closed Entries" Value="RecentClosedEntries" />
    <asp:ListItem Text="Unbilled Entries" Value="Unbilled" />
    <asp:ListItem Text="All Entries" Value="All Entries" />
</ww:wwDropDownList>    
</small>
</div>        

<div id="lstContainer" style="height: 411px;  overflow-y: scroll; overflow-x: hidden;">
    <asp:ListView runat="server" ItemPlaceHolderID="layoutContainer" ID="lstEntries" >
    <LayoutTemplate>
    <div style="">
        <div id="layoutContainer" runat="server" />
    </div>
    </LayoutTemplate>

    <EmptyItemTemplate>
    <div class="ErrorDisplay">
    There are no matching entries for this filter selection
    </div>
    </EmptyItemTemplate>

    <ItemTemplate>
    <div id="itemtemplate" 
     onclick="window.location='<%# this.GetPunchOutUrl( (int) Eval("pk")  ) %>';">        
     <div class="itemcontrols">            
        <%# this.GetTimeDetail( Container.DataItem as TimeTrakker.EntryEntity )  %>
        <br />
        <a href='Punchout.aspx?Action=delete&id=<%# Eval("Pk") %>' class="hoverbutton"><img src="images/remove.gif" /></a>
    </div>                   
    <div>
    <b><a href='javascript: window.location=\'<%# this.GetPunchOutUrl( (int) Eval("pk")  ) %>\''><%# Eval("Title") %></a></b><br /> 
    <small><i><%# ((EntryEntity) Container.DataItem).ProjectEntity.ProjectName %></i></small><br />
    </div>
    </div>
    </ItemTemplate>
    </asp:ListView>
    </div>
    <div id="divListStatus" class="toolbarcontainer" runat="server">
    Ready...
    </div>
