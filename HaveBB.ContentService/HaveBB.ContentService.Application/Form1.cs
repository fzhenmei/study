using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using NCrawler;
using NCrawler.HtmlProcessor;
using NCrawler.Interfaces;
using NCrawler.Services;

namespace HaveBB.ContentService.App
{
    public partial class Form1 : Form
    {
        private BackgroundWorker _backgroundWorker1;
        private Crawler _crawler;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            txtStartUrl.Text = "http://www.cnblogs.com";
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            rtbEcho.Text += (string) e.UserState + Environment.NewLine;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = ProcessCrawl(sender as BackgroundWorker, e);
        }

        private bool ProcessCrawl(BackgroundWorker worker, DoWorkEventArgs e)
        {
            Thread newThread = new Thread(new ThreadStart(_crawler.Crawl));
            newThread.Start();

            while (true)
            {
                Thread.Sleep(1000);
                if (_backgroundWorker1.CancellationPending)
                {
                    _crawler.Cancel();
                    newThread.Abort();
                    e.Cancel = true;
                    break;
                }
            }
            return true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            CreateWorker();

            CreateCrawler();

            _backgroundWorker1.RunWorkerAsync();
        }

        private void CreateCrawler()
        {
            ServicePointManager.MaxServicePoints = 999999;
            ServicePointManager.DefaultConnectionLimit = 999999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.EnableDnsRoundRobin = true;

            var echo = new EchoStep();
            echo.OneWorkFinished += ShowText;

            _crawler = new Crawler(
                new Uri("http://www.cnblogs.com"),
                new HtmlDocumentProcessor(),
                echo
                )
                           {
                               MaximumThreadCount = 1,
                               MaximumCrawlDepth = 3,
                               ExcludeFilter = new[]
                                                   {
                                                       new RegexFilter(
                                                           new Regex(@"(\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico)",
                                                                     RegexOptions.Compiled |
                                                                     RegexOptions.CultureInvariant |
                                                                     RegexOptions.IgnoreCase))
                                                   },
                           };
        }

        private void CreateWorker()
        {
            _backgroundWorker1 = new BackgroundWorker
                                     {
                                         WorkerReportsProgress = true,
                                         WorkerSupportsCancellation = true
                                     };
            _backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            _backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            _backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                rtbEcho.Text += "CANCEL!!!";
            }
            else
            {
                rtbEcho.Text += "DONE!!!";    
            }
        }

        private void ShowText(string text)
        {
            _backgroundWorker1.ReportProgress(100, text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopWork();
        }

        private void StopWork()
        {
            if (_backgroundWorker1.IsBusy)
            {
                _backgroundWorker1.CancelAsync();    
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopWork();

            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        #region Nested type: EchoStep

        internal class EchoStep : IPipelineStep
        {
            #region Delegates

            public delegate void OneWorkFinishedHandler(string title);

            #endregion

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

                lock (this)
                {
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
                }
            }

            #endregion

            public event OneWorkFinishedHandler OneWorkFinished;

            public void InvokeOneWorkFinished(string title)
            {
                OneWorkFinishedHandler handler = OneWorkFinished;
                if (handler != null) handler(title);
            }
        }

        #endregion

        #region Nested type: ShowTextHandler

        private delegate void ShowTextHandler(string text);

        #endregion
    }
}