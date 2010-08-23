/*
 
JavaScript Minifier application that allows stripping of comments and white
space from JavaScript files. You can use this tool to minify individual 
JavaScript files or a whole directory by using batched path names
with wildcards (*.js).
 
This tool supports both UI operation or basic command line operation so
it can be integrated into a build process
  
 * Copyright (c) 2007 Rick Strahl
 * www.west-wind.com

 * uses Douglas Crockfords Minifier (JavaScriptMinifier.js)
 
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
using System.Windows.Forms;
using System.Threading;

namespace JsMinifier
{
    
    
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // *** Create a handler that contains base settings to be displayed
            //     by the UI
            jsMinifierHandler handler = new jsMinifierHandler();

            // *** Parse command arguments if any and set on handler          
            bool noUI = ProcessCommandLineArguments(args, handler);

            if (noUI)
            {
                if (handler.InputFile == "")
                    return;

                if (!handler.MiniFy())
                    MessageBox.Show("Failed to process: " + handler.ErrorMessage,
                        "JavaScript Minifier",MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
            else
                Application.Run(new JsMinifierForm(handler));        
        }

        /// <summary>
        /// Processes command line arguments and parses them into the handler instance 
        /// passed in.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="handler"></param>
        /// <returns>true if Non-UI processing is to occur. False UI processing should occur</returns>
        private static bool ProcessCommandLineArguments(string[] args, jsMinifierHandler handler)
        {
            bool NoUI = false;

            if (args.Length > 0)
                handler.InputFile = args[0];

            if (handler.InputFile.ToLower() == "help" || handler.InputFile == "/?")
            {
                DisplayCommandArguments();
                handler.InputFile = "";
                return true;
            }

            if (args.Length > 1)
            {
                handler.OutputFile = args[1];
                if (handler.OutputFile.ToLower() == "noui")
                {
                    handler.OutputFile = "";
                    NoUI = true;
                }
            }

            // *** Automatically assume .min.js file extension
            if (string.IsNullOrEmpty(handler.OutputFile))
                handler.OutputFile = handler.GetOutputFile();

            if ((args.Length > 2 && args[2].ToLower() == "noui"))
                NoUI = true;

            return NoUI;
        }

        public static void DisplayCommandArguments()
        {
            MessageBox.Show(
@"JavaScript Minifier Utility Command Line Arguments:

    JsMinifier.exe [Input JavaScript File] [Output File] [NoUI]

NoUI is used to force the application to run without user interface.
NoUI can be specified as the second or third parameter. If second
the output file name is assumed to be changed to .min.js.

    JsMinifier.exe [path\*.js] [.min.js] [NoUI]

If using wildcards before the .js extension all .js files are
converted in the specified path. Second parm is optional and 
can be the new generated extension.
", "JavaScript Minifier",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);        

        }

    }
}
