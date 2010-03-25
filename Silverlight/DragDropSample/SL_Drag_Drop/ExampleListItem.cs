using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SL_Drag_Drop
{
    public enum UC
    {
        Home,
        DefaultBehaviour,
        FirstBehaviour,
        SecondBehaviour,
        ThirdBehaviour,
        FourthBehaviour,
        AnyDropTarget,
        DragDropListBox,
        DragDropListBox2,
        DragDropListBox3,
        DragDropCart,
        ReturnAnimations
    }

    public class ExampleListItem
    {
        public string Description { get; set; }
        public UC UCType { get; set; }

    }
}
