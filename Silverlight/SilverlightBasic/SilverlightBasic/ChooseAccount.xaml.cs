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

namespace SilverlightApplication1
{
    public partial class ChooseAccount : UserControl
    {
        private List<UserAccount> accountList;
        public ChooseAccount()
        {
            InitializeComponent();
            accountList = new List<UserAccount> {new UserAccount("User1", 9.00D), new UserAccount("User2", 10.00D)};
            accountListBox.DataContext = accountList;
        }
    }
}
