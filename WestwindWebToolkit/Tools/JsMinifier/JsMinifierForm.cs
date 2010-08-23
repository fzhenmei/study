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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Westwind.Tools;

namespace JsMinifier
{
    public partial class JsMinifierForm : Form
    {

        public jsMinifierHandler Handler
        {
            get { return _Handler; }
            set { _Handler = value; }
        }
        private jsMinifierHandler _Handler = null;


        public string InputFile
        {
            get { return _InputFile; }
            set { _InputFile = value; }
        }
        private string _InputFile = "";


        public string OutputFile
        {
            get { return _OutputFile; }
            set { _OutputFile = value; }
        }
        private string _OutputFile = "";

        
        public string MinifiedOutput
        {
            get { return _MinifiedOutput; }
            set { _MinifiedOutput = value; }
        }
        private string _MinifiedOutput = "";


        public JsMinifierForm(jsMinifierHandler handler)
        {
            this.Handler = handler;
            InitializeComponent();
        }

        private void JsMinifierForm_Load(object sender, EventArgs e)
        {
            this.txtInputFile.Text = this.Handler.InputFile;
            this.txtOutputFile.Text = this.Handler.OutputFile;            
        }

        private void btnCompress_Click(object sender, EventArgs e)
        {
            this.SetStatus("");

            this.Handler.InputFile = this.txtInputFile.Text;
            this.Handler.OutputFile = this.txtOutputFile.Text;

            if ( string.IsNullOrEmpty(this.Handler.OutputFile))
            {
                if (this.Handler.InputFile.ToLower().Contains("*.js"))
                {
                    // *** Force to .min.js extension
                    this.Handler.OutputFile = ".min.js";
                }
                else
                {
                    this.Handler.OutputFile = this.Handler.InputFile.ToLower().Replace(".js", ".min.js");
                }
            }


            if (!this.Handler.MiniFy())
            {
                MessageBox.Show("Unable to convert:\r\n\r\n" + this.Handler.ErrorMessage,
                                "JavaScript Minifier",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            this.txtOutput.Text = this.Handler.OutputText;
            if (Handler.FilesProcessed > 0)
                this.SetStatus( "JavaScript compressed. Files: " + Handler.FilesProcessed.ToString() +
                                        "    Bytes: " + this.Handler.InputSize.ToString("n0") +
                                        " --> " + this.Handler.OutputSize.ToString("n0") +
                                        "  -- " + (100M - ((decimal)((decimal)Handler.OutputSize / (decimal)Handler.InputSize) * 100)).ToString("n2") + "% reduction" );
            else
                this.SetStatus( "No files processed");


            //MessageBox.Show(this.Handler.MinifyString("// Hello\r\nalert('Test  ')  ;\r\n" + this.Handler.OutputText ) );
        }

        private void btnInputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();            
            dialog.RestoreDirectory = true;
            if (!string.IsNullOrEmpty(this.txtOutput.Text))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(this.txtInputFile.Text);
                dialog.FileName = this.txtInputFile.Text;
            }

            dialog.Filter = "JavaScript (*.js)|*.js|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.txtInputFile.Text = dialog.FileName;
            }
        }

        private void btnOutputFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            
            dialog.RestoreDirectory = true;
            if (!string.IsNullOrEmpty(this.txtOutput.Text))
            {
                dialog.FileName = this.txtOutputFile.Text;
                dialog.InitialDirectory = Path.GetDirectoryName(this.txtOutputFile.Text);
            }
            dialog.Filter = "JavaScript (*.js)|*.js|All files (*.*)|*.*";
            dialog.CreatePrompt = false;
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() == DialogResult.OK)
               this.txtOutputFile.Text = dialog.FileName;
            
        }

        private void tbOpenFolder_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(this.txtInputFile.Text);

            ProcessStartInfo start = new ProcessStartInfo();
            start.UseShellExecute = true;
            start.FileName = path;

            Process proc = new Process();
            proc.StartInfo = start;
            proc.Start();
        }

        private void Toolbar_Paint(object sender, PaintEventArgs e)
        {
            this.tbCopyText.Enabled = (this.txtOutput.Text != "");
        }

        private void tbCopyText_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.txtOutput.Text);
            this.SetStatus("Text copied to clipboard.");
        }

        
        /// <summary>
        /// Timer used to clear status message back to its default
        /// </summary>
        System.Threading.Timer statusTimer;

        private void SetStatus(string text)
        {
            if (text == null)
                this.StatusLabel.Text = "Ready";
            else
                this.StatusLabel.Text = text;

            if (!string.IsNullOrEmpty(text))
            {                
                statusTimer = new System.Threading.Timer(new TimerCallback(this.SetStatus),null, 10000, Timeout.Infinite);
                Application.DoEvents();                
            }
        }
        private void SetStatus()
        {
            this.SetStatus(null);
        }
        private void SetStatus(object text)
        {
            try
            {                
                this.BeginInvoke(new ThreadStart(this.SetStatus),null);
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);  }
        }

        private void tbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imgLogo_Click(object sender, EventArgs e)
        {
            wwUtils.GoUrl("http://www.west-wind.com/");
        }

        private void txtInputFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        }

        private void txtInputFile_DragDrop(object sender, DragEventArgs e)
        {
            Array a =  (Array) e.Data.GetData( DataFormats.FileDrop);
            if (a != null)
            {
                string file = a.GetValue(0).ToString();
                ((TextBox)sender).Text = file;                
            }

            // *** automatically add the output filename
            if (sender == this.txtInputFile)
            {
                this.txtOutputFile.Text = this.Handler.GetOutputFile(this.txtInputFile.Text);
            }
        }

  



    }
}
