using System;
using System.Linq;

namespace stillbreathing.co.uk.WTester.Actions.Forms
{
    /// <summary>
    /// Activates a click event on the current element
    /// </summary>
    public class Click : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = String.Format("Clicking the current element: <{0}>[{1}]", Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
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
                    Success = true;
                    Test.CurrentElements.ElementAt(Test.CurrentElementIndex).Click();
                    PostActionMessage = String.Format("Clicked the current element: <{0}>[{1}]", Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
                }
                else
                {
                    PostActionMessage = "There is no current element";
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
            Text = text;
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Typing '{0}' into element <{1}>[{2}]", Text, Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
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
                    Success = true;
                    Test.CurrentElements.ElementAt(Test.CurrentElementIndex).SendKeys(Text);
                    PostActionMessage = String.Format("Typing '{0}' into element <{1}>[{2}]", Text, Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
                }
                else
                {
                    PostActionMessage = "The current element is not set, or is not a valid text input element";
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
