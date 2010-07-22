using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using NCrawler;
using NCrawler.Extensions;
using NCrawler.HtmlProcessor;
using NCrawler.Interfaces;
using NCrawler.Services;

namespace HaveBB.ContentService.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private BackgroundWorker backgroundWorker1 = new BackgroundWorker();

        private void Form1_Load(object sender, EventArgs e)
        {
            this.txtStartUrl.Text = "http://www.cnblogs.com";
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            rtbEcho.Text += (string)e.UserState;
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var echo = new EchoStep();
            echo.OneWorkFinished += ShowText;

            using (var c = new Crawler(
                new Uri("http://www.cnblogs.com"),
                new HtmlDocumentProcessor(),
                echo
                ))
            {
                c.MaximumThreadCount = 1;
                c.MaximumCrawlDepth = 3;
                c.ExcludeFilter = new[]
                                      {
                                          new RegexFilter(
                                              new Regex(@"(\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico)",
                                                        RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                        RegexOptions.IgnoreCase))
                                      };
                c.Crawl();
            }
        }

        void c_AfterDownload(object sender, NCrawler.Events.AfterDownloadEventArgs e)
        {
            //e.Response.Title;
            
        }

        private delegate void ShowTextHandler(string text);
       
        private void btnStart_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
           
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            //var uri = new Uri(txtStartUrl.Text.Trim());

            //ServicePointManager.MaxServicePoints = 999999;
            //ServicePointManager.DefaultConnectionLimit = 999999;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            //ServicePointManager.CheckCertificateRevocationList = true;
            //ServicePointManager.EnableDnsRoundRobin = true;

            
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rtbEcho.Text += "DONE!!!";
            rtbEcho.Text += Environment.NewLine;  
        }

        private void ShowText(string text)
        {
            backgroundWorker1.ReportProgress(50, text);
        }

        

        internal class EchoStep : IPipelineStep
        {
            public delegate void OneWorkFinishedHandler(string title);

            public event OneWorkFinishedHandler OneWorkFinished;

            public void InvokeOneWorkFinished(string title)
            {
                OneWorkFinishedHandler handler = OneWorkFinished;
                if (handler != null) handler(title);
            }

            protected RichTextBox EchoControl { get; set; }

            #region IPipelineStep Members

            /// <summary>
            /// </summary>
            /// <param name="crawler">
            /// The crawler.
            /// </param>
            /// <param name="propertyBag">
            /// The property bag.
            /// </param>
            public void Process(Crawler crawler, PropertyBag propertyBag)
            {
                //CultureInfo contentCulture = (CultureInfo)propertyBag["LanguageCulture"].Value;
                //string cultureDisplayValue = "N/A";
                //if (!contentCulture.IsNull())
                //{
                //    cultureDisplayValue = contentCulture.DisplayName;
                //}


                //lock (this)
                //{

                    //EchoControl.Invoke(new ShowTitleDelegate(ShowTitle), propertyBag.Title);
                    InvokeOneWorkFinished(propertyBag.Title);
                    //Console.Out.WriteLine(ConsoleColor.Gray, "Url: {0}", propertyBag.Step.Uri);
                    //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tContent type: {0}", propertyBag.ContentType);
                    //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tContent length: {0}", propertyBag.Text.IsNull() ? 0 : propertyBag.Text.Length);
                    //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tDepth: {0}", propertyBag.Step.Depth);
                    //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tCulture: {0}", cultureDisplayValue);
                    //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tThreadId: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tThread Count: {0}", crawler.ThreadsInUse);
                    //Console.Out.WriteLine();
                //}
            }

            #endregion
        }
    }
}