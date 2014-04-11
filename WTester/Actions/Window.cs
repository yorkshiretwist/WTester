using System;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using OpenQA.Selenium;
using ScreenOrientation = OpenQA.Selenium.ScreenOrientation;
using System.Collections.Generic;

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

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                parameters.Add(new ActionParameter
                {
                    Name = "width",
                    Type = typeof(int),
                    Description = "The new width of the window",
                    IsOptional = false,
                    DefaultValue = null
                });
                parameters.Add(new ActionParameter
                {
                    Name = "height",
                    Type = typeof(int),
                    Description = "The new height of the window",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Rotates the window, switching between landscape and portrait
    /// </summary>
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

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                return parameters;
            }
        }
    }

    /// <summary>
    /// Maximises the browser window
    /// </summary>
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

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                return parameters;
            }
        }
    }

    /// <summary>
    /// Minimises the browser window
    /// </summary>
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

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                return parameters;
            }
        }
    }

    /// <summary>
    /// Resets the window size
    /// </summary>
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

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                return parameters;
            }
        }
    }

    /// <summary>
    /// Opens a new tab
    /// </summary>
    public class NewTab : BaseAction
    {
        private string URI;

        /// <summary>
        /// Opens a new tab with a specified URL
        /// </summary>
        public NewTab(string uri)
        {
            URI = uri;
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Opening a new tab for {0}", URI);
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
                SendKeys.SendWait(String.Format("{{0}}", URI));
                // press enter
                SendKeys.SendWait("{ENTER}");
                PostActionMessage = String.Format("Opened a new tab for {0}", URI);
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                parameters.Add(new ActionParameter
                {
                    Name = "uri",
                    Type = typeof(string),
                    Description = "The URI to open in the new tab",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Goes to the tab with the given index
    /// </summary>
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

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                parameters.Add(new ActionParameter
                {
                    Name = "tabNumber",
                    Type = typeof(int),
                    Description = "The tab number to go to",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Closes the current tab
    /// </summary>
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

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                return parameters;
            }
        }
    }

    /// <summary>
    /// Goes to the next tab
    /// </summary>
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

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                return parameters;
            }
        }
    }

    /// <summary>
    /// Goes to the previous tab
    /// </summary>
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

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                return parameters;
            }
        }
    }

    /// <summary>
    /// Sends the given keys to the browser
    /// </summary>
    public class KeyPress : BaseAction
    {
        public string KeysText;
        public string Keys;

        /// <summary>
        /// Presses the given keys
        /// </summary>
        public KeyPress(string keysText)
        {
            KeysText = keysText;

            if (!KeysText.Contains("{"))
            {
                GetKeys();
            }
        }

        public override void PreAction()
        {
            PreActionMessage = "Pressing keys";
        }

        /// <summary>
        /// Translates the given keys text into the correct keys class
        /// </summary>
        private void GetKeys()
        {
            if (string.IsNullOrWhiteSpace(KeysText))
            {
                return;
            }

            try
            {
                MemberInfo[] keysMembers = typeof(OpenQA.Selenium.Keys).GetMembers(BindingFlags.Static | BindingFlags.Public);
                foreach (MemberInfo keysMember in keysMembers)
                {
                    if (keysMember.MemberType == MemberTypes.Field && keysMember.Name.ToLower() == KeysText.ToLower())
                    {
                        Keys = ((FieldInfo)keysMember).GetValue(null).ToString();
                        return;
                    }
                }
            }
            catch
            {

            }
        }

        public override void Execute()
        {
            try
            {
                Success = true;
                SendKeys.SendWait(Keys == null ? KeysText : Keys);
                PostActionMessage = String.Format("Sent keys '{0}'", KeysText);
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }

        /// <summary>
        /// The parameters for this method
        /// </summary>
        internal static List<ActionParameter> Parameters
        {
            get
            {
                List<ActionParameter> parameters = new List<ActionParameter>();
                parameters.Add(new ActionParameter
                {
                    Name = "keysText",
                    Type = typeof(string),
                    Description = "The keys to send to the tab",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }
}
