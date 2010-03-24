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

namespace XAMLTour
{
    public partial class ChooseAccount : Page
    {
        private List<Account> accountList;
        public ChooseAccount()
        {
            // Required to initialize variables
            InitializeComponent();
            accountList = new List<Account>();
            accountList.Add(new Account("Checking", 500.00));
            accountList.Add(new Account("Savings", 23100.19));
            accountListBox.DataContext = accountList;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
