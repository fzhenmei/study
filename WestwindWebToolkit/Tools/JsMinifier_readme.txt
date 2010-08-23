JavaScript Minifier Application
-------------------------------

by Rick Strahl
(c) West Wind Technologies
www.west-wind.com
uses Douglas Crockfords Minifier (JavaScriptMinifier.js)


The JavaScript Minifier application that allows stripping of comments and white
space from JavaScript files. You can use this tool to minify individual 
JavaScript files or a whole directory by using batched path names
with wildcards (*.js).
 
This tool supports both UI operation or basic command line operation so
it can be integrated into a build process.

UI Operation
------------
You can start the application from Explorer or the command line without
parameters or by passing an input filename and output filename as the first
two parameters. The input filename can contain wildcards for the filename
to indicate conversion of each file in the specified directory.

The allows compression to file as well as to the clipboard.

CommandLine Operation
---------------------
To run in command line mode Pass NoUI as the second or third parameter to
the executable. The first parameter specifies the input file or wildcard
directory path to operate on.

Syntax:
JsMinifier.exe [InputFile] [OutputFile] [NoUI]

Examples:
JsMinifier "c:\projects 2008\weblog\scripts\page.js" "c:\projects 2008\weblog\scripts\page.min.js" NOUI
JsMinifier "c:\projects 2008\weblog\scripts\page.js" NOUI
JsMinifier "c:\projects 2008\weblog\scripts\*.js" NOUI
JsMinifier "c:\projects 2008\weblog\scripts\*.js" ".compressed.js" NOUI

If the output file is omitted the output file will be created with the
same name as the input file with a .min.js extension.

If passing wildcards you can optionally specify a result extension as in 
the 4th example above to force all .js files to be compressed with 
a given extension.

If NoUI is not passed the Windows UI will pop up prepopulated with the
values specified on the command line.

Using JsMinifier as a Build Event in Visual Studio
--------------------------------------------------
It's useful to use jsMinifier as a build event handler in Visual Studio
for Web projects so you can automatically create .min.js files.

For example, you have a tools folder at the same level as your Web project
you can hook up jsMinifier like this to compile all .js files in the scripts
directory of your app:

"$(ProjectDir)..\tools\jsminifier" "$(ProjectDir)scripts\*.js" NOUI


License
-------  
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