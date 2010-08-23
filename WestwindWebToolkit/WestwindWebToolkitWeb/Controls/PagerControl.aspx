<%@ Page Title="" Language="C#" MasterPageFile="~/WestWindWebToolkit.master" 
        AutoEventWireup="true" CodeBehind="PagerControl.aspx.cs" Inherits="Westwind.WebToolkit.Controls.PagerControl"          
%>

<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
<link href="../Ajax/StockPortfolio/JsonStockClient.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Pager Control Samples</h1>
<div class="descriptionheader">
    The Pager control provides a control and data independent pager interface that
    allows full control over paging either manually via pure property interface or
    via several filter helper methods that can take IQueryable, IEnumerable or
    DataTable data sources and automatically filter them. The control is easily CSS 
    stylable using straight forward HTML markup and use HREF based linking for
    SEO friendly navigation. Note that because the pager is not using Postbacks
    to handle page links state on multiple pagers (and any other controls) is 
    lost upon paging. If you need Postback based paging stick with DataPager
    or native GridView paging.
</div>
<div class="toolbarcontainer">
    <div class="hoverbutton rightalign"></div>
    <a href="./" class="hoverbutton"><img src="../css/images/home.gif" />Home</a> |
    <a href="PagerControl.aspx" class="hoverbutton"><img src="../css/images/Refresh.gif" />Refresh</a> |
    <ww:HelpControl ID="HelpControl" runat="server" HelpTopic="_2te14z3xe.htm" CssClass="hoverbutton" />
</div>
    
<div class="containercontent" style="width: 800px;">
   
   <div class="samplebox">
    
     <h3>Base Pager with a ListView Control</h3> 
    <p class="smalltext">
        This example uses the stock styling from the style sheet with default styles applied.
        The pager pages over a very small list of portfolio items, so the paged list only 
        displays a few pages.
    </p>
       <style type="text/css">
.itemtemplate
{
    border-bottom: dashed 1px teal;
    padding: 9px;    
    background: white;
}
.custicon
{
	float: left;
	background-image: url(../css/images/users.gif);
	background-repeat: no-repeat;	
	width: 20px;
	height: 20px;
	padding: 0px 0px 10px 10px;	
}
.custnameheader
{
	font-weight: bold;
	padding: bottom: 3px;
	color: steelblue;
	height: 20px;
}
.itemdetail
{
    font-size: 8pt;
    margin-left: 40px;
}


       </style>
       <div style="overflow: hidden; border: solid 2px navy;height: auto;">   
        <div class="gridheader">Customer List</div>
        <asp:ListView runat="server" ID="CustomerListView" ItemPlaceholderID="CustomerItemTemplate">
            <LayoutTemplate>
              <div id="CustomerItemTemplate" runat="server" />     
            </LayoutTemplate>  
            <ItemTemplate>
              <div id="<%# Eval("Pk") %>_STOCK" class="itemtemplate" >
                <div class="custicon"></div>
                <div class="custnameheader"><%# Eval("Company") %></div>
                <div class="itemdetail rightalign">
                    Entered: <%# Eval("Entered","{0:d}") %>
                </div>
                <div class="itemdetail">
                    <%# Eval("Name") %><br />
                    <%# Eval("Address") %><br /> 
                    <%# Eval("Country") %>                   
                </div>
              </div>
            </ItemTemplate>      
        </asp:ListView>           
    </div>

  
    <ww:Pager runat="server" ID="ListViewPager" 
                  PageSize="4" 
                  MaxPagesToDisplay="4"
                  RenderContainerDiv="true" />
      
   </div>
   
   
    <div class="samplebox">            
       <h3>Using a DataGrid Control with a pager</h3>
       <p class="smalltext">
        <small>This example demonstrates how to display a pager in combination
        with a plain jane GridView control using similar styles to make the pager appear
        to be integrated with the grid.
       </p>
       
       <div style="width: 700px; padding: 20px; ">
            <asp:DataGrid runat="server" ID="gdCustomers" 
                          AutoGenerateColumns="True" 
                          CssClass="blackborder" 
                          style="width: 700px;">
            <AlternatingItemStyle CssClass="gridalternate" /> 
            <HeaderStyle CssClass="gridheader" />
            </asp:DataGrid>
            
           
            <ww:Pager runat="server" ID="Pager" 
                  CssClass="gridpager"
                  ContainerDivCssClass="gridpagercontainer"
                  PageLinkCssClass="gridpagerbutton"
                  SelectedPageCssClass="gridpagerbutton-selected"
                  QueryStringPageField="CustomerListPage" 
                  PageSize="8" 
                  RenderContainerDiv="true"
                  MaxPagesToDisplay="4"  />    
       </div>
    
    </div>

</div>   

<div class="toolbarcontainer">
    <div class="toolbarcontainer">
        <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
            displaystate="Button" codefile="PagerControl.aspx" />
        <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
            displaystate="Button" codefile="PagerControl.aspx.cs" />
    </div>
</div>
   
    </asp:Content>
