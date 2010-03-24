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
namespace chapter5
{
    public partial class DataForm : UserControl
    {
        public DataForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DataForm_Loaded);
        }

        void DataForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }
        public ObservableCollection<Consultant> Consultants
        {
            get
            {
                if (consultants == null)
                {
                    consultants = new ObservableCollection<Consultant>();
                    consultants.Add(new Consultant()
                    {
                        FirstName = "Ashish",
                        LastName = "Ghoda",
                        Email = "aghoda@TechnologyOpinion.com",
                        Website = "www.TechnologyOpinion.com"
                    });

                    consultants.Add(new Consultant()
                    {
                        FirstName = "Jay",
                        LastName = "Nanavaty",
                        Email = "jnanavaty@TechnologyOpinion.com",
                        Website = "www.TechnologyOpinion.com"
                    });
                }
                return (consultants);

            }
        }
        private ObservableCollection<Consultant> consultants;
    }
}
