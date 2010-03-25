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
    public partial class sucDragDrop1 : UserControl
    {
        public sucDragDrop1()
        {
            InitializeComponent();

            InitControl();
        }

        private void InitControl()
        {
            // add droptargets to wrappanel

            DropTarget dropTarget1 = new DropTarget() { Ghost = new DropTargetGhost() 
            , Width = 100, Height = 100};
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
               
                DropTargets = dropTargets
            };

            DragSource dragSource2 = new DragSource()
            {
                Content = new DragSourceContent() { DataContext = new Dummy() { DummyText = "2" } },
               
                DropTargets = dropTargets
            };

            DragSource dragSource3 = new DragSource()
            {
                Content = new DragSourceContent() { DataContext = new Dummy() { DummyText = "3" } },
                
                DropTargets = dropTargets
            };

            // add dragsources to wrappanel
            PanelDragSources.Children.Add(dragSource1);
            PanelDragSources.Children.Add(dragSource2);
            PanelDragSources.Children.Add(dragSource3);
        }

       
    }
}
