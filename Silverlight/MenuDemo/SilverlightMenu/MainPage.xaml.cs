using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SilverlightMenu.MenuDemoService;
using SL_Drag_Drop_BaseClasses;

namespace SilverlightMenu
{
    public partial class MainPage : UserControl
    {
        public System.Collections.ObjectModel.ObservableCollection<MenuDemoService.MenuData> MenuDataList;

        public MainPage()
        {
            InitializeComponent();
            InitialValues.ContainingLayoutPanel = LayoutRoot;
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loading.IsBusy = true;
            var srv = new MenuDemoClient();
            srv.GetAllMenuNodesCompleted += new EventHandler<GetAllMenuNodesCompletedEventArgs>(srv_GetAllMenuNodesCompleted);
            srv.GetAllMenuNodesAsync();
        }

        void srv_GetAllMenuNodesCompleted(object sender, GetAllMenuNodesCompletedEventArgs e)
        {
            MenuDataList = e.Result;
            Dispatcher.BeginInvoke(BindMenuItem);
            Loading.IsBusy = false;
            Loading.Visibility = Visibility.Collapsed;
        }

        private void BindMenuItem()
        {
            LeftMenu.Items.Clear();

            var dropTagets = new List<DropTarget>();

            foreach (var node in MenuDataList)
            {
                AccordionItem item;
                if (node.IsTop)
                {
                    //only Top node can accept drag drop
                    var target = new DropTarget
                                     {
                                         Ghost = new MenuGroupHeaderContent() { DataContext = node}
                                     };
                    target.DragSourceDropped += target_DragSourceDropped;
                    dropTagets.Add(target);
                    item = new AccordionItem { Header = target };
                }
                else
                {
                    item = new AccordionItem()
                               {
                                   Header = 
                                       new MenuGroupHeaderContent() {DataContext = node}
                               };
                }

                item.Content = new MenuItemContent() { DataSource = node.MenuItems};
                LeftMenu.Items.Add(item);
            }
        }

        void target_DragSourceDropped(object sender, DropEventArgs args)
        {
            var id = (int) args.DragSource.Tag;
            var target = MenuDataList.Where(m => m.IsTop).Single();
            foreach (var data in MenuDataList)
            {
                if (data.IsTop)
                {
                    continue;
                }

                var obj = data.MenuItems.Where(m => m.Id == id).SingleOrDefault();
                if (obj != null)
                {
                    data.MenuItems.Remove(obj);
                    target.MenuItems.Add(obj);
                    BindMenuItem();
                    break;
                }
            }
        }
    }
}