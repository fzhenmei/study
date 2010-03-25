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
using System.Windows.Media.Imaging;

namespace SL_Drag_Drop
{
    public partial class sucDragDropListbox2 : UserControl
    {
        private ObservableCollection<ListContent> itemList;

        public class ListContent
        {
            public string Dummy { get; set; }
            public ImageSource ImageSrc { get; set; }
        }
        
        public sucDragDropListbox2()
        {
            InitializeComponent();

            FillListsWithDummyData();

            lstbxItems.ItemsSource = itemList;
        }

        private GradientBrush getGradient()
        {
            LinearGradientBrush brush = new LinearGradientBrush();

            brush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, 109, 196, 229), Offset = 0 });
            brush.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(35, 201, 201, 201), Offset = 1 });

            return brush;
        }

        private void FillListsWithDummyData()
        {
            itemList = new ObservableCollection<ListContent>()
            {
                new ListContent() { Dummy="Camera", ImageSrc =  new BitmapImage(new Uri("Images/camera.png", UriKind.RelativeOrAbsolute)) }
                , new ListContent() { Dummy="Key", ImageSrc =  new BitmapImage(new Uri("Images/key.png", UriKind.RelativeOrAbsolute)) }
                , new ListContent() { Dummy="Globe", ImageSrc =  new BitmapImage(new Uri("Images/globe.png", UriKind.RelativeOrAbsolute))}
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

            int sourceIndex = itemList.IndexOf(droppedContent);
            int targetIndex = itemList.IndexOf(toBeReplacedContent);

            // replace items

         
                if (sourceIndex < targetIndex)
                {
                    itemList.RemoveAt(targetIndex);
                    itemList.Insert(sourceIndex, toBeReplacedContent);
                    itemList.RemoveAt(sourceIndex + 1);
                    itemList.Insert(targetIndex, droppedContent);
                }
                else if (sourceIndex > targetIndex)
                {
                    itemList.RemoveAt(sourceIndex);
                    itemList.Insert(targetIndex, droppedContent);
                    itemList.RemoveAt(targetIndex + 1);
                    itemList.Insert(sourceIndex, toBeReplacedContent);
                }

        }

    }
}
