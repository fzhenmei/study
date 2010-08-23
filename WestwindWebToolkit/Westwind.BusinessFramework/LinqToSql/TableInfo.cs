using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Westwind.BusinessFramework.LinqToSql
{
    /// <summary>
    /// Field structure for holding table information.
    /// A Table Info object is specific for a mapped entity
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// Default constructor - no assignments of any sort are applied
        /// </summary>
        public TableInfo() { }

        /// <summary>
        /// Initializes the TableInfo with information
        /// from a provided context
        /// </summary>
        /// <param name="context"></param>
        public TableInfo(DataContext context, Type entityType)
        {
            // Retrieve the name of the mapped table from the schema            
            MetaTable metaTable = context.Mapping.GetTable(entityType);

            Tablename = metaTable.TableName;


            if (metaTable.RowType.IdentityMembers.Count < 0)
                throw new ApplicationException(Tablename + " doesn't have a primary key. Not supported for a business object mapping table.");

            PkField = metaTable.RowType.IdentityMembers[0].Name;
            PkFieldType = metaTable.RowType.IdentityMembers[0].Type;

            if (metaTable.RowType.VersionMember == null)
            { }
            //    throw new InvalidOperationException(Tablename + " doesn't have a version field. Business object tables mapped require a version field.");
            else
                VersionField = metaTable.RowType.VersionMember.Name;
        }

        /// <summary>
        /// The name of the table that is mapped by the main Entity associated
        /// with this business object.
        /// </summary>
        public string Tablename;

        /// <summary>
        /// The version field used by this table. Version fields are required
        /// </summary>
        public string VersionField;

        /// <summary>
        /// The primary key id field used by this table.
        /// </summary>
        public string PkField;

        /// <summary>
        /// The type of the PK field.
        /// </summary>
        public Type PkFieldType;
    }
}
