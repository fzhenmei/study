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

namespace SL_Drag_Drop
{
    public partial class sucDragDropAny : UserControl
    {

        public ObservableCollection<Dummy> collToBind { get; set; }


        public sucDragDropAny()
        {
            InitializeComponent();

            collToBind = new ObservableCollection<Dummy>();

            for (int i = 0; i < 250; i++)
            {
                collToBind.Add(new Dummy() { DummyText = i.ToString() });
            }

            icTargets.ItemsSource = collToBind;
        }
    }
}
