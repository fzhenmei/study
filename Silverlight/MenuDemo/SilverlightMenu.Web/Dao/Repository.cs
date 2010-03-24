using System.Configuration;

namespace SilverlightMenu.Web.Dao
{
    public abstract class Repository
    {
        protected DatabaseDataContext Database;

        protected Repository()
        {
            Database = new DatabaseDataContext(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
        }
    }
}