using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverlightMenu.MenuDemoService;

namespace SilverlightMenu
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var srv = new MenuDemoClient();
            srv.GetAllMenuNodesCompleted += new EventHandler<GetAllMenuNodesCompletedEventArgs>(srv_GetAllMenuNodesCompleted);
            srv.GetAllMenuNodesAsync();
        }

        void srv_GetAllMenuNodesCompleted(object sender, GetAllMenuNodesCompletedEventArgs e)
        {
            var data = e.Result;
            Dispatcher.BeginInvoke(() =>
                                       {
                                           var topNodes = data.Where(n => n.ParentId == 0);
                                           foreach (var node in topNodes)
                                           {
                                               var item = new AccordionItem { Header = node.NodeName };
                                               var currentNode = node;
                                               var child = data.Where(d => d.ParentId == currentNode.Id).FirstOrDefault();
                                               if (child != null)
                                               {
                                                   item.Content = child.NodeName;
                                               }
                                               LeftMenu.Items.Add(item);
                                           }
                                       });
        }
    }
}