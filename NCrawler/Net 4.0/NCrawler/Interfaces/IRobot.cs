using System;

namespace NCrawler.Interfaces
{
	public interface IRobot
	{
		bool IsAllowed(string userAgent, Uri uri);
	}
}
