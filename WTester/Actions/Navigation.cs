using System;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace stillbreathing.co.uk.WTester.Actions.Navigation
{
    /// <summary>
    /// Loads the page at the given URI
    /// </summary>
    public class Load : BaseAction
    {
        private Uri Uri;
        private string BrowserType;

        /// <summary>
        /// Load the given Uri
        /// </summary>
        public Load(string uri)
        {
            Uri = GetUri(uri);
        }
        public Load(string uri, string browserType)
        {
            Uri = GetUri(uri);
            BrowserType = browserType;
        }

        private Uri GetUri(string uri)
        {
            // requests for /some/page.html
            if (uri.StartsWith("/"))
            {
                uri = Test.CurrentUri.Host + uri;
            }
            // requests for www.google.com
            if (!uri.StartsWith("http"))
            {
                uri = "http://" + uri;
            }
            return new Uri(uri);
        }

        public override void PreAction()
        {
            Test.CurrentUri = Uri;
            PreActionMessage = BrowserType == "" ? String.Format("Loading URI '{0}'", Uri) : String.Format("Loading URI '{0}' in browser '{1}'", Uri, BrowserType);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            Test.CurrentUri = Uri;
            try
            {
                Test.Browser.Navigate().GoToUrl(Test.CurrentUri);
                Success = true;
                PostActionMessage = String.Format("Loaded URI '{0}'", Test.CurrentUri);
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
                    Description = "The URI to load",
                    IsOptional = false,
                    DefaultValue = null
                });
                parameters.Add(new ActionParameter
                {
                    Name = "browserType",
                    Type = typeof(string),
                    Description = "The type of browser to use (either 'firefox', 'ie' or 'chrome')",
                    IsOptional = true,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Closes the current browser instance
    /// </summary>
    public class Close : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Closing the browser";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Test.Browser.Close();
                Test.Browser.Dispose();
                Test.Browser = null;
                Success = true;
                PostActionMessage = "Closed the browser";
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
    /// Refreshes the current page
    /// </summary>
    public class Refresh : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Refreshing the page";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Success = true;
                Test.Browser.Navigate().Refresh();
                PostActionMessage = "Page refreshed";
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
    /// Goes the the previous page
    /// </summary>
    public class Back : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Going to the previous page";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Test.Browser.Navigate().Back();
                PostActionMessage = "Gone to the previous page";
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
    /// Goes the the next page (after previously going back)
    /// </summary>
    public class Forward : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Going forward to the next page";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Test.Browser.Navigate().Forward();
                Success = true;
                PostActionMessage = "Gone forward to the next page";
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
}
