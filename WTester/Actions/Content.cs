using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace stillbreathing.co.uk.WTester.Actions.Content
{
    /// <summary>
    /// Searches for text on the current page
    /// </summary>
    public class Search : BaseAction
    {
        public string Query;
        public string SearchType;

        /// <summary>
        /// Searches for text on the current page
        /// </summary>
        /// <returns></returns>
        public Search(string query)
        {
            this.Query = query;
            this.SearchType = "standard";
        }
        public Search(string query, string searchType)
        {
            this.Query = query;
            this.SearchType = searchType;
        }

        public override void PreAction()
        {
            if (this.SearchType == "regex")
            {
                this.PreActionMessage = String.Format("Searching for '{0}' as a Regular Expression", this.Query);
            }
            else
            {
                this.PreActionMessage = String.Format("Searching for '{0}'", this.Query);
            }
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (this.SearchType == "" || this.SearchType.ToLower() == "standard")
                {
                    this.Success = this.Test.Browser.ContainsText(this.Query);
                    this.PostActionMessage = String.Format("Searching for '{0}'", this.Query);
                }
                if (this.SearchType.ToLower() == "regex")
                {
                    Regex regex = new Regex(this.Query);
                    this.Success = this.Test.Browser.ContainsText(regex);
                    this.PostActionMessage = String.Format("Searched for '{0}' as a Regular Expression", this.Query);
                }
                this.Success = false;
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Returns the title of the current page
    /// </summary>
    public class Title : BaseAction
    {
        public Title() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Getting the page title";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (this.Test.CurrentPage == null)
                {
                    this.PostActionMessage = "There is no current page";
                    this.Success = false;
                }
                else
                {
                    this.PostActionMessage = String.Format("Title for current page is: ", this.Test.Browser.Title);
                    this.Success = true;
                }
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Highlights the current element
    /// </summary>
    public class Highlight : BaseAction
    {
        public Highlight() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Highlighting the current element(s)";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (this.Test.CurrentElements != null && this.Test.CurrentElements.Count > 0)
                {
                    int x = 0;
                    foreach (Element el in this.Test.CurrentElements)
                    {
                        el.Highlight(true);
                        x++;
                    }
                    this.PostActionMessage = String.Format("Elements highlighted: {0}", x);
                    this.Success = true;
                }
                else
                {
                    this.PostActionMessage = "There are no elements currently selected";
                    this.Success = false;
                }
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Returns the inner HTML of the current element
    /// </summary>
    public class InnerHtml : BaseAction
    {
        public InnerHtml() {  }

        public override void PreAction()
        {
            this.PreActionMessage = "Getting the inner HTML of the current element";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (this.Test.CurrentElements != null && this.Test.CurrentElements.Count > 0)
                {
                    string html = this.Test.CurrentElements[0].InnerHtml;
                    this.PostActionMessage = String.Format("Element inner HTML: {0}", html);
                    this.Success = true;
                }
                else
                {
                    this.PostActionMessage = "There are no elements currently selected";
                    this.Success = false;
                }
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Returns the outer HTML of the current element
    /// </summary>
    public class OuterHtml : BaseAction
    {
        public OuterHtml() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Getting the outer HTML of the current element";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (this.Test.CurrentElements != null && this.Test.CurrentElements.Count > 0)
                {
                    string html = this.Test.CurrentElements[0].OuterHtml;
                    this.PostActionMessage = String.Format("Element outer HTML: {0}", html);
                    this.Success = true;
                }
                else
                {
                    this.PostActionMessage = "There are no elements currently selected";
                    this.Success = false;
                }
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Returns the text of the current element
    /// </summary>
    public class Text : BaseAction
    {
        public Text() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Getting the text of the current element";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (this.Test.CurrentElements != null && this.Test.CurrentElements.Count > 0)
                {
                    string html = this.Test.CurrentElements[0].Text;
                    this.PostActionMessage = String.Format("Element text: {0}", html);
                    this.Success = true;
                }
                else
                {
                    this.PostActionMessage = "There are no elements currently selected";
                    this.Success = false;
                }
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }
}
