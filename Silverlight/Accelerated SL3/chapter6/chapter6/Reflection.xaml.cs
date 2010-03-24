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
//added
using System.Windows.Media.Imaging;

namespace chapter6
{
    public partial class Reflection : UserControl
    {
        public Reflection()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Reflection_Loaded);
        }

        void Reflection_Loaded(object sender, RoutedEventArgs e)
        {
            WriteableBitmap bmp = new WriteableBitmap((int)source.Width,

            (int)source.Height, PixelFormats.Bgr32);

            bmp.Render(source, new TranslateTransform());

            target.Source = bmp;
        }
    }
}
