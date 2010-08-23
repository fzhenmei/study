<%@ Page Language="C#" AutoEventWireup="true" Inherits="Westwind.WebToolkit.Admin.BannerManTest"
      MaintainScrollPositionOnPostback="true"
     Codebehind="BannerAdmin.aspx.cs" 
     EnableViewState="false"  
%>
<%@ Import Namespace="Westwind.Web.Banners" %>
<%@ Register Assembly="Westwind.Web" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Banner Administration</title>
	<link href="../css/Westwind.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
	    .bannerid { color: teal; font-weight: bold; font-size: 12pt;}
	    .actionlinks { border-top: dashed 1px teal;padding:10px;margin-top:7px;background-image: url(../css/images/lightbluegradient.png); }	    
	    .editlabel { margin-top: 7px; font-weight: bold; } 	    
	    .linkheader { margin-top: 10px; margin-bottom: 10px; }
	</style>
</head>
<body style="margin:0px;">
    <form id="form1" runat="server">
    <div style="min-width: 800px;">
        
    <a href="/"><img title="West Wind Technologies" src="../images/wwtoollogo.jpg" style="border-width:0px;display:block" /></a>
        
    <div class="banner" >
        <div class="bannersubtitle">
            Banner Administration
        </div>
        <div  style="float:right;padding-right:5px;">
                <a class="bannertext" href="http://www.west-wind.com/westwindwebtoolkit/" title="Home" target="_blank">Home</a> | 
                <a class="bannertext" href="http://www.west-wind.com/westwindwebtoolkit/docs/" title="Documentation" target="_blank">Docs</a> | 
                <a class="bannertext" href="http://www.west-wind.com/wwThreads/default.asp" title="Message Board and Support" target="_blank">Support</a> |
                <a class="bannertext" href="http://www.west-wind.com/weblog" title="Rick Strahl's WebLog" target="_blank">WebLog</a> |                 
                <a class="bannertext" href="http://www.west-wind.com/wwstore/item.aspx?sku=WebToolkit" title="Web Store" target="_blank">Purchase</a> | 
                <a class="bannertext" href="http://www.west-wind.com/contact.asp" title="Contact us" target="_blank">Contact us</a> 
	    </div>
    </div>
    
    <div class="toolbarcontainer">
            <a href="<%= this.ResolveUrl(BannerConfiguration.Current.HomeUrl) %>" class="hoverbutton" ><img src="../css/images/home.gif" alt="Home"/> Home</a> | 
            <a href="BannerAdmin.aspx" class="hoverbutton"><img src="../css/images/refresh.gif" alt="Refresh"/> Refresh Page</a> |            
            <a href="javascript:NewBanner();" class="hoverbutton"> <img src="../css/images/new.gif" alt="New Banner"/> New Banner</a> |    
            <asp:LinkButton runat="Server" id="btnCreateTable"  Visible="false" 
                onclick="btnCreateTable_Click" class="hoverbutton">Create Banner Tables</asp:LinkButton>        
    </div>        
        <br />
        <ww:ErrorDisplay runat="server" ID="ErrorDisplay" />
        
        <center>
        <div runat="server" id="AdminContainer" style="width:840px;text-align:left;">
        
        <asp:Repeater runat="server" ID="BannerAdmin" OnItemCommand="BannerAdmin_OnItem" >        
        <ItemTemplate>
            <div style="border-bottom: solid 1px teal;margin:15px;">   
                <div style="float:right;text-align:right">
                    <%#  (bool)Eval("Active") ? "" : "<span style='color:red;'><b>inactive</b>&nbsp;&nbsp;</span>"%><span id="Pos_<%#Eval("BannerId") %>" class="bannerid"><%# Eval("BannerId") %></span><br />
                    <br />                    
                    <div style="text-align: right;font-size: 8pt;">
                        Impressions: <b><%# Eval("ResetHits") %></b><br />
                        Clicks: <b><%# Eval("ResetClicks")  %></b><br />             
                        Max Impressions: <%# Eval("MaxHits") %><br />
                        CTR: <%# Ctr( (int)Eval("ResetClicks"),(int)Eval("ResetHits")).ToString("n3") %>%<br />
                        <asp:LinkButton runat="server" ID="btnResetStats" 
                               CommandArgument='<%# Eval("BannerId") %>'
                                CommandName="ResetStats"                                                         
                                Text="Reset" ></asp:LinkButton><br />  
                        <br />
                        Total Impressions: <%# Eval("Hits") %><br />
                        Total Clicks: <%# Eval("Clicks") %><br />
                        Total CTR: <%#  Ctr( (int)Eval("Clicks"),(int)Eval("Hits")).ToString("n3") %>%<br />
                        
                        <br />
                        SortOrder: <%# Eval("SortOrder") %></div>
                </div>                
                <%# ((BannerItem) Container.DataItem).RenderLink()  %><br />
                <small id='imgSize_<%# Eval("BannerID") %>'><a href="javascript:UpdateBannerSize('<%#Eval("BannerId") %>');"><%# Eval("Width") %>x <%# Eval("Height") %></a></small><br />
                <br />
                <table cellpadding="4">
                <tr>
                    <td align="right">Image:</td>
                    <td><%# Eval("ImageUrl") %></td>
                 </tr>
                 <tr>
                    <td align="right">Navigate:</td>
                    <td><%# StringUtils.Href( Eval("NavigateUrl") as string) %></td>
                 </tr>
                 <tr>
                    <td align="right">Redirect:</td>
                    <td><%# StringUtils.Href( ((BannerItem)Container.DataItem).GetBasicTrackedUrl()) %></td>
                </tr>
                <tr>
                    <td align="right">Group:</td>
                    <td><%# Eval("BannerGroup") %></td>
                </tr>
                <tr>
                    <td align="right">Entered:</td>
                    <td><%# Eval("Entered") %>&nbsp; &nbsp; Last Updated: <%# Eval("Updated") %></td>
                </tr>
                </table>
                <div class="actionlinks">
                    <a href="javascript:ShowBanner('<%# Eval("BannerId") %>');" class="hoverbutton">Edit</a> | 
                    <asp:LinkButton runat="server" ID="btnDelete" 
                                   CommandArgument='<%# Eval("BannerId") %>'                                                    
                                    CommandName="Delete"                                                         
                                    Text="Delete" class="hoverbutton"></asp:LinkButton> |
                     <a href="javascript:ShowLinks('<%# Eval("BannerId") %>','<%# Eval("BannerGroup") %>')" class="hoverbutton">Show Embeddable Links</a>
                </div>
            </div>
            
        </ItemTemplate>        
        </asp:Repeater>
       </div>
       </center>

        <ww:DragPanel ID="ShowLinks" runat="server" class="dialog"
                      DragHandleID="ShowLinks_Header"
                      closable="true"
                      shadowoffset="5"
                      style="display:none;"
        >
                      
            <div id="ShowLinks_Header" class="dialog-header">Embeddable Links</div>
            <div id="ShowLinks_Content" class="dialog-bottom">
            
            </div>
        </ww:DragPanel>
                      
        
        <ww:DragPanel ID="BannerEditor" runat="server" class="dialog" 
                        Draggable="true" Closable="true"
                        DragHandleID="BannerEditor_Header"
                        ShadowOffset="5" 
                        style="position:absolute;background-color:White;top:10px;right:-40px;width:540px;display:none;"                                              
        >
        <div id="BannerEditor_Header" class="dialog-header" style="padding:4px;">Edit Banner</div>
        <div style="padding:15px;">
            <div class="editlabel">Image Url:</div>
            <asp:TextBox runat="server" ID="txtImageUrl" width="500px"></asp:TextBox>

           <div class="editlabel">Navigate Url:</div>
            <asp:TextBox runat="server" ID="txtNavigateUrl" width="500px"></asp:TextBox>                        
            
            <div class="editlabel">Banner Group:</div>
            <asp:TextBox runat="server" ID="txtBannerGroup" width="500px"></asp:TextBox>
            
           <div class="editlabel">Hits and Clicks:</div>                     
            <div class="gridalternate" style="padding: 8px; border: solid 1px teal" >
            Hits: <asp:TextBox runat="server" ID="txtResetHits" width="75px"></asp:TextBox> &nbsp;&nbsp;&nbsp;&nbsp;
            Clicks: <asp:TextBox runat="server" ID="txtResetClicks" width="75px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            Max Hits: <asp:TextBox runat="server" ID="txtMaxHits" width="75px"></asp:TextBox>            
            </div>
            
            <br />
            <div style="float:right;">Sort Order: <asp:TextBox runat="server" ID="txtSortOrder" width="40px"></asp:TextBox></div>
            <asp:CheckBox runat="server" ID="chkActive" Text="Active in Rotation" Checked="True" />
            
            <hr />
           <asp:Button runat="server" ID="btnSave" OnClientClick="SaveBanner();return false;"  Text="Save" />
        </div>
        </ww:DragPanel>
        
        <ww:AjaxMethodCallback runat="server" ID="Callback">
        </ww:AjaxMethodCallback>
        
        <ww:ScriptContainer runat="server" ID="ScriptContainer">
            <Scripts>
                <script src="~/scripts/jquery.js" resource="jquery" rendermode="HeaderTop"></script>
                <script src="~/scripts/ww.jQuery.js" resource="ww.jquery" rendermode="Header"></script>
            </Scripts>
        </ww:ScriptContainer>
        
        
        </div>    

<script type="text/javascript">

var EditPanel = $("#BannerEditor");
var ActiveBanner = null;

function ShowBanner(BannerId)
{
    Callback.GetBanner(BannerId,ShowBanner_Callback,OnPageError);
}
function ShowBanner_Callback(result)
{
    ActiveBanner = result;
        
    $("#txtNavigateUrl")
        .val( result.NavigateUrl );
    
    $("#txtImageUrl")
        .val( result.ImageUrl );
    
    $("#txtBannerGroup")
        .val( result.BannerGroup );
    
    $("#txtResetHits")
        .val( result.ResetHits );
    
    $("#txtResetClicks")
        .val( result.ResetClicks );
    
    $("#txtMaxHits")
        .val( result.MaxHits );
    
    $("#chkActive").attr("checked",result.Active);
    
    var Panel = EditPanel; //BannerEditor_DragBehavior.windowObj;
    Panel
        .show()
        .shadow()
        .centerInClient();    
}
function SaveBanner()
{
    ActiveBanner.NavigateUrl = $("#txtNavigateUrl").val();
    ActiveBanner.ImageUrl = $("#txtImageUrl").val();
    ActiveBanner.BannerGroup = $("#txtBannerGroup").val;
    ActiveBanner.Active = $("#chkActive").get(0).checked;
    ActiveBanner.SortOrder = getInt("txtSortOrder");
       
    ActiveBanner.ResetHits = getInt("txtResetHits");
    ActiveBanner.ResetClicks = getInt("txtResetClicks");
    ActiveBanner.MaxHits = getInt("txtMaxHits");        
        
    Callback.SaveBanner(ActiveBanner,SaveBanner_Callback,OnPageError);
}
function getInt(id)
{
    var val = parseInt($("#" + id).val());
    if (val == null || isNaN(val))
        return 0;
    return val;
}
function SaveBanner_Callback(result)
{
    <%= this.GetPostBackEventReference(this) %>;
}
function NewBanner()
{
    Callback.NewBanner(NewBanner_Callback,OnPageError);
}
function NewBanner_Callback(result)
{
    ActiveBanner = result;    
    EditPanel.show();    
    EditPanel.centerInClient();
}
var SizeElement = null;
function UpdateBannerSize(bannerId)
{
    var Ctl = $("imgSize_" + bannerId);
    Ctl.innerHTML = "updating...";
    Callback.UpdateBannerSize(bannerId,UpdateBannerSize_Callback,OnPageError);
    
 
}
function ShowLinks(bannerId)
{
    Callback.GetBannerLinks(bannerId,function(html) {
                $("#ShowLinks_Content").html(html);
                $("#ShowLinks")
                    .show()
                    .centerInClient();
        },OnPageError);
}
function UpdateBannerSize_Callback(result)
{        
    var Ctl = $("imgSize_" + result.BannerId);
    if (result == null)
        Ctl.innerHTML = "not available";
    else
        Ctl.innerHTML = result.Width + " x " + result.Height;
}
function OnPageError(Error)
{
    onPageError(Error.message);
}
</script>
    </form>
</body>
</html>
