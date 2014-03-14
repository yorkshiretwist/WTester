using System;
using System.Linq;
using stillbreathing.co.uk.WTester.Extensions;
using OpenQA.Selenium;

namespace stillbreathing.co.uk.WTester.Actions.Elements
{
    /// <summary>
    /// Finds elements on the current page
    /// </summary>
    public class Find : BaseAction
    {
        public int Timeout;

        /// <summary>
        /// Find a single element
        /// </summary>
        /// <returns></returns>
        public Find(string selector)
        {
            Selector = selector.Trim().Trim('\'');
        }

        /// <summary>
        /// Find a single element, waiting for the specified period for the element to appear
        /// </summary>
        /// <returns></returns>
        public Find(string selector, int timeout)
        {
            Selector = selector.Trim().Trim('\'');
            Timeout = timeout;
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Finding the element(s) matching this selector: {0}[{1}]", Selector, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (Timeout == 0)
                {
                    Test.CurrentElements = Test.Browser.FindElements(By.CssSelector(Selector));
                }
                else
                {
                    Test.CurrentElements = Test.Browser.FindElements(By.CssSelector(Selector), Timeout);
                }
                if (Test.CurrentElements.Any())
                {
                    Success = true;
                    PostActionMessage = String.Format("Found {0} elements for selector '{1}'", Test.CurrentElements.Count(), Selector);
                }
                else
                {
                    Success = false;
                    PostActionMessage = String.Format("Found no elements for selector '{0}'", Selector);
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
