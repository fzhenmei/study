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
using System.Windows.Browser;

namespace Interoperating
{
    [ScriptableType]
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlElement menu = HtmlPage.Document.GetElementById("colorMenu");
            menu.AttachEvent("onchange", new EventHandler<HtmlEventArgs>(this.onColorChanged));
            HtmlPage.Document.DocumentReady += new EventHandler(Document_DocumentReady);

            HtmlElement option = HtmlPage.Document.CreateElement("option");
            option.SetAttribute("value", "blue");
            option.SetAttribute("innerHTML", "Blue");
            menu.AppendChild(option);

            HtmlElement option2 = HtmlPage.Document.CreateElement("option");
            option2.SetAttribute("value", "white");
            option2.SetAttribute("innerHTML", "White");
            menu.AppendChild(option2);

            HtmlElement option3 = HtmlPage.Document.CreateElement("option");
            option3.SetAttribute("value", "red");
            option3.SetAttribute("innerHTML", "Red");
            menu.AppendChild(option3);


            HtmlElement option4 = HtmlPage.Document.CreateElement("option");
            option4.SetAttribute("value", "green");
            option4.SetAttribute("innerHTML", "Green");
            menu.AppendChild(option4);

        }
        public void onColorChanged(object sender, HtmlEventArgs e)
        {
            HtmlElement menu = (HtmlElement)e.Source;
            string color = (string)menu.GetProperty("value");
            Color c;
            if (color == "blue")
            c = Color.FromArgb(255, 0, 0, 255);
            else if (color == "red")
            c = Color.FromArgb(255, 255, 0, 0);
            else if (color == "green")
            c = Color.FromArgb(255, 0, 255, 0);
            else
            c = Color.FromArgb(255, 255, 255, 255);
            choiceTB.Text = color;
            LayoutRoot.Background = new SolidColorBrush(c);
            }
        void Document_DocumentReady(object sender, EventArgs e)
        {
            // code to manipulate DOM after HTML page is initialized
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {



        }

    }
}
