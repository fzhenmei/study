#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          © West Wind Technologies, 2009
 *          http://www.west-wind.com/
 * 
 * Created: 09/12/2009
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 **************************************************************  
*/
#endregion

using System;
using System.IO;
using System.Text;
using Microsoft.Win32;

using System.Runtime.InteropServices;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Westwind.Utilities
{
	/// <summary>
	/// wwUtils class which contains a set of common utility classes for 
	/// Formatting strings
	/// Reflection Helpers
	/// Object Serialization
	/// </summary>
	public partial class Utils
	{
		
		/// <summary>
		/// Returns the logon password stored in the registry if Auto-Logon is used.
		/// This function is used privately for demos when I need to specify a login username and password.
		/// </summary>
		/// <param name="getUserName"></param>
		/// <returns></returns>
		public static string GetSystemPassword(bool getUserName) 
		{
			RegistryKey RegKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon");
			if (RegKey == null)
				return string.Empty;           
			
			string Password;
			if (!getUserName)
				Password = (string) RegKey.GetValue("DefaultPassword");
			else
				Password = (string) RegKey.GetValue("DefaultUsername");

			if (Password == null) 
				return string.Empty;

			return (string) Password;
		}


   

        /// <summary>
        /// Simple method to retrieve HTTP content from the Web quickly
        /// </summary>
        /// <param name="url"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static string HttpGet(string url, ref string errorMessage)
        {
            string responseText = string.Empty;

            WebClient Http = new WebClient();

            // Download the Web resource and save it into a data buffer.
            try
            {
                byte[] Result = Http.DownloadData(url);
                responseText = Encoding.Default.GetString(Result);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }

            return responseText;
        }

        /// <summary>
        /// Retrieves a buffer of binary data from a URL using
        /// a plain HTTP Get.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static byte[] HttpGetBytes(string url, ref string errorMessage)
        {
            byte[] result = null;

            WebClient Http = new WebClient();

            try
            {
                result = Http.DownloadData(url);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }

            return result;
        }



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

			int Result = ShellExecute(0,"OPEN",Url, string.Empty,TPath,1);
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

	}

}
