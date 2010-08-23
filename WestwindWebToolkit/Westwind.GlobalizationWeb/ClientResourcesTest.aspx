<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientResourcesTest.aspx.cs" Inherits="Westwind.GlobalizationWeb.LocalizationAdmin.ResourceTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>Sample Configuration</title>
		<link href="../WestWind.css" rel="stylesheet" type="text/css" />
        <script src="../scripts/jquery.js" type="text/javascript"></script>	
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    
    </div>
    
    <script type="text/javascript">
        $(document).ready(function() {

            alert(globalRes.CustomerSaved);

            // local resources from admin form
            alert(localRes.BackupFailed);
        });


      </script> 
    </form>
  
</body>
</html>

