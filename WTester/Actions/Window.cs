using System;
using System.Drawing;
using System.Windows.Forms;
using OpenQA.Selenium;
using ScreenOrientation = OpenQA.Selenium.ScreenOrientation;

namespace stillbreathing.co.uk.WTester.Actions.Window
{
    public class Resize : BaseAction
    {
        private int Width;
        private int Height;

        /// <summary>
        /// Resizes the browser window
        /// </summary>
        /// <param name="width">The new window width</param>
        /// <param name="height">The new window height</param>
        public Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Resizing window to {0}x{1}", Width, Height);
        }

        public override void Execute()
        {
            try
            {
                Test.Browser.Manage().Window.Size = new Size(Width, Height);
                Success = true;
                PostActionMessage = String.Format("Resized window to {0}x{1}", Width, Height);
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class Rotate : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Rotating the window";
        }

        public override void Execute()
        {
            try
            {
                var rot = Test.Browser as IRotatable;
                rot.Orientation = rot.Orientation == ScreenOrientation.Portrait
                                      ? ScreenOrientation.Landscape
                                      : ScreenOrientation.Portrait;
                Success = true;
                PostActionMessage = "Rotated the window";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class Maximise : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Maximising the window";
        }

        public override void Execute()
        {
            try
            {
                Test.Browser.Manage().Window.Maximize();
                Success = true;
                PostActionMessage = "Maximized the window";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class Minimise : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Minimising the window";
        }
        
        public override void Execute()
        {
            try
            {
                Success = false;
                PostActionMessage = "Sorry, not currently supported";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class Reset : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Resetting the window size";
        }

        public override void Execute()
        {
            try
            {
                Success = false;
                PostActionMessage = "Sorry, not currently supported";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class NewTab : BaseAction
    {
        private string URL;

        /// <summary>
        /// Opens a new tab with a specified URL
        /// </summary>
        public NewTab(string url)
        {
            URL = url;
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Opening a new tab for {0}", URL);
        }

        public override void Execute()
        {
            try
            {
                Success = true;
                // open a new tab
                SendKeys.SendWait("^{t}");
                // select the address bar text
                SendKeys.SendWait("%{d}");
                // enter the new URL
                SendKeys.SendWait(String.Format("{{0}}", URL));
                // press enter
                SendKeys.SendWait("{ENTER}");
                PostActionMessage = String.Format("Opened a new tab for {0}", URL);
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class GoToTab : BaseAction
    {
        private int TabNumber;

        /// <summary>
        /// Go to the tab with the specified number
        /// </summary>
        public GoToTab(int tabNumber)
        {
            TabNumber = tabNumber;
        }

        public override void PreAction()
        {
            PreActionMessage = "Going to the tab number " + TabNumber;
        }

        public override void Execute()
        {
            try
            {
                SendKeys.SendWait(String.Format("^{{0}}", TabNumber));
                PostActionMessage = "Gone to tab number " + TabNumber;
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class CloseTab : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Closing the current tab";
        }

        public override void Execute()
        {
            try
            {
                Success = true;
                SendKeys.SendWait("^{w}");
                PostActionMessage = "Closed the current tab";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class NextTab : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Going to the next tab";
        }

        public override void Execute()
        {
            try
            {
                Success = true;
                SendKeys.SendWait("^{TAB}");
                PostActionMessage = "Gone to the next tab";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class PreviousTab : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Going to the previous tab";
        }
        
        public override void Execute()
        {
            try
            {
                Success = true;
                SendKeys.SendWait("^+{TAB}");
                PostActionMessage = "Gone to the previous tab";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    public class KeyPress : BaseAction
    {
        private string Keys;

        /// <summary>
        /// Presses the given keys
        /// </summary>
        public KeyPress(string keys)
        {
            Keys = keys;
        }

        public override void PreAction()
        {
            PreActionMessage = "Pressing keys";
        }

        public override void Execute()
        {
            try
            {
                Success = true;
                SendKeys.SendWait(Keys);
                PostActionMessage = String.Format("Sent keys '{0}'", Keys);
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }
}
