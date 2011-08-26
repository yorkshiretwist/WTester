using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace stillbreathing.co.uk.WTester.Actions.Output
{
    /// <summary>
    /// Saves the HTML of the page to a file
    /// </summary>
    public class Save : BaseAction
    {
        public string FileName;

        public Save(string fileName)
        {
            this.FileName = fileName;
        }

        public override void PreAction()
        {
            this.PreActionMessage = String.Format("Saving HTML to {0}", this.FileName);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                this.Success = true;
                File.WriteAllText(this.FileName, this.Test.Browser.Html);
                this.PostActionMessage = String.Format("Saved HTML to {0}", this.FileName);
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Saves a screenshot of the page to a file
    /// </summary>
    public class Screenshot : BaseAction
    {
        public string FileName;

        public Screenshot(string fileName)
        {
            this.FileName = fileName;
        }

        public override void PreAction()
        {
            this.PreActionMessage = String.Format("Saving screenshot to {0}", this.FileName);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                this.Success = true;
                this.Test.Browser.CaptureWebPageToFile(this.FileName);
                this.PostActionMessage = String.Format("Saved screenshot to {0}", this.FileName);
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }
}
