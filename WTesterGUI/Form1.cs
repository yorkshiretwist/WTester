using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using stillbreathing.co.uk.WTester;

namespace stillbreathing.co.uk.WTesterGUI
{
    public partial class WTesterForm : Form
    {
        private Thread _testThread;

        public WTesterForm()
        {
            InitializeComponent();
            btnPause.Enabled = false;
            btnCancel.Enabled = false;
            pbProgress.Enabled = false;
            rtbHelp.LoadFile(File.OpenRead("help.rtf"), RichTextBoxStreamType.RichText);
        }

        /// <summary>
        /// Open a test script file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "WTest files (*.wtest)|*.wtest|All files|*.*";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var sr = new StreamReader(openFileDialog1.FileName);
                var text = sr.ReadToEnd();
                string normalized = Regex.Replace(text, @"\r\n|\n\r|\n|\r", "\r\n");
                tbSource.Text = normalized;
                sr.Close();
            }
        }

        /// <summary>
        /// Run the test script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            btnPause.Enabled = true;
            btnCancel.Enabled = true;
            pbProgress.Enabled = true;

            if (_testThread != null && _testThread.ThreadState == ThreadState.Suspended)
            {
                _testThread.Resume();
                return;
            }

            tbOutput.Text = "";
            tabs.SelectTab(1);

            // Do in another thread so the UI does not get sticky.
            _testThread = new Thread(delegate()
                {
                    var test = new WTest(tbSource.Lines);
                    test.TestProgress += TestProgress;
                    test.RunTest(new WTest.ActionResultDelegate(WriteResult));
                });
            _testThread.SetApartmentState(ApartmentState.STA);
            _testThread.Start();
        }

        /// <summary>
        /// When the test thread reports on progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="progressChangedEventArgs"></param>
        private void TestProgress(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, ProgressChangedEventArgs>(TestProgress), new object[] { sender, progressChangedEventArgs });
                return;
            }

            if (progressChangedEventArgs.ProgressPercentage == 100)
            {
                btnCancel.Enabled = false;
                btnPause.Enabled = false;
                btnRun.Enabled = true;
                pbProgress.Value = 0;
                pbProgress.Enabled = false;
                return;
            }
            pbProgress.Value = progressChangedEventArgs.ProgressPercentage;
        }

        /// <summary>
        /// When a keydown event happens in the source textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.A)
            {
                tbSource.SelectAll();
            }
        }

        /// <summary>
        /// Writes out the result to the GUI
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="parameters"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        private void WriteResult(string functionName, List<object> parameters, bool success, string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, List<object>, bool, string>(WriteResult), new object[] { functionName, parameters, success, message });
                return;
            }

            string callingFunction = "$." + functionName + "(";
            foreach (object parameter in parameters)
            {
                callingFunction += parameter is string ? "\"" + parameter + "\", " : parameter + ", ";
            }
            callingFunction = callingFunction.Trim().Trim(',');
            callingFunction += ")";
                
            tbOutput.SelectionColor = Color.Gray;
            tbOutput.AppendText(callingFunction + Environment.NewLine);

            tbOutput.SelectionColor = success ? Color.Green : Color.Red;
            tbOutput.AppendText(message + Environment.NewLine);
        }

        /// <summary>
        /// Pauses the currently running test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPause_Click(object sender, EventArgs e)
        {
            btnPause.Enabled = false;
            btnRun.Enabled = true;
            if (_testThread != null && _testThread.ThreadState == ThreadState.Running) _testThread.Suspend();
        }

        /// <summary>
        /// Cancels the currently running test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnPause.Enabled = false;
            btnRun.Enabled = true;
            pbProgress.Value = 0;
            pbProgress.Enabled = false;
            if (_testThread != null) _testThread.Abort();
        }
    }
}
