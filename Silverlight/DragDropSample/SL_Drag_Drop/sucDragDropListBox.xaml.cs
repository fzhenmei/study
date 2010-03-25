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
using System.Collections.ObjectModel;
using SL_Drag_Drop_BaseClasses;

namespace SL_Drag_Drop
{
    public partial class sucDragDropListBox : UserControl
    {
        // TODO: draggen tussen 2 listboxen (mannekes/vrouwkes)
        // draggen van een lijst naar een listbox (hele listbox als geldig droptarget nemen?)
        // (bvb: een paar zaken die je mee op vakantie wil nemen ofzo, de te selecteren zaken blijven staan,
        // per drop komt er eentje bij in de lijst of wordt de lijst met 1 verhoogd), of todo list: send mail, 
        // phone someone, ...  of batteries: 3 green batteries added, 4 blue ones etc

        private ObservableCollection<ListContent> leftItems;
        private ObservableCollection<ListContent> rightItems;

        public class ListContent
        {
            public string Dummy { get; set; }
            public GradientBrush Background { get; set; }
        }
        

        public sucDragDropListBox()
        {
            InitializeComponent();

            FillListsWithDummyData();

            leftList.ItemsSource = leftItems;
            rightList.ItemsSource = rightItems;
        }

        private GradientBrush getGradient(bool male)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0.5, 0);
            brush.EndPoint = new Point(0.5, 1);

            if (male)
            {
                brush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, 255,255,255), Offset = 0 });
                brush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, 125, 197, 246), Offset = 1 });
            }
            else
            {
                brush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, 255, 255, 255), Offset = 0 });
                brush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, 201, 201, 201), Offset = 1 });
            }

            return brush;
        }

        private void FillListsWithDummyData()
        {
            leftItems = new ObservableCollection<ListContent>()
            {
                new ListContent() { Dummy="Kevin", Background = getGradient(true) }
                , new ListContent() { Dummy="Tom", Background = getGradient(true) }
                , new ListContent() { Dummy="Bob", Background = getGradient(true) }
                , new ListContent() { Dummy="Kenneth", Background = getGradient(true) }
            };


            rightItems = new ObservableCollection<ListContent>()
            {
                new ListContent() { Dummy="Ann", Background = getGradient(false)}
                , new ListContent() { Dummy="Katie", Background = getGradient(false)}
                , new ListContent() { Dummy="Eve", Background = getGradient(false)}
                , new ListContent() { Dummy="Caroline", Background = getGradient(false)}
            };

        }

        private void DropTarget_DragSourceDropped(object sender, SL_Drag_Drop_BaseClasses.DropEventArgs args)
        {
            // switch items in list

            // dropped content
            ListContent droppedContent = (ListContent)args.DragSource.DataContext;

            // dragsource that should be replaced
            ListContent toBeReplacedContent = (ListContent)(((DropTarget)sender).DataContext);
            
            // find the items
            ObservableCollection<ListContent> sourceList;
            ObservableCollection<ListContent> targetList;

            if (leftItems.IndexOf(droppedContent) > -1)
            {
                sourceList = leftItems;
            }
            else
            {
                sourceList = rightItems;
            }

            if (leftItems.IndexOf(toBeReplacedContent) > -1)
            {
                targetList = leftItems;
            }
            else
            {
                targetList = rightItems;
            }

            
            int sourceIndex = sourceList.IndexOf(droppedContent);
            int targetIndex = targetList.IndexOf(toBeReplacedContent);

            // replace items

            if (sourceList != targetList)
            {
                sourceList.RemoveAt(sourceIndex);
                targetList.RemoveAt(targetIndex);
                sourceList.Insert(sourceIndex, toBeReplacedContent);
                targetList.Insert(targetIndex, droppedContent);
            }
            else
            {
                if (sourceIndex < targetIndex)
                {
                    sourceList.RemoveAt(targetIndex);
                    sourceList.Insert(sourceIndex, toBeReplacedContent);
                    sourceList.RemoveAt(sourceIndex +1);
                    sourceList.Insert(targetIndex, droppedContent);
                }
                else if (sourceIndex > targetIndex)
                {
                    sourceList.RemoveAt(sourceIndex);
                    sourceList.Insert(targetIndex, droppedContent);
                    sourceList.RemoveAt(targetIndex +1);
                    sourceList.Insert(sourceIndex, toBeReplacedContent);
                }
            }
        }

    }
}
