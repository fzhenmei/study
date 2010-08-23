using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using Westwind.BusinessFramework.LinqToSql;


namespace Westwind.WebToolkit
{
    public class busPortfolioItem : BusinessObjectLinq<PortfolioItem,StockContext>
    {

        /// <summary>
        /// Returns all the portfolio items ofr a given user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IQueryable<PortfolioItem> GetItemsForUser(string userName)
        {
            return 
                from item in this.Context.PortfolioItems
                where item.User.Email == userName
                select item;        
        }

        public IQueryable<PortfolioItem> GetItemsForUser(int userPk)
        {
            return
                from item in this.Context.PortfolioItems
                where item.User.Pk == userPk
                select item;
        }

        /// <summary>
        /// Updates the loaded entity with the contents from a quote object
        /// </summary>
        /// <param name="quote"></param>
        public void UpdateEntityFromStockQuote(StockQuote quote)
        {
            if (this.Entity == null)
                return;

            if ( !string.IsNullOrEmpty(quote.Company) )
                this.Entity.Company = quote.Company;

            // if the price isn't set service is down or not providing data at the moment
            if ( quote.LastPrice == 0.00M )
                return;

            this.Entity.LastPrice = quote.LastPrice;
            this.Entity.LastDate = quote.LastQuoteTime;
            this.Entity.OpenPrice = quote.OpenPrice;
            this.Entity.Change = quote.NetChange;
            this.Entity.LastDate = quote.LastQuoteTime;

            this.Entity.ItemValue = this.Entity.LastPrice * this.Entity.Qty;
        }

        /// <summary>
        /// Retrieve total value of portfolio for the user
        /// </summary>
        /// <returns></returns>
        public decimal GetPortfolioTotalValue(int userPk)
        {
            decimal? value = 
                (from item in this.Context.PortfolioItems
                 where item.UserPk == userPk
                 select item)          
                 .Sum(item => (decimal?) item.ItemValue);

            if (!value.HasValue)
                return 0.0M;

            return value.Value;
        }

        /// <summary>
        /// Returns a count of items in the portfolio
        /// </summary>
        /// <param name="userPk"></param>
        /// <returns></returns>
        public int GetPortfolioItemCount(int userPk)
        {
            int? count = 
                (from item in this.Context.PortfolioItems
                 where item.UserPk == userPk
                 select item).Count();

            if (!count.HasValue)
                return 0;

            return count.Value;
        }
        
        /// <summary>
        /// Overridden to update stored item total
        /// </summary>
        /// <returns></returns>
        public override bool Save()
        {
            this.Entity.ItemValue = this.Entity.LastPrice * this.Entity.Qty;            
            return base.Save();
        }

        public override PortfolioItem NewEntity()
        {
            PortfolioItem item = base.NewEntity();
            if (item == null)
                return null;

            item.LastDate = new DateTime(1900, 1, 1);            

            return item;
        }

        /// <summary>
        /// Returns a 
        /// </summary>
        /// <param name="letters"></param>
        /// <returns></returns>
        public IQueryable<StockSymbol> GetSymbolList(string letters)
        {
            if (string.IsNullOrEmpty(letters))
                return null;

            return Context.StockSymbols
                    .Where(stock => (stock.Symbol == letters || stock.Symbol.StartsWith(letters) || stock.Company.StartsWith(letters)) && stock.Exchange > 1)
                    .OrderBy(stock => stock.Company);
        }

        /// <summary>
        /// Checks to see if a symbol exists in the symbol list and then
        /// adds it if it doesn't.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="company"></param>
        public void UpdateSymbolList(string symbol, string company)
        {
            if (string.IsNullOrEmpty(symbol) || string.IsNullOrEmpty(company))
                return;

            string retrievedSymbol = Context.StockSymbols.Where(stock => stock.Symbol == symbol).Select(stock => stock.Symbol).SingleOrDefault();
            if (retrievedSymbol == null)
            {
                StockSymbol stockSymbol = new StockSymbol()
                {
                    Symbol = symbol,
                    Company = company,
                    Exchange = 3
                };
                Context.StockSymbols.InsertOnSubmit(stockSymbol);
                Context.SubmitChanges();
            }
        }
    }

    public class PortfolioTotals
    { 
        public int ItemCount {get; set;}
        public decimal TotalValue { get; set; }
    }
}
