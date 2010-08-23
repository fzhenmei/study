using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Westwind.BusinessFramework
{
    /// <summary>
    /// A base class that can be used as a base an EntityBase
    /// class which can be specified explicitly using the 
    /// EntityBase="Westwind.BusinessFramework.LinqToSql.EntityBase" 
    /// on the Database key of the DMBL file.
    /// 
    /// Currently not used for anything but potentially add
    /// functionality in the future.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public partial class EntityBase
    {
        [NonSerialized]
        [XmlIgnore]
        public EntityState EntityState =  new EntityState();        
    }

    /// <summary>
    /// Class that maintains some entity related settings
    /// on an EntityBase instance
    /// </summary>
    public partial class EntityState
    {
        /// <summary>
        /// Set to true when an entity is created with NewEntity
        /// and unset when Save() is successfully called on it
        /// 
        /// Note affects only entities created with NewEntity
        /// </summary>
        public bool IsNew { get; set; }
    }
}
