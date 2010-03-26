using System;
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
        public List<MenuData> GetAllMenuNodes()
        {
            var source = new NodeRepository().FindAll();
            var groupRootData = source.Where(s => s.ParentId == 0);
            var menuItemsData = source.Where(s => s.ParentId > 0);
            var menuData = new List<MenuData>();
            foreach (var root in groupRootData)
            {
                menuData.Add(new MenuData()
                                 {
                                     Id = root.Id,
                                     GroupName = root.NodeName,
                                     MenuItems = GetMenuItems(root.Id, menuItemsData),
                                     IsTop = root.Id == 1 ? true : false
                                 });
            }

            return menuData;
        }

        private List<MenuItem> GetMenuItems(int id, IQueryable<Nodes> menuItemsData)
        {
            var menuItems = new List<MenuItem>();
            var query = menuItemsData.Where(m => m.ParentId == id);
            foreach (var node in query)
            {
                menuItems.Add(new MenuItem()
                                  {
                                      Id = node.Id,
                                      Caption = node.NodeName,
                                      Url = "http://www.163.com"
                                  });
            }

            return menuItems;
        }
    }
}
