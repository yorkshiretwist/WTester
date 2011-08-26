using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;

namespace stillbreathing.co.uk.WTester.Actions.Elements
{
    /// <summary>
    /// Finds elements on the current page
    /// </summary>
    public class Find : BaseAction
    {
        public string Selector;

        /// <summary>
        /// Find a single element
        /// </summary>
        /// <returns></returns>
        public Find(string selector)
        {
            this.Selector = selector;
        }

        public override void PreAction()
        {
            this.PreActionMessage = String.Format("Finding the element(s) matching this selector: {0}[{1}]", this.Selector, this.Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                this.Test.CurrentElements = this.Test.Browser.Elements.Filter(WatiN.Core.Find.BySelector(this.Selector));
                if (this.Test.CurrentElements.Count > 0)
                {
                    this.Success = true;
                    this.PostActionMessage = String.Format("Found {0} elements", this.Test.CurrentElements.Count);
                }
                else
                {
                    this.Success = false;
                    this.PostActionMessage = "Found no elements";
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
