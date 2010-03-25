/*
 * Kevin Dockx, 02/2009, update: 04/2009
 * 
 * Sample implementations of the drag drop manager classes.  All controls 
 * that are created in codebehind can just as easily be created in XAML if you wish.
 * 
 * Project@CodePlex: http://www.codeplex.com/silverlightdragdrop/
 * Author site: http://kevindockx.blogspot.com/

 */



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

    public partial class Page : UserControl
    {

        public List<ExampleListItem> lstItems { get; set; }

        public Page()
        {
            InitializeComponent();

            SL_Drag_Drop_BaseClasses.InitialValues.ContainingLayoutPanel = this.LayoutRoot;

            lstItems = new List<ExampleListItem>();

            lstItems.Add(new ExampleListItem() { Description = "Home", UCType = UC.Home });
            lstItems.Add(new ExampleListItem() { Description = "Default behaviour", UCType=UC.DefaultBehaviour});
            lstItems.Add(new ExampleListItem() { Description = "Luggage organiser", UCType = UC.DragDropCart });
            lstItems.Add(new ExampleListItem() { Description = "ItemsControl", UCType = UC.DragDropListBox2 });
            lstItems.Add(new ExampleListItem() { Description = "ListBox: add to", UCType = UC.DragDropListBox3 });
            lstItems.Add(new ExampleListItem() { Description = "ListBox: in and between", UCType = UC.DragDropListBox });
            lstItems.Add(new ExampleListItem() { Description = "Return animations", UCType = UC.ReturnAnimations });
            lstItems.Add(new ExampleListItem() { Description = "Custom behaviour", UCType = UC.FirstBehaviour });
            lstItems.Add(new ExampleListItem() { Description = "Custom behaviour", UCType = UC.SecondBehaviour });
            lstItems.Add(new ExampleListItem() { Description = "Custom behaviour", UCType = UC.ThirdBehaviour });
            lstItems.Add(new ExampleListItem() { Description = "Custom behaviour", UCType = UC.FourthBehaviour });
            
            

            icItemMenu.ItemsSource = lstItems;

            brdWrapper.Child = new Home() ;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            switch (((ExampleListItem)((Button)sender).DataContext).UCType)
            {
                case UC.Home:
                    brdWrapper.Child = new Home();
                    break;
                case UC.DefaultBehaviour:
                    brdWrapper.Child = new sucDragDrop0();
                    break;
                case UC.FirstBehaviour:
                    brdWrapper.Child = new sucDragDrop1();
                    break;
                case UC.SecondBehaviour:
                    brdWrapper.Child = new sucDragDrop2();
                    break;
                case UC.ThirdBehaviour:
                    brdWrapper.Child = new sucDragDrop3();
                    break;
                case UC.FourthBehaviour:
                    brdWrapper.Child = new sucDragDrop4();
                    break;
                case UC.AnyDropTarget:
                    brdWrapper.Child = new sucDragDropAny();
                    break;
                case UC.DragDropListBox:
                    brdWrapper.Child = new sucDragDropListBox();
                    break;
                case UC.DragDropListBox2:
                    brdWrapper.Child = new sucDragDropListbox2();
                    break;
                case UC.DragDropListBox3:
                    brdWrapper.Child = new sucDragDropListbox3();
                    break;
                case UC.DragDropCart:
                    brdWrapper.Child = new sucDragDropCart();
                    break;
                case UC.ReturnAnimations:
                    brdWrapper.Child = new sucDragDropAnimation();
                    break;
                default:
                    break;
            }
        }



        //private void btnExample1_Click(object sender, RoutedEventArgs e)
        //{
        //    grdWrapper.Children.Clear();
        //    grdWrapper.Children.Add(new sucDragDrop1());
        //}

        //private void btnExample2_Click(object sender, RoutedEventArgs e)
        //{
        //    grdWrapper.Children.Clear();
        //    grdWrapper.Children.Add(new sucDragDrop2());
        //}

        //private void btnExample0_Click(object sender, RoutedEventArgs e)
        //{
        //    grdWrapper.Children.Clear();
        //    grdWrapper.Children.Add(new sucDragDrop0());
        //}

        //private void btnExample3_Click(object sender, RoutedEventArgs e)
        //{
        //    grdWrapper.Children.Clear();
        //    grdWrapper.Children.Add(new sucDragDrop3());
        //}

        //private void btnExample4_Click(object sender, RoutedEventArgs e)
        //{
        //    grdWrapper.Children.Clear();
        //    grdWrapper.Children.Add(new sucDragDrop4());
        //}

        //private void btnExample5_Click(object sender, RoutedEventArgs e)
        //{
        //    grdWrapper.Children.Clear();
        //    grdWrapper.Children.Add(new sucDragDropListBox());
        //}





    }
}
