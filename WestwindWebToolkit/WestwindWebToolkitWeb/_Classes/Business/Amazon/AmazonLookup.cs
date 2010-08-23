
using System;
using System.Collections.Generic;
using System.Linq;
using wsAmazon;
using System.Configuration;
using Westwind.WebToolkit;


namespace Amazon
{    
    /// <summary>
    /// Retrieves a list of search results from the Amazon Web Service
    /// filling an array of SearchResults
    /// </summary>
    [Serializable]
    public partial class AmazonLookup
    {
        public enum SearchCriteria
        {
            Title = 0,
            Author = 1
        }

        public List<AmazonSearchResult> SearchForBook(SearchCriteria criteria, string queryText)
        {
            return SearchForBook(criteria, queryText, "Books");
        }

        public List<AmazonSearchResult> SearchForBook(SearchCriteria criteria, string queryText, string searchGroup)
        {
            // What it does: Search for a book based on criteria specified and query text

            // Check for null querytext
            if (queryText.Length == 0)
                throw new ApplicationException("Please enter some text to search for");

            List<AmazonSearchResult> results = new List<AmazonSearchResult>();
            List<ItemSearchRequest> searchRequests = new List<ItemSearchRequest>();

            // Create new instance of Amazon.com eCommerce Web Service
            AWSECommerceService service = new AWSECommerceService();

            // The following block uses WSE3 Helper extensions to sign the message
            service.Destination = new Uri(App.AmazonConfiguration.AmazonServiceEndPointUrl);
            AmazonHmacAssertion amazonHmacAssertion =
                    new AmazonHmacAssertion(App.AmazonConfiguration.AmazonAWSAccessKey,
                                            App.AmazonConfiguration.AmazonSecretId);
            service.SetPolicy(amazonHmacAssertion.Policy());

            ItemSearch itemSearch = new ItemSearch();
            itemSearch.SubscriptionId = App.AmazonConfiguration.AmazonAWSAccessKey; 
            itemSearch.AssociateTag = App.AmazonConfiguration.AmazonAssociateId;


            string[] groups = { searchGroup };
            foreach (string group in groups)
            {
                // Create ItemSearch Request
                ItemSearchRequest request = new ItemSearchRequest();

                // Set ResponseGroup details to return
                request.ResponseGroup = new string[] { "Images", "Medium", "ItemAttributes" };

                // Set Search Parameters
                request.SearchIndex = group;

                if (criteria == SearchCriteria.Author)
                {
                    if (request.SearchIndex == "Books")
                        request.Author = queryText;
                    else if (request.SearchIndex == "Music")
                        request.Artist = queryText;
                    else if (request.SearchIndex == "DigitalMusic" || request.SearchIndex == "DVD")
                        request.Actor = queryText;
                    else if (request.SearchIndex == "Software" || request.SearchIndex == "PCHardware")
                        request.Manufacturer = queryText;
                }

                if (criteria == SearchCriteria.Title)
                    request.Title = queryText;

                searchRequests.Add(request);

            }
            // Setup Request
            itemSearch.Request = searchRequests.ToArray(); // new ItemSearchRequest[] { request };


            // Get the Response
            ItemSearchResponse response = service.ItemSearch(itemSearch);
            if (response == null)
            {
                throw new Exception("Server Error...No Response Received.");
            }

            Items[] itemsArray = response.Items;

            // Check for errors, stop at first error...
            if (itemsArray == null)
            {
                throw new Exception("Server Error ... Empty Response!");
            }
            if (itemsArray[0].Request.Errors != null)
            {
                throw new Exception(itemsArray[0].Request.Errors[0].Message);
            }

            // Response Group Images
            foreach (Items items in itemsArray)
            {
                // Items:Item
                Item[] itemArray = items.Item;          

                if (itemArray != null)
                {
                    foreach (Item item in itemArray)
                    {
                        if (item != null)
                        {
                            // Create new result instance                                
                            AmazonSearchResult result = new AmazonSearchResult(); ;
                            results.Add(result);

                            result.Id = item.ASIN;

                            // Item:Image
                            Image image = item.MediumImage; // SmallImage;

                            if (image != null)
                                result.ImageUrl = image.URL;

                            image = item.SmallImage;
                            if (image != null)
                                result.SmallImageUrl = image.URL;

                            // Details Page URL
                            result.ItemUrl = item.DetailPageURL;

                            // Item:ItemAttributes
                            ItemAttributes itemAttributes = item.ItemAttributes;

                            if (item.EditorialReviews != null && item.EditorialReviews.Length > 0)
                                result.Abstract = item.EditorialReviews[0].Content;

                            if (itemAttributes != null)
                            {
                                result.Title = itemAttributes.Title;
                                result.PublicationDate = itemAttributes.PublicationDate;

                                // Try to format the publication date into something nicer looking
                                DateTime date = DateTime.MinValue;
                                if (DateTime.TryParse(result.PublicationDate, out date))
                                    result.PublicationDate = date.ToString("MMMM d, yyyy");

                                result.Publisher = itemAttributes.Label;
                                                                

                                string[] authorArray = null;

                                if (itemAttributes.Author != null)
                                    authorArray = itemAttributes.Author;
                                else if (itemAttributes.Artist != null)
                                    authorArray = itemAttributes.Artist;
                                else if (itemAttributes.Actor != null)
                                    authorArray = itemAttributes.Actor;

                                if (authorArray != null)
                                {
                                    foreach (string author in authorArray)
                                    {
                                        if (authorArray.Length > 1)
                                            result.Author += author + ", ";
                                        else
                                            result.Author = author;
                                    }
                                }

                                if (string.IsNullOrEmpty(result.Author) && itemAttributes.Manufacturer != null)
                                    result.Author = itemAttributes.Manufacturer;

                                // Trim "," at end of authors
                                result.Author = result.Author.Trim().TrimEnd(Convert.ToChar(","));
                            }
                        }                    
                    }
                }
            }

            // Return the results
            return results ;

        }
    }
}
