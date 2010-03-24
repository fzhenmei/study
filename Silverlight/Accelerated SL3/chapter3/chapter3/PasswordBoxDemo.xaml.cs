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

namespace chapter3
{
    public partial class PasswordBoxDemo : UserControl
    {
        public PasswordBoxDemo()
        {
            InitializeComponent();
        }
        private void EnterPassword_PasswordChanged(object sender,RoutedEventArgs e)
        {
            DisplayPassword.Text = EnterPassword.Password;
        }
    }
}
