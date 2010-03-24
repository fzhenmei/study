using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;


namespace chapter4.Web
{

    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GetBookInfo
    {
        //Sample book data array
        private BookInfo[] books = new BookInfo[3];

        //initialize the books object
        private void initBooks()
        {
            if (books != null)
            {
                books[0] = new BookInfo();
                books[0].Title = "Pro Silverlight for the Enterprise";
                books[0].Author = "Ashish Ghoda";
                books[0].Chapters = new List<string> 
        { "Chapter 1- Understanding Silverlight", 
        "Chapter 2-Developing a Simple Silverlight Application", 
        "Chapter 3-Silverlight: An Enterprise-Ready Technology Platform",
        "Chapter 4-Silverlight and Service-Oriented Architecture",
        "Chapter 5-Developing a Service-Oriented Enterpise RIA"};
                books[0].ISBN = "978-1-4302-1867-8";

                books[1] = new BookInfo();
                books[1].Title = "Pro Silverlight 2 in C# 2008";
                books[1].Author = "Matthew MacDonald";
                books[1].Chapters = new List<string> 
        { "Chapter 1-Introducing Silverlight", 
        "Chapter 2-XAML", 
        "Chapter 3-Layout",
        "Chapter 4-Dependency Properties and Routed Events",
        "Chapter 5-Elements"};
                books[1].ISBN = "978-1-59059-949-5";

                books[2] = new BookInfo();
                books[2].Title = "Silverlight 2 Visual Essentials";
                books[2].Author = "Matthew MacDonald";
                books[2].Chapters = new List<string> 
        { "Chapter 1-Introducing Silverlight", 
        "Chapter 2-Layout", 
        "Chapter 3-Dependency Properties and Routed Events",
        "Chapter 4-Elements",
        "Chapter 5-The Application Model"};
                books[2].ISBN = "978-1-4302-1582-0";
            }

        }

        //Get books by title
        [OperationContract]
        [FaultContract(typeof(BookNotFound))]
        public BookInfo GetByTitle(string Title)
        {
            initBooks();
            foreach (var item in books)
            {
                if (item.Title.ToUpper() == Title.ToUpper())
                    return item;
            }
            return null;
        }
        public class BookNotFound
        {
            public string NotFoundMessage { get; set; }
        }
        //Get all book titles
        [OperationContract]
        public List<string> GetAllTitle()
        {
            initBooks();
            List<string> allTitles = new List<string>();
            foreach (var item in books)
            {
                allTitles.Add(item.Title);
            }
            return allTitles;
        }
    }
    //


    //
  
    
}


 

