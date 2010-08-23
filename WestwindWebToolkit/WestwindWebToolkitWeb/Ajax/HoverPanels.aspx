<%@ Page Title="Pop up Windows with HoverPanel" 
         Language="C#" MasterPageFile="~/WestWindWebToolkit.master" 
         AutoEventWireup="True" 
         CodeBehind="HoverPanels.aspx.cs" Inherits="Westwind.WebToolkit.HoverPanels" 
         EnableViewState="false"
         EnableEventValidation="false"
         ValidateRequest="false"         
%>

<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>

<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
    table.blackborder td, table.blackborder th
    {
	    font-size: 8pt;
    }
    </style>
    <script type="text/javascript" src="HoverPanels.js"></script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">    
    <h1>Pop up Windows with HoverPanel</h1>
    <div class="toolbarcontainer">
        <div class="smallprogressright"></div>
        <div id="imgLoading" class="smallprogressright"></div>        
    
        <a href="./" class="hoverbutton"><asp:Image runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
            <asp:Image  runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
        </asp:LinkButton> |         
        <small>Page created: <%= DateTime.Now.ToString() %></small><br />
    </div>
    <div class="descriptionheader">
    HoverPanel makes it easy to display pop up content from data retrieved on a server. HoverPanels are
    assigned to an HTML element that is filled with server AJAX retrieved data and then displayed 
    when the results returned, optionally at the current mouse position.
    </div>
    
    <div class="containercontent">

        <div class="samplebox">
        <h3>Simple HoverPanel Popup</h3>
        <small>The following example demonstrates how to quickly pop up a content window at the current mouse position -
               in this case at the button location. Here content is loaded from another ASPX page and displayed. The ASPX
               page returns only an HTML fragment rather than a full HTML page. The window popped up is draggable and
               closable.             
        </small>
        
        <p>
            Enter a Stock Symbol: <asp:TextBox runat="server" ID="txtStockSymbol" Text="MSFT" />
            <asp:Button runat="server" ID="btnGetStockQuote" Text="Get Quote" OnClientClick="GetStockQuote(event);return false;" 
                         UseSubmitBehavior="false" />
        </p>
        </div>

        
        <div class="samplebox">
        <h3>Grid Detail Popup</h3>
        <small>A common scenario is to pop up content 'inline' of list based controls like a GridView. Popping up content like
        this makes it easy to see information at a glance without 'losing context'. This example hooks up .hover() behavior
        for invoice links using a custom orderId attribute to identify what data to display. Btw, the configured 300ms delay 
        before the hover operation starts is intentional to avoid excessive and overlapping requests to the server.
        </small>
        
        <div class="containercontent">
            
        <!-- Note: Hover events hooked up with jQuery in HoverPanels.js file. Read first column text to get id -->
        <asp:GridView ID="gdInvoiceList" runat="server"  CellPadding="4" 
                              CssClass="blackborder" AutoGenerateColumns="false"
                              Width="700"  GridLines="None"
                              HorizontalAlign="Center" AllowPaging="True" 
                              PagerSettings-Mode="NumericFirstLast" PageSize="10" PagerSettings-Position="Bottom"
                              OnPageIndexChanging="gdInvoiceList_PageIndexChanging"
                              >
                    <PagerStyle CssClass="gridpagercontainer" />                              
                    <FooterStyle cssclass="gridheader" />                    
                    <AlternatingRowStyle CssClass="gridalternate" />
                    <HeaderStyle cssclass="gridheader"  Font-Bold="True" />                                        
                    <Columns>
                        <asp:TemplateField  ItemStyle-HorizontalAlign="Center">
                            <ControlStyle Width="200px" />
                            <HeaderTemplate>Invoice No.</HeaderTemplate>
                            <ItemTemplate>
                	            <a href="javascript:alert('Invoice page would activate here.')">
                	            <%# Eval("OrderId") %>
                                </a>
                            </ItemTemplate>                                
				        </asp:TemplateField>

                       <asp:TemplateField >
                        <HeaderTemplate>Date</HeaderTemplate>
                        <ItemTemplate>
                            <%# ((DateTime)Eval("OrderDate")).ToString("d") %>
                         </ItemTemplate>
				        </asp:TemplateField>
                        
                        <asp:TemplateField >
                        <HeaderTemplate>Company</HeaderTemplate>
                        <ItemTemplate><%# Eval("Company") %></ItemTemplate>
				        </asp:TemplateField>

                        <asp:TemplateField >
                        <HeaderTemplate>Name</HeaderTemplate>
                        <ItemTemplate>
                			        <%# Eval("Name") %>
                        </ItemTemplate>
				        </asp:TemplateField>

                        <asp:TemplateField  ItemStyle-HorizontalAlign="Right" >
                        <HeaderTemplate>Order Total</HeaderTemplate>
                        <ItemTemplate>
                			        <%# ((decimal)Eval("OrderTotal")  + (decimal) Eval("Freight")).ToString("n2") %>
                        </ItemTemplate>
				        </asp:TemplateField>
                    </Columns>
        </asp:GridView>
            
            
        </div>                
    </div>
        
    <div class="toolbarcontainer">
        <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
            displaystate="Button" codefile="HoverPanels.aspx" />
        <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
            displaystate="Button" codefile="HoverPanels.aspx.cs" />
      <manoli:viewsourcecontrol id="ViewSourceControl2" runat="server" text="Show JavaScript"
                displaystate="Button" codefile="HoverPanels.js" />            
    </div>
    
    
    
        <!-- *** LineItem Popup Window HoverPanel -->
        <ww:HoverPanel ID="lineItemPanel" runat="server" Style="width: 450px; background: white;
            display: none;" class="blackborder"
             NavigateDelay="300"
             ShadowOffset="4"
             ShadowOpacity=".25"
             EventHandlerMode="ShowHtmlAtMousePosition"            
             AdjustWindowPosition="true"             
        >
        </ww:HoverPanel>
        

        <!-- *** Stock Lookup Popup Window -->
        <ww:HoverPanel ID="StockDisplayPanel" runat='server' 
                       Style="width: 445px; background: white; display: none;"
                       ServerUrl="StockDisplay.aspx"
                       AutoCloseHoverWindow="false"
                       AdjustWindowPosition="false"
                       Draggable="true"
                       DragHandleId="stockDisplayHeader"
                       ShadowOffset="5"
                       Closable="true"
                       CssClass="dialog"
                       HtmlTargetClientId="stockContent"
                       EventHandlerMode="ShowHtmlAtMousePosition"
                        >
            <div id="stockDisplayHeader" class="dialog-header">Stock Display</div>                       
            <div id="stockContent"></div>
        </ww:HoverPanel>
    </div>
</asp:Content>
