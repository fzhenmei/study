using System;
using System.Web;
using System.Web.UI;

using System.Reflection;
using System.Text;
using System.IO;

using Westwind.Utilities;
using Westwind.Utilities.Configuration;

namespace Westwind.WebToolkit
{

	/// <summary>
	/// Summary description for ConfigClassTests.
	/// </summary>
    public partial class ApplicationConfigurationDemo : System.Web.UI.Page
    {
        /// <summary>
        /// Local instance of the configuration class that we'll end up binding to
        /// </summary>
        public ApplicationConfiguration AppConfig = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ((WestWindWebToolkitMaster)this.Master).SubTitle = this.Title;

            // Default Web.Config read with Constructor
            // at first access
            if (this.txtSource.SelectedValue == "Default Web.Config")
            {
                // Simply assign the default config object - it gets loaded via 
                // the default constructor defined in AppConfig.cs
                this.AppConfig = App.Configuration;
            }


            // Explicit object creation for the remaining objects, 
            // but you can use the same static constructor approach
            // as with the above

            // Separate Web.Config Section
            else if (this.txtSource.SelectedValue == "Different Web.Config Section")
            {
                var provider = new ConfigurationFileConfigurationProvider<ApplicationConfiguration>()
                {
                     ConfigurationSection = "WebStore",
                     PropertiesToEncrypt = "ConnectionString,MailServerPassword", 
                     EncryptionKey = "SUPERSECRET"
                };
                this.AppConfig = new ApplicationConfiguration(provider);
                this.AppConfig.Read();
            }

                // Separate Web.AppConfig Section
            else if (this.txtSource.SelectedValue == "Different .Config File")
            {
                
                var provider = new ConfigurationFileConfigurationProvider<ApplicationConfiguration>()
                {
                    ConfigurationFile= Server.MapPath("~/WebStore.config"),
                    ConfigurationSection = "WebStoreConfiguration",
                    PropertiesToEncrypt = "ConnectionString,MailServerPassword",
                    EncryptionKey = "SUPERSECRET"
                };

                this.AppConfig = new ApplicationConfiguration(provider);
                this.AppConfig.Read();
            }

            else if (this.txtSource.SelectedValue == "Simple Xml File")
            {
                var provider = new XmlFileConfigurationProvider<ApplicationConfiguration>()
                {
                    XmlConfigurationFile = Server.MapPath("WebStoreConfig.xml"),
                    PropertiesToEncrypt = "ConnectionString,MailServerPassword",
                    EncryptionKey = "SUPERSECRET"
                };

                this.AppConfig = new ApplicationConfiguration(provider);
                this.AppConfig.Read();
            }
            else if (this.txtSource.SelectedValue == "String")
            {

                string XmlString = this.ViewState["txtXmlString"] as string;
                if (XmlString != null)
                {
                    this.AppConfig = new ApplicationConfiguration();

                    // You can always read from an XML Serialization string w/o
                    // any provider setup
                    this.AppConfig.Read(XmlString);
                }
                else
                    // just load the default configuration 
                    this.AppConfig = new ApplicationConfiguration();
            }

            // Not implemented since you will need a database
            // this example uses the connection string configured in the Web.Config
            else if (this.txtSource.SelectedValue == "Database")
            {
                var provider = new SqlServerConfigurationProvider<ApplicationConfiguration>()
                {
                    ConnectionString = App.Configuration.ConnectionString,
                    Tablename = "ConfigData",
                    Key=1,
                    PropertiesToEncrypt = "ConnectionString,MailServerPassword",
                    EncryptionKey = "SUPERSECRET"
                };

                this.AppConfig = new ApplicationConfiguration(provider);
                if (!this.AppConfig.Read())
                    this.ShowError(
                      "Unable to connect to the Database.<hr>" +
                      "This database samle uses the connection string in the configuration settings " +
                      "with a table named 'ConfigData' and a field named 'ConfigData' to hold the " +
                      "configuration settings. If you have a valid connection string you can click " +
                      "on Save Settings to force the table and a single record to be created.<br><br>" +
                      "Note: The table name is parameterized (and you can change it in the default.aspx.cs page), but the field name always defaults to ConfigData.<hr/>" +
                      AppConfig.ErrorMessage);
            }


        }


        protected void btnSaveSettings_Click(object sender, System.EventArgs e)
        {
            if (this.AppConfig == null)
                // Load the raw Config object without loading anything
                this.AppConfig = new ApplicationConfiguration();

            // Read all Formvars back into the Config Object
            WebUtils.FormVarsToObject(this.AppConfig, "txt");

            if (this.txtSource.SelectedValue == "Default Web.Config")
            {
                // This is simple
                if (this.AppConfig.Write())
                    this.ShowMessage( "Keys have been written to Web.Config in the AppConfiguration Section.<hr>" +
                        "For best observation open your web.config and view as values get changed. Note " +
                        "that the Connection String and	MailServerPassword fields are encrypted. Based " +
                        "on the constructor which calls SetEncryption with the field names.");
                else
                    this.ShowError("Writing the keys to config failed... Make sure your permissions are set. " + 
                                   AppConfig.ErrorMessage);
            }


                // Writing to WebStoreConfig section in Web.Config
            else if (this.txtSource.SelectedValue == "Different Web.Config Section")
            {
                if (this.AppConfig.Write())
                    this.ShowMessage("Keys have been written to Web.Config in the WebStoreConfig Section.<hr>" +
                        "For best observation open your web.config and view as values get changed. Note " +
                        "that the Connection String and	MailServerPassword fields are encrypted. Based " +
                        "on the constructor which calls SetEncryption with the field names.");
                else
                    this.ShowError("Writing the keys to config failed... Make sure your permissions are set. " +
                                   AppConfig.ErrorMessage);
            }

            else if (this.txtSource.SelectedValue == "Different .Config File")
            {
                if (this.AppConfig.Write())
                    this.ShowMessage("Keys have been written to a separate WebStore.Config in a WebStoreConfig Section.<hr>" +
                        "For best observation open WebStore.Config and view as values get changed. Note that changes in " +
                        "an external file will not automatically cause ASP.NET to recycle the Web application. However if you use " +
                        "a static property for the config object (unlike this example) the changes will be visible to all instances anyway");
                else
                    this.ShowError("Writing the keys to WebStore.Config failed... Make sure your permissions are set. " +
                                   AppConfig.ErrorMessage);
            }
            else if (this.txtSource.SelectedValue == "Simple Xml File")
            {
                if (this.AppConfig.Write())
                    this.ShowMessage("Keys have been written to a separate XML file WebStoreConfig.Xml.<hr>" +
                        "For best observation open this file and view as values get changed. Note that changes in " +
                        "an external file will not automatically cause ASP.NET to recycle the Web application. However if you use " +
                        "a static property for the config object (unlike this example) the changes will be visible to all instances anyway");
                else
                    this.ShowError("Writing the keys to WebStoreConfig.Xml failed... Make sure your permissions are set." + 
                                   AppConfig.ErrorMessage);

            }
            else if (this.txtSource.SelectedValue == "String")
            {
                string XmlContent = this.AppConfig.WriteAsString();



                if (!string.IsNullOrEmpty(XmlContent))

                {       
                        this.ShowMessage("Keys have been written to a string .<hr>" +
                        "The output looks like this:<p><pre>" +
                        Server.HtmlEncode(XmlContent) +
                        "</pre>" +
                        "This string is now written into View state and then retrieved from the Config object when reloading this page." +
                        "To see the persisted value, make a change to the settings, save, then move to a different mode, then come back - the change should be persisted. " +
                        "Note that with string and file output you can persist complex objects as long as objects are serializable.");
                        this.ViewState["txtXmlString"] = XmlContent;
                }
                else
                    this.ShowError("Writing the keys to string failed... Make sure your permissions are set. " + 
                                   AppConfig.ErrorMessage);

            }

            else if (this.txtSource.SelectedValue == "Database")
            {                
                if (this.AppConfig.Write())
                    this.ShowMessage("Keys have been written to the database .<hr>");
                else
                    this.ShowError("Writing the keys to the database failed <hr>" +
                                   "The connecton string or table namea and PK are incorrect. Use the database settings in AppConfiguration " +
                                   "to set up a connection string, then create a table called Config and add a text field called ConfigData and a PK field.");
            }


        }

        /// <summary>
        /// Creates a simplistic Property Grid to display of an object
        /// </summary>
        /// <returns></returns>
        public string ShowPropertyGrid(object sourceObject)
        {
            if (sourceObject == null)
                return "<hr/>No object passed.<hr/>";

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter hWriter = new HtmlTextWriter(sw))
                {
                    hWriter.WriteBeginTag("table");
                    hWriter.WriteAttribute("border", "1");
                    hWriter.WriteAttribute("cellpadding", "5");
                    hWriter.WriteAttribute("class", "blackborder");
                    hWriter.Write(" style='border-collapse:collapse;'");
                    hWriter.Write(HtmlTextWriter.TagRightChar);
                    MemberInfo[] miT = this.AppConfig.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, null);
                    foreach (MemberInfo Field in miT)
                    {
                        string Name = Field.Name;
                        object Value = null;
                        if (Field.MemberType == MemberTypes.Field)
                        {
                            Value = ((FieldInfo)Field).GetValue(this.AppConfig);
                            continue;
                        }
                        else
                            if (Field.MemberType == MemberTypes.Property)
                                Value = ((PropertyInfo)Field).GetValue(this.AppConfig, null);
                        hWriter.WriteFullBeginTag("tr");
                        hWriter.WriteFullBeginTag("td");
                        hWriter.Write(Name);
                        hWriter.WriteEndTag("td");
                        hWriter.WriteLine();
                        hWriter.WriteFullBeginTag("td");
                        hWriter.WriteBeginTag("input");
                        hWriter.WriteAttribute("name", "txt" + Name);
                        hWriter.WriteAttribute("value", ReflectionUtils.TypedValueToString(Value));
                        hWriter.Write(" style='Width:400px' ");
                        hWriter.Write(HtmlTextWriter.TagRightChar);
                        hWriter.WriteEndTag("td");
                        hWriter.WriteLine();
                        hWriter.WriteEndTag("tr");
                        hWriter.WriteLine();
                    }
                    hWriter.WriteEndTag("table");
                    //string TableResult = sb.ToString();
                    hWriter.Close();
                }
                sw.Close();
            }

            return sb.ToString();
        }

       
            
        


        protected void ShowMessage(string message)
        {            
            this.ErrorDisplay.ShowMessage(message);
        }
        protected void ShowError(string message)
        {
            this.ErrorDisplay.ShowError(message);
        }


    }

}
