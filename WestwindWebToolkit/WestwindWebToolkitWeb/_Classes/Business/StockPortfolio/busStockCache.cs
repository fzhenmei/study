using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using Westwind.BusinessFramework.LinqToSql;

namespace Westwind.WebToolkit
{
    public class busStockCache :  BusinessObjectLinq<StockCache,StockContext>
    {
        /// <summary>
        /// Stores a quote to the cache.
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public bool CacheQuote(StockQuote quote)
        {
            if (string.IsNullOrEmpty(quote.Symbol))
                return true;

            StockCache cache = this.Context.StockCaches
                                    .SingleOrDefault(sym => sym.Symbol == quote.Symbol);
            if (cache == null)
                cache = new StockCache();

            cache.Symbol = quote.Symbol;
            cache.Company = quote.Company;
            cache.LastPrice = quote.LastPrice;
            cache.OpenPrice = quote.OpenPrice;
            cache.NetChange = quote.NetChange;
            cache.QuoteTime = quote.LastQuoteTime;

            return this.Save(cache);
        }

        /// <summary>
        /// Retrieves a quote from the cache. NUll if not found
        /// </summary>
        /// <param name="Symbol"></param>
        /// <returns></returns>
        public StockQuote GetCachedQuote(string Symbol)
        {
            if (string.IsNullOrEmpty(Symbol))
                return null;

            StockCache cache = this.Context.StockCaches.SingleOrDefault(sym => sym.Symbol == Symbol);
            if (cache == null)
                return null;

            StockQuote quote = new StockQuote()
            {
                Symbol = cache.Symbol,
                Company = cache.Company,
                OpenPrice = cache.OpenPrice,
                LastPrice = cache.LastPrice,
                NetChange = cache.NetChange,
                LastQuoteTime = cache.QuoteTime
            };

            return quote;
        }

    }
}
