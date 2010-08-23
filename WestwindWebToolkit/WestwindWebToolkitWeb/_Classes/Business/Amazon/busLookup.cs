using System.Collections.Generic;
using System.Linq;
using Westwind.BusinessFramework.LinqToSql;
using System.Data.SqlClient;

namespace Amazon
{

    /// <summary>
    /// Weblog Entry business object
    /// 
    /// EntryType = 1
    /// </summary>
    public class busLookup : BusinessObjectLinq<AmazonLookupList,AmazonContext>
    {

        /// <summary>
        /// Returns all books
        /// </summary>
        public List<string> GetCategories()
        {
            return this.Context.AmazonLookupLists
                    .Where(lookup => lookup.Type == "BOOKCATEGORY")
                    .OrderBy(lookup => lookup.cData)
                    .Select(lookup => lookup.cData)
                    .ToList();                    
        }

        public void AddCategory(string category)
        {
            this.NewEntity();
            this.Entity.cData = category;
            this.Entity.Type = "BOOKCATEGORY";
            this.Save();
        }
        public bool DeleteCategory(string Category)
        {
            int count = this.ExecuteNonQuery("delete from " + this.TableInfo.Tablename +
                                 " where type='BOOKCATEGORY' and cData==@Category",
                                 new SqlParameter("@Category", Category));

            return (count > -1);
        }
    }
}
