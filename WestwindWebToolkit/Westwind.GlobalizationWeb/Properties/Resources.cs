using System;
using System.Web;

namespace AppResources
{
	public class Commonphrases
	{
		public static System.String Yesterday
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Commonphrases","Yesterday"); }
		}

		public static System.String Today
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Commonphrases","Today"); }
		}

		public static System.String Save
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Commonphrases","Save"); }
		}

	}

	public class Resources
	{
		public static System.String NoGermanResources
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Resources","NoGermanResources"); }
		}

		public static System.String Yesterday
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Resources","Yesterday"); }
		}

		public static System.String CustomerSaved
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Resources","CustomerSaved"); }
		}

		public static System.String CouldNotCreateNewCustomer
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Resources","CouldNotCreateNewCustomer"); }
		}

		public static System.String Today
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Resources","Today"); }
		}

		public static System.String ThisIsATest
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Resources","ThisIsATest"); }
		}

		public static System.String CouldNotLoadCustomer
		{
			get { return (System.String) HttpContext.GetGlobalResourceObject("Resources","CouldNotLoadCustomer"); }
		}

		public static System.Drawing.Bitmap rick120
		{
			get { return (System.Drawing.Bitmap) HttpContext.GetGlobalResourceObject("Resources","rick120"); }
		}

		public static System.Drawing.Bitmap Sailbig
		{
			get { return (System.Drawing.Bitmap) HttpContext.GetGlobalResourceObject("Resources","Sailbig"); }
		}

	}

}
