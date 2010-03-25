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
using SL_Drag_Drop_BaseClasses;

namespace SL_Drag_Drop
{
    public partial class sucDragDrop3 : UserControl
    {
        public sucDragDrop3()
        {
            InitializeComponent();

            InitControl();
        }

        private void InitControl()
        {
            // add droptargets to wrappanel

            // droptarget 1 has no visible ghost
            // if that is the case, we must add a width & height.  Default = auto, 
            // so if we leave it at that it will have 0 width/height
            DropTarget dropTarget1 = new DropTarget()
            {
                Ghost = new DropTargetGhost(),
                GhostVisibility = Visibility.Collapsed,
                Width = 100,
                Height = 100
            };
            DropTarget dropTarget2 = new DropTarget() { Ghost = new DropTargetGhost()
            , Width = 100, Height = 100};
            DropTarget dropTarget3 = new DropTarget() { Ghost = new DropTargetGhost()
            , Width = 100, Height = 100};

            PanelDropTargets.Children.Add(dropTarget1);
            PanelDropTargets.Children.Add(dropTarget2);
            PanelDropTargets.Children.Add(dropTarget3);

            // create list of droptargets to pass to the dragsources

            List<DropTarget> dropTargets = new List<DropTarget>() { dropTarget1, dropTarget2, dropTarget3 };


            // add dragsources to wrappanel

            DragSource dragSource1 = new DragSource()
            {
                Content = new DragSourceContent() { DataContext = new Dummy() { DummyText = "1" } },
                Ghost = new DragSourceGhost(),
                DropTargets = dropTargets
            };

            DragSource dragSource2 = new DragSource()
            {
                Content = new DragSourceContent() { DataContext = new Dummy() { DummyText = "2" } },
                //Ghost = new DragSourceGhost(),
                DropTargets = dropTargets
            };


            List<DropTarget> dropTargetsThird = new List<DropTarget>() { dropTarget1, dropTarget2 };

            // dragsource 3 cannot be dropped in droptarget 3
            // dragsource 3 has no visible ghost.  We can set width/height if needed, but in this case,
            // it will take the widht/height of the Content (= DragSource)
            DragSource dragSource3 = new DragSource()
            {
                Content = new DragSourceContent() { DataContext = new Dummy() { DummyText = "3" } },
                Ghost = new DragSourceGhost(),
                GhostVisibility = Visibility.Collapsed,
                DropTargets = dropTargetsThird
            };


            // add dragsources to wrappanel
            PanelDragSources.Children.Add(dragSource1);
            PanelDragSources.Children.Add(dragSource2);
            PanelDragSources.Children.Add(dragSource3);
        }
    }
}
