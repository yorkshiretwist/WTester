using System;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

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
            Query = query;
            SearchType = "standard";
        }
        public Search(string query, string searchType)
        {
            Query = query;
            SearchType = searchType;
        }

        public override void PreAction()
        {
            if (SearchType == "regex")
            {
                PreActionMessage = String.Format("Searching for '{0}' as a Regular Expression", Query);
            }
            else
            {
                PreActionMessage = String.Format("Searching for '{0}'", Query);
            }
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (SearchType == "" || SearchType.ToLower() == "standard")
                {
                    Success = Test.Browser.PageSource.Contains(Query);
                    PostActionMessage = String.Format("Searching for '{0}'", Query);
                }
                if (SearchType.ToLower() == "regex")
                {
                    Success = Regex.IsMatch(Test.Browser.PageSource, Query);
                    PostActionMessage = String.Format("Searched for '{0}' as a Regular Expression", Query);
                }
                Success = false;
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    /// <summary>
    /// Returns the title of the current page
    /// </summary>
    public class Title : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Getting the page title";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                IWebElement el = Test.Browser.FindElement(By.TagName("title"));
                if (el == null)
                {
                    PostActionMessage = "There is no title element";
                    Success = false;
                }
                else
                {

                    PostActionMessage = String.Format("Title for current page is: ", el.Text);
                    Success = true;
                }
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    /// <summary>
    /// Highlights the current element
    /// </summary>
    public class Highlight : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Highlighting the current element(s)";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (Test.CurrentElements != null && Test.CurrentElements.Any())
                {
                    int x = 0;
                    foreach (IWebElement el in Test.CurrentElements)
                    {
                        var js = Test.Browser as IJavaScriptExecutor;
                        // TODO
                        //js.ExecuteScript(".setAttribute('style', '{1}');", el., "color: yellow; border: 2px solid yellow;");
                        x++;
                    }
                    PostActionMessage = String.Format("Elements highlighted: {0}", x);
                    Success = true;
                }
                else
                {
                    PostActionMessage = "There are no elements currently selected";
                    Success = false;
                }
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    /// <summary>
    /// Returns the inner HTML of the current element
    /// </summary>
    public class InnerHtml : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Getting the inner HTML of the current element";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (Test.CurrentElements != null && Test.CurrentElements.Any())
                {
                    // TODO
                    string html = Test.CurrentElements.First().Text;
                    PostActionMessage = String.Format("Element inner HTML: {0}", html);
                    Success = true;
                }
                else
                {
                    PostActionMessage = "There are no elements currently selected";
                    Success = false;
                }
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    /// <summary>
    /// Returns the outer HTML of the current element
    /// </summary>
    public class OuterHtml : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Getting the outer HTML of the current element";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (Test.CurrentElements != null && Test.CurrentElements.Any())
                {
                    // TODO
                    string html = Test.CurrentElements.First().Text;
                    PostActionMessage = String.Format("Element outer HTML: {0}", html);
                    Success = true;
                }
                else
                {
                    PostActionMessage = "There are no elements currently selected";
                    Success = false;
                }
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    /// <summary>
    /// Returns the text of the current element
    /// </summary>
    public class Text : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Getting the text of the current element";
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (Test.CurrentElements != null && Test.CurrentElements.Any())
                {
                    string html = Test.CurrentElements.First().Text;
                    PostActionMessage = String.Format("Element text: {0}", html);
                    Success = true;
                }
                else
                {
                    PostActionMessage = "There are no elements currently selected";
                    Success = false;
                }
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }
}
