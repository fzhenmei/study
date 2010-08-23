using System;
using System.Collections.Generic;
using System.Linq;
using Westwind.BusinessFramework.LinqToSql;

namespace Amazon
{

    /// <summary>
    /// Weblog Entry business object
    /// 
    /// EntryType = 1
    /// </summary>
    public class busBook :  BusinessObjectLinq<AmazonBook,AmazonContext>
    {

        /// <summary>
        /// Returns all books
        /// </summary>
        public IQueryable<AmazonBook> GetBooks()
        {
            return this.Context.AmazonBooks
                    .OrderByDescending( book => book.SortOrder)
                    .ThenBy( book=> book.Title);
        }

        public IQueryable<AmazonBook> GetHighlightedBooks()
        {
            return this.Context.AmazonBooks.Where( book => book.Highlight)
                    .OrderByDescending(book => book.SortOrder)
                    .ThenBy(book => book.Title);           
        }

        public List<AmazonBook> GetRandomHighlightedBooks(int count)
        {

            List<AmazonBook> books =
                (from book in this.Context.AmazonBooks
                where book.Highlight
                select book).ToList();

            int recCount = books.Count;

            // if there are less books selected just return the list
            if ( recCount <= count)
                return books;
            
            Random rnd = new Random( Guid.NewGuid().GetHashCode() );

            // Run through all 'extras' and delete random items
            for (int i = 0; i < recCount - count; i++)
            {                
                int z = rnd.Next(books.Count);
                books.RemoveAt(z);
            } 
            return books;
        }


        public IQueryable<AmazonBook> GetRecentBooks()
        {
            return this.Context.AmazonBooks.OrderByDescending(book => book.Entered).Take(5);

        }


        public override AmazonBook NewEntity()
        {
             if (base.NewEntity() == null)
                 return null;

             this.Entity.Entered = DateTime.Now;
             return this.Entity;
        }
    }
}
