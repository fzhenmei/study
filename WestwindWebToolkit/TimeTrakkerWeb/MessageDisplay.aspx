<%@ Page language="c#" Inherits="TimeTrakkerWeb.MessageDisplay" 
                       CodeBehind="MessageDisplay.aspx.cs"  
                       enableViewState="false"  MasterPageFile="~/TimeTrakkerMaster.Master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
                  <br>
                  <table border="0" width="97%">
                     <tr>
                        <td class="gridheader"  align="center" height="35">
                           <asp:label ID="lblHeader" RUNAT="server" FONT-BOLD="True" Font-Size="16pt"></asp:label>
                        </td>
                     </tr>
                     <tr>
                        <td>
                           <br>
                           <blockquote>
                              <asp:label ID="lblMessage" RUNAT="server"></asp:label>
                              <br>
                              <p></p>
                           </blockquote>
                           <center><small><asp:label ID="lblRedirectHyperLink" RUNAT="server"></asp:label></small></center>
                        </td>
                     </tr>
                  </table>
         <!-- End Custom Form Stuff -->
</asp:Content>       