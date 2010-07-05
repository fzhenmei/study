using System;

namespace NGet
{
	internal class Program
	{
		#region Class Methods

		private static void Main(string[] args)
		{
			if (args == null || args.Length == 0)
			{
				Arguments.ShowUsageShort();
			}
			else
			{
				Arguments.m_StartupArgumentOptionSet.Parse(args);
			}

			Console.ReadLine();
		}

		#endregion
	}
}