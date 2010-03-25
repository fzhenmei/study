using System.Collections.Generic;
using System.Windows.Controls;
using SL_Drag_Drop_BaseClasses;

namespace DragDropLearn
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {
            //Important
            //see:http://silverlightdragdrop.codeplex.com/Thread/View.aspx?ThreadId=72180
            SL_Drag_Drop_BaseClasses.InitialValues.ContainingLayoutPanel = this.LayoutRoot;

            var target1 = new DropTarget() { Ghost = new DropTargetContentGhost(), Width = 100, Height = 50 };
            var target2 = new DropTarget() { Ghost = new DropTargetContentGhost(), Width = 100, Height = 50 };
            var target3 = new DropTarget() { Ghost = new DropTargetContentGhost(), Width = 100, Height = 50 };
            var target4 = new DropTarget() { Ghost = new DropTargetContentGhost(), Width = 100, Height = 50 };
            var target5 = new DropTarget() { Ghost = new DropTargetContentGhost(), Width = 100, Height = 50 };
            var target6 = new DropTarget() { Ghost = new DropTargetContentGhost(), Width = 100, Height = 50 };

            PanelDropTargets.Children.Add(target1);
            PanelDropTargets.Children.Add(target2);
            PanelDropTargets.Children.Add(target3);

            PanelDragSources.Children.Add(target4);
            PanelDragSources.Children.Add(target5);
            PanelDragSources.Children.Add(target6);

            var targets = new List<DropTarget> { target1, target2, target3, target4, target5, target6 };

            //create objects you want to drag...
            var dragSource1 = new DragSource
                                  {
                                      Content = new DragSourceContent { DataContext = new TextLabel { LabelText = "label1" } }, 
                                      DropTargets = targets,
                                      Ghost = new DragSourceContentGhost(),
                                      DragHandleMode = DragSource.DragHandleModeType.FullDragSource,
                                  };
            var dragSource2 = new DragSource
                                  {
                                      Content = new DragSourceContent { DataContext = new TextLabel { LabelText = "label2" } }, 
                                      DropTargets = targets,
                                      Ghost = new DragSourceContentGhost(),
                                      DragHandleMode = DragSource.DragHandleModeType.FullDragSource
                                  };
            var dragSource3 = new DragSource
                                  {
                                      Content = new DragSourceContent { DataContext = new TextLabel { LabelText = "label3" } },
                                      DropTargets = targets,
                                      Ghost = new DragSourceContentGhost(),
                                      DragHandleMode = DragSource.DragHandleModeType.FullDragSource
                                  };

            target1.Content = dragSource1;
            target2.Content = dragSource2;
            target3.Content = dragSource3;
        }
    }
}