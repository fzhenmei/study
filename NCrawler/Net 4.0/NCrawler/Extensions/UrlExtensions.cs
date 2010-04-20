using System;

namespace NCrawler.Extensions
{
	public static class UrlExtensions
	{
		public static string NormalizeUri(this string url, string baseUrl)
		{
			if (url.IsNullOrEmpty())
			{
				return baseUrl;
			}

			if (Uri.IsWellFormedUriString(url, UriKind.Relative))
			{
				if (!baseUrl.IsNullOrEmpty())
				{
					Uri absoluteBaseUrl = new Uri(baseUrl, UriKind.Absolute);
					return new Uri(absoluteBaseUrl, url).ToString();
				}

				return new Uri(url, UriKind.Relative).ToString();
			}

			if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
			{
				// Only handle same schema as base uri
				Uri baseUri = new Uri(baseUrl);
				Uri uri = new Uri(url);

				bool schemaMatch;

				// Special case for http/https
				if ((baseUri.Scheme == Uri.UriSchemeHttp) ||
					(baseUri.Scheme == Uri.UriSchemeHttps))
				{
					schemaMatch = string.Compare(Uri.UriSchemeHttp, uri.Scheme, StringComparison.OrdinalIgnoreCase) == 0 ||
						string.Compare(Uri.UriSchemeHttps, uri.Scheme, StringComparison.OrdinalIgnoreCase) == 0;
				}
				else
				{
					schemaMatch = string.Compare(baseUri.Scheme, uri.Scheme, StringComparison.OrdinalIgnoreCase) == 0;
				}

				if (schemaMatch)
				{
					return new Uri(url, UriKind.Absolute).ToString();
				}
			}

			return null;
		}
	}
}
