using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace NCrawler.Extensions
{
	public static class StreamExtensions
	{
		#region Class Methods

		/// <summary>
		/// Copies any stream into a local MemoryStream
		/// </summary>
		/// <param name="stream">The source stream.</param>
		/// <returns>The copied memory stream.</returns>
		public static MemoryStream CopyToMemory(this Stream stream)
		{
			MemoryStream memoryStream = new MemoryStream((int) stream.Length);
#if NCRAWLER35
			byte[] buffer = stream.ReadAllBytes();
			memoryStream.Write(buffer, 0, buffer.Length);
#else
			stream.CopyTo(memoryStream);
#endif
			return memoryStream;
		}

#if NCRAWLER35
		/// <summary>
		/// Reads the entire stream and returns a byte array.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>The byte array</returns>
		public static byte[] ReadAllBytes(this Stream stream)
		{
			using (MemoryStream s = stream.CopyToMemory())
			{
				return s.ToArray();
			}
		}
#endif

		public static TResult FromBinary<TResult>(this Stream s) where TResult : class, new()
		{
			DataContractSerializer dc = new DataContractSerializer(typeof (TResult));
			return (TResult) dc.ReadObject(s);
		}

		/// <summary>
		/// Opens a StreamReader using the default encoding.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>The stream reader</returns>
		public static StreamReader GetReader(this Stream stream)
		{
			return stream.GetReader(null);
		}

		/// <summary>
		/// Opens a StreamReader using the specified encoding.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>The stream reader</returns>
		public static StreamReader GetReader(this Stream stream, Encoding encoding)
		{
			if (!stream.CanRead)
			{
				throw new InvalidOperationException("Stream does not support reading.");
			}

			return encoding.IsNull()
				? new StreamReader(stream, true)
				: new StreamReader(stream, encoding);
		}

		/// <summary>
		/// Reads all text from the stream using the default encoding.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>The result string.</returns>
		public static string ReadToEnd(this Stream stream)
		{
			return stream.ReadToEnd(null);
		}

		/// <summary>
		/// Reads all text from the stream using a specified encoding.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>The result string.</returns>
		public static string ReadToEnd(this Stream stream, Encoding encoding)
		{
			using (StreamReader reader = stream.GetReader(encoding))
			{
				return reader.ReadToEnd();
			}
		}

		#endregion
	}
}