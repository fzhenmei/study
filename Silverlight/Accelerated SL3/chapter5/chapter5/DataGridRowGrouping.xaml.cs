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

namespace chapter5
{
    public partial class DataGridRowGrouping : UserControl
    {
        public DataGridRowGrouping()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DataGridRowGrouping_Loaded);
        }

        void DataGridRowGrouping_Loaded(object sender, RoutedEventArgs e)
        {
            Employee[] emps = new Employee[10];
            emps[0] = new Employee ();
            emps[0].Name="Ashish Ghoda";
            emps[0].Email = "aghoda@TechnologyOpinion.com";
            emps[0].City="New Jersey";
            emps[0].Pincode=07974;
            emps[0].State="New york";

            emps[3] = new Employee();
            emps[3].Name = "Pratixa Ghoda";
            emps[3].Email = "pghoda@TechnologyOpinion.com";
            emps[3].City = "New Jersey";
            emps[3].Pincode = 07974;
            emps[3].State = "New york";

            emps[1] = new Employee();
            emps[1].Name = "Jay Nanavaty";
            emps[1].Email = "jnanavaty@TechnologyOpinion.com";
            emps[1].City = "Baroda";
            emps[1].Pincode = 390023;
            emps[1].State = "Gujarat";

            emps[2] = new Employee();
            emps[2].Name = "Kruti Vaishnav";
            emps[2].Email = "kvaishnav@TechnologyOpinion.com";
            emps[2].City = "Delhi";
            emps[2].Pincode = 350025;
            emps[2].State = "Maharashtra";





            myDataGrid.ItemsSource = emps;
        }

    }
}
