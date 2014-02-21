using System;
using System.Drawing;
using System.IO;
using OpenQA.Selenium;
using System.Drawing.Imaging;
using stillbreathing.co.uk.WTester.Helpers;

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
                Success = false;

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

                // Chrome has a bug which stops it capturing the whole of the screen
                // see https://code.google.com/p/chromedriver/issues/detail?id=294
                if (Test.BrowserType == BrowserType.Chrome)
                {
                    var ss = new ChromeScreenshot();
                    Bitmap bitmap = ss.GetScreenshot(Test);
                    if (bitmap == null)
                    {
                        PostActionMessage = "Screenshot could not be generated";
                        return;
                    }

                    bitmap.Save(fileName, format);
                    if (!File.Exists(fileName))
                    {
                        PostActionMessage = String.Format("Screenshot could not be saved to {0}", FileName);
                        return;
                    }

                    Success = true;
                    PostActionMessage = String.Format("Saved screenshot to {0}", FileName);
                }
                else
                {
                    var shot = Test.Browser as ITakesScreenshot;

                    if (shot == null)
                    {
                        PostActionMessage = String.Format("Current browser ({0}) does not implement ITakesScreenshot", Test.BrowserType);
                        return;
                    }

                    var ss = shot.GetScreenshot();
                    ss.SaveAsFile(fileName, format);
                    if (!File.Exists(fileName))
                    {
                        PostActionMessage = String.Format("Screenshot could not be saved to {0}", FileName);
                        return;
                    }

                    Success = true;
                    PostActionMessage = String.Format("Saved screenshot to {0}", FileName);
                }
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
        private readonly WTest _test;

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
