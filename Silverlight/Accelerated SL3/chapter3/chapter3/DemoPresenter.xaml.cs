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
using System.Windows.Navigation;

namespace chapter3
{
    public partial class DemoPresenter : Page
    {
        private Dictionary<string, string> demos = new Dictionary<string, string>();

        public DemoPresenter()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DemoPresenter_Loaded);
        }

        void DemoPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            addDemo("Canvas", "/CanvasDemo.xaml");
            addDemo("StackPanel", "/StackPanelDemo.xaml");
            addDemo("Simple Grid", "/GridDemo.xaml");
            addDemo("Star Sized Grid", "/StarSizingDemo.xaml");
            addDemo("GridSplitter", "/GridSplitterDemo.xaml");
            addDemo("Buttons", "/ButtonsDemo.xaml");
            addDemo("TextBlock", "/TextBlockDemo.xaml");
            addDemo("TextBox", "/TextBoxDemo.xaml");
            addDemo("ListBox", "/ListBoxDemo.xaml");
            addDemo("Slider", "/SliderDemo.xaml");
            addDemo("AutoCompleteBox", "/AutoCompleteBoxDemo.xaml");
            addDemo("DocPanel", "/DocPanelDemo.xaml");
            addDemo("PasswordBox", "/PasswordBoxDemo.xaml");
            addDemo("ProgressBar", "/ProgressBarDemo.xaml");
            addDemo("ScrollBar", "/ScrollBarDemo.xaml");
            addDemo("ScrollViewer", "/ScrollViewerDemo.xaml");
            addDemo("TreeView", "/TreeViewDemo.xaml");
            addDemo("PopUp", "/PopUpDemo.xaml");
            addDemo("TabControl", "/TabControlDemo.xaml");
            addDemo("ToolTipService", "/ToolTipServiceDemo.xaml");
            addDemo("HeaderedItemsControl", "/HeaderedItemsControlDemo.xaml");
            addDemo("HeaderedContentControl", "/HeaderedContentControlDemo.xaml");
            addDemo("CalendarDatePicker", "/CalendarDatePickerDemo.xaml");

        }
        internal void addDemo(string title, string uri)
        {
          
            UriMapper mapper =
            (UriMapper)MainFrame.Resources["uriMapper"];
            if (mapper == null)
            {
                mapper = new UriMapper();
                MainFrame.Resources.Add("uriMapper", mapper);
            }
            UriMapping mapping = new UriMapping();
            mapping.Uri = new Uri(title, UriKind.Relative);
            mapping.MappedUri = new Uri(uri, UriKind.Relative);
            mapper.UriMappings.Add(mapping);

            demos.Add(title, uri);
            demoList.Items.Add(title);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void demoList_SelectionChanged(object sender,SelectionChangedEventArgs e)
        {
            MainFrame.Navigate(new Uri((string)demoList.SelectedItem,UriKind.Relative));
          //  MainFrame.Navigate(new Uri(demos[(string)demoList.SelectedItem],UriKind.Relative));
        }

    }
}
