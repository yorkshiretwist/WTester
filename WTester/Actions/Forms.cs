using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;

namespace stillbreathing.co.uk.WTester.Actions.Forms
{
    /// <summary>
    /// Activates a click event on the current element
    /// </summary>
    public class Click : BaseAction
    {
        /// <summary>
        /// Activates a click
        /// </summary>
        public Click() { }

        public override void PreAction()
        {
            this.PreActionMessage = String.Format("Clicking the current element: <{0}>[{1}]", this.Test.CurrentElements[this.Test.CurrentElementIndex].TagName, this.Test.CurrentElementIndex);
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
                    this.Success = true;
                    this.Test.CurrentElements[this.Test.CurrentElementIndex].Click();
                    this.PostActionMessage = String.Format("Clicked the current element: <{0}>[{1}]", this.Test.CurrentElements[this.Test.CurrentElementIndex].TagName, this.Test.CurrentElementIndex);
                }
                else
                {
                    this.PostActionMessage = "There is no current element";
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
    /// Types text into the current element
    /// </summary>
    public class TypeText : BaseAction
    {
        public string Text;

        /// <summary>
        /// Types text into the current element
        /// </summary>
        public TypeText(string text)
        {
            this.Text = text;
        }

        public override void PreAction()
        {
            this.PreActionMessage = String.Format("Typing '{0}' into element <{1}>[{2}]", this.Text, this.Test.CurrentElements[this.Test.CurrentElementIndex].TagName, this.Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (this.Test.CurrentElements != null && this.Test.CurrentElements.Count > 0 && this.Test.CurrentElements[this.Test.CurrentElementIndex] is TextField)
                {
                    this.Success = true;
                    ((TextField)this.Test.CurrentElements[this.Test.CurrentElementIndex]).TypeText(this.Text);
                    this.PostActionMessage = String.Format("Typing '{0}' into element <{1}>[{2}]", this.Text, this.Test.CurrentElements[this.Test.CurrentElementIndex].TagName, this.Test.CurrentElementIndex);
                }
                else
                {
                    this.PostActionMessage = "The current element is not set, or is not a valid text input element";
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
