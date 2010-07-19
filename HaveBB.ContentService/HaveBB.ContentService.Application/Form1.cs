using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
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

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var uri = new Uri(txtStartUrl.Text.Trim());

            ServicePointManager.MaxServicePoints = 999999;
            ServicePointManager.DefaultConnectionLimit = 999999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.EnableDnsRoundRobin = true;

            using (var c = new Crawler(
                uri, 
                new HtmlDocumentProcessor(),
                new EchoStep(rtbEcho)
                ))
            {
                c.MaximumThreadCount = 3;
                c.MaximumCrawlDepth = 10;
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

        internal class EchoStep : IPipelineStep
        {
            public EchoStep(RichTextBox echoControl)
            {
                EchoControl = echoControl;
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

                lock (this)
                {
                    EchoControl.Text += propertyBag.Title;
                    EchoControl.Text += "\r\n";

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
        }
    }
}