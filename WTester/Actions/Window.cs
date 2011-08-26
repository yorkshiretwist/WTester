using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WatiN.Core;

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
            this.Width = width;
            this.Height = height;
        }

        public override void PreAction()
        {
            this.PreActionMessage = String.Format("Resizing window to {0}x{1}", this.Width, this.Height);
        }

        public override void Execute()
        {
            try
            {
                this.Test.Browser.SizeWindow(this.Width, this.Height);
                this.Success = true;
                this.PostActionMessage = String.Format("Resized window to {0}x{1}", this.Width, this.Height);
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    public class Maximise : BaseAction
    {
        /// <summary>
        /// Maximises the window
        /// </summary>
        public Maximise() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Maximising the window";
        }

        public override void Execute()
        {
            try
            {
                this.Test.Browser.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.ShowMaximized);
                this.Success = true;
                this.PostActionMessage = "Maximized the window";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    public class Minimise : BaseAction
    {
        /// <summary>
        /// Minimises the window
        /// </summary>
        public Minimise() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Minimising the window";
        }
        
        public override void Execute()
        {
            try
            {
                this.Test.Browser.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.ShowMinimized);
                this.Success = true;
                this.PostActionMessage = "Minimized the window";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    public class Reset : BaseAction
    {
        /// <summary>
        /// Resets the window size back to the original size
        /// </summary>
        public Reset() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Resetting the window size";
        }

        public override void Execute()
        {
            try
            {
                this.Test.Browser.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.ShowNormal);
                this.Success = true;
                this.PostActionMessage = "Reset the window size";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
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
            this.URL = url;
        }

        public override void PreAction()
        {
            this.PreActionMessage = String.Format("Opening a new tab for {0}", this.URL);
        }

        public override void Execute()
        {
            try
            {
                this.Success = true;
                // open a new tab
                SendKeys.SendWait("^{t}");
                // select the address bar text
                SendKeys.SendWait("%{d}");
                // enter the new URL
                SendKeys.SendWait(String.Format("{{0}}", this.URL));
                // press enter
                SendKeys.SendWait("{ENTER}");
                this.PostActionMessage = String.Format("Opened a new tab for {0}", this.URL);
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
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
            this.TabNumber = tabNumber;
        }

        public override void PreAction()
        {
            this.PreActionMessage = "Going to the tab number " + this.TabNumber;
        }

        public override void Execute()
        {
            try
            {
                SendKeys.SendWait(String.Format("^{{0}}", this.TabNumber));
                this.PostActionMessage = "Gone to tab number " + this.TabNumber;
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    public class CloseTab : BaseAction
    {
        /// <summary>
        /// Closes the current tab
        /// </summary>
        public CloseTab() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Closing the current tab";
        }

        public override void Execute()
        {
            try
            {
                this.Success = true;
                SendKeys.SendWait("^{w}");
                this.PostActionMessage = "Closed the current tab";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    public class NextTab : BaseAction
    {
        /// <summary>
        /// Go to the next tab
        /// </summary>
        public NextTab() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Going to the next tab";
        }

        public override void Execute()
        {
            try
            {
                this.Success = true;
                SendKeys.SendWait("^{TAB}");
                this.PostActionMessage = "Gone to the next tab";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    public class PreviousTab : BaseAction
    {
        /// <summary>
        /// Go to the previous tab
        /// </summary>
        public PreviousTab() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Going to the previous tab";
        }
        
        public override void Execute()
        {
            try
            {
                this.Success = true;
                SendKeys.SendWait("^+{TAB}");
                this.PostActionMessage = "Gone to the previou tab";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }
}
