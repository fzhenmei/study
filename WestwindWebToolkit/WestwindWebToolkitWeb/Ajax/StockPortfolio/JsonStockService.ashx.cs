using System.Linq;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using System;

using Westwind.Web.Controls;
using System.Web;
using Westwind.Web;

namespace Westwind.WebToolkit
{

    /// <summary>
    /// AJAX Callback handler for the JsonStock Client Stock Portfolio Manager
    /// 
    /// This class simply derives from CallbackHandler and marks any methods
    /// that are to be AJAX accessible with [CallbackMethod].
    /// 
    /// The AjaxMethodCallback Control (or the ww.jquery.js client version)
    /// can then call these service methods directly. 
    /// </summary>
    public class JsonStockService : CallbackHandler
    {
        /// <summary>
        /// Server object that performs stock retrieval operations
        /// with data from yahoo.
        /// </summary>
        private StockServer Stocks = new StockServer();

        /// <summary>
        /// User instance that is set after a call to Validate()
        /// This identifies the user that is logged in for special
        /// operations. Note not all methods of the service require
        /// logins - only the portfolio related methods do.
        /// </summary>
        private User User = null;

        [CallbackMethod]
        public StockQuote GetStockQuote(string symbol)
        {
            return Stocks.GetStockQuote(symbol);
        }

        [CallbackMethod]
        public StockQuote[] GetStockQuotes(string[] symbols)
        {
            return Stocks.GetStockQuotes(symbols);
        }

        [CallbackMethod]
        public StockQuote[] GetStockQuotesSafe(string symbolList)
        {
            string[] symbols = symbolList.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return Stocks.GetStockQuotes(symbols);
        }
        
        [CallbackMethod]
        public StockHistory[] GetStockHistory(string symbol, int years)
        {
            return Stocks.GetStockHistory(symbol, years);
        }
       
        
        /// <summary>
        /// Methods can return streams which return raw data to a client
        /// Very useful for returning things like image data or resources.
        /// 
        /// Can be accessed via URL like this
        /// JsonStockService.ashx?Method=GetStockHistoryGraphSafe&symbollist=LDK,MSFT,F,GLD,STP&title=Portfolio%20History&width=620&height=400&years=2&t=76        
        /// 
        /// Note the 'Safe' version that allows for string parameter for the Symbol array.
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        [CallbackMethod]        
        public Stream GetStockHistoryGraphSafe(string symbolList, string title, int width, int height, int years)
        {
            string[] symbols = symbolList.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return this.GetStockHistoryGraph(symbols, title, width, height, years);
        }

        /// <summary>
        /// Same as above but you can't access this via URL due to the array parameter.
        /// This method can be called with POST and JSON data.
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        [CallbackMethod]
        public Stream GetStockHistoryGraph(string[] symbols, string title, int width, int height, int years)
        {
            if (years < 1)
                years = 2;

            HttpContext.Current.Response.ContentType = "image/png";

            byte[] imgData = Stocks.GetStockHistoryGraph(symbols, title, width, height, years);
            MemoryStream ms = new MemoryStream(imgData);
            return ms;
        }


        /// <summary>
        /// Returns a single symbol history chart.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        [CallbackMethod(ContentType="image/png")]        
        public Stream GetStockHistoryGraphSingle(string symbol, string title, int width, int height, int years)
        {            
            byte[] imgData = Stocks.GetStockHistoryGraph(new string[1] { symbol }, title, width, height, years);

            // this works too, but the above is more explicit and removes HttpContext dependence
            // in your service code
            //HttpContext.Current.Response.ContentType = "image/png";

            MemoryStream ms = new MemoryStream(imgData);
            return ms;
        }


        /// <summary>
        /// Adds a new item to the portfolio based on symbol and qty.
        /// 
        /// The method call tries to look up the symbol with the StockServer
        /// and then returns the updated and recalculated Portfolio item.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="qty"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        [CallbackMethod]
        public PortfolioMessage AddPortfolioItem(string symbol, int qty, string userToken)
        {
            // Validate and get this.User instance
            this.ValidateToken(userToken);

            busPortfolioItem item = new busPortfolioItem();

            item.NewEntity();

            item.Entity.Symbol = symbol;
            item.Entity.Qty = qty;
            item.Entity.UserPk = this.User.Pk;

            StockQuote quote = Stocks.GetStockQuote(symbol);

            if (quote == null)
                throw new ApplicationException("Invalid stock symbol or stock server unavailable");

            item.UpdateEntityFromStockQuote(quote);
            if (!item.Save())
                throw new ApplicationException(item.ErrorMessage);

            // Create complex message object that contains multiple components
            PortfolioMessage msg = new PortfolioMessage
            {
                SingleItem = item.Entity,
                TotalValue = item.GetPortfolioTotalValue(item.Entity.UserPk),
                TotalItems = item.GetPortfolioItemCount(item.Entity.UserPk)
            };

            return msg;
        }

        /// <summary>
        /// Updates an existing portfolio item based on symbol and qty
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="symbol"></param>
        /// <param name="qty"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        [CallbackMethod]
        public PortfolioMessage UpdatePortfolioItem(int pk, string symbol, int qty, string userToken)
        {
            // Validate and get this.User instance
            this.ValidateToken(userToken);

            busPortfolioItem item = new busPortfolioItem();

            item.Load(pk);

            if (this.User.Pk != item.Entity.UserPk)
                throw new AccessViolationException("Access denied to this portfolio item.");

            item.Entity.Symbol = symbol;
            item.Entity.Qty = qty;
            item.Entity.UserPk = this.User.Pk;

            StockQuote quote = Stocks.GetStockQuote(symbol);
            item.UpdateEntityFromStockQuote(quote);

            item.Save();


            // Create complex message object that contains multiple components
            PortfolioMessage msg = new PortfolioMessage
            {
                SingleItem = item.Entity,
                TotalValue = item.GetPortfolioTotalValue(item.Entity.UserPk),
                TotalItems = item.GetPortfolioItemCount(item.Entity.UserPk)
            };

            return msg;
        }

        /// <summary>
        /// Deletes an quote item from the portfolio based on the item's pk
        /// </summary>
        /// <param name="Pk"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        [CallbackMethod]
        public PortfolioMessage DeletePortfolioItem(int Pk, string userToken)
        {
            // Validate and get this.User instance
            this.ValidateToken(userToken);

            busPortfolioItem item = new busPortfolioItem();
            if (item.Load(Pk) == null)
                throw new ArgumentException("Invalid stock Id passed.");

            // Validate the user on the portfolio
            if (item.Entity.Pk != Pk)
                throw new AccessViolationException("This item doesn't match the current user.");


            if (!item.Delete())
                throw new InvalidOperationException("Failed to delete user: " + item.ErrorMessage);

            // Return totals
            PortfolioMessage msg = new PortfolioMessage
            {
                TotalValue = item.GetPortfolioTotalValue(item.Entity.UserPk),
                TotalItems = item.GetPortfolioItemCount(item.Entity.UserPk)
            };

            return msg;
        }

        /// <summary>
        /// Returns a full list of items to the caller in the Items
        /// property for the PortfolioMessage
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="updateValues">Get the latest live quotes</param>
        /// <returns></returns>
        [CallbackMethod]                
        public PortfolioMessage GetPortfolioItems(string userToken, bool updateValues)
        {
            // Validate and get this.User instance
            this.ValidateToken(userToken);

            busPortfolioItem portfolioItem = new busPortfolioItem();

            List<PortfolioItem> items = portfolioItem.GetItemsForUser(this.User.Pk).ToList();
            
            // Update quotes with latest values from quote server (yahoo)
            if (updateValues)
            {
                string[] symbols = items.Select( item => item.Symbol).ToArray();
                       
                StockServer server = new StockServer();
                StockQuote[] quotes = server.GetStockQuotes(symbols);

                for (int i = 0; i < items.Count; i++)
                {
                    items[i].LastPrice = quotes[i].LastPrice;
                    items[i].LastDate = quotes[i].LastQuoteTime;
                }
            }

            PortfolioMessage msg = new PortfolioMessage
            {
                TotalValue = portfolioItem.GetPortfolioTotalValue(this.User.Pk),
                TotalItems = portfolioItem.GetPortfolioItemCount(this.User.Pk),
                Items = items
            };

            return msg;
        }


        /// <summary>
        ///  Validates a user token by looking it up in db
        ///  and if found sets the User instance on this
        ///  class with the full user object.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>true or falls</returns>
        private bool ValidateToken(string tokenId)
        {
            this.User = null;

            busUserToken token = new busUserToken();
            this.User = token.GetUserFromToken(tokenId);

            if (this.User == null)
                throw new AccessViolationException("This operation requires a valid session token. Please login first.");

            return true;
        }

        /// <summary>
        /// Logs in a user and returns a valid user token that can be used
        /// to access the portfolio data tied to a user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [CallbackMethod]
        public string GetUserToken(string username, string password)
        {
            busUser user = new busUser();

            User userEntity = user.AuthenticateAndLoad(username, password);
            if (userEntity == null)
                throw new AccessViolationException(user.ErrorMessage);

            busUserToken token = new busUserToken();
            string tokenId = token.GetOrCreateToken(user.Entity.Pk);

            return tokenId;
        }

    }


    /// <summary>
    /// Message Item sent to client. Contains Portfolio detail
    /// about a single item or complete portfolio
    /// </summary>
    [DataContract]
    public class PortfolioMessage
    {
        [DataMember]
        public decimal TotalValue { get; set; }

        [DataMember]
        public int TotalItems { get; set; }

        [DataMember]
        public PortfolioItem SingleItem { get; set; }

        [DataMember]
        public List<PortfolioItem> Items { get; set; }
    }
}
