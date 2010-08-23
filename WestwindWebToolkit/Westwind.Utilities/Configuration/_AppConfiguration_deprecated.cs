#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          © West Wind Technologies, 2009
 *          http://www.west-wind.com/
 * 
 * Created: 09/12/2009
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 **************************************************************  
*/
#endregion

using System;

using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Xml;
using System.Globalization;

using System.Data.SqlClient;
using System.Xml.Serialization;

namespace Westwind.Utilities.Configuration
{
    /// <summary>
    /// Configuration object class that persists its public members to the
    /// application's .Config file in a type safe manner. This class manages
    /// reading and writing the Config settings and providing them in a consistent
    /// and type-safe manner.
    /// 
    /// You implement a configuration object by creating a new subclass of the
    /// AppConfiguration class and adding public Fields to the class. Every new
    /// member you add will be persisted to the application's .Config file. On
    /// first use the class also writes out default values into the .Config file so
    ///  the data is always there. The data is always returned in a fully typed
    /// manner - you simply reference the properties of this object.
    /// 
    /// Values are stored in the AppSettings section of the .Config file
    ///  and uses ApplicationSettings to internally retrieve this data. However,
    /// the data is always returned in the proper type format rather than as string
    ///  and null instances are never a problem as there will always be a default
    /// value returned. This reduces the amount of code that goes along with
    /// pulling data out of the .Config file.
    /// 
    /// Supported fields type are any simple types (string, decimal, double, int,
    /// boolean, datetime etc. ) as well as enums. Enums must be persisted into the
    ///  front end interface using strings (ie. if you use it in a listbox value
    /// the value must be string).
    /// 
    /// The class also provides the ability to encrypt keys by implementing a
    /// custom constructor that passes a field list and an encryption key to be
    /// used for encrypting one or more keys in the configuration.
    /// <seealso>Managing Configuration Settings with AppConfiguration</seealso>
    /// </summary>
    [Obsolete("This class has been deprecated. Please use the AppConfigurationWithProvider class.")]
    public abstract class _AppConfiguration
    {
        /// <summary>
        /// Contains an error message if a method returns false or the object fails to 
        /// load the configuration data.
        /// <seealso>Class AppConfiguration</seealso>
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        public string ErrorMessage = string.Empty;

        /// <summary>
        /// List of fields that are to be encrypted.
        /// </summary>
        private string EncryptFieldList = string.Empty;

        /// <summary>
        /// Key used for encryption. If this key is null the default
        /// of the wwEncrypt class is used.
        /// </summary>
        private string EncryptKey = string.Empty;

        /// <summary>
        /// Name of the Configuration Section to be written to in the Config file
        /// </summary>
        private string ConfigSectionName = string.Empty;

        /// <summary>
        /// Optionally an external file that holds the configuration Settings
        /// </summary>
        private string ConfigFilename = string.Empty;

        /// <summary>
        /// Internally used reference to the Namespace Manager object
        /// used to make sure we're searching the proper Namespace
        /// for the appSettings section when reading and writing manually
        /// </summary>
        private XmlNamespaceManager XmlNamespaces = null;

        //Internally used namespace prefix for the default namespace
        private string XmlNamespacePrefix = "ww:";


        /// <summary>
        /// The constructor of this class should be overridden to create custom
        /// configuration management behavior. The default constructor will not
        /// load any configuration data.
        /// 
        /// To implement your own configuration class that stores in .config
        /// and a custom MyApplication Section:
        /// 
        /// public class MyConfig : AppConfiguration
        /// {
        ///     public MyConfig()
        ///     {
        ///         SetEncryption("MailUsername,MailPassword",STR_APP_SECRETKEY);
        ///         SetSection("MyApplication");
        ///         ReadKeysFromConfig();
        ///     }
        ///     
        ///     public string ConnectionString { get; set; }
        ///     public string ApplicationName { get; set; }
        ///     public int MaxListItems {get; set; }       
        ///     public string MailUsername { get; set; }
        ///     public string MailPassword { get; set; }
        /// }
        /// </summary>               
        public _AppConfiguration()
        {
            
        }

        /// <summary>
        /// This version of the constructor doesn't do any default
        /// configuration.
        /// </summary>
        /// <param name="noConfiguration"></param>
        public _AppConfiguration(bool noConfiguration)
        {
        }

        /// <summary>
        /// Sets the Configuration File Section if the default is section is not used
        /// </summary>
        /// <param name="ConfigurationFileSection"></param>
        public virtual void SetConfigurationSection(string ConfigurationFileSection)
        {
            ConfigSectionName = ConfigurationFileSection;
        }

        /// <summary>
        /// Sets the Configuration File Section if the default is section is not used
        /// <seealso>Class AppConfiguration</seealso>
        /// </summary>
        /// <param name="ConfigurationFileSection"></param>
        /// <returns>Void</returns>
        /// <example>
        /// WebStoreConfig Config = new WebStoreConfig(false);
        /// Config.SetConfigurationSection("WebStore");
        /// Config.ReadKeysFromConfig();
        /// </example>
        public virtual void SetEncryption(string EncryptFields, string EncryptKey)
        {
            EncryptFieldList = "," + EncryptFields.ToLower() + ",";
            EncryptKey = EncryptKey;
        }


        /// <summary>
        /// Reads all the configuration settings from the .Config file into the public 
        /// fields of this object.
        /// 
        /// If the keys don't exist in the file the values are returned as the default 
        /// values set on the fields. If keys missing they are written into the .Config
        /// file with their default values ensuring that the class and the config file
        /// are always in sync.
        /// 
        /// Keys are written to the config file only if rights exist to do so. If
        /// rights are not available the method will fail the write silently and
        /// without any errors which is the same behavior you get from built in objects.
        /// <seealso>Class AppConfiguration</seealso>
        /// </summary>
        /// <returns>void</returns>
        public virtual void ReadKeysFromConfig()
        {
            if (ConfigFilename != string.Empty)
            {
                ReadKeysFromConfig(ConfigFilename);
                return;
            }

            Type typeWebConfig = GetType();
            MemberInfo[] Fields = typeWebConfig.GetMembers(BindingFlags.Public | BindingFlags.Instance);

            // Set a flag for missing fields
            // If we have any we'll need to write them out into .config
            bool MissingFields = false;

            // Loop through all fields and properties                 
            foreach (MemberInfo Member in Fields)
            {
                string TypeName = null;

                FieldInfo Field = null;
                PropertyInfo Property = null;
                Type FieldType = null;

                if (Member.MemberType == MemberTypes.Field)
                {
                    Field = (FieldInfo)Member;
                    FieldType = Field.FieldType;
                    TypeName = FieldType.Name.ToLower();
                }
                else if (Member.MemberType == MemberTypes.Property)
                {
                    Property = (PropertyInfo)Member;
                    FieldType = Property.PropertyType;
                    TypeName = FieldType.Name.ToLower();
                }
                else
                    continue;

                string Fieldname = Member.Name.ToLower();

                // Error Message is an internal public property
                if (Fieldname == "errormessage")
                    continue;

                string Value = null;
                if (ConfigSectionName == string.Empty)
                    Value = ConfigurationManager.AppSettings[Fieldname];
                else
                {
                    NameValueCollection Values = (NameValueCollection)ConfigurationManager.GetSection(ConfigSectionName);
                    if (Values != null)
                        Value = Values[Fieldname];
                }

                if (Value == null)
                {
                    MissingFields = true;
                    continue;
                }

                // If we're encrypting decrypt any field that are encyrpted
                if (Value != string.Empty && EncryptFieldList.IndexOf("," + Fieldname + ",") > -1)
                    Value = Encryption.DecryptString(Value, EncryptKey);

                try
                {
                    // Assign the value to the property
                    ReflectionUtils.SetPropertyEx(this, Fieldname,
                        ReflectionUtils.StringToTypedValue(Value, FieldType, CultureInfo.InvariantCulture));
                }
                catch {;}
            }

            // We have to write any missing keys
            if (MissingFields)
                WriteKeysToConfig();
        }


        /// <summary>
        /// Version of ReadKeysFromConfig that reads and writes an external Config file
        /// that is not controlled through the ConfigurationSettings class.
        /// </summary>
        /// <param name="Filename">The filename to read from. If the file doesn't exist it is created if permissions are available</param>
        public virtual void ReadKeysFromConfig(string Filename)
        {
            Type typeWebConfig = GetType();
            MemberInfo[] Fields = typeWebConfig.GetMembers(BindingFlags.Public |
               BindingFlags.Instance);

            // Set a flag for missing fields
            // If we have any we'll need to write them out 
            bool MissingFields = false;

            XmlDocument Dom = new XmlDocument();

            try
            {
                Dom.Load(Filename);
            }
            catch
            {
                // Can't open or doesn't exist - so create it
                if (!WriteKeysToConfig(Filename))
                    return;

                // Now load again
                Dom.Load(Filename);
            }

            // Retrieve XML Namespace information to assign default 
            // Namespace explicitly.
            GetXmlNamespaceInfo(Dom);

            // Save the configuration file
            ConfigFilename = Filename;

            string ConfigSection = ConfigSectionName;
            if (ConfigSection == string.Empty)
                ConfigSection = "appSettings";

            foreach (MemberInfo Member in Fields)
            {
                FieldInfo Field = null;
                PropertyInfo Property = null;
                Type FieldType = null;
                string TypeName = null;

                if (Member.MemberType == MemberTypes.Field)
                {
                    Field = (FieldInfo)Member;
                    FieldType = Field.FieldType;
                    TypeName = Field.FieldType.Name.ToLower();
                }
                else if (Member.MemberType == MemberTypes.Property)
                {
                    Property = (PropertyInfo)Member;
                    FieldType = Property.PropertyType;
                    TypeName = Property.PropertyType.Name.ToLower();
                }
                else
                    continue;

                string Fieldname = Member.Name;

                XmlNode Section = Dom.DocumentElement.SelectSingleNode(XmlNamespacePrefix + ConfigSection, XmlNamespaces);
                if (Section == null)
                {
                    Section = CreateConfigSection(Dom, ConfigSectionName);
                    Dom.DocumentElement.AppendChild(Section);
                }

                string Value = GetNamedValueFromXml(Dom, Fieldname, ConfigSection);
                if (Value == null)
                {
                    MissingFields = true;
                    continue;
                }

                Fieldname = Fieldname.ToLower();

                // If we're encrypting decrypt any field that are encyrpted
                if (Value != string.Empty && EncryptFieldList.IndexOf("," + Fieldname + ",") > -1)
                    Value = Encryption.DecryptString(Value, EncryptKey);

                //SetPropertyFromString(FieldType,Fieldname,Value);

                // Assign the Property
                ReflectionUtils.SetPropertyEx(this, Fieldname,
                                     ReflectionUtils.StringToTypedValue(Value, FieldType, CultureInfo.InvariantCulture));
            }

            // We have to write any missing keys
            if (MissingFields)
                WriteKeysToConfig(Filename);

        }


        /// <summary>
        /// Static Factory methods of ReadKeysFromConfig that return an instance of a Config object.
        /// Note that the Constructor still fires on these!
        /// </summary>
        /// <param name="ConfigType"></param>
        /// <returns></returns>
        public static T ReadKeysFromConfig<T>(Type ConfigType)  where T : _AppConfiguration
        {
            T Config = Activator.CreateInstance(ConfigType) as T;
            return Config;
        }

        /// <summary>
        /// Static Factory methods of ReadKeysFromConfig that return an instance of a Config object.
        /// Note that the Constructor still fires on these!
        /// </summary>
        /// <param name="ConfigType"></param>
        /// <returns></returns>
        public static _AppConfiguration ReadKeysFromConfig(string Filename, Type ConfigType)
        {
            _AppConfiguration Config = (_AppConfiguration)Activator.CreateInstance(ConfigType);
            Config.ReadKeysFromConfig(Filename);
            return Config;
        }

        /// <summary>
        /// Returns a single key from the Configuration Settings Section.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        /// <param name="Filename"></param>
        /// <remarks>Requires permissions to read the external file</remarks>
        /// <returns></returns>
        public virtual string GetKeyFromConfigSection(string Key, string Section, string Filename)
        {
            if (Filename != null)
            {
                // Just use the built in handling if we're reading from the stock
                // Config file
                NameValueCollection nv = (NameValueCollection)ConfigurationManager.GetSection(Section);
                if (nv == null)
                    return null;

                return (string)nv[Key];
            }

            // Otherwise we have to use XML to read this data
            XmlDocument Dom = new XmlDocument();

            try
            {
                Dom.Load(Filename);
            }
            catch
            {
                return null;
            }

            return GetNamedValueFromXml(Dom, Key, Section);
        }

        /// <summary>
        /// Returns a key from the Configuration Settings Section.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        /// <remarks>Requires permissions to read the external file</remarks>
        /// <returns></returns>
        public virtual string GetKeyFromConfigSection(string Key, string Section)
        {
            return GetKeyFromConfigSection(Key, Section, null);
        }



        /// <summary>
        /// Returns all the keys in the Configuration Section as string key value pairs.
        /// Internally uses Configurations class for default configuration and Xml 
        /// to retrieve in external files. 
        /// </summary>
        /// <remarks>Requires permissions to read the external file</remarks>
        /// <param name="Section"></param>
        /// <param name="Filename"></param>
        /// <returns></returns>
        public virtual NameValueCollection GetAllKeysFromConfigSection(string Section, string Filename)
        {
            if (Filename == null)
                return (NameValueCollection)ConfigurationManager.GetSection(Section);

            // Otherwise we have to use XML to read this data
            XmlDocument Dom = new XmlDocument();

            try
            {
                Dom.Load(Filename);
            }
            catch
            {
                return null;
            }

            GetXmlNamespaceInfo(Dom);

            XmlNodeList Nodes = Dom.DocumentElement.SelectNodes(XmlNamespacePrefix + Section + "/" + XmlNamespacePrefix + "add");
            if (Nodes == null)
                return null;

            // Add to NvCollection
            NameValueCollection nv = new NameValueCollection(Nodes.Count);
            foreach (XmlNode Node in Nodes)
                nv.Add(Node.Attributes["key"].Value, Node.Attributes["value"].Value);

            return nv;
        }

        /// <summary>
        /// Returns all the keys in the Configuration Section as string key value pairs.
        /// Internally uses Configurations class for default configuration and Xml 
        /// to retrieve in external files. 
        /// </summary>
        /// <remarks>Requires permissions to read the external file</remarks>
        /// <param name="Section"></param>
        /// <returns></returns>
        public virtual NameValueCollection GetAllKeysFromConfigSection(string Section)
        {
            return GetAllKeysFromConfigSection(Section, null);
        }

        /// <summary>
        /// Returns a single value from the XML in a configuration file.
        /// </summary>
        /// <param name="Dom"></param>
        /// <param name="Key"></param>
        /// <param name="ConfigSection"></param>
        /// <returns></returns>
        protected string GetNamedValueFromXml(XmlDocument Dom, string Key, string ConfigSection)
        {
            XmlNode Node = Dom.DocumentElement.SelectSingleNode(
                   XmlNamespacePrefix + ConfigSection + @"/" +
                   XmlNamespacePrefix + "add[@key='" + Key + "']", XmlNamespaces);

            if (Node == null)
                return null;

            return Node.Attributes["value"].Value;
        }


        /// <summary>
        /// Used to load up the default namespace reference and prefix
        /// information. This is required so that SelectSingleNode can
        /// find info in 2.0 or later config files that include a namespace
        /// on the root element definition.
        /// </summary>
        /// <param name="Dom"></param>
        protected void GetXmlNamespaceInfo(XmlDocument Dom)
        {
            // Load up the Namespaces object so we can 
            // reference the appropriate default namespace
            if (Dom.DocumentElement.NamespaceURI == null || Dom.DocumentElement.NamespaceURI == string.Empty)
            {
                XmlNamespaces = null;
                XmlNamespacePrefix = string.Empty;
            }
            else
            {
                if (Dom.DocumentElement.Prefix == null || Dom.DocumentElement.Prefix == string.Empty)
                    XmlNamespacePrefix = "ww";
                else
                    XmlNamespacePrefix = Dom.DocumentElement.Prefix;

                XmlNamespaces = new XmlNamespaceManager(Dom.NameTable);
                XmlNamespaces.AddNamespace(XmlNamespacePrefix, Dom.DocumentElement.NamespaceURI);

                XmlNamespacePrefix += ":";
            }
        }

        /// <summary>
        /// Writes all of the configuration file properties to the configuration file.
        /// 
        /// The keys are written into the standard .Config files (web.config or 
        /// YourApp.exe.config for example).
        /// 
        /// &lt;&lt;b&gt;&gt;Note: &lt;&lt;/b&gt;&gt;
        /// In Web Application updating this call causes the Web application to reload 
        /// itself as a change is made to web.config
        /// <seealso>Class AppConfiguration</seealso>
        /// </summary>
        /// <returns>Void</returns>
        public virtual bool WriteKeysToConfig()
        {
            if (ConfigFilename != string.Empty)
                return WriteKeysToConfig(ConfigFilename);

            return WriteKeysToConfig(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        /// <summary>
        /// Writes all of the configuration file properties to a specified 
        /// configuration file.
        /// 
        /// The format written is in standard .Config file format, but this method  
        /// allows writing out to a custom .Config file.
        /// 
        /// Uses XmlDom and file output to manipulate web.config rather than the .NET 
        /// configuration APIs to avoid security issues in medium trust. This code can 
        /// run in medium trust as long as the file is writable by the  account running
        ///  the application.
        /// <seealso>Class AppConfiguration</seealso>
        /// </summary>
        /// <param name="Filename">
        /// The name of the file to write the configuration data to. Format is 
        /// XmlSerialization formatted.
        /// </param>
        /// <returns>Void</returns>
        /// <example>
        /// // Overridden constructor
        /// public WebStoreConfig() : base(false)
        /// {
        ///    SetEnryption(&amp;quot;ConnectionString,MailPassword,MerchantPassword&amp;quot;,&amp;quot;WebStorePassword&amp;quot;);
        ///        ///    // Use a custom Config file
        ///    
        /// ReadKeysFromConfig(@&amp;quot;d:\projects\wwWebStore\MyConfig.config&a
        /// mp;quot;);
        /// }
        /// 
        /// </example>
        public virtual bool WriteKeysToConfig(string Filename)
        {
            // Asp.NET 2.0 can write to config file
            //*** for now not implemented - continue using 1.1 compatible code
            //*** because it only works with web.config and because of security
            //*** requirements: Doesn't work in medium trust.
            //Configuration Conf = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            //AppSettingsSection settings = Conf.AppSettings;Westwind.WebStore.Configuration
            //object s = Conf.GetSection("Westwind.WebStore.Configuration") ;
            //AppSettingsSection settings =  s as AppSettingsSection; // doesn't work
            //settings.Settings.Add("NameX", DateTime.Now.ToString());            
            //Conf.Save(ConfigurationSaveMode.Modified);
            //return true;            

            // Load the config file into DOM parser
            XmlDocument Dom = new XmlDocument();

            try
            {
                Dom.Load(Filename);
            }
            catch
            {
                // Can't load the file - create an empty document
                string Xml =
               @"<?xml version='1.0'?>
<configuration>
</configuration>";

                Dom.LoadXml(Xml);
            }

            // Load up the Namespaces object so we can 
            // reference the appropriate default namespace
            GetXmlNamespaceInfo(Dom);

            // Parse through each of hte properties of the properties
            Type typeWebConfig = GetType();
            MemberInfo[] Fields = typeWebConfig.GetMembers(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public);

            foreach (MemberInfo Field in Fields)
            {

                // If we can't find the key - write it out to the document
                string Value = null;
                object RawValue = null;
                if (Field.MemberType == MemberTypes.Field)
                    RawValue = ((FieldInfo)Field).GetValue(this);
                else if (Field.MemberType == MemberTypes.Property)
                    RawValue = ((PropertyInfo)Field).GetValue(this, null);
                else
                    continue; // not a property or field

                // Don't persist ErrorMessage property
                if (Field.Name == "ErrorMessage")
                    continue;


                Value = ReflectionUtils.TypedValueToString(RawValue, CultureInfo.InvariantCulture);

                // Encrypt the field if in list
                if (EncryptFieldList.IndexOf("," + Field.Name.ToLower() + ",") > -1)
                    Value = Encryption.EncryptString(Value, EncryptKey);

                string ConfigSection = "appSettings";
                if (!string.IsNullOrEmpty(ConfigSectionName))
                    ConfigSection = ConfigSectionName;


                XmlNode Node = Dom.DocumentElement.SelectSingleNode(
                    XmlNamespacePrefix + ConfigSection + "/" +
                    XmlNamespacePrefix + "add[@key='" + Field.Name + "']", XmlNamespaces);

                if (Node == null)
                {
                    // Create the node and attributes and write it
                    Node = Dom.CreateNode(XmlNodeType.Element, "add", Dom.DocumentElement.NamespaceURI);

                    XmlAttribute Attr2 = Dom.CreateAttribute("key");
                    Attr2.Value = Field.Name;
                    XmlAttribute Attr = Dom.CreateAttribute("value");
                    Attr.Value = Value;

                    Node.Attributes.Append(Attr2);
                    Node.Attributes.Append(Attr);

                    XmlNode Parent = Dom.DocumentElement.SelectSingleNode(
                        XmlNamespacePrefix + ConfigSection, XmlNamespaces);

                    if (Parent == null)
                        Parent = CreateConfigSection(Dom, ConfigSection);

                    Parent.AppendChild(Node);
                }
                else
                {
                    // just write the value into the attribute
                    Node.Attributes.GetNamedItem("value").Value = Value;
                }


                string XML = Node.OuterXml;

            } // for each

            try
            {
                Dom.Save(Filename);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

            return true;
        }


        /// <summary>
        /// Creates a Configuration section and also creates a ConfigSections section for new 
        /// non appSettings sections.
        /// </summary>
        /// <param name="Dom"></param>
        /// <param name="ConfigSection"></param>
        /// <returns></returns>
        private XmlNode CreateConfigSection(XmlDocument Dom, string ConfigSection)
        {

            // Create the actual section first and attach to document
            XmlNode AppSettingsNode = Dom.CreateNode(XmlNodeType.Element,
                ConfigSection, Dom.DocumentElement.NamespaceURI);

            XmlNode Parent = Dom.DocumentElement.AppendChild(AppSettingsNode);

            // Now check and make sure that the section header exists
            if (ConfigSection != "appSettings")
            {
                XmlNode ConfigSectionHeader = Dom.DocumentElement.SelectSingleNode(XmlNamespacePrefix + "configSections",
                                XmlNamespaces);
                if (ConfigSectionHeader == null)
                {
                    // Create the node and attributes and write it
                    XmlNode ConfigSectionNode = Dom.CreateNode(XmlNodeType.Element,
                             "configSections", Dom.DocumentElement.NamespaceURI);

                    // Insert as first element in DOM
                    ConfigSectionHeader = Dom.DocumentElement.InsertBefore(ConfigSectionNode,
                             Dom.DocumentElement.ChildNodes[0]);
                }

                // Check for the Section
                XmlNode Section = ConfigSectionHeader.SelectSingleNode(XmlNamespacePrefix + "section[@name='" + ConfigSection + "']",
                        XmlNamespaces);

                if (Section == null)
                {
                    Section = Dom.CreateNode(XmlNodeType.Element, "section",
                             null);

                    XmlAttribute Attr = Dom.CreateAttribute("name");
                    Attr.Value = ConfigSection;
                    XmlAttribute Attr2 = Dom.CreateAttribute("type");
                    Attr2.Value = "System.Configuration.NameValueSectionHandler,System,Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                    XmlAttribute Attr3 = Dom.CreateAttribute("requirePermission");
                    Attr3.Value = "false";
                    Section.Attributes.Append(Attr);
                    Section.Attributes.Append(Attr3);
                    Section.Attributes.Append(Attr2);
                    ConfigSectionHeader.AppendChild(Section);
                }
            }

            return Parent;
        }


        /// <summary>
        /// Serializes the the current object into a  file in XML format on disk
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool WriteKeysToFile(string fileName)
        {
            if (EncryptFieldList != string.Empty)
                EncryptFields();

            bool Result = SerializationUtils.SerializeObject(this, fileName, false);

            if (EncryptFieldList != string.Empty)
                DecryptFields();

            return Result;
        }

        /// <summary>
        /// Reads the configuration settings from an XML file created with WriteKeysToFile.
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ConfigurationObjectType"></param>
        /// <returns></returns>
        public static _AppConfiguration ReadKeysFromFile(string FileName, Type ConfigurationObjectType)
        {
            _AppConfiguration Config = (_AppConfiguration)SerializationUtils.DeSerializeObject(FileName, ConfigurationObjectType, false);
            if (Config == null)
                return (_AppConfiguration)Activator.CreateInstance(ConfigurationObjectType);

            if (Config.EncryptFieldList != string.Empty)
                Config.DecryptFields();

            return Config;
        }


        /// <summary>
        /// Serializes the current object into a string in XML format
        /// </summary>
        /// <param name="XmlResultString"></param>
        /// <returns></returns>
        public bool WriteKeysToString(out string XmlResultString)
        {
            if (EncryptFieldList != string.Empty)
                EncryptFields();

            XmlResultString = string.Empty;
            bool Result = SerializationUtils.SerializeObject(this, out XmlResultString);

            if (EncryptFieldList != string.Empty)
                DecryptFields();

            return Result;
        }


        /// <summary>
        /// Reads the configuration settings from an XML string created with 
        /// WriteKeysToString.
        /// <seealso>Class AppConfiguration</seealso>
        /// </summary>
        /// <param name="Xml">
        /// The XML string that contains a serialized instance of this configuration 
        /// object.
        /// </param>
        /// <param name="ConfigurationObjectType">
        /// The type of the configuration object.
        /// </param>
        /// <returns>Westwind.Utilities.AppConfiguration</returns>
        public static _AppConfiguration ReadKeysFromString(string Xml, Type ConfigurationObjectType)
        {
            _AppConfiguration Config = (_AppConfiguration)SerializationUtils.DeSerializeObject(Xml, ConfigurationObjectType);
            if (Config == null)
                return (_AppConfiguration)Activator.CreateInstance(ConfigurationObjectType);

            if (Config.EncryptFieldList != string.Empty)
                Config.DecryptFields();

            return Config;
        }


        /// <summary>
        /// Writes the Configuration Settings into a SQL Server of your choice table and a field called ConfigData.
        /// If the table doesn't exist it's created for you (requires appropriate permissions)
        /// </summary>
        /// <param name="ConnectionString">Sql Server ConnectionString</param>
        /// <param name="TableName">Name of the Table to write to</param>
        /// <param name="Key">Integer ID value of the configuration item - you can have more than one item</param>
        /// <returns>true or false</returns>
        public bool WriteKeysToSqlServer(string ConnectionString, string TableName, int Key)
        {
            SqlCommand Command = DataUtils.GetSqlCommand(ConnectionString,
               "Update [" + TableName + "] set ConfigData=@ConfigData where id=" + Key.ToString());

            string XmlConfig = string.Empty;
            WriteKeysToString(out XmlConfig);

            Command.Parameters.Add(new SqlParameter("@ConfigData", XmlConfig));

            int Result = 0;
            try
            {
                Result = Command.ExecuteNonQuery();
            }
            catch
            {
                Command.CommandText =
    @"CREATE TABLE [" + TableName + @"]  
( [id] [int] , [ConfigData] [text] COLLATE SQL_Latin1_General_CP1_CI_AS)";
                try
                {
                    Command.ExecuteNonQuery();
                }
                catch
                {
                    DataUtils.CloseConnection(Command);
                    return false;
                }
            }

            // Check for missing record
            if (Result == 0)
            {
                Command.CommandText = "Insert [" + TableName + "] (id,configdata) values (" + Key.ToString() + ",@ConfigData)";

                try
                {
                    Result = Command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    DataUtils.CloseConnection(Command);
                    string Message = ex.Message;
                    return false;
                }
                if (Result == 0)
                {
                    DataUtils.CloseConnection(Command);
                    return false;
                }

            }
            DataUtils.CloseConnection(Command);

            return true;
        }

        /// <summary>
        /// Reads keys from a SQL Database from a table you specify.
        /// 
        /// The table must contain ID (int) and ConfigData (Text) fields. The value 
        /// written is a single XML string value of the serialized configuration 
        /// object.
        /// <seealso>Class AppConfiguration</seealso>
        /// </summary>
        /// <param name="ConnectionString">
        /// Connection string to the SQL database
        /// </param>
        /// <param name="TableName">
        /// Name of the table. If the table doesn't exist it's created
        /// </param>
        /// <param name="Key">
        /// id of this configuration record. Use 0 for a single record.
        /// </param>
        /// <param name="ConfigurationObjectType">
        /// provide a type instance of the type to create (typeof(WebStoreConfig) for 
        /// example)
        /// </param>
        /// <returns>object reference or null on failure</returns>
        public static object ReadkeysFromSqlServer(string ConnectionString, string TableName, int Key, Type ConfigurationObjectType)
        {
            SqlCommand Command = DataUtils.GetSqlCommand(ConnectionString,
               "select * from [" + TableName + "] where id=" + Key.ToString());

            SqlDataReader reader = null;
            try
            {
                reader = Command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 218)
                {

                    Command.CommandText =
@"CREATE TABLE [" + TableName + @"]  
( [id] [int] , [ConfigData] [text] COLLATE SQL_Latin1_General_CP1_CI_AS)";
                    try
                    {
                        Command.ExecuteNonQuery();
                    }
                    catch
                    {
                        DataUtils.CloseConnection(Command);
                        return null;
                    }
                    return ReadkeysFromSqlServer(ConnectionString,TableName,Key,ConfigurationObjectType);
                }

            }
            catch
            {
                DataUtils.CloseConnection(Command);
                return null;
            }

            reader.Read();
            string XmlConfig = (string)reader["ConfigData"];
            reader.Close();

            DataUtils.CloseConnection(Command);

            return ReadKeysFromString(XmlConfig, ConfigurationObjectType);
        }


        /// <summary>
        /// Encrypts all the fields in the current object based on the EncryptFieldList
        /// </summary>
        /// <returns></returns>
        private void EncryptFields()
        {
            MemberInfo[] mi = GetType().FindMembers(MemberTypes.Property | MemberTypes.Field,
               ReflectionUtils.MemberAccess, null, null);

            foreach (MemberInfo Member in mi)
            {
                string FieldName = Member.Name.ToLower();

                // Encrypt the field if in list
                if (EncryptFieldList.IndexOf("," + FieldName + ",") > -1)
                {
                    object Value = string.Empty;

                    if (Member.MemberType == MemberTypes.Field)
                        Value = ((FieldInfo)Member).GetValue(this);
                    else
                        Value = ((PropertyInfo)Member).GetValue(this, null);

                    Value = Encryption.EncryptString((string)Value, EncryptKey);

                    if (Member.MemberType == MemberTypes.Field)
                        ((FieldInfo)Member).SetValue(this, Value);
                    else
                        ((PropertyInfo)Member).SetValue(this, Value, null);

                }
            }
        }

        /// <summary>
        /// Internally decryptes all the fields in the current object based on the EncryptFieldList
        /// </summary>
        /// <returns></returns>
        private void DecryptFields()
        {
            MemberInfo[] mi = GetType().FindMembers(MemberTypes.Property | MemberTypes.Field,
               ReflectionUtils.MemberAccess, null, null);

            foreach (MemberInfo Member in mi)
            {
                string FieldName = Member.Name.ToLower();

                // Encrypt the field if in list
                if (EncryptFieldList.IndexOf("," + FieldName + ",") > -1)
                {
                    object Value = string.Empty;

                    if (Member.MemberType == MemberTypes.Field)
                        Value = ((FieldInfo)Member).GetValue(this);
                    else
                        Value = ((PropertyInfo)Member).GetValue(this, null);

                    Value = Encryption.DecryptString((string)Value, EncryptKey);

                    if (Member.MemberType == MemberTypes.Field)
                        ((FieldInfo)Member).SetValue(this, Value);
                    else
                        ((PropertyInfo)Member).SetValue(this, Value, null);

                }
            }
        }


    }
}
