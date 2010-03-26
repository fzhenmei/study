using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace SilverlightMenu
{
    public partial class MenuItemContent : UserControl
    {
        private ObservableCollection<MenuDemoService.MenuItem> _dataSource;
        public ObservableCollection<SilverlightMenu.MenuDemoService.MenuItem> DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                lstMenuItem.ItemsSource = _dataSource;
            }
        }

        public MenuItemContent()
        {
            InitializeComponent();
        }
    }
}