<%@ Application Language="C#" %>
<%@ Import Namespace="Microsoft.Practices.Unity" %>
<%@ Import Namespace="Microsoft.Practices.Unity.Configuration" %>

<script runat="server">

  void Application_Start(object sender, EventArgs e) 
  {
    // create and populate a new Unity container from configuration
    UnityContainer myContainer = new UnityContainer();
    UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
    section.Containers["containerOne"].Configure(myContainer);
    Application["MyContainer"] = myContainer;
  }
  
  void Application_End(object sender, EventArgs e) 
  {
    Application["MyContainer"] = null;
  }
       
</script>
