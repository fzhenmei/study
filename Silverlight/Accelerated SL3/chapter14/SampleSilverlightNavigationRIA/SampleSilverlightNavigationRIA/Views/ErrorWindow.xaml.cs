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

namespace SampleSilverlightNavigationRIA
{
    public partial class ErrorWindow : ChildWindow
    {
        public ErrorWindow(Exception e)
            : this(e, null)
        {
        }

        public ErrorWindow(Exception e, Uri uri)
        {
            InitializeComponent();
            if (uri != null)
                ErrorDetails = "Page not found: \"" + uri.ToString() + "\"";
            else
                ErrorDetails = e.Message + "\n\n" + e.StackTrace;
        }

        public static DependencyProperty ErrorDetailsProperty = DependencyProperty.Register("ErrorDetails", typeof(String), typeof(ErrorWindow), null);

        string ErrorDetails
        {
            get { return (string)GetValue(ErrorDetailsProperty); }
            set { SetValue(ErrorDetailsProperty, value); }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
