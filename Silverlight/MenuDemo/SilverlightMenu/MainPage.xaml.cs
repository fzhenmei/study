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
        public MainPage()
        {
            InitializeComponent();
            InitialValues.ContainingLayoutPanel = LayoutRoot;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
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
            var data = e.Result;
            Dispatcher.BeginInvoke(() =>
                                       {
                                           var topNodes = data.Where(n => n.ParentId == 0);
                                           var dropTagets = new List<DropTarget>();
                                           var dragSource = new List<DragSource>();

                                           foreach (var node in topNodes)
                                           {
                                               var target = new DropTarget
                                               {
                                                   Content = new DragSource() { Content = new HyperlinkButton { Content = node.NodeName}, DraggingEnabled = false},
                                                   Width = 200,
                                                   Height = 30,
                                               };
                                               target.DragSourceDropped += new DropEventHandler(target_DragSourceDropped);
                                               dropTagets.Add(target);

                                               var item = new AccordionItem { Header = target };

                                               var currentNode = node;
                                               var children = data.Where(d => d.ParentId == currentNode.Id).ToList();
                                               if (children.Count() > 0)
                                               {
                                                   var wrapper = new StackPanel();//this is a container,should have targets
                                                   foreach (var child in children)
                                                   {
                                                       var childTarget = new DropTarget()
                                                                             {
                                                                                 Ghost =
                                                                                     new HyperlinkButton() { Content = child.NodeName },
                                                                             };
                                                       var source = new DragSource()
                                                                        {
                                                                            Content = new HyperlinkButton() { Content = child.NodeName, Width = 100, Height = 50},
                                                                            //DragHandleMode = DragSource.DragHandleModeType.FullDragSource
                                                                            Tag = child.NodeName
                                                                        };
                                                       
                                                       dragSource.Add(source);
                                                       childTarget.Content = source;
                                                       dropTagets.Add(childTarget);

                                                       wrapper.Children.Add(childTarget);
                                                   }
                                                   item.Content = wrapper;
                                               }
                                               else
                                               {
                                                   item.Content = "没有菜单项";
                                               }

                                               LeftMenu.Items.Add(item);
                                           }

                                           for (int i = 0; i < dragSource.Count; i++)
                                           {
                                               dragSource[i].DropTargets = dropTagets;
                                           }
                                       });
            Loading.IsBusy = false;
            Loading.Visibility = System.Windows.Visibility.Collapsed;
        }

        void target_DragSourceDropped(object sender, DropEventArgs args)
        {
            MessageBox.Show(args.DragSource.Tag.ToString());
        }
    }
}