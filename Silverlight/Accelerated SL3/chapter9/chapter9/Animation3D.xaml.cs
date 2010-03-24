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

namespace chapter9
{
    public partial class Animation3D : UserControl
    {
        public Animation3D()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Animation3D_Loaded);
        }

        void Animation3D_Loaded(object sender, RoutedEventArgs e)
        {
            Rotate.Begin();
        }
    }
}
