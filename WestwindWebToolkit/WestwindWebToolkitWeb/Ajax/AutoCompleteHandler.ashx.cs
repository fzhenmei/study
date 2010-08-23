using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Westwind.Web;
using System.Text;
using System.IO;
using Westwind.WebToolkit.NorthwindCustomers;
using System.Drawing;
using System.Drawing.Imaging;

namespace Westwind.WebToolkit.Ajax
{
    /// <summary>
    /// Callback Handler for AutoComplete page. Returns stock
    /// information
    /// </summary>
    public class AutoCompleteHandler : CallbackHandler
    {
        /// <summary>
        /// Returns a list of symbols (Symbol Company fields)
        /// based on a partial stock name. Routine searches
        /// the name and symbol for matches.
        /// 
        /// This method is used for autocomplete operation
        /// </summary>
        /// <param name="partial"></param>
        /// <returns></returns>
        [CallbackMethod(ReturnAsRawString = true, ContentType = "text/plain")]
        public string GetStockLookup(string partial)
        {
            HttpRequest Request = HttpContext.Current.Request;

            if (partial == null)
                // From Autocomplete plug-in
                partial = Request.QueryString["q"] as string;

            int maxItems = 50;
            int.TryParse(Request.QueryString["limit"], out maxItems);

            StringBuilder sb = new StringBuilder();

            busPortfolioItem stocks = new busPortfolioItem();
            var symbolList = stocks.GetSymbolList(partial)
                                  .Select(stock => new { Company = stock.Company, Symbol = stock.Symbol })
                                  .Take(maxItems);

            foreach (var symbol in symbolList)
            {
                sb.AppendLine(symbol.Symbol + "|" + symbol.Company.Trim());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns an individual stock quote from a symbol name
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [CallbackMethod]
        public StockQuote GetStockQuote(string symbol)
        {
            StockServer server = new StockServer();
            StockQuote quote = server.GetStockQuote(symbol);

            return quote;
        }


        /// <summary>
        /// Returns a stock chart image as a stream. The streamed
        /// data can be accessed from images or other loaded direclty
        /// into a browser.
        /// 
        /// This is useful in order to keep 'service' functionality isolated
        /// to a single service handler rather than offloading non-JSON
        /// requests to another service/handler.
        /// 
        /// Call by Url:
        /// AutocompleteHandler.ashx?Method=GetStockHistoryGraph&symbol=msft
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [CallbackMethod(ContentType="image/png")]
        public byte[] GetStockHistoryGraph(string symbol)
        {
            StockServer server = new StockServer();
            byte[] img = server.GetStockHistoryGraph(new[] { symbol },
                                                        "History for " + symbol.ToUpper(), 
                                                        450, 300, 2);            
            return img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        [CallbackMethod()]
        public Bitmap GetImage(string imageFile)
        {
            Bitmap img = new Bitmap(HttpContext.Current.Server.MapPath("~/images/" + imageFile));
            return img;
        }

        /// <summary>
        /// Auto complete list of cities available in Northwind
        /// customer list.
        /// </summary>
        /// <returns></returns>
        [CallbackMethod(ReturnAsRawString=true,ContentType="text/plain; charset=UTF-8")]
        public string GetCities()
        {
            var searchFor = HttpContext.Current.Request.QueryString["q"];

            // Heineous code - but for demonstration of where the vals are coming from <s>
            busCustomer customer = new busCustomer();
            var res = (from cust in customer.Context.nw_Customers
                        where cust.City.Contains(searchFor)
                        orderby cust.City
                        select new { City = cust.City, cust.CustomerID }).Distinct();

            StringBuilder sb = new StringBuilder();
            foreach (var city in res)
            {
                sb.AppendLine(city.City + "|" + city.CustomerID);
            }

            return sb.ToString();
        }




    }
}
