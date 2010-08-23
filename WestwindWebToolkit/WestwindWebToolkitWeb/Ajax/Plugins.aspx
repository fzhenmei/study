<%@ Page Title="" Language="C#" MasterPageFile="~/WestWindWebToolkit.master" AutoEventWireup="true" CodeBehind="Plugins.aspx.cs" Inherits="Westwind.WebToolkit.Ajax.Plugins" %>
<%@ Register Assembly="CSharpFormat" Namespace="Manoli.Utils.CSharpFormat" TagPrefix="manoli" %>
<asp:Content ID="Headers" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">        
    .contentEditable { background: azure; padding: 10px; border: solid 1px orange;}
    .comment { padding: 10px; border-bottom: dotted 1px teal;  }
    </style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Examples for Various jQuery Plug-ins</h1>
    <div class="toolbarcontainer">
        <a href="./" class="hoverbutton"><asp:Image runat="server" ImageUrl="~/css/images/Home.gif"  AlternateText="Home" /> Home</a> | 
        <asp:LinkButton runat="server" ID="lnkRefresh" class="hoverbutton">
            <asp:Image runat="server" ImageUrl="~/css/images/Refresh.gif"  AlternateText="Reset Page"/> Reset Page
        </asp:LinkButton> |         
        <small>Page created:&nbsp;<%= DateTime.Now.ToString() %></small><br />
    </div>
    <div class="descriptionheader">
    
    </div>
    
    <div class="containercontent">        
        
        <div class="samplebox">                    
        
        <h3>contentEditable Plug-in</h3>
           <br />
           <small>This plug in makes any text content editable with all formatting intact by
           setting the DOM element to contentEditable. A 
           </small>
        
        
            <div>                
                <p><b>Simple Text to edit:</b></p>
                                    
                <!-- edit button -->
                <a id="btnEditSingleContent" 
                   href="javascript:{}" 
                   onclick="singleContentEdit();" 
                   class="hoverbutton" 
                   style="float: right"><img src="../css/images/editwhite.gif"/>edit</a>
                
                <div id="divSingleContentToEdit" style="padding: 0px 35px;">                
                    <p>After an evening of mucking around with this I was able to 
                    get <i>LINQ to SQL</i> to work with my custom provider. The process 
                    involved is actually quite simple, although it’s not real 
                    obvious to discover. <i>LINQ to SQL</i> doesn’t have any explicit 
                    support for plugging in new providers (unfortunately) 
                    however, you CAN pass it an instance of a Connection object 
                    like this:</p>
                <pre class="code"><span style="color: rgb(43, 145, 175);">WebRequestConnection </span>conn = <span 
                    style="color: blue;">new </span><span 
                    style="color: rgb(43, 145, 175);">WebRequestConnection</span>();  <font 
                    color="#008000">// my custom provider’s Connection class</font>
conn.ConnectionString = <span style="color: rgb(163, 21, 21);">&quot;Data Source=http://rasnote/PraWeb/DataService.ashx;uid=ricks;pwd=secret&quot;</span>;                           
</span><span style="color: rgb(43, 145, 175);">TimeTrakkerDataContext </span>context = <strong><span 
                    style="color: blue;">new </span><span 
                    style="color: rgb(43, 145, 175);">TimeTrakkerDataContext</span>(conn);</strong>  </pre>
                <p>
                    Here I create an instance of my custom provider’s&nbsp; 
                    <b>WebRequestConnection() class</b>, set its connection string and 
                    pass it to the constructor of the DataContext. And lo and 
                    behold – assuming your provider is compatible with SQL 
                    syntax and implements the DbProvider classes properly LINQ 
                    to SQL works using the custom provider.
                </p>
                
                </div>
                            
                
                <b>List Editing:</b>
                <br />
                <div style="padding: 10px 35px;">
                    <div id="divCommentContainer" class="blackborder" style="width: 400px;">
                        <div class="gridheader">Comments</div>
                        <div id="divCommentList"  style="overflow-y: scroll;height: 300px;">                    
                        </div>
                    </div>                                   
                </div>
            
            </div>
        
        
        </div>        
        
    </div>
    
    <!-- comment item template for ajax load -->
    <script type="text/html" id="commentTemplate">        
     <# for(var i = 0; i < Global.comments.length; i++) {
        var comment = Global.comments[i];
    #>
    
    <div id="cmt_<#= comment.Id #>" class="comment">
        <a href="javascript:{}" class="hoverbutton" style="float: right"><img src="../css/images/editwhite.gif"/>edit</a>
        
        <div class="commentContent">
            <b><#= comment.Title #></b>
            <br />
            <small><#= comment.Body #></small>
        </div>
    </div>
    <# } #>
    </script>

    <ww:AjaxMethodCallback runat="server" id="Proxy"  PageProcessingMode="PageInit"/>
    
    <ww:ScriptContainer runat="server" ID="ScriptContainer">
        <Scripts>
            <script src="~/scripts/jquery.js" resource="jquery" rendermode="HeaderTop"></script>
            <script src="~/scripts/ww.jQuery.js" resource="ww.jquery" rendermode="Header"></script>
        </Scripts>
    </ww:ScriptContainer>
    
   <div class="toolbarcontainer">
        <manoli:viewsourcecontrol id="ViewSourceControl" runat="server" text="Show ASPX"
            displaystate="Button" codefile="Plugins.aspx" />
        <manoli:viewsourcecontrol id="ViewSourceControl1" runat="server" text="Show CodeBehind"
            displaystate="Button" codefile="Plugins.aspx.cs" />
            
    </div>

    <script type="text/javascript">
        var Global = { comments: null };

        $(document).ready(function() {

            // load comments for contentEditable list example
            Proxy.GetComments(function(result) {
                Global.comments = result;
                var commentTemplate = $("#commentTemplate").html();
                var html = parseTemplate(commentTemplate, Global.comments);
                $("#divCommentList").empty().append(html);
                
                // hook up contentEdit comment list events
                $(".comment .hoverbutton").click(listContentEdit);
            });
                        
        });
               
        function singleContentEdit() {
            var editButt = $("#btnEditSingleContent");
            editButt.hide();

            $("#divSingleContentToEdit")
                    .contentEditable({ saveHandler: function(e) {
                        alert("Thanks for editing. You entered this HTML:\r\n\r\n" +
                                                  $(this).html());
                        editButt.show();
                        return true;
                    },
                        editClass: "contentEditable"
                    });            
        }

        function listContentEdit(e) {
            var butt = $(e.target);

            var content = $(butt).next();
            butt.hide();

            content.contentEditable(
                { editClass: "contentEditable",
                    saveHandler: function(e) {
                        butt.show();
                        return true;
                    }
                });
            
        }
        
        
    </script>
    
</asp:Content>
