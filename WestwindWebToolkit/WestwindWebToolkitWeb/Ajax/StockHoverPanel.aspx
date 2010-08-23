<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockHoverPanel.aspx.cs" 
        Inherits="WestWindWebToolkit.StockHoverPanel"
        MasterPageFile="~/WestWindWebToolkit.master"        
        Title="HoverPanel content loading"
%>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server" ID="content">
    <h1>jQuery ajax.load() with Stock Data</h1>
    <div class="toolbarcontainer">
        <a href="../" class="hoverbutton"><asp:Image ID="Image1" runat="server" ImageUrl="~/css/images/home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
            <asp:Image runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
        </asp:LinkButton> |         
        <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
    </div>

    <div class="containercontent">
        Stock Symbol: 
        <asp:TextBox runat="server" ID="txtSymbol" Text="MSFT" />        
        <input type="button" id="btnSubmit" value="Get Quote" onclick="showStockQuote();" />
        <img src="../images/loading_small.gif" id="imgLoading" style="display: none"/>
        <hr />
        
        <ww:HoverPanel ID="StockDisplayPanel" runat='server' 
                       Style="width: 445px; background: white; display: none;"
                       ServerUrl="StockDisplay.aspx"
                       AutoCloseHoverWindow="false"
                       Draggable="true"
                       DragHandleId="stockDisplayHeader"
                       ShadowOffset="6"
                       Closable="true"
                       HtmlTargetClientId="stockContent"
                       EventHandlerMode="ShowHtmlInPanel"                       
                        >
            <div id="stockDisplayHeader" class="gridheader">Stock Display</div>                       
            <div id="stockContent"></div>
        </ww:HoverPanel>
        
    </div>
    
    <div class="toolbarcontainer">
        <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
            displaystate="Button" codefile="StockHoverPanel.aspx" />
        <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
            displaystate="Button" codefile="StockHoverPanel.aspx.cs" />
        <manoli:viewsourcecontrol id="ViewSourceControl2" runat="server" text="Show StockDisplay CodeBehind"
            displaystate="Button" codefile="StockDisplay.aspx.cs" />            
        <manoli:viewsourcecontrol id="ViewSourceControl3" runat="server" text="Show StockDisplay ASPX"
            displaystate="Button" codefile="StockDisplay.aspx" />            
            
     </div>        
        
    <%--ScriptContainer Control provides resource based loading and intellisense support
        this is purely optional but allows much control over script loading.--%>    
    <ww:ScriptContainer ID="ScriptContainer1" runat="server">
        <Scripts>
            <script src="~/scripts/jquery.js" Resource="jquery"></script>
            <script src="~/scripts/ww.jquery.js" Resource="ww.jquery"></script>
        </Scripts>
    </ww:ScriptContainer>

<script type="text/javascript">
var txtSymbol = "<%= txtSymbol.ClientID %>";

function showStockQuote() {
    showProgress();
    StockDisplayPanel.completed = function(res) {
        showProgress(true);
        $("#StockDisplay").centerInClient();
    };
    StockDisplayPanel.startCallback(null, "symbol=" + $("#" + txtSymbol).val());

    return;
    var div = $("#divResult");        
    
    div.slideUp(function() {
            div.load("StockDisplay.aspx",
                     { symbol: $("#" + txtSymbol).val() },
                     function() {
                         $(this).slideDown();
                         showProgress(true);
                     });
        });                                                  
}
function showProgress(hide) {
    var img = $("#imgLoading");
    if (hide)
        img.hide();
    else
        img.show();
}
</script>
</asp:Content>   
