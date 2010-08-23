/*
 **************************************************************
 * DbReourceManager Class
 **************************************************************
 *  Author: Rick Strahl 
 *          (c) West Wind Technologies
 *          http://www.west-wind.com/
 * 
 * Created: 10/10/2006
 * 
 * based in part on code provided in:
 * ----------------------------------
 * .NET Internationalization
 *      Addison Wesley Books
 *      by Guy Smith Ferrier
 * 
 **************************************************************  
*/

using System;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Reflection;

namespace Westwind.Globalization
{
    /// <summary>
    /// This class provides a databased implementation of a ResourceManager.
    /// 
    /// A ResourceManager holds each of the ResourceSets for a given group
    /// of resources. In ResX files a group is a file group wiht the same name
    /// (ie. Resources.resx, Resources.en.resx, Resources.de.resx). In this
    /// database driven provider the group is determined by the ResourceSet
    /// and the LocaleId as stored in the database. This class is instantiated
    /// and gets passed both of these values for identity.
    /// 
    /// An application can have many ResourceManagers - one for each localized
    /// page and one for each global resource with each hold multiple resourcesets
    /// for each of the locale's that are part of that resourceSet.
    /// 
    /// This class implements only the GetInternalResourceSet method to
    /// provide the ResourceSet from a database. It also implements all the
    /// base class constructors and captures only the BaseName which 
    /// is the name of the ResourceSet (ie. a global or local resource group)
    /// 
    /// Dependencies:
    /// DbResourceDataManager for data access
    /// DbResourceConfiguration which holds and reads config settings
    /// 
    /// DbResourceSet
    /// DbResourceReader
    /// </summary>
    public class DbResourceManager : ResourceManager
    {
        
        // Duplicate the Resource Manager Constructors below
        // Key feature of these overrides is to set up the BaseName
        // which is the name of the resource set (either a local
        // or global resource. Each ResourceManager controls one set
        // of resources (global or local) and manages the ResourceSet
        // for each of cultures that are part of that ResourceSet

        /// <summary>
        /// Critical Section lock used for loading/adding resource sets
        /// </summary>
        private static object SyncLock = new object();

        /// <summary> 
        /// Constructs a DbResourceManager object
        /// </summary>
        /// <param name="baseName">The qualified base name which the resources represent</param>
        public DbResourceManager(string baseName)
		{
			Initialize(baseName, null);
		}

        public override Type  ResourceSetType
        {
        	get 
        	{ 
        		 return typeof(DbResourceSet);
        	}
        }

        /// <summary>
        /// Constructs a DbResourceManager object. Match base constructors.
        /// </summary>
        /// <param name="resourceType">The Type for which resources should be read/written</param>
		public DbResourceManager(Type resourceType)
		{
			this.Initialize(resourceType.Name, resourceType.Assembly);
		}
        public DbResourceManager(string baseName, Assembly assembly)
        {
            this.Initialize( baseName,null);
        }
        public DbResourceManager(string baseName, Assembly assembly, Type usingResourceSet)
        {
            this.Initialize(baseName, null);
        }

        /// <summary>
        /// Core Configuration method that sets up the ResourceManager. For this 
        /// implementation we only need the baseName which is the ResourceSet id
        /// (ie. the local or global resource set name) and the assembly name is
        /// simply ignored.
        /// 
        /// This method essentially sets up the ResourceManager and holds all
        /// of the culture specific resource sets for a single ResourceSet. With
        /// ResX files each set is a file - in the database a ResourceSet is a group
        /// with the same ResourceSet Id.
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="assembly"></param>
        protected void Initialize(string baseName, Assembly assembly)
        {                     
            this.BaseNameField = baseName;

            // ResourceSets contains a set of resources for each locale
            this.ResourceSets = new Hashtable();            
        }
                
        

        /// <summary>
        /// This is the only method that needs to be overridden as long as we
        /// provide implementations for the ResourceSet/ResourceReader/ResourceWriter
        /// </summary>
        /// <param name="Culture"></param>
        /// <param name="createIfNotExists"></param>
        /// <param name="tryParents"></param>
        /// <returns></returns>
        protected override ResourceSet InternalGetResourceSet(CultureInfo Culture, bool createIfNotExists, bool tryParents)
        {             
            // retrieve cached instance - outside of lock for perf
            if (this.ResourceSets.Contains(Culture.Name))
                return this.ResourceSets[Culture.Name] as ResourceSet;

            lock(SyncLock)
            {
                // have to check again to ensure still not existing
                if (this.ResourceSets.Contains(Culture.Name))
                    return this.ResourceSets[Culture.Name] as ResourceSet;
            
                // Otherwise create a new instance, load it and return it
                DbResourceSet rs = new DbResourceSet(this.BaseNameField, Culture);
                
                // Add the resource set to the cached set
                this.ResourceSets.Add(Culture.Name, rs);
                
                // And return an instance
                return rs;
            }            
        }

        public override void ReleaseAllResources()
        {
            base.ReleaseAllResources();
            this.ResourceSets.Clear();
        }

#if true
        // GetObject implementations to retrieve values - not required but useful to see operation
        /// <summary>
        /// Core worker method on the manager that returns resource. This
        /// override returns the resource for the currently active UICulture
        /// for this manager/resource set.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetObject(string name)
        {
            object Value = base.GetObject(name);
            return Value;
        }

        /// <summary>
        /// Core worker method that returnsa  resource value for a
        /// given culture from the this resourcemanager/resourceset.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object GetObject(string name, CultureInfo culture)
        {
            object Value = base.GetObject(name, culture);
            return Value;
        }
#endif
    } 

}
