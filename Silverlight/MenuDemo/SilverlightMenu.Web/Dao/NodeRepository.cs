using System.Linq;

namespace SilverlightMenu.Web.Dao
{
    public class NodeRepository : Repository
    {
        public IQueryable<Nodes> FindAll()
        {
            return Database.Nodes;
        }
    }
}
