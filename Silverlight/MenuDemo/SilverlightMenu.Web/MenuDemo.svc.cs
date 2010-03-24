using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using SilverlightMenu.Web.Dao;

namespace SilverlightMenu.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MenuDemo
    {
        [OperationContract]
        public List<Nodes> GetAllMenuNodes()
        {
            return new NodeRepository().FindAll().ToList();
        }
    }
}
