using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using stillbreathing.co.uk.WTester;

namespace stillbreathing.co.uk.WTesterGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                tbSource.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            tbOutput.Text = "";
            WTest test = new WTest(tbSource.Lines);
            test.RunTest(new WTest.ActionResultDelegate(WriteResult));
        }

        private void WriteResult(string functionName, List<object> parameters, bool success, string message)
        {
            string callingFunction = "$." + functionName + "(";
            foreach (object parameter in parameters)
            {
                if (parameter is string)
                {
                    callingFunction += "\"" + parameter.ToString() + "\", ";
                }
                else
                {
                    callingFunction += parameter.ToString() + ", ";
                }
            }
            callingFunction = callingFunction.Trim().Trim(',');
            callingFunction += ")";
            tbOutput.SelectionColor = Color.Gray;
            tbOutput.AppendText(callingFunction + Environment.NewLine);
            if (success)
            {
                tbOutput.SelectionColor = Color.Green;
            }
            else
            {
                tbOutput.SelectionColor = Color.Red;
            }
            tbOutput.AppendText(message + Environment.NewLine);
        }
    }
}
