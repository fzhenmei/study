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

namespace SL_Drag_Drop
{
    public partial class sucDragDropCart : UserControl
    {

        public sucDragDropCart()
        {
            InitializeComponent();

        }

        private void DropTarget_DragSourceDropped(object sender, SL_Drag_Drop_BaseClasses.DropEventArgs args)
        {
            if (args.DragSource.Tag.ToString() == "Camera")
            {
                lblContent.Text += "You've added a camera to your luggage. \n";
            }
            else if (args.DragSource.Tag.ToString() == "Coffee")
            {
                lblContent.Text += "You've added a cup of coffee to your luggage (don't spill!). \n";
            }
            else if (args.DragSource.Tag.ToString() == "News")
            {
                lblContent.Text += "You've added today's newspaper to your luggage. \n";
            }
        }
    }
}
