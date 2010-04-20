using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using HtmlAgilityPack;

using NCrawler.Extensions;
using NCrawler.HtmlProcessor.Extensions;
using NCrawler.HtmlProcessor.Properties;
using NCrawler.Interfaces;
using NCrawler.Utils;

namespace NCrawler.HtmlProcessor
{
	public class HtmlDocumentProcessor : CrawlerRules, IPipelineStep
	{
		#region Constructors

		public HtmlDocumentProcessor()
			: this(null, null)
		{
		}

		public HtmlDocumentProcessor(Dictionary<string, string> filterTextRules,
			Dictionary<string, string> filterLinksRules)
			: base(filterTextRules, filterLinksRules)
		{
		}

		#endregion

		#region Instance Methods

		protected virtual string NormalizeLink(string baseUrl, string link)
		{
			return link.NormalizeUri(baseUrl);
		}

		#endregion

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

			if (!IsHtmlContent(propertyBag.ContentType))
			{
				return;
			}

			HtmlDocument htmlDoc = new HtmlDocument
				{
					OptionAddDebuggingAttributes = false,
					OptionAutoCloseOnEnd = true,
					OptionFixNestedTags = true,
					OptionReadEncoding = true
				};
			using (MemoryStream reader = propertyBag.GetResponseStream())
			{
				Encoding documentEncoding = htmlDoc.DetectEncoding(reader);
				reader.Seek(0, SeekOrigin.Begin);
				if (!documentEncoding.IsNull())
				{
					htmlDoc.Load(reader, documentEncoding, true);
				}
				else
				{
					htmlDoc.Load(reader, true);
				}
			}

			string originalContent = htmlDoc.DocumentNode.OuterHtml;
			if (HasTextStripRules || HasSubstitutionRules)
			{
				string content = StripText(originalContent);
				content = Substitute(content, propertyBag.Step);
				using (TextReader tr = new StringReader(content))
				{
					htmlDoc.Load(tr);
				}
			}

			HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//title");
			// Extract Title
			if (!nodes.IsNull())
			{
				propertyBag.Title = string.Join(";", nodes.
					Select(n => n.InnerText).
					ToArray()).Trim();
			}

			// Extract Meta Data
			nodes = htmlDoc.DocumentNode.SelectNodes("//meta[@content and @name]");
			if (!nodes.IsNull())
			{
				propertyBag["Meta"].Value = (
					from entry in nodes
					let name = entry.Attributes["name"]
					let content = entry.Attributes["content"]
					where !name.IsNull() && !name.Value.IsNullOrEmpty() && !content.IsNull() && !content.Value.IsNullOrEmpty()
					select name.Value + ": " + content.Value).ToArray();
			}

			propertyBag.Text = htmlDoc.ExtractText().Trim();
			if (HasLinkStripRules || HasTextStripRules)
			{
				string content = StripLinks(originalContent);
				using (TextReader tr = new StringReader(content))
				{
					htmlDoc.Load(tr);
				}
			}

			// Extract Links
			DocumentWithLinks links = htmlDoc.GetLinks();
			foreach (string link in links.Links.Union(links.References))
			{
				if (link.IsNullOrEmpty())
				{
					continue;
				}

				string baseUrl = propertyBag.ResponseUri.GetLeftPart(UriPartial.Path);
				string decodedLink = ExtendedHtmlUtility.HtmlEntityDecode(link);
				string normalizedLink = NormalizeLink(baseUrl, decodedLink);
				if (normalizedLink.IsNullOrEmpty())
				{
					continue;
				}

				crawler.AddStep(new Uri(normalizedLink), propertyBag.Step.Depth + 1,
					propertyBag.Step, new Dictionary<string, object>
						{
							{Resources.PropertyBagKeyOriginalUrl, link},
							{Resources.PropertyBagKeyOriginalReferrerUrl, propertyBag.ResponseUri}
						});
			}
		}

		#endregion

		#region Class Methods

		private static bool IsHtmlContent(string contentType)
		{
			return contentType.StartsWith("text/html", StringComparison.OrdinalIgnoreCase);
		}

		#endregion
	}
}