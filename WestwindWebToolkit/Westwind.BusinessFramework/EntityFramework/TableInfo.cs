using System;
using System.Data.Metadata.Edm;

namespace Westwind.BusinessFramework.EntityFramework
{
    /// <summary>
    /// Field structure for holding table information.
    /// A Table Info object is specific for a mapped entity
    /// </summary>
    public class TableInfo<TEntity> 
        where TEntity : class, new()
    {
        /// <summary>
        /// Default constructor - no assignments of any sort are applied.
        /// Use this constructor if you want to manually assign and override the
        /// values of the TableInfo structure
        /// </summary>
        public TableInfo() {}

        /// <summary>
        /// Initializes the TableInfo with information
        /// from a provided context
        /// </summary>
        /// <param name="context">instance of the context</param>
        /// <param name="entityType">The type of the entity</param>
        public TableInfo(ObjectContextSql context)
        {
            Type entityType = typeof (TEntity);

            EntityTablename = context.DefaultContainerName + "." + entityType.Name;
            DbTableName = entityType.Name;
            
            EntityType type = context.MetadataWorkspace.GetItem<EntityType>(typeof(TEntity).FullName, DataSpace.OSpace);
            PkField = type.KeyMembers[0].Name;            
        }

        /// <summary>
        /// The name of the table that is mapped by the main Entity associated
        /// with this business object. 
        /// 
        /// This will be the containername + tablename (WebStoreEntities.wws_Lookup)
        /// </summary>
        public string EntityTablename;

        /// <summary>
        /// The raw table name without container prefix.
        /// </summary>
        public string DbTableName;

        /// <summary>
        /// The version field used by this table. Version fields are required
        /// </summary>
        //public string VersionField;

        /// <summary>
        /// The primary key id field used by this table.
        /// </summary>
        public string PkField = "Id";

        /// <summary>
        /// The type of the PK field.
        /// </summary>
        //public Type PkFieldType;
    }
}