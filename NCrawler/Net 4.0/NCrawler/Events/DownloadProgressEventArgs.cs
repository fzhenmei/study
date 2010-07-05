using System;

namespace NCrawler.Events
{
	public class DownloadProgressEventArgs : EventArgs
	{
		#region Instance Properties

		public CrawlStep Step { get; set; }
		public uint BytesReceived { get; set; }
		public uint TotalBytesToReceive { get; set; }
		public TimeSpan DownloadTime { get; set; }

		#endregion
	}
}