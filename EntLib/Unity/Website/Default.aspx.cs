using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MyObjects;

public partial class _Default : System.Web.UI.Page 
{

  private IUnityContainer myContainer = null;
  private String outMessage = String.Empty;

  protected void Page_Load(object sender, EventArgs e)
  {
    // get the existing populated Unity container from Application object if available
    Object retrievedContainer = Application["MyContainer"];
    if (retrievedContainer == null)
    {
      outMessage = "ERROR: Unity Container not populated in Global.asax.<p />";
    }
    else
    {
      // found an existing container, so cast it to the correct type
      myContainer = retrievedContainer as IUnityContainer;
      outMessage = "Retrieved the populated Unity Container from the Application object.<p />";
    }
    lbl_Output.Text += outMessage;
  }

  protected void btn_GetInterface_Click(object sender, EventArgs e)
  {
    // resolve an instance of the class registered for IMyInterface
    IMyInterface  myInstance = myContainer.Resolve<IMyInterface>();
    // display the result of calling the class method
    lbl_Output.Text += myInstance.GetObjectStringResult();
  }

  protected void btn_RegisterNew_Click(object sender, EventArgs e)
  {
    // register a new default (un-named) mapping with the container
    // this replaces the existing mapping in the container for IMyInterface
    myContainer.RegisterType<IMyInterface, MyOtherObject>();
    lbl_Output.Text += "Registered a new mapping for IMyInterface to MyOtherObject.";
  }

  protected void btn_GetLogger_Click(object sender, EventArgs e)
  {
    // get the logger name selected in the list box 
    // this name specifies the type of logger required
    String loggerType = lst_LoggerType.SelectedValue;
    // resolve an instance of the appropriate class registered for ILogger 
    // using the specified mapping name (may be empty for the default mapping)
    ILogger myInstance = myContainer.Resolve<ILogger>(loggerType);
    // get the logger to write a message and display the result
    lbl_Output.Text += myInstance.WriteMessage("HELLO UNITY!");
  }

  protected void btn_ConstructorInjection_Click(object sender, EventArgs e)
  {
    // resolve an instance of the concrete MyObjectWithInjectedLogger class 
    // this class contains a reference to ILogger in the constructor parameters
    MyObjectWithInjectedLogger myInstance = myContainer.Resolve<MyObjectWithInjectedLogger>();
    // get the injected logger to write a message and display the result
    lbl_Output.Text += myInstance.GetObjectStringResult();
  }

  protected void btn_ReloadContainer_Click(object sender, EventArgs e)
  {
    // reload the existing Unity container configuration from the Web.config file 
    UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
    section.Containers["containerOne"].Configure(myContainer);
    Application["MyContainer"] = myContainer;
    lbl_Output.Text += "Reloaded the original Unity Container configuration.";
  }
}
