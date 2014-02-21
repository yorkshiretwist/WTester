using System;
using System.Linq;
using OpenQA.Selenium;

namespace stillbreathing.co.uk.WTester.Actions.Elements
{
    /// <summary>
    /// Finds elements on the current page
    /// </summary>
    public class Find : BaseAction
    {
        /// <summary>
        /// Find a single element
        /// </summary>
        /// <returns></returns>
        public Find(string selector)
        {
            Selector = selector.Trim().Trim('\'');
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
                Test.CurrentElements = Test.Browser.FindElements(By.CssSelector(Selector));
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
