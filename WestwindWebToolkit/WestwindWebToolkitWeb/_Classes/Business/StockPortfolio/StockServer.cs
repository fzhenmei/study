using System;
using System.Net;

using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Westwind.InternetTools;

namespace Westwind.WebToolkit
{

    /// <summary>
    /// Class that retrieves stock data from Yahoo's standard site Urls in form
    /// of .csv style comma delimited lists. This class republishes this data in
    /// object format for retrieving single quotes, groups of quotes or quote history.
    /// 
    /// For info on single quote parameters:
    /// http://www.gummy-stuff.org/Yahoo-data.htm
    /// </summary>
    public class StockServer
    {

        private const string STR_YAHOOFINANCE_STOCK_BASEURL = "http://download.finance.yahoo.com/d/quotes.csv?s=";
        
        /// <summary>
        /// s - Symbol l1 - last trade d1 - last trade date t1 - last trade time c1 - change 
        /// o - open price h - day's high g - day's low n - name
        /// </summary>
        private const string STR_STOCK_FORMATTING = "&f=sl1d1t1c1ohgn";                                                        
        
        /// <summary>
        /// Retrieves an individual Stock quote based on a ticker symbol
        /// </summary>
        /// <param name="symbol">Stock Ticker Symbol (ie. MSFT, INTC, YUM)</param>
        /// <returns>Quote object or null</returns>
        public StockQuote GetStockQuote(string symbol)
        {         
            StockQuote quote = null;
            try
            {
                HttpClient http = new HttpClient();
                http.Timeout = 4;
                string quoteString = http.GetUrl(STR_YAHOOFINANCE_STOCK_BASEURL + symbol +
                                                 STR_STOCK_FORMATTING);

                //WebClient http = new WebClient();
                //string quoteString = http.DownloadString(STR_YAHOOFINANCE_STOCK_BASEURL + symbol +
                //                                         STR_STOCK_FORMATTING);

                quote = this.ParseStockQuote(quoteString);
                if (quote != null)
                {
                    this.CacheQuote(quote);
                    this.UpdateSymbolList(quote.Symbol, quote.Company);
                }                    
                
            }
            catch 
            {
                // if above failed try to get a cached quote
                quote = this.GetCachedQuote(symbol);
            }

            return quote;
        }


        /// <summary>
        /// Retrieves a set of Stockquote objects for a given number of stock symbols.
        /// </summary>
        /// <param name="symbols">A string array of Stock Ticker Symbols</param>
        /// <returns></returns>
        public StockQuote[] GetStockQuotes(string[] symbols)
        {
            string url = STR_YAHOOFINANCE_STOCK_BASEURL;

            WebClient http = new WebClient();

            // Stocks are concatenated with commas
            foreach (string symbol in symbols)
            {
                url += symbol + ",";
            }
            url = url.TrimEnd(',');
            url += STR_STOCK_FORMATTING;

            string rawQuoteString = http.DownloadString(url);

            // Break up into each individual quote
            string[] quoteStrings = rawQuoteString.Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Break up each quote CSV component and parse it into object
            StockQuote[] quotes = new StockQuote[quoteStrings.Length];

            for (int x = 0; x < quoteStrings.Length; x++)
            {
                quotes[x] = this.ParseStockQuote(quoteStrings[x]);
            }

            return quotes;
        }

        /// <summary>
        /// Returns the stock history for the last year (or years asked for).
        /// http://itable.finance.yahoo.com/table.csv?s=MSFT&g=m&q=q&a=0&b=1&c=2005&d=3&e=10&&f=2006&
        /// </summary>
        /// <param name="Symbol"></param>
        /// <returns></returns>
        public StockHistory[] GetStockHistory(string symbol, int years)
        {
            // s=symbol a=startmonth b=startday c=startyear, d=endingmonth e:endingday g:timeperios (d=daily,w=weekly,m=monhtly)
            //  Months are 0 based!
            string url = "http://itable.finance.yahoo.com/table.csv?s=" + symbol + "&g=m&q=q";

            // Get data for the last year
            DateTime Now = DateTime.Now;
            DateTime Start = Now.AddMonths(((years * 12) - 1) * -1);

            url = url + "&a=" + (Start.Month - 1).ToString() + "&b=1" + "&c=" + Start.Year.ToString();
            url = url + "&d=" + (Now.Month - 1).ToString() + "&e=5" + "&f=" + Now.Year.ToString();

            WebClient http = new WebClient();

            // CSV String: 
            string RawQuoteString = http.DownloadString(url);

            string[] quoteStrings = RawQuoteString.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // We'll skip over the first line returned (CVS Header)
            StockHistory[] history = new StockHistory[quoteStrings.Length - 1];

            // Note starting at 1 to skip over header 
            // Reading backwards to return in chronological order
            int counter = 0;
            for (int x = quoteStrings.Length - 1; x > 0; x--)
            {
                string[] detail = quoteStrings[x].Split(',');
                StockHistory hist = new StockHistory();
                
                hist.Symbol = symbol;
                      
                // 2008-10-31
                string dateString = detail[0].Replace("\"", "");

                hist.QuoteDate = DateTime.Parse(dateString, CultureInfo.InvariantCulture);
                hist.QuoteDateString = hist.QuoteDate.ToString("MM-yy");

                decimal WorkNumber = 0.0M;
                decimal.TryParse(detail[6], out WorkNumber);
                hist.LastPrice = WorkNumber;
                
                history[counter] = hist;
                counter++;
            }

            return history;
        }

        /// <summary>
        /// Creates a historical stock chart PNG image as a binary buffer.
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public byte[] GetStockHistoryGraph(string[] symbols, string title, int width, int height, int years)
        {
            if (years < 1)
                years = 2;

            Graph graph = new Graph();
            graph.YAxisTitle = "Stock Price";
            graph.XAxisTitle = "Date";
            graph.GraphTitle = title;
            graph.ChartHeight = (double) height;
            graph.ChartWidth = (double) width;

            foreach (string Symbol in symbols)
            {
                StockServer stock = new StockServer();
                StockHistory[] history = stock.GetStockHistory(Symbol, years);
                DataPointSet dataPointSet = new DataPointSet
                {
                    Legend = Symbol,
                };
                foreach (StockHistory item in history)
                {
                    dataPointSet.Data.Add(item.QuoteDate, (double)item.LastPrice);
                }
                graph.DataPointSets.Add(dataPointSet);
            }

            return graph.LineChart();
        }

        
        /// <summary>
        /// Returns a string that describes when the market reopens.
        /// 
        /// Market Opens in h hours and m minutes"
        /// Market is closed for the weekend
        /// </summary>
        /// <returns></returns>
        public string TimeUntilClose()
        {
            TimeSpan ts;
            DateTime Now = DateTime.UtcNow.AddHours(-4);

            if (Now.Hour < 9 || Now.Hour >= 17)
            {
                DateTime Open = new DateTime(Now.Year, Now.Month, Now.AddDays(1).Day, 9, 0, 0);

                if (Open.DayOfWeek == DayOfWeek.Saturday || Open.DayOfWeek == DayOfWeek.Sunday)
                    return "Market is closed for the weekend.";

                ts = Open.Subtract(Now);
                if (ts.Hours > 24)
                    return "Market is closed.";

                return "Market re-opens in:<br> " +
                        ts.Hours.ToString() + " hours and " + ts.Minutes.ToString() + " minutes";
            }

            DateTime Close = new DateTime(Now.Year, Now.Month, Now.Day, 17, 0, 0);
            ts = Close.Subtract(Now);

            return ts.Hours.ToString() + " hrs " + ts.Minutes.ToString() + " mins until close.";
        }

        /// <summary>
        /// Parses an individual QUote String
        /// </summary>
        /// <param name="QuoteString"></param>
        /// <returns></returns>
        private StockQuote ParseStockQuote(string QuoteString)
        {
            // "MSFT",27.17,"3/10/2006","4:00pm",+0.17,27.04,27.22,26.88,"MICROSOFT CP"
            //   0      1       2          3        4    5     6     7     8   
            StockQuote Quote = new StockQuote();

            string[] Details = QuoteString.Split(',');

            Quote.Symbol = Details[0].Replace("\"", "");
            Quote.Company = Details[8].Replace("\"", "").Replace("\n", "").Replace("\r","").Trim();

            string Work = Details[1];
            decimal WorkNumber = 0M;
            decimal.TryParse(Work, out WorkNumber);
            Quote.LastPrice = WorkNumber;

            Work = Details[5];
            WorkNumber = 0M;
            decimal.TryParse(Work, out WorkNumber);
            Quote.OpenPrice = WorkNumber;

            Work = Details[4];
            WorkNumber = 0.00M;
            decimal.TryParse(Work, out WorkNumber);
            Quote.NetChange = WorkNumber;

            Work = Details[2] + " " + Details[3];
            Work = Work.Replace("\"", "");
            DateTime WorkDate = DateTime.UtcNow;
            DateTime.TryParse(Work, out WorkDate); // CultureInfo.GetCultureInfo("en-us"), DateTimeStyles.AssumeLocal, out WorkDate);

            if (WorkDate < StockQuote.DATE_EMPTY)
                return null;
            else
                Quote.LastQuoteTime = WorkDate;

            return Quote;
        }

        public bool CacheQuote(StockQuote quote)
        {
            busStockCache cache = new busStockCache();
            return cache.CacheQuote(quote);            
        }

        /// <summary>
        /// Gets a stock quote from the database cache
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public StockQuote GetCachedQuote(string symbol)
        {
            busStockCache cache = new busStockCache();
            StockQuote quote = cache.GetCachedQuote(symbol);
            if (quote == null)
            {
                quote = new StockQuote();
                quote.Symbol = symbol;
                quote.Company = "symbol not available";                
            }

            return quote;
        }

        /// <summary>
        /// Updates the symbol list if an entry doesn't exist
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="Company"></param>
        public void UpdateSymbolList(string symbol, string Company)
        {
            busPortfolioItem stock = new busPortfolioItem();
            stock.UpdateSymbolList(symbol, Company);
        }
    }

    /// <summary>
    /// Contains simple stock information for use as a stock
    /// history collection.
    /// </summary>
    [DataContract]
    public class StockHistory
    {
        /// <summary>
        /// The stock ticker symbol for the stock 
        /// </summary>
        [DataMember]
        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }
        private string _Symbol = "";

        /// <summary>
        /// The last price for the given date
        /// </summary>
        [DataMember]
        public decimal LastPrice
        {
            get { return _LastPrice; }
            set { _LastPrice = value; }
        }
        private decimal _LastPrice = 0.00M;


        /// <summary>
        /// String expression of the date as a month/year value: 01/05 (January 20005)
        /// </summary>
        [DataMember]
        public string QuoteDateString
        {
            get { return _DateString; }
            set { _DateString = value; }
        }
        private string _DateString = "";

        /// <summary>
        /// The date of the quote
        /// </summary>
        [DataMember]
        public DateTime QuoteDate
        {
            get { return _QuoteDate; }
            set { _QuoteDate = value; }
        }
        private DateTime _QuoteDate = DateTime.MinValue;

    }

    /// <summary>
    /// Stock information class that provides core statistics about an
    /// individual stock.
    /// </summary>
    [DataContract]
    public class StockQuote
    {

        public static DateTime DATE_EMPTY = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The stock exchange symbol. Example: MSFT, INTC, YUM, XOM, AMZN
        /// </summary>        
        [DataMember]
        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }
        private string _Symbol = "";

        /// <summary>
        /// The stock's company name
        /// </summary>        
        [DataMember]
        public string Company
        {
            get { return _Company; }
            set { _Company = value; }
        }
        private string _Company = "";

        /// <summary>
        /// The price of the stock at opening of current or last session.
        /// </summary>        
        [DataMember]
        public decimal OpenPrice
        {
            get { return _OpenPrice; }
            set { _OpenPrice = value; }
        }
        private decimal _OpenPrice = 0.00M;

        /// <summary>
        /// The current or last price for the stock.
        /// </summary>        
        [DataMember]
        public decimal LastPrice
        {
            get { return _LastPrice; }
            set { _LastPrice = value; }
        }
        private decimal _LastPrice = 0.00M;

        /// <summary>
        /// The net change from opening of the current or last trading day.
        /// </summary>
        [DataMember]
        public decimal NetChange
        {
            get { return _NetChange; }
            set { _NetChange = value; }
        }
        private decimal _NetChange = 0.00M;

        /// <summary>
        /// Time of last quote relative. Time zone is in NYSE format.
        /// </summary>
        [DataMember]
        public DateTime LastQuoteTime
        {
            get { return _LastQuoteTime; }
            set { _LastQuoteTime = value; }
        }
        private DateTime _LastQuoteTime = DATE_EMPTY; // DateTime.MinValue;

        /// <summary>
        /// Formatted string optimized for display
        /// </summary>
        [DataMember]
        public string LastQuoteTimeString
        {
            get { return LastQuoteTime.ToString("MMM d, h:mmtt"); }
            set { _LastQuoteTimeString = value; }
        }
        private string _LastQuoteTimeString = "";
    }


}