using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;

namespace stillbreathing.co.uk.WTester.Actions.Navigation
{
    /// <summary>
    /// Loads the page at a given Uri
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
            this.Uri = GetUri(uri);
        }
        public Load(string uri, string browserType)
        {
            this.Uri = GetUri(uri);
            this.BrowserType = browserType;
        }

        private Uri GetUri(string uri)
        {
            // requests for /some/page.html
            if (uri.StartsWith("/"))
            {
                uri = this.Test.CurrentUri.Host + uri;
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
            this.Test.CurrentUri = this.Uri;
            if (this.BrowserType == "")
            {
                this.PreActionMessage = String.Format("Loading URI '{0}'", this.Uri.ToString());
            }
            else
            {
                this.PreActionMessage = String.Format("Loading URI '{0}' in browser '{1}'", this.Uri.ToString(), this.BrowserType);
            }
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            this.Test.CurrentUri = this.Uri;
            try
            {
                this.Test.Browser.GoTo(this.Test.CurrentUri);
                this.Test.CurrentPage = this.Test.Browser.Page<WTestPage>();
                this.Success = true;
                this.PostActionMessage = String.Format("Loaded URI '{0}'", this.Test.CurrentUri.ToString());
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Closes the current WatiN browser instance
    /// </summary>
    public class Close : BaseAction
    {
        public Close() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Closing the browser";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                this.Test.Browser.Close();
                this.Test.Browser.Dispose();
                this.Test.Browser = null;
                this.Success = true;
                this.PostActionMessage = "Closed the browser";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Waits until the page has completely loaded
    /// </summary>
    public class Wait : BaseAction
    {
        public Wait() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Waiting until the page has completely loaded";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                this.Test.Browser.WaitForComplete();
                this.Success = true;
                this.PostActionMessage = "The page has completely loaded";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Refreshes the current page
    /// </summary>
    public class Refresh : BaseAction
    {
        /// <summary>
        /// Refreshes the current page
        /// </summary>
        public Refresh() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Refreshing the page";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                this.Success = true;
                this.Test.Browser.Refresh();
                this.PostActionMessage = "Page refreshed";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Goes the the previous page
    /// </summary>
    public class Back : BaseAction
    {
        /// <summary>
        /// Goes the the previous page
        /// </summary>
        public Back() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Going to the previous page";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                this.Test.Browser.Back();
                this.PostActionMessage = "Gone to the previous page";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Goes the the next page
    /// </summary>
    public class Forward : BaseAction
    {
        /// <summary>
        /// Goes the the next page
        /// </summary>
        public Forward() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Going forward to the next page";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                this.Test.Browser.Forward();
                this.Success = true;
                this.PostActionMessage = "Gone forward to the next page";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }
}
