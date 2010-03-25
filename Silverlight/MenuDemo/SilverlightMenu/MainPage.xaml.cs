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
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loading.IsBusy = true;
            //var srv = new MenuDemoClient();
            //srv.GetAllMenuNodesCompleted += new EventHandler<GetAllMenuNodesCompletedEventArgs>(srv_GetAllMenuNodesCompleted);
            //srv.GetAllMenuNodesAsync();
            //init drop & drag
            InitDragDrop();
        }

        private void InitDragDrop()
        {
            var target1 = new DropTarget() { Width = 100, Height = 20 };
            var target2 = new DropTarget() { Width = 100, Height = 20 };
            PanelDropTargets.Children.Add(target1);
            PanelDropTargets.Children.Add(target2);
            var targets = new List<DropTarget>() { target1, target2 };
            var dragSouce1 = new DragSource() { Content = new HyperlinkButton() { Content = "Test Source 1" }, DropTargets = targets, Ghost = new HyperlinkButton()};
            var dragSouce2 = new DragSource() { Content = new HyperlinkButton() { Content = "Test Source 2" }, DropTargets = targets, Ghost = new HyperlinkButton()};
            PanelDropSources.Children.Add(dragSouce1);
            PanelDropSources.Children.Add(dragSouce2);
        }

        void srv_GetAllMenuNodesCompleted(object sender, GetAllMenuNodesCompletedEventArgs e)
        {
            var data = e.Result;
            Dispatcher.BeginInvoke(() =>
                                       {
                                           var topNodes = data.Where(n => n.ParentId == 0);
                                           var dropTagets = new List<DropTarget>();
                                           foreach (var node in topNodes)
                                           {
                                               //todo: add dragtarget for header...
                                               var target = new DropTarget
                                                                       {
                                                                           Width = 100,
                                                                           Height = 30,
                                                                       };
                                               var source = new DragSource() { Content = new HyperlinkButton() { Content = node.NodeName } };
                                               source.DropTargets.Add(target);
                                               target.Content = source;

                                               target.DragSourceDropped += new DropEventHandler(target_DragSourceDropped);
                                               var item = new AccordionItem { Header = target };

                                               var currentNode = node;
                                               var children = data.Where(d => d.ParentId == currentNode.Id).ToList();
                                               if (children.Count() > 0)
                                               {
                                                   //todo: add dragtarget for content
                                                   //todo: add dragsource for content
                                                   var wrapper = new StackPanel();
                                                   foreach (var child in children)
                                                   {
                                                       wrapper.Children.Add(new HyperlinkButton() { Content = child.NodeName });
                                                   }
                                                   item.Content = wrapper;
                                               }
                                               else
                                               {
                                                   item.Content = "没有菜单项";
                                               }

                                               LeftMenu.Items.Add(item);
                                           }
                                       });
            Loading.IsBusy = false;
            Loading.Visibility = System.Windows.Visibility.Collapsed;
        }

        void target_DragSourceDropped(object sender, DropEventArgs args)
        {
            //throw new NotImplementedException();
        }
    }
}