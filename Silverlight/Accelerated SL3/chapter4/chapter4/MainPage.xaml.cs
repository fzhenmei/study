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
using System.ServiceModel;


namespace chapter4
{
    public partial class MainPage : UserControl
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void getDetail_Click(object sender, RoutedEventArgs e)
        {
            GetBookInfoClient GetBook = new GetBookInfoClient();
            GetBook.GetByTitleCompleted += new EventHandler<GetByTitleCompletedEventArgs>(GetBook_GetByTitleCompleted);
            if (txtTitle.Text != string.Empty )
            {
                GetBook.GetByTitleAsync(txtTitle.Text);
            }
            else
            {
                GetBookInfo GetBook1 = GetBook;
                GetBook1.BeginGetAllTitle(GetAllTitle_AsyncCallBack, GetBook1);
            }
        }
        void GetAllTitle_AsyncCallBack(IAsyncResult ar)
        {
          string [] items =
                 ((GetBookInfo)ar.AsyncState).EndGetAllTitle(ar);

            Dispatcher.BeginInvoke(delegate()
            {
                chapters.ItemsSource = items;
            });
        }

        void GetBook_GetByTitleCompleted(object sender, GetByTitleCompletedEventArgs e)
        {
            InfoPanel.DataContext = e.Result;
        }

    }
}
