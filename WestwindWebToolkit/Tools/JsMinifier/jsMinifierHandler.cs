/*

 * Copyright (c) 2007 Rick Strahl
 * www.west-wind.com
 
Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

The Software shall be used for Good, not Evil.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Westwind.Web;

namespace JsMinifier
{
    public class jsMinifierHandler
    {
        
        public string InputFile
        {
            get { return _InputFile; }
            set { _InputFile = value; }
        }
        private string _InputFile = "";


        /// <summary>
        /// When set causes a .min.js file to be generated
        /// </summary>
        public bool GenerateMinFile
        {
            get { return _GenerateMinFile; }
            set { _GenerateMinFile = value; }
        }
        private bool _GenerateMinFile = true;


        /// <summary>
        /// File to which output is sent
        /// </summary>
        public string OutputFile
        {
            get { return _OutputFile; }
            set { _OutputFile = value; }
        }
        private string _OutputFile = "";

        
        public string OutputText
        {
            get { return _OutputText; }
            set { _OutputText = value; }
        }
        private string _OutputText = "";

        
        public int InputSize
        {
            get { return _InputSize; }
            set { _InputSize = value; }
        }
        private int _InputSize = 0;

        
        public int OutputSize
        {
            get { return _OutputSize; }
            set { _OutputSize = value; }
        }
        private int _OutputSize = 0;

        
        public int FilesProcessed
        {
            get { return _FilesProcessed; }
            set { _FilesProcessed = value; }
        }
        private int _FilesProcessed = 0;


        
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
        private string _ErrorMessage = "";


        /// <summary>
        /// Minifies JavaScript content based on the properties of this class.
        /// Takes file input and minifies into OutputFile if set and OutputText
        /// </summary>
        /// <returns>true or false</returns>
        public bool MiniFy()
        {
            try
            {
                this.FilesProcessed = 0;

                JavaScriptMinifier min = new JavaScriptMinifier();

                if (this.InputFile.ToLower().Contains("*.js"))                
                    return this.MinifyBatch();


                bool result =  this.MinifyFile(this.InputFile, this.OutputFile);

                if (!result)
                    return false;

                if (!string.IsNullOrEmpty(this.OutputFile))
                {
                    StreamReader sr = StringUtilities.OpenStreamReaderWithEncoding(this.OutputFile);
                    this.OutputText = sr.ReadToEnd();
                    sr.Close();
                    this.OutputSize = this.OutputText.Length;
                }
               
                this.FilesProcessed = 1;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            	return false;
            }

            return true;            
        }

        public bool MinifyBatch()
        {
            try
            {
                this.FilesProcessed = 0; 
                JavaScriptMinifier min = new JavaScriptMinifier();

                string scriptPath = Path.GetDirectoryName(this.InputFile);
                string renameTo = ".min.js";
                if (!string.IsNullOrEmpty(this.OutputFile) && this.OutputFile.StartsWith("."))
                    renameTo = this.OutputFile.ToLower();
                
                int TotalInputSize = 0;
                int TotalOutputSize = 0;

                string[] scriptFiles = Directory.GetFiles(scriptPath, "*.js");
                foreach (string file in scriptFiles)
                {
                    string scriptFile = file.ToLower();

                    // Skip VS Intellisense files
                    if (scriptFile.Contains("-vsdoc"))
                        continue;

                    if (scriptFile.EndsWith(renameTo))
                        continue;

                    this.MinifyFile(scriptFile, scriptFile.Replace(".js", renameTo));
                    this.FilesProcessed++;

                    TotalInputSize += this.InputSize;
                    TotalOutputSize += this.OutputSize;
                }

                this.InputSize = TotalInputSize;
                this.OutputSize = TotalOutputSize;
            }
            catch (Exception ex)
            {                
                this.ErrorMessage = ex.Message;
                return false;
            }


            return true;
        }

        /// <summary>
        /// Minifies a JavaScript string into a minified JavaScript string
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public string MinifyString(string InputString)
        {
            try
            {
                this.FilesProcessed = 0;

                this.InputSize = InputString.Length;

                JavaScriptMinifier min = new JavaScriptMinifier();
                this.OutputText = min.MinifyString(InputString);
                this.OutputSize = this.OutputText.Length;

                this.FilesProcessed = 1;
                return this.OutputText;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Minifies a JavaScript File into a Minified JavaScript File
        /// </summary>
        /// <param name="InputFileName"></param>
        /// <param name="OutputFileName"></param>
        /// <returns></returns>
        public bool MinifyFile(string InputFileName, string OutputFileName)
        {
            try
            {
                this.FilesProcessed = 0;

                JavaScriptMinifier min = new JavaScriptMinifier();
                min.Minify(InputFileName, OutputFileName);

                Stream file = File.OpenRead(InputFileName);
                this.InputSize = (int) file.Length;
                file.Close();

                file = File.OpenRead(OutputFileName);
                this.OutputSize = (int) file.Length;
                file.Close();

                this.FilesProcessed = 1;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                
            }

            return true;
        }

        /// <summary>
        /// Minifies the inputFile and generates a .min.js file of the same name
        /// </summary>
        /// <param name="InputFileName"></param>
        /// <returns></returns>
        public bool MinifyFile(string InputFileName)
        {
            try
            {
                string OutputFileName = GetOutputFile(InputFileName);
                
                JavaScriptMinifier min = new JavaScriptMinifier();
                min.Minify(InputFileName, OutputFileName);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }

            return true;
        }

        public string GetOutputFile()
        {
            return this.GetOutputFile(this.InputFile);            
        }
        public string GetOutputFile(string InputFileName)
        {
            string OutputFileName = InputFileName.ToLower().Replace(".js", ".min.js");
            return OutputFileName;
        }
    }
}
