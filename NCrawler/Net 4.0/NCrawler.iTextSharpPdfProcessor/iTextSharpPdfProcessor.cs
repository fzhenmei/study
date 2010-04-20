using System;
using System.Globalization;
using System.Net;
using System.Text;

using iTextSharp.text.pdf;

using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.iTextSharpPdfProcessor
{
	public class iTextSharpPdfProcessor : IPipelineStep
	{
		#region IPipelineStep Members

		public void Process(Crawler crawler, PropertyBag propertyBag)
		{
			AspectF.Define.
				NotNull(crawler, "crawler").
				NotNull(propertyBag, "propertyBag");

			if (propertyBag.StatusCode != HttpStatusCode.OK)
			{
				return;
			}

			if (!IsPdfContent(propertyBag.ContentType))
			{
				return;
			}

			PdfReader pdfReader = new PdfReader(propertyBag.Response);
			try
			{
				object title = pdfReader.Info["Title"];
				if (!title.IsNull())
				{
					string pdfTitle = Convert.ToString(title, CultureInfo.InvariantCulture).Trim();
					if (!pdfTitle.IsNullOrEmpty())
					{
						propertyBag.Title = pdfTitle;
					}
				}

				StringBuilder sb = new StringBuilder();
				// Following code from:
				// http://www.vbforums.com/showthread.php?t=475759
				for (int p = 1; p <= pdfReader.NumberOfPages; p++)
				{
					byte[] pageBytes = pdfReader.GetPageContent(p);

					if (pageBytes.IsNull())
					{
						continue;
					}

					PRTokeniser token = new PRTokeniser(pageBytes);
					while (token.NextToken())
					{
						int tknType = token.TokenType;
						string tknValue = token.StringValue;

						if (tknType == PRTokeniser.TK_STRING)
						{
							sb.Append(token.StringValue);
							sb.Append(" ");
						}
						else if (tknType == 1 && tknValue == "-600")
						{
							sb.Append(" ");
						}
						else if (tknType == 10 && tknValue == "TJ")
						{
							sb.Append(" ");
						}
					}
				}

				propertyBag.Text = sb.ToString();
			}
			finally
			{
				pdfReader.Close();
			}
		}

		#endregion

		#region Class Methods

		private static bool IsPdfContent(string contentType)
		{
			return contentType.StartsWith("application/pdf", StringComparison.OrdinalIgnoreCase);
		}

		#endregion
	}
}