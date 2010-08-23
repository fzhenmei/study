<%@ Page Title="Making Ajax Requests with AjaxMethodCallback" Language="C#" MasterPageFile="~/WestWindWebToolkit.master" 
         AutoEventWireup="true" 
         CodeBehind="AjaxMethodCallbacks.aspx.cs" Inherits="Westwind.WebToolkit.Ajax.AjaxMethodCallbacks" 
         EnableEventValidation="false"         
%>
<%@ Register assembly="Westwind.Web" namespace="Westwind.Web.Controls" tagprefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
    .fielderror
    {
    	background-color: lightred;
    	border: solid 1px darkred;
    	font-size: 20pt;
    	font-weight: bold;
    	color: Red;
    }

    .label
    {
    	float: left;
    	width: 90px;
    	font-weight: bold;
    }
    #divCustomerEdit input[type=text]
    {
    	display: block;
    	width: 250px;    	
    }
    #divCustomerEdit>div
    {
    	margin-top: 4px;
    }
    </style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    
    <h1>Making Ajax Requests with AjaxMethodCallback</h1>
    <div class="toolbarcontainer">
        <a href="./" class="hoverbutton"><asp:Image runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
            <asp:Image runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
        </asp:LinkButton> |         
        <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
    </div>
    <div class="descriptionheader">
    This page shows a few simple function examples how to use the AjaxMethodCallback control to make
    Ajax callbacks to the same page by using [CallbackMethod] attribute on Page methods. Messages are 
    passed using JSON, but effectively you can server methods directly either on the same page (as in
    this example) or in a separte Callback HttpHandler for better efficiency.
    This example also uses optionally the ScriptVariables component to normalize
    ClientIds, and the ScriptContainer control to host scripts on the page.
    </div>
    
    <div class="containercontent">
    
    <div class="samplebox">
        <h3>Hello World Callback</h3>
        <small>Most basic callback that simply echoes back a string without a Postback</small>
        
        <div class="containercontent">
            Please enter your name:
            <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
            <input type="button" value="Say Hello" id="btnSayHello"  onclick="SayHello();" />    
        
            <div id="divHelloMessage" class="errordisplay" style="display:none">
            </div>
        </div>
                
    </div>
    
    <div class="samplebox">
        <h3>Add Two Values</h3>
        <small>Another simple example that adds two values by passing numeric values between client and server. 
        Server method simply accepts and returns numeric values which are marshalled back to the client.</small>
        
        <div class="containercontent">
            Please enter two numbers to add:
            <asp:TextBox runat="server" ID="txtNum1" style="width: 40px">11</asp:TextBox> + 
            <asp:TextBox runat="server" ID="txtNum2"  style="width: 40px">31</asp:TextBox>  
            <input type="button" value=" = (add) " id="btnAddNumbers"  onclick="AddNumbers();" />    
            <asp:Label runat="server" ID="lblAddResult" class="errormessage"></asp:Label>
        </div>
    </div>    
    
    <div class="samplebox">
        <h3>Return Complex Objects</h3>
        <small>You can also easily pass complex objects to and from the server. This example retrieves a stock quote 
        and displays some simple data. To see data on the wire you can use <a href="http://getfirebug.com">FireBug</a> (FireFox) or <a href="http://fiddlertool.com">Fiddler</a> (generic)</small>
        
        <div class="containercontent">
            Please enter a stock symbol:
            <asp:TextBox runat="server" ID="txtStockSymbol" style="width: 100px">MSFT</asp:TextBox>
            <input type="button" value="Get Quote" id="btnGetQuote"  onclick="GetStockQuote();" />    
            
            <div class="errordisplay" id="divStockQuoteResult" class="errormessage" style="display:none">
            </div>
        </div>
    </div>    
    
    
    <div class="samplebox">
        <h3>Return Lists of Objects</h3>
        <small>Lists can also be returned easily. Most common list types serialize into JSON</small>
        
        <div class="containercontent">
            <asp:DropDownList runat="server" ID="lstCustomerPk" Width="300px" onchange="LoadCustomer()"></asp:DropDownList> 
            <input type="button" value="Load List" id="btnLoadCustomers" onclick="LoadCustomerList()" />
            <input type="button" value="Clear List" id="btnClearCustomers" onclick="ClearCustomerList()" />
        </div>
        
        <h3>Load an object and then save it</h3>
        This object is loaded when the list above is updated. You can then change values and save to update
        the data on the server.
                
        <div id="divCustomerEdit" class="containercontent" 
             style="border: solid 1px navy;background-color: lightsteelblue; width: 350px; margin-top: 10px; padding: 15px;">            
            <div>
                <div class="label">Last Name:</div>
                <asp:TextBox runat="server" ID="txtLastName"></asp:TextBox>
            </div>

            <div>
                <div class="label">First Name:</div>
                <asp:TextBox runat="server" ID="txtFirstName"></asp:TextBox>
            </div>
            
            <div>
                <div class="label">Company:</div>
                <asp:TextBox runat="server" ID="txtCompany"></asp:TextBox>
            </div>

            <div>
                <div class="label">Bill Rate:</div>
                <asp:TextBox runat="server" ID="txtBillRate"></asp:TextBox>
            </div>
            
            <hr />
            <input type="button" id="btnSave" value="Save Customer" onclick="SaveCustomer();"/>
        </div>
                                
    </div>    
    
    <div class="samplebox">
        <h3>Errors and Timeout Results</h3>
        <small>Callbacks that fail will bring back exceptions as JSON objects and they are automatically
        routed the the error handler. The handler provides an object regardless of the type of failure.
        </small>
        
        <div style="margin-top: 10px;">
        <a href="javascript: ThrowException()" class="hoverbutton">Throw an exception</a> | 
        <a id="lnkLongRunning" href="javascript: LongRunning()" class="hoverbutton">Cause a Timeout</a>
        <span id="lnkLongRunning_Progress" class="smallprogress">&nbsp;&nbsp;&nbsp;</span>
        </div>
        
    </div>
    
    </div>
    
    <div class="toolbarcontainer">
        <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
            displaystate="Button" codefile="AjaxMethodCallbacks.aspx" />
        <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
            displaystate="Button" codefile="AjaxMethodCallbacks.aspx.cs" />
        <manoli:viewsourcecontrol id="ViewSourceControl2" runat="server" text="Show Javascript"
            displaystate="Button" codefile="AjaxMethodCallbacks.js" />
            
    </div>
    
    <p>&nbsp;</p>
    
    
    
    <!-- Callback control calling back to Page Methods -->           
    <ww:AjaxMethodCallback ID="Proxy" runat="server" 
                           Timeout="4000"  GenerateClientProxyClass="Inline" />    
    
    
    <!-- Script container allows for relative paths and easier ClientID access
            $("#" + serverVars.txtCompanyId).val(customer.Company);
            $("#" + serverVars.txtFirstNameId).val(customer.FirstName);
            $("#" + serverVars.txtLastNameId).val(customer.LastName);
            $("#" + serverVars.txtBillRateId).val(customer.BillingRate.formatNumber("n2"));

         Note: you can use ~ syntax and there are additional properties 
         (no Intellisense though)
    -->
    <ww:ScriptContainer runat="server" ID="Scripts">
        <Scripts>            
            <script src="~/scripts/jquery.js" Resource="jquery" RenderMode="HeaderTop"></script>
            <script src="~/scripts/ww.jQuery.js" Resource="ww.jquery" RenderMode="Header"></script>                        
            <script src="AjaxMethodCallbacks.js" RenderMode="Header" ></script>
        </Scripts>
    </ww:ScriptContainer>
    
</asp:Content>
