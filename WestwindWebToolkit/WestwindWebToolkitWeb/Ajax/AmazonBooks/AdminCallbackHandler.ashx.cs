using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Westwind.Web.Controls;
using Amazon;
using wsAmazon;
using System.Collections.Generic;
using Westwind.Utilities;
using System.Reflection;
using Westwind.Web;

namespace Westwind.WebLog
{
    /// <summary>
    /// Callback handler for Amazon Books Page
    /// </summary>
    public class AdminCallbackHandler : CallbackHandler
    {
        private busBook books = new  busBook();

        [CallbackMethod]
        public bool DeleteBook(int id)
        {
            if (!books.Delete(id))
                throw new InvalidOperationException(books.ErrorMessage);

            return true;
        }

        [CallbackMethod]
        public AmazonBook GetBook(int id)
        {
            if (id < 1)
            {
                if (books.NewEntity() == null)
                    throw new InvalidOperationException("Couldn't load book: " + books.ErrorMessage);

                return books.Entity;
            }

            if (books.Load(id) == null)
                throw new InvalidOperationException("Couldn't load book: " + books.ErrorMessage);

            return books.Entity;
        }

        [CallbackMethod]
        public List<AmazonBook> GetBooks(string filter)
        {
            if (filter == null)
                filter = string.Empty;

            IQueryable<AmazonBook> bookList = null;

            if (filter == "Highlighted")
                bookList = books.GetHighlightedBooks();
            else if (filter == "Recent")
                bookList = books.GetRecentBooks();
            else
                bookList = books.GetBooks();

            return bookList.ToList();
        }

        /// <summary>
        /// Saves a book to the database. Accepts new or updated content.
        /// </summary>
        /// <param name="bookEntity"></param>
        /// <returns>int - Pk of the item saved (so a new record can get this info)</returns>
        [CallbackMethod]
        public int SaveBook(AmazonBook bookEntity)
        {            
            if (bookEntity.Pk > 0)
                books.Load(bookEntity.Pk);
            else
                books.NewEntity();
            
            // Update entity from message object
            //books.Entity.Title = bookEntity.Title;
            //books.Entity.Author = bookEntity.Author;
            //books.Entity.Category = bookEntity.Category;
            //books.Entity.AmazonUrl = bookEntity.AmazonUrl;
            //books.Entity.AmazonImage = bookEntity.AmazonImage;
            //books.Entity.AmazonSmallImage = bookEntity.AmazonSmallImage;
            //books.Entity.Highlight = bookEntity.Highlight;            
            
            // Copy property values from the received entity skip over Pk
            DataUtils.CopyObjectData(bookEntity, books.Entity,
                                     "Pk,Published,tStamp", 
                                     BindingFlags.Instance | BindingFlags.Public);

            if (!books.Save())
                throw new InvalidOperationException("Couldn't save book: " + books.ErrorMessage);

            // Return the Pk so client knows what he's got
            return books.Entity.Pk;
        }

        /// <summary>
        /// Updates the sort order for the client side books by passing in an already
        /// sorted list of integer Pks. This code then loads each object updates the
        /// sort order with a new value and saves each item.
        /// </summary>
        /// <param name="itemOrder"></param>
        /// <returns></returns>
        [CallbackMethod]
        public bool UpdateSortOrder(int[] itemOrder)
        {
            for (int i = 0; i < itemOrder.Length; i++)
            {
                if (books.Load(itemOrder[i]) != null)
                {
                    books.Entity.SortOrder = (itemOrder.Length - i) * 10;
                    books.Save();
                }
            }

            return true;
        }

        /// <summary>
        /// Retrieves a list of Items from Amazon's Web Service based on query parameters
        /// </summary>
        /// <param name="search"></param>
        /// <param name="type"></param>
        /// <param name="amazonGroup"></param>
        /// <returns></returns>
        [CallbackMethod]
        public List<Amazon.AmazonSearchResult> GetAmazonItems(string search, string type, string amazonGroup)
        {
            AmazonLookup lookup = new AmazonLookup();
            List<AmazonSearchResult> result = lookup.SearchForBook(
                    (type == "Title") ?
                        Amazon.AmazonLookup.SearchCriteria.Title :
                        Amazon.AmazonLookup.SearchCriteria.Author,
                    search,
                    amazonGroup);

            return result;
        }


    }
}
