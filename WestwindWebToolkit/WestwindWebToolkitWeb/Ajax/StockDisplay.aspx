<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockDisplay.aspx.cs" Inherits="Westwind.WebToolkit.StockDisplay" %>
<!-- Note: This page is only a Page Fragment to return rather than a full page
               the result can be used in an AJAX callback using $().load()
-->           
<div id="divStockDisplay" class="errordisplay" style="margin: 0px">         
    <asp:Image id="imgStockQuoteGraph"  runat="server"  Height="250px" Width="400px" />
    <table cellpadding="5">
        <tr>
            <td><b>Company:</b></td>
            <td id="StockName">
                <b><asp:label runat="server" id="lblCompany" text="" /></b>
            </td>
        </tr>
        <tr>
            <td>Last Price:</td>
            <td id="LastPrice">
                <asp:label runat="server" id="lblPrice" text="" />
            </td>                    
        </tr>
        <tr>
            <td>Open Price:</td>
            <td id="OpenPrice">
                <asp:label runat="server" id="lblOpenPrice" text="" />
            </td>
        </tr>
        <tr>
            <td>Net Change:</td>
            <td id="NetChange">
                <asp:label runat="server" id="lblNetChange" text="" />
            </td>
        </tr>
        <tr>                    
            <td>Quote Time:</td>
            <td id="QuoteTime">
                <asp:label runat="server" id="lblQuoteTime" text="" />
            </td>
        </tr>
    </table>                        
</div>