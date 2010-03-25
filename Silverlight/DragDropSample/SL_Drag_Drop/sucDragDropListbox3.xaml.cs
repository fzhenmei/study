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
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace SL_Drag_Drop
{
    public partial class sucDragDropListbox3 : UserControl
    {

        public class ListContent : INotifyPropertyChanged
        {

            private string pItemText;
            public string ItemText {
                get
                {
                    return pItemText;
                }
                set
                {
                    pItemText = value;
                    NotifyPropertyChanged("ItemText");
                    
                }
            }
            public ImageSource ImageSrc { get; set; }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion
        }

        private ObservableCollection<ListContent> lstItems;
        private ListBox lstbxItems;

        public sucDragDropListbox3()
        {
            InitializeComponent();

            
            

        }

        private void DropTarget_DragSourceDropped(object sender, SL_Drag_Drop_BaseClasses.DropEventArgs args)
        {
            if (args.DragSource.Tag.ToString() == "FullBattery")
            {
                lstItems.Add(new ListContent() { ItemText = "Full"
                , ImageSrc = new BitmapImage(new Uri("Images/battery_full.png", UriKind.RelativeOrAbsolute))
                });
            }
            else if (args.DragSource.Tag.ToString() == "HalfBattery")
            {
                lstItems.Add(new ListContent() { ItemText = "Half"
                , ImageSrc = new BitmapImage(new Uri("Images/battery_half.png", UriKind.RelativeOrAbsolute))
                });
            }
            else if (args.DragSource.Tag.ToString() == "EmptyBattery")
            {
                lstItems.Add(new ListContent() { ItemText = "Empty"
                , ImageSrc = new BitmapImage(new Uri("Images/battery_empty.png", UriKind.RelativeOrAbsolute))
                });
            }

        }

        private void itemsList_Loaded(object sender, RoutedEventArgs e)
        {
            lstbxItems = (ListBox)sender;
            lstItems = new ObservableCollection<ListContent>();
            lstbxItems.ItemsSource = lstItems;
        }
    }
}
