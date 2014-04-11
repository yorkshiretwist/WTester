using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using stillbreathing.co.uk.WTester;
using FastColoredTextBoxNS;
using System.Text;

namespace stillbreathing.co.uk.WTesterGUI
{
    public partial class WTesterForm : Form
    {
        #region Fields

        /// <summary>
        /// The thread the test will run on
        /// </summary>
        private Thread _testThread;

        #endregion

        #region Constructor

        public WTesterForm()
        {
            InitializeComponent();

            // set up the controls
            btnPause.Enabled = false;
            btnCancel.Enabled = false;
            pbProgress.Enabled = false;

            // load the help file
            rtbHelp.LoadFile(File.OpenRead("help.rtf"), RichTextBoxStreamType.RichText);

            // set up the Scintilla editor
            SetupEditor();
        }

        #endregion

        #region Editor

        /// <summary>
        /// The auto-complete menu
        /// </summary>
        private FastColoredTextBoxNS.AutocompleteMenu popupMenu;

        /// <summary>
        /// The style for highlighting the dollar
        /// </summary>
        TextStyle dollarStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);

        /// <summary>
        /// The style for highlighting strings
        /// </summary>
        TextStyle stringStyle = new TextStyle(Brushes.Brown, null, FontStyle.Regular);

        /// <summary>
        /// The style for highlighting links
        /// </summary>
        TextStyle linkStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);

        /// <summary>
        /// The style for highlighting comments
        /// </summary>
        TextStyle commentStyle = new TextStyle(Brushes.Green, null, FontStyle.Regular);

        /// <summary>
        /// The style for highlighting numbers
        /// </summary>
        TextStyle numberStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);

        /// <summary>
        /// Sets up the Scintilla editor
        /// </summary>
        private void SetupEditor()
        {
            // create the new popup menu
            popupMenu = new FastColoredTextBoxNS.AutocompleteMenu(editor);
            popupMenu.MinFragmentLength = 1;
            // TODO: stop the menu appearing if the caret is within ( brackets )
            //popupMenu.SearchPattern = 
            popupMenu.AppearInterval = 150;
            popupMenu.ToolTipDuration = 1000 * 60 * 5; // 5 minutes
            popupMenu.Items.MaximumSize = new System.Drawing.Size(200, 300);
            popupMenu.Width = 300;
            popupMenu.Items.SetAutocompleteItems(GetActions());

            // set some default text
            editor.Text = "$.browser(\"firefox\")" + Environment.NewLine + "$.load(\"http://www.google.com\")";

            // set the font
            editor.Font = new Font(FontFamily.GenericMonospace, 10);
        }

        /// <summary>
        /// Gets the registered action names
        /// </summary>
        /// <returns></returns>
        private List<AutocompleteItem> GetActions()
        {
            List<AutocompleteItem> items = new List<AutocompleteItem>();
            WTest test = new WTest();
            foreach (KeyValuePair<string, ActionType> kvp in test.ActionTypes)
            {
                StringBuilder parameterNames = new StringBuilder();
                StringBuilder description = new StringBuilder();
                description.Append(kvp.Value.Description);
                if (kvp.Value.Parameters != null && kvp.Value.Parameters.Any())
                {
                    int x = 0;
                    foreach(ActionParameter parameter in kvp.Value.Parameters)
                    {
                        if (x > 0)
                        {
                            parameterNames.Append(", ");
                        }
                        if (parameter.Type == typeof(string))
                        {
                            parameterNames.Append("'" + parameter.Name + "'");
                        }
                        else
                        {
                            parameterNames.Append(parameter.Name);
                        }
                        description.Append(Environment.NewLine);
                        description.AppendFormat("- {0} ({1}, {2})", 
                            parameter.Name,
                            parameter.Type.Name,
                            parameter.IsOptional ? "optional" : "required");
                        x++;
                    }
                }
                items.Add(new AutocompleteItem(
                    string.Format(".{0}({1})", kvp.Key, parameterNames.ToString()),
                    0,
                    kvp.Key + kvp.Value.ParameterString,
                    kvp.Value.Name,
                    description.ToString()));
            }
            return items.OrderBy(i => i.MenuText).ToList();
        }

        /// <summary>
        /// When the text in the editor has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            // set the default highlighter
            //editor.SyntaxHighlighter.InitStyleSchema(Language.JS);
            //editor.SyntaxHighlighter.JScriptSyntaxHighlight(editor.Range);

            e.ChangedRange.ClearStyle(commentStyle, dollarStyle, stringStyle, numberStyle, linkStyle);

            // highlight comments
            e.ChangedRange.SetStyle(commentStyle, new Regex(@"^[\s]*//.*$", RegexOptions.Multiline | RegexOptions.Compiled));

            // highlight dots after dollars or closing braces
            e.ChangedRange.SetStyle(dollarStyle, @"\$\.|\)\.");

            // highlight links
            e.ChangedRange.SetStyle(linkStyle, @"['""](http|https):\/\/([^""']+)", RegexOptions.Compiled);

            // highlight string parameters
            e.ChangedRange.SetStyle(stringStyle, new Regex(@"['""][^'""]*['""]", RegexOptions.Compiled));

            // highlight numbers
            e.ChangedRange.SetStyle(numberStyle, new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b", RegexOptions.Compiled));

            // the background of the current line
            editor.CurrentLineColor = Color.LightYellow;
        }

        /// <summary>
        /// Check if the given place in the editor is in a hyperlink
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        bool CharIsHyperlink(Place place)
        {
            var mask = editor.GetStyleIndexMask(new Style[] { linkStyle });
            if (place.iChar < editor.GetLineLength(place.iLine))
            {
                if ((editor[place].style & mask) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// When the mouse move event happens in the editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editor_MouseMove(object sender, MouseEventArgs e)
        {
            var p = editor.PointToPlace(e.Location);
            if (CharIsHyperlink(p))
            {
                editor.Cursor = Cursors.Hand;
            }
            else
            {
                editor.Cursor = Cursors.IBeam;
            }
        }

        /// <summary>
        /// When the mouse down event happens in the editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editor_MouseDown(object sender, MouseEventArgs e)
        {
            var p = editor.PointToPlace(e.Location);
            if (CharIsHyperlink(p))
            {
                string line = editor.GetRange(p, p).GetFragment(@"[\S]").Text;
                Match match = Regex.Match(line, @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return;
                }
                string url = match.Value;
                System.Diagnostics.Process.Start(url);
            }
        }

        #endregion

        #region File handling

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
                editor.Text = normalized;
                sr.Close();
            }
        }

        /// <summary>
        /// Saves the current test with a "Save As" dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "WTest files (*.wtest)|*.wtest|All files|*.*";
            saveFileDialog1.AddExtension = false;
            saveFileDialog1.DefaultExt = ".wtest";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                if (!filename.ToLower().EndsWith(".wtest"))
                {
                    filename += ".wtest";
                }
                File.WriteAllText(filename, editor.Text);
            }
        }

        #endregion

        #region Test running

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
                    var test = new WTest(editor.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
                    test.TestProgress += TestProgress;
                    test.RunTest(new WTest.ActionResultDelegate(WriteResult));
                });
            _testThread.SetApartmentState(ApartmentState.STA);
            _testThread.Start();
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
            if (_testThread != null && _testThread.ThreadState == ThreadState.Running)
            {
                _testThread.Suspend();
            }
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

        #endregion

        #region Test progress

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

        #endregion
    }
}
