using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace stillbreathing.co.uk.WTester.Actions
{
    /// <summary>
    /// The base Action from which all internal WTester Action derive
    /// </summary>
	public abstract class BaseAction : IAction
	{
        /// <summary>
        /// Gets or sets the WTest that this Action belongs to
        /// </summary>
        public WTest Test { get; set; }

        /// <summary>
        /// Gets or sets the message shown just before the action is invoked
        /// </summary>
        public string PreActionMessage { get; set; }

        /// <summary>
        /// Gets or sets the message shown just after the action is invoked
        /// </summary>
        public string PostActionMessage { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating the succes or failure of the Action
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The method that is called just before the action is executed
        /// </summary>
        public abstract void PreAction();

        /// <summary>
        /// The method that is invoked when the Action is run
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// The selector given to find the elements on which to perform this action
        /// </summary>
        public string Selector;

        /// <summary>
        /// The elements on which the action will be performed
        /// </summary>
        public IEnumerable<IWebElement> Elements;

        /// <summary>
        /// Gets the first element in the elements found for the current selection
        /// </summary>
        public IWebElement FirstElement
        {
            get 
            { 
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null)
                {
                    return null;
                }

                return elements.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the last element in the elements found for the current selection
        /// </summary>
        public IWebElement LastElement
        {
            get
            {
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null)
                {
                    return null;
                }

                return elements.LastOrDefault();
            }
        }

        /// <summary>
        /// Finds elements with the given selector and assigns them to the Elements property
        /// </summary>
        /// <param name="selector"></param>
        protected void FindElements(string selector)
        {
            if (!string.IsNullOrWhiteSpace(selector))
            {
                Elements = Test.Browser.FindElements(By.CssSelector(selector));
            }
        }

        /// <summary>
        /// Gets the elements to be used
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<IWebElement> GetElements()
        {
            if (Elements != null && Elements.Any())
            {
                return Elements;
            }

            return Test.CurrentElements;
        }
	}
}
