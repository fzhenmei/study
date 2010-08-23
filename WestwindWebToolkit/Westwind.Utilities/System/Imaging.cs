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

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Westwind.Utilities
{
	/// <summary>
	/// Summary description for wwImaging.
	/// </summary>
	public class Imaging
	{
		/// <summary>
		/// Creates a resized bitmap from an existing image on disk. Resizes the image by 
		/// creating an aspect ratio safe image. Image is sized to the larger size of width
		/// height and then smaller size is adjusted by aspect ratio.
		/// 
		/// Image is returned as Bitmap - call Dispose() on the returned Bitmap object
		/// </summary>
		/// <param name="lcFilename"></param>
		/// <param name="lnWidth"></param>
		/// <param name="lnHeight"></param>
		/// <returns>Bitmap or null</returns>
		public static Bitmap ResizeImage(string lcFilename,int lnWidth, int lnHeight)
		{
			Bitmap bmpOut = null;

			try 
			{
				Bitmap loBMP = new Bitmap(lcFilename);
				ImageFormat loFormat = loBMP.RawFormat;

				decimal lnRatio;
				int lnNewWidth = 0;
				int lnNewHeight = 0;

				//*** If the image is smaller than a thumbnail just return it
				if (loBMP.Width < lnWidth && loBMP.Height < lnHeight) 
					return loBMP;
			
				if (loBMP.Width > loBMP.Height)
				{
					lnRatio = (decimal) lnWidth / loBMP.Width;
					lnNewWidth = lnWidth;
					decimal lnTemp = loBMP.Height * lnRatio;
					lnNewHeight = (int)lnTemp;
				}
				else 
				{
					lnRatio = (decimal) lnHeight / loBMP.Height;
					lnNewHeight = lnHeight;
					decimal lnTemp = loBMP.Width * lnRatio;
					lnNewWidth = (int) lnTemp;
				}

				bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
				Graphics g = Graphics.FromImage(bmpOut);
				g.InterpolationMode =InterpolationMode.HighQualityBicubic;
				g.FillRectangle( Brushes.White,0,0,lnNewWidth,lnNewHeight);
				g.DrawImage(loBMP,0,0,lnNewWidth,lnNewHeight);

				//System.Drawing.Image imgOut = loBMP.GetThumbnailImage(lnNewWidth,lnNewHeight,null,IntPtr.Zero);
				loBMP.Dispose();
			}
			catch 
			{
				return null;
			}
		
			return bmpOut;
		}	

		public static bool ResizeImage(string lcFilename, string lcOutputFilename, 
			                           int lnWidth, int lnHeight)
		{
			Bitmap bmpOut = null;

			try 
			{
				Bitmap loBMP = new Bitmap(lcFilename);
				ImageFormat loFormat = loBMP.RawFormat;

				decimal lnRatio;
				int lnNewWidth = 0;
				int lnNewHeight = 0;

				//*** If the image is smaller than a thumbnail just return it
				if (loBMP.Width < lnWidth && loBMP.Height < lnHeight) 
				{ 
					loBMP.Save(lcOutputFilename);
					return true;
				}

				if (loBMP.Width > loBMP.Height)
				{
					lnRatio = (decimal) lnWidth / loBMP.Width;
					lnNewWidth = lnWidth;
					decimal lnTemp = loBMP.Height * lnRatio;
					lnNewHeight = (int)lnTemp;
				}
				else 
				{
					lnRatio = (decimal) lnHeight / loBMP.Height;
					lnNewHeight = lnHeight;
					decimal lnTemp = loBMP.Width * lnRatio;
					lnNewWidth = (int) lnTemp;
				}

				bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
				Graphics g = Graphics.FromImage(bmpOut);
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.FillRectangle( Brushes.White,0,0,lnNewWidth,lnNewHeight);
				g.DrawImage(loBMP,0,0,lnNewWidth,lnNewHeight);

				//System.Drawing.Image imgOut = loBMP.GetThumbnailImage(lnNewWidth,lnNewHeight,null,IntPtr.Zero);
				loBMP.Dispose();

				bmpOut.Save(lcOutputFilename,loFormat);
				bmpOut.Dispose();
			}
			catch 
			{
				return false;
			}
		
			return true;
		}	
	}
}
