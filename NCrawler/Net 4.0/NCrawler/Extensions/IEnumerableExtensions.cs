using System;
using System.Collections.Generic;
using System.Linq;

namespace NCrawler.Extensions
{
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Iterates through all sequence and performs specified action on each
		/// element
		/// </summary>
		/// <typeparam name="T">Sequence element type</typeparam>
		/// <param name="enumerable">Target enumeration</param>
		/// <param name="action">Action</param>
		/// <exception cref="System.ArgumentNullException">One of the input agruments is null</exception>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (T elem in enumerable)
			{
				action(elem);
			}
		}

		public static string Join<T>(this IEnumerable<T> target, string separator)
		{
			return target.IsNull()
				? string.Empty
				: string.Join(separator, target.Select(i => i.ToString()).ToArray());
		}
	}
}