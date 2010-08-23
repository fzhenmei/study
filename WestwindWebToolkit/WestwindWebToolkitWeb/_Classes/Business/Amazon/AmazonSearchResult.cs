using System;

namespace Amazon
{
    public class AmazonSearchResult
    {

        /// <summary>
        /// Item id - ISBN number or other id
        /// </summary>
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _Id = "";


        /// <summary>
        /// The title of the Item
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private string _Title = "";

        
        /// <summary>
        /// Name of the Author or Authors. Multiple authors are comma delimited
        /// </summary>
        public string Author
        {
            get { return _Author; }
            set { _Author = value; }
        }
        private string _Author = "";


        /// <summary>
        /// The Url to the item back at Amazon
        /// </summary>
        public string ItemUrl
        {
            get { return _ItemUrl; }
            set { _ItemUrl = value; }
        }
        private string _ItemUrl = "";


        /// <summary>
        /// Medium sized Image Url for this item
        /// </summary>
        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }
        private string _ImageUrl = "";

        
        /// <summary>
        /// Small image url for this item
        /// </summary>
        public string SmallImageUrl
        {
            get { return _SmallImageUrl; }
            set { _SmallImageUrl = value; }
        }
        private string _SmallImageUrl = "";

        
        /// <summary>
        /// Determines whether the large or small image is displayed
        /// </summary>
        public bool UseSmallImage
        {
            get { return _UseSmallImage; }
            set { _UseSmallImage = value; }
        }
        private bool _UseSmallImage = false;

        
        /// <summary>
        /// Determines whether the layout is web based and meant
        /// for a sidebar rather than as main content in the post.
        /// </summary>
        public bool UseWebLayout
        {
            get { return _UseWebLayout; }
            set { _UseWebLayout = value; }
        }

        private bool _UseWebLayout = false;


        /// <summary>
        /// Date the book or item was published or made available
        /// </summary>
        public string PublicationDate
        {
            get { return _PublicationDate; }
            set { _PublicationDate = value; }
        }
        private string _PublicationDate = null;

        /// <summary>
        /// Name of the publisher of the book or product
        /// </summary>
        public string Publisher
        {
            get { return _Publisher; }
            set { _Publisher = value; }
        }
        private string _Publisher = null;

        /// <summary>
        /// Not used (not available)
        /// </summary>
        public string Abstract
        {
            get { return _Abstract; }
            set { _Abstract = value; }
        }
        private string _Abstract = null;
    }
}
