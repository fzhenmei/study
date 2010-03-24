using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace XAMLTour
{
    public class Account
    {
        public string AccountName { get; set; }
        public double AccountBalance { get; set; }
        public Account(string n, double b)
        {
            this.AccountName = n;
            this.AccountBalance = b;
        }
    }
}
