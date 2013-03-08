using System;
using System.IO;
using OpenQA.Selenium;
using System.Drawing.Imaging;

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
            FileName = fileName;
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Saving HTML to {0}", FileName);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Success = true;
                File.WriteAllText(FileName, Test.Browser.PageSource);
                PostActionMessage = String.Format("Saved HTML to {0}", FileName);
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
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
            FileName = fileName;
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Saving screenshot to {0}", FileName);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Success = true;
                var shot = Test.Browser as ITakesScreenshot;
                var ss = shot.GetScreenshot();
                ImageFormat format = ImageFormat.Jpeg;
                string ext = Path.GetExtension(FileName);
                switch (ext.ToLower())
                {
                    case ".png":
                        format = ImageFormat.Png;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                string fileName = FileName;
                fileName = new FilenameFormatter(Test).Format(fileName);
                ss.SaveAsFile(fileName, format);
                PostActionMessage = String.Format("Saved screenshot to {0}", FileName);
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class FilenameFormatter
    {
        private WTest _test;

        public FilenameFormatter(WTest test)
        {
            _test = test;
        }

        public string Format(string fileName)
        {
            fileName = fileName.Replace("[width]", _test.Browser.Manage().Window.Size.Width.ToString());
            fileName = fileName.Replace("[height]", _test.Browser.Manage().Window.Size.Height.ToString());
            var cap = _test.Browser as IHasCapabilities;
            fileName = fileName.Replace("[browser]", cap.Capabilities.BrowserName);
            fileName = fileName.Replace("[version]", cap.Capabilities.Version);
            return fileName;
        }
    }
}
