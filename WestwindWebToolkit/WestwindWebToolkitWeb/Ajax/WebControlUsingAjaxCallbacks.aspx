<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebControlUsingAjaxCallbacks.aspx.cs" Inherits="Westwind.WebToolkit.Ajax.WebControlUsingAjaxCallbacks" %>

<%@ Register assembly="Westwind.WebToolkit" namespace="Westwind.WebToolkit" tagprefix="cc1" %>
<%@ Register assembly="Westwind.Web" namespace="Westwind.Web.Controls" tagprefix="ww" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../css/Westwind.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="text" name="txtVal1" id="txtVal1" value="2" /> 
        <input type="text" name="txtVal2" id="txtVal2" value="11" />
        <input type="button" id="btnSubmit" value="Go" onclick="sayHello()" />
    
    </div>
    <cc1:CustomControlUsingAjaxCallbacks ID="MyControl" runat="server" />   
    
    <%--<ww:AjaxMethodCallback runat="server" ID="Proxy"></ww:AjaxMethodCallback> --%>
    

    <script type="text/javascript">
        function sayHello() {
            
            var proxy = MyControl_Callback_GetProxy();
            proxy.callMethod("Add", [
                                     parseInt($("#txtVal1").val()),
                                     parseInt($("#txtVal2").val())
                                    ],
                            function(result) { alert(result); } );
        }        
    </script>
    
    </form>
</body>
</html>
