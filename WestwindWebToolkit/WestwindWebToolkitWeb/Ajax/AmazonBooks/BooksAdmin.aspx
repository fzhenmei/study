<%@ Page Language="C#" 
         MasterPageFile="~/WestWindWebToolkit.Master" 
         AutoEventWireup="true" 
         Inherits="Westwind.WebToolkit.BooksAdmin" 
         Title="Amazon Book List Administration" Codebehind="BooksAdmin.aspx.cs" 
         EnableViewState="false"
         EnableEventValidation="false"
%>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content ID="headers" ContentPlaceHolderID="Header" runat="server">
    <link href="BooksAdmin.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
   <div class="toolbarcontainer">
        <a href="../default.aspx" class="hoverbutton"><asp:Image  runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
            <asp:Image  runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
        </asp:LinkButton> |         
        <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
    </div>    

<div id="divMainBookList" class="blackborder">
    <div class="gridheader">My Amazon Book List</div>
    <div class="toolbarcontainer">
        <div style="float: right;">
            <input id="btnUpdateSortOrder" type="button" 
                   value="Update Sort Order"  style="display:none"
                    />
        </div>
    <div id="divProgress" class="smallprogressright"></div>
    <a href="javascript:{}" class="hoverbutton"                        
       onclick="editBook(this,-1);"><img src="../../css/images/new.gif" border="0" /> Add Book </a>
    <a href="javascript:{}" class="hoverbutton"
       onclick="updateSortOrder(this,event)"><img src="../../css/images/sort.gif" border="0" /> Sort List</a>
    <a href="javascript:{}" onclick="loadBooks();" class="hoverbutton"><img src="../../css/images/refresh.gif" border="0" /> Refresh</a>
           
    
    &nbsp;&nbsp;
    Filter: 
    <asp:DropDownList runat="server" ID="lstFilters" AutoPostBack="true">
        <asp:ListItem Text="All Books" Value="All" />
        <asp:ListItem Text="Highlighted Books" Value="Highlighted" />
        <asp:ListItem Text="Recent Books" Value="Recent" />
    </asp:DropDownList>        

    </div>

    <div id="divBookListWrapper" style="position: relative">                 
        <%-- Content is rendered on the client with parseTemplate  --%>    
    </div>
    
    <div class="toolbarcontainer">
        <small>
            <asp:Label runat="server" ID="lblStatus" Text="Ready" />        
        </small>
    </div>       
   
   </div>

    <div class="toolbarcontainer" style="margin-top: 20px;margin-bottom: 10px;">
        <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
            displaystate="Button" codefile="BooksAdmin.aspx" />
        <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
            displaystate="Button" codefile="BooksAdmin.aspx.cs" />
        <manoli:viewsourcecontrol id="ViewSourceControl2" runat="server" text="Javascript Code"
            displaystate="Button" codefile="BooksAdmin.js" />
        <manoli:viewsourcecontrol id="ViewSourceControl3" runat="server" text="Ajax Service Handler"
            displaystate="Button" codefile="AdminCallbackHandler.ashx.cs" />
            
    </div> 
    
    <br />
    <br />

   <!-- Edit Book Form as a draggable Panel --> 
   <ww:DragPanel runat="server" id="panEditBook"                    
                   Closable="true" 
                   DragHandleID="divEditBook_Header"
                   CssClass="dialog"                   
                   ShadowOffset="6"
                   style="position: absolute;width: 635px; left: 80px; top: 200px; display: none;z-index: 1000;"               
                   >                                              
        <div id="divEditBook_Header" class="dialog-header">Book Information</div>
        <div id="divEditBook_Content" class="containercontent">
            <div style="float: left">
                <img id="imgAmazonImage" src="" style="float: left; margin-bottom: 100px;" />
            </div>
            <div style="margin-left: 150px;">
            Book Title:<br />
            <asp:TextBox runat="server" ID="txtTitle" /> 
            <input id="btnShowAmazonPicklist" type="button" value="..." style="height: 23px;  padding: 0px;"  onclick="showAmazonList(this)"  />       

            <br />
            Author:<br />
            <asp:TextBox runat="server" ID="txtAuthor" />         
            
            <br />
            Category:<br />
            <asp:DropDownList runat="server" id="txtBookCategory">                
            </asp:DropDownList>
            
            
            <br />
            Amazon Link Url:<br />
            <asp:TextBox runat="server" ID="txtAmazonUrl" />         
            
            <br />
            Amazon Image Url:<br />
            <asp:TextBox runat="server" ID="txtAmazonImage" /> 
            
            <br />
            <asp:CheckBox runat="server" ID="chkHighlight" Text="Is book featured?" />
            &nbsp;&nbsp;&nbsp;
            Sort Order: <asp:TextBox runat="server" ID="txtSortOrder"  style="width: 30px;" ToolTip="Higher numbers sort to the top" />
            <hr />
            
            <input type="button" value="Save" onclick="saveBook(this);" />        
            </div>
        </div>   
   </ww:DragPanel>

    <!-- The Book list to select new books from  -->
    <ww:DragPanel runat="server" id="panBookList"                    
                   Closable="true" 
                   DragHandleID="divBookList_Header"
                   CssClass="dialog"                   
                   ShadowOffset="6"                   
                   style="position: absolute; width: 550px; right:40px; top: 100px;display: none;z-Index:1010;"               
                   >                                              
        <div id="divBookList_Header" class="dialog-header">Search Amazon Books</div>
        <div class="dialog-toolbar">
            <div id="divProgressSearch" class="smallprogressright" style="display:none;"></div>
            <small>
            <asp:RadioButtonList runat="server" ID="radSearchType" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Text="Author" Value="Author"></asp:ListItem>
                <asp:ListItem Text="Title" Value="Title" Selected="True"></asp:ListItem>
            </asp:RadioButtonList> | 
            <asp:DropDownList runat="server" ID="txtAmazonGroup">
                <asp:ListItem Text="Books" Selected="True" />
                <asp:ListItem Text="Music" />
                <asp:ListItem Text="Electronics" />
                <asp:ListItem Text="PCHardware" />                
                <asp:ListItem Text="DVD" />
            </asp:DropDownList>
            |
            Search: <asp:TextBox runat="server" ID="txtSearchBooks" style="width: 170px" ></asp:TextBox>  
            <input type="button" value="Go" onclick="showAmazonList(this)" />              
            </small>
        </div>
        <div id="divBookList_Content" style="height: 350px; overflow: scroll; overflow-x: hidden; ">
            
        </div>  
        <div class="dialog-statusbar">
        Ready
        </div> 
    </ww:DragPanel>

   
<ww:AjaxMethodCallback ID="Proxy" runat="server" ServerUrl="AdminCallbackHandler.ashx" PostBackMode="PostNoViewstate" />
<script type="text/html" id="item_template">  
<div id="divBookItem" class="bookitem">
    <div style="float: right;" id="divBookOptions">        
        <br />
        <small><#= SortOrder #></small>
    </div>
    <img src="<#= AmazonSmallImage #>" id="imgAmazon"/>
    <b><a href="<#= AmazonUrl #>" target="_blank" ><#= Title#></a></b>
    <br/>
    <small><#= Author #></small>
    <br/>
    <# if (Highlight) { #>
        <small><i>Highlighted</i></small>
    <# } #>   
</div>    
</script>        

<script type="text/html" id="amazon_item_template">    
<# for (var i=0; i<bookList.length; i++) { 
    var book = bookList[i];
#>
<div class="amazonitem" ondblclick="selectBook(this);" tag="<#= book.Id #>">
    <img src="<#= book.SmallImageUrl #>" style="float: left; margin-right: 10px;" />
    <div><b><#= book.Title #></b></div>
    <div><i><#= book.Publisher #> &nbsp; (<#= book.PublicationDate #>)</i></div>
    <small><#= book.Author #></small>
</div>
<# } #>
</script>


    <ww:ScriptContainer runat="server" RenderMode="Header" MinScriptExtension=".min.js">
    <Scripts>    
        <script src="~/scripts/jquery.js" type="text/javascript" Resource="jquery" RenderMode="HeaderTop" ></script>
        <script src="~/scripts/jquery-ui-custom.js" type="text/javascript" AllowMinScript="true" RenderMode="Header"></script>
        <script src="~/scripts/ww.jquery.js" type="text/javascript" Resource="ww.jquery" RenderMode="Header"></script>                
        <script src="booksadmin.js" type="text/javascript"></script>
    </Scripts>
</ww:ScriptContainer>
</asp:Content>
