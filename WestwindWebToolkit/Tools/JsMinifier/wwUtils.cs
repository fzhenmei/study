using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.Win32;

using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Web;
using System.Collections;

namespace Westwind.Tools
{
	/// <summary>
	/// wwUtils class which contains a set of common utility classes for 
	/// Formatting strings
	/// Reflection Helpers
	/// Object Serialization
	/// </summary>
	public partial class wwUtils
	{
		#region Miscellaneous Routines 
		
		/// <summary>
		/// Returns the logon password stored in the registry if Auto-Logon is used.
		/// This function is used privately for demos when I need to specify a login username and password.
		/// </summary>
		/// <param name="GetUserName"></param>
		/// <returns></returns>
		public static string GetSystemPassword(bool GetUserName) 
		{
			RegistryKey RegKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon");
			if (RegKey == null)
				return "";           
			
			string Password;
			if (!GetUserName)
				Password = (string) RegKey.GetValue("DefaultPassword");
			else
				Password = (string) RegKey.GetValue("DefaultUsername");

			if (Password == null) 
				return "";

			return (string) Password;
		}

        /// <summary>
        /// Converts the passed date time value to Mime formatted time string
        /// </summary>
        /// <param name="Time"></param>
        public static string MimeDateTime(DateTime Time)
        {
            TimeSpan Offset = TimeZone.CurrentTimeZone.GetUtcOffset(Time);
            
            string sOffset = Offset.Hours.ToString().PadLeft(2, '0');
            if (Offset.Hours < 0)
                sOffset = "-" + (Offset.Hours * -1).ToString().PadLeft(2, '0');

            sOffset += Offset.Minutes.ToString().PadLeft(2,'0');
            
           return "Date: " + DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss",
                                                         System.Globalization.CultureInfo.InvariantCulture) +
                                                         " " + sOffset ;
        }

        /// <summary>
        /// Simple method to retrieve HTTP content from the Web quickly
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, ref string ErrorMessage)
        {
            string ResponseText = "";

            System.Net.WebClient Http = new System.Net.WebClient();

            // Download the Web resource and save it into a data buffer.
            try
            {
                byte[] Result = Http.DownloadData(Url);
                ResponseText = Encoding.Default.GetString(Result);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

            return ResponseText;
        }

        /// <summary>
        /// Retrieves a buffer of binary data from a URL using
        /// a plain HTTP Get.
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static byte[] HttpGetBytes(string Url, ref string ErrorMessage)
        {
            byte[] Result = null;

            System.Net.WebClient Http = new System.Net.WebClient();

            try
            {
                Result = Http.DownloadData(Url);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

            return Result;
        }

        /// <summary>
        /// Copies the content of the one stream to another.
        /// Streams must be open and stay open.
        /// </summary>
        public static void CopyStream(Stream source, Stream dest, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int read;
            while ( (read = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, read);
            }
        }
    

		#endregion

		#region Path Functions
		/// <summary>
		/// Returns the full path of a full physical filename
		/// </summary>
		/// <param name="Path"></param>
		/// <returns></returns>
		public static string JustPath(string Path) 
		{
			FileInfo fi = new FileInfo(Path);
			return fi.DirectoryName + "\\";
		}

        /// <summary>
        /// Returns a fully qualified path from a partial or relative
        /// path.
        /// </summary>
        /// <param name="Path"></param>
        public static string GetFullPath(string Path)
        {
            if (string.IsNullOrEmpty(Path))
                return "";

            return System.IO.Path.GetFullPath(Path);
        }

		/// <summary>
		/// Returns a relative path string from a full path.
		/// </summary>
		/// <param name="FullPath">The path to convert. Can be either a file or a directory</param>
		/// <param name="BasePath">The base path to truncate to and replace</param>
		/// <returns>
		/// Lower case string of the relative path. If path is a directory it's returned without a backslash at the end.
		/// 
		/// Examples of returned values:
		///  .\test.txt, ..\test.txt, ..\..\..\test.txt, ., ..
		/// </returns>
		public static string GetRelativePath(string FullPath, string BasePath ) 
		{
			// *** Start by normalizing paths
			FullPath = FullPath.ToLower();
			BasePath = BasePath.ToLower();

			if ( BasePath.EndsWith("\\") ) 
				BasePath = BasePath.Substring(0,BasePath.Length-1);
			if ( FullPath.EndsWith("\\") ) 
				FullPath = FullPath.Substring(0,FullPath.Length-1);

			// *** First check for full path
			if ( (FullPath+"\\").IndexOf(BasePath + "\\") > -1) 
				return  FullPath.Replace(BasePath,".");

			// *** Now parse backwards
			string BackDirs = "";
			string PartialPath = BasePath;
			int Index = PartialPath.LastIndexOf("\\");
			while (Index > 0) 
			{
				// *** Strip path step string to last backslash
				PartialPath = PartialPath.Substring(0,Index );
			
				// *** Add another step backwards to our pass replacement
				BackDirs = BackDirs + "..\\" ;

				// *** Check for a matching path
				if ( FullPath.IndexOf(PartialPath) > -1 ) 
				{
					if ( FullPath == PartialPath )
						// *** We're dealing with a full Directory match and need to replace it all
						return FullPath.Replace(PartialPath,BackDirs.Substring(0,BackDirs.Length-1) );
					else
						// *** We're dealing with a file or a start path
						return FullPath.Replace(PartialPath+ (FullPath == PartialPath ?  "" : "\\"),BackDirs);
				}
				Index = PartialPath.LastIndexOf("\\",PartialPath.Length-1);
			}

			return FullPath;
		}
		#endregion

		#region Shell Functions for displaying URL, HTML, Text and XML
		[DllImport("Shell32.dll")]
		private static extern int ShellExecute(int hwnd, string lpOperation, 
			string lpFile, string lpParameters, 
			string lpDirectory, int nShowCmd);

		/// <summary>
		/// Uses the Shell Extensions to launch a program based or URL moniker.
		/// </summary>
		/// <param name="lcUrl">Any URL Moniker that the Windows Shell understands (URL, Word Docs, PDF, Email links etc.)</param>
		/// <returns></returns>
		public static int GoUrl(string Url)
		{
			string TPath = Path.GetTempPath();

			int Result = ShellExecute(0,"OPEN",Url, "",TPath,1);
			return Result;
		}

		/// <summary>
		/// Displays an HTML string in a browser window
		/// </summary>
		/// <param name="HtmlString"></param>
		/// <returns></returns>
		public static int ShowString(string HtmlString,string extension) 
		{
			if (extension == null)
				extension = "htm";

			string File = Path.GetTempPath() + "\\__preview." + extension;
			StreamWriter sw = new StreamWriter(File,false,Encoding.Default);
			sw.Write( HtmlString);
			sw.Close();									

			return GoUrl(File);
		}

		public static int ShowHtml(string HtmlString) 
		{
			return ShowString(HtmlString,null);
		}

		/// <summary>
		/// Displays a large Text string as a text file.
		/// </summary>
		/// <param name="TextString"></param>
		/// <returns></returns>
		public static int ShowText(string TextString) 
		{
			string File = Path.GetTempPath() + "\\__preview.txt";

			StreamWriter sw = new StreamWriter(File,false);
			sw.Write(TextString);
			sw.Close();

			return GoUrl(File);
		}
		#endregion
	}

}
