#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          � West Wind Technologies, 2008
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
using Westwind.Utilities.Properties;
using System.Diagnostics;

namespace Westwind.Utilities.Configuration
{
    /// <summary>
    /// This class provides a strongly typed configuration base class that can read
    ///  and write configuration data from various configuration stores including 
    /// .NET .config files, plain XML files, strings and SQL Server databases.
    /// 
    /// This mechanism uses a class-first approach to configuration management 
    /// where you declare your configuration settings as simple class properties on
    ///  a subclass of this abstract class. The class then automatically reads 
    /// configuration information from the configuration store into the class' 
    /// properties. If settings don't exist they are auto-created, permissions 
    /// permitting. You can create multiple configuration objects. The advantage of
    ///  this class centric approach is that your configuration is not bound to any
    ///  particular environment - you can use the configuration in a Web, desktop, 
    /// console or service app using the same mechanism.
    /// 
    /// Using this class is easy: You create a subclass of AppConfiguration add a 
    /// constructor (or two) and then simply add properties to the class. The 
    /// implementation will handle persistence transparently. Typical 
    /// implementation defines the configuration class as a static property on a 
    /// global object of some sort (ie. App.Configuration.MySetting).
    /// 
    /// Storage is configured via a configuration provider that configures provider
    ///  specific features. The configuration class then uses the provider to read 
    /// and write data from the configuration store. The base providers also 
    /// support encryption of individual fields.
    /// <seealso>Managing Configuration Settings with AppConfiguration</seealso>
    /// </summary>
    /// <example>
    /// &lt;&lt;/pre&gt;&gt;&lt;&lt;b&gt;&gt;Class Implementation&lt;&lt;/b&gt;&gt;    
    /// &lt;&lt;code lang=&quot;C#&quot;&gt;&gt;/// &lt;summary&gt;
    /// /// Your application specific config class
    /// /// &lt;/summary&gt;
    /// public class ApplicationConfiguration : 
    /// Westwind.Utilities.Configuration.AppConfiguration
    /// {
    ///     public ApplicationConfiguration()
    ///     {
    ///     }
    /// 
    ///     public ApplicationConfiguration(IConfigurationProvider provider)
    ///         : this()
    ///     {
    ///         if (provider == null)
    ///         {
    ///             Provider = new
    ///                  
    /// ConfigationFileConfigurationProvider&lt;ApplicationConfiguration&gt;()
    ///             {
    ///                 PropertiesToEncrypt = 
    /// &quot;MailServerPassword,ConnectionString&quot;,
    ///                 EncryptionKey = &quot;secret&quot;,
    ///                 ConfigurationSection = &quot;ApplicationConfiguration&quot;
    ///             };
    /// 
    ///             // Example of Sql configuration
    ///             //Provider = new
    ///             //   
    /// SqlServerConfigurationProvider&lt;ApplicationConfiguration&gt;()
    ///             //{
    ///             //    FieldsToEncrypt = 
    /// &quot;MailServerPassword,ConnectionString&quot;,
    ///             //    EncryptKey = &quot;secret&quot;,
    ///             //    ConnectionString = &quot;DevSampleConnectionString&quot;,
    ///             //    Tablename = &quot;Configuration&quot;,
    ///             //    Key = 1
    ///             //};
    ///             // Example of external XML configuration
    ///             //Provider = new 
    /// XmlFileConfigurationProvider&lt;ApplicationConfiguration&gt;()
    ///             //{
    ///             //    FieldsToEncrypt = 
    /// &quot;MailServerPassword,ConnectionString&quot;,
    ///             //    EncryptKey = &quot;secret&quot;,
    ///             //    XmlConfigurationFile =
    ///             //         
    /// HttpContext.Current.Server.MapPath(&quot;~/Configuration.xml&quot;)
    ///             //};
    ///         }
    ///         else
    ///             Provider = provider;
    /// 
    ///         Provider.Read(this);
    ///     }
    /// 
    /// 	// persisted configuration properties - note type suppport
    ///     public string ApplicationTitle
    ///     {
    ///         get { return _ApplicationTitle; }
    ///         set { _ApplicationTitle = value; }
    ///     }
    ///     private string _ApplicationTitle = &quot;West Wind Web Toolkit&quot;;
    /// 
    ///     public string ConnectionString
    ///     {
    ///         get { return _ConnectionString; }
    ///         set { _ConnectionString = value; }
    ///     }
    ///     private string _ConnectionString = &quot;&quot;;
    /// 
    ///     public DebugModes DebugMode
    ///     {
    ///         get { return _DebugMode; }
    ///         set { _DebugMode = value; }
    ///     }
    ///     private DebugModes _DebugMode =
    ///                            DebugModes.ApplicationErrorMessage;
    /// 
    ///     public int MaxPageItems
    ///     {
    ///         get { return _MaxPageItems; }
    ///         set { _MaxPageItems = value; }
    ///     }
    ///     private int _MaxPageItems = 20;
    /// }&lt;&lt;/code&gt;&gt;
    /// 
    /// &lt;&lt;b&gt;&gt;Class Hookup as a static Property&lt;&lt;/b&gt;&gt;
    /// 
    /// &lt;&lt;code lang=&quot;C#&quot;&gt;&gt;public class App
    /// {
    ///     public static ApplicationConfiguration Configuration
    ///     {
    ///         get { return _Configuration; }
    ///         set { _Configuration = value; }
    ///     }
    ///     private static ApplicationConfiguration _Configuration;
    /// 
    ///     static App()
    ///     {
    ///         /// Load the general Application config properties from the Config 
    /// file
    ///         Configuration = new ApplicationConfiguration(null);
    /// 
    ///     }
    /// }
    /// &lt;&lt;/code&gt;&gt;
    /// 
    /// &lt;&lt;b&gt;&gt;Usage from anywhere in the Application&lt;&lt;/b&gt;&gt;
    /// 
    /// &lt;&lt;code lang=&quot;C#&quot;&gt;&gt;var title = 
    /// App.Configuration.ApplicationTitle;
    /// if (App.Configuration.DebugMode == DebugModes.Default)
    ///    throw new ApplicationException(&quot;Boo&quot;);
    /// &lt;&lt;/code&gt;&gt;
    /// </example>
    public abstract class AppConfiguration          
    {

        /// <summary>
        /// An instance of a IConfigurationProvider that
        /// needs to be passed in via constructor or set
        /// explicitly to read and write from the configuration
        /// store.
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        public IConfigurationProvider Provider = null;

        /// <summary>
        /// Contains an error message if a method returns false or the object fails to 
        /// load the configuration data.
        /// <seealso>Class AppConfiguration</seealso>
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        public string ErrorMessage = string.Empty;

        
        /// <summary>
        /// Default constructor of this class MUST be implemented in
        /// every subclass to allow serialization instantiation.        
        /// 
        /// Typically subclass implementations should leave this blank
        /// and implement the version that passes in a provider instance
        /// or null (default provider).
        /// </summary>         
        public AppConfiguration()
        {
        }

        /// <summary>
        /// This version of the constructor accepts a premade 
        /// instance of a provider - this is the recommended
        /// constructor to use so that the default constructor 
        /// creates a clean instance.
        /// 
        /// Pass null and create a default provider implementation
        /// here - typically ConfigFileConfigurationProvider.
        /// </summary>
        /// <param name="provider"></param>
        public AppConfiguration(IConfigurationProvider provider)
        {
        }

        /// <summary>
        /// This version of the constructor doesn't do any default
        /// configuration.
        /// </summary>
        /// <param name="noConfiguration"></param>
        public AppConfiguration(bool noConfiguration)
        {
        }


        /// <summary>
        /// Writes the current configuration information data to the
        /// provider's configuration store.
        /// </summary>
        /// <returns></returns>
        public virtual bool Write()
        {
            if (!Provider.Write(this))
            {
                ErrorMessage = Provider.ErrorMessage;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Writes the current configuration information to an
        /// XML string. String is in .NET XML Serialization format.
        /// </summary>
        /// <returns></returns>
        public virtual string WriteAsString()
        {
            string xml = string.Empty;
            Provider.EncryptFields(this);

            SerializationUtils.SerializeObject(this, out xml);

            if (!string.IsNullOrEmpty(xml))
                Provider.DecryptFields(this);

            return xml;
        }

        /// <summary>
        /// Reads the configuration information from the 
        /// provider's store and returns a new instance
        /// of an configuration object.
        /// </summary>
        /// <typeparam name="T">This configuration class type</typeparam>
        /// <returns></returns>
        public virtual T Read<T>()
                where T : AppConfiguration, new()
        {
            var inst = Provider.Read<T>();
            if (inst == null)
            {
                ErrorMessage = Provider.ErrorMessage;
                return null;
            }

            return inst;
        }

        /// <summary>
        /// Reads the configuration from the provider's store
        /// into the current object instance.
        /// </summary>
        /// <returns></returns>
        public virtual bool Read()
        {
            if (!Provider.Read(this))
            {
                ErrorMessage = Provider.ErrorMessage;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads configuration data from a string and populates the current
        /// instance with the values.
        /// 
        /// Data should be serialized in XML Searlization format created
        /// with <seealso cref="WriteAsString" />
        /// </summary>
        /// <param name="xml">Xml string in XML Serialization format</param>
        /// <returns>true or false</returns>
        public virtual bool Read(string xml)
        {
            var newInstance = SerializationUtils.DeSerializeObject(xml, GetType());

            DataUtils.CopyObjectData(newInstance, this, "Provider,Errormessage");

            if (newInstance != null)
            {
                Provider.DecryptFields(this);
            }

            return true;
        }

        /// <summary>
        /// Reads configuration based on a provider configuration
        /// and returns a new instance of the configuration object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider">A configured <seealso cref="IConfiguration" /> provider</param>
        /// <returns>instance of configuration or null on failure</returns>
        public static T Read<T>(IConfigurationProvider provider)
            where T : AppConfiguration, new()
        {            
            return provider.Read<T>() as T;            
        }

        /// <summary>
        /// Creates a new instance of the config object and retrieves
        /// configuration information from the provided string. String 
        /// should be in XML Serialization format or created by the 
        /// <seealso cref="WriteAsString"/> method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="provider">Required if encryption decryption is desired</param>
        /// <returns>config instance or null on failure</returns>
        public static T Read<T>(string xml, IConfigurationProvider provider)
            where T : AppConfiguration, new()
        {                        
            T result =  SerializationUtils.DeSerializeObject(xml, typeof(T)) as T;            

            if (result != null && provider != null)
                provider.DecryptFields(result);

            return result;
        }

        /// <summary>
        /// Creates a new instance of the config object and retrieves
        /// configuration information from the provided string. String 
        /// should be in XML Serialization format or created by the 
        /// <seealso cref="WriteAsString"/> method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns>config instance or null on failure</returns>
        public static T Read<T>(string xml)
            where T : AppConfiguration, new()
        {
            return Read<T>(xml, null);
        }

#if false
        #region  Obsolete method notifications

        [Obsolete("This method is no longer supported. Use a Provider Configuration instead.")]
        public virtual void SetConfigurationSection(string ConfigurationFileSection)            
        {
            throw new NotSupportedException(Resources.ConfigurationMethodNoLongerSupported);
        }
        [Obsolete("This method is no longer supported. Use a Provider Configuration instead.")]
        public virtual void SetEncryption(string EncryptFields, string EncryptKey)
        {
            throw new NotSupportedException(Resources.ConfigurationMethodNoLongerSupported);
        }
        [Obsolete("This method is no longer supported. Use a Provider Configuration instead.")]
        public virtual void ReadKeysFromConfig()
        {
            throw new NotSupportedException(Resources.ConfigurationMethodNoLongerSupported);
        }
        [Obsolete("This method is no longer supported. Use a Provider Configuration instead.")]
        public virtual void ReadKeysFromConfig(string Filename)
        {
            throw new NotSupportedException(Resources.ConfigurationMethodNoLongerSupported);
        }

        #endregion
#endif

    }

    /// <summary>
    /// Sample class for diagram display
    /// </summary>
    class MyAppConfiguration : AppConfiguration
    {
        public MyAppConfiguration()
        {
        }

        public MyAppConfiguration(IConfigurationProvider provider)
        { 
        }
        
        public string MyProperty { get; set; }
        public int MaxPageListItems { get; set; }
        public string ApplicationTitle { get; set; }
    }
}
