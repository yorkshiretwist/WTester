using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using stillbreathing.co.uk.WTester.Helpers;

namespace stillbreathing.co.uk.WTester.Actions.Forms
{
    /// <summary>
    /// Activates a click event on the current selected element
    /// </summary>
    public class Click : BaseAction
    {
        /// <summary>
        /// Activates a click event on the element matching the given selector
        /// </summary>
        /// <param name="selector"></param>
        public Click(string selector)
        {
            Selector = selector;
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Clicking the current element: <{0}>[{1}]", Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (FirstElement != null)
                {
                    Success = true;
                    FirstElement.Click();
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Activates a click event on the last selected element
    /// </summary>
    public class ClickLast : BaseAction
    {
        /// <summary>
        /// Activates a click event on the last element matching the given selector
        /// </summary>
        /// <param name="selector"></param>
        public ClickLast(string selector)
        {
            Selector = selector;
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Clicking the current element: <{0}>[{1}]", Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (LastElement != null)
                {
                    Success = true;
                    LastElement.Click();
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Selects a random option in the current select element
    /// </summary>
    public class SelectRandom : BaseAction
    {
        /// <summary>
        /// Selects a random item in the elements matching the given selector
        /// </summary>
        public SelectRandom(string selector)
        {
            Selector = selector;
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Selecting random item in the current element: <{0}>[{1}]", Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null || !elements.Any())
                {
                    PostActionMessage = "There are no current elements";
                    Success = false;
                    return;
                }

                int succeededElements = 0;
                int failedElements = 0;
                Exception lastException = null;

                foreach (IWebElement element in elements)
                {
                    string tag = element.TagName.ToLower();
                    if (tag != "select")
                    {
                        failedElements++;
                    }
                    else
                    {
                        try
                        {
                            SelectElement select = new SelectElement(element);
                            // get the indexes of items with a value in a list
                            List<int> indexes = new List<int>();
                            for (int x = 0; x < select.Options.Count; x++)
                            {
                                IWebElement option = select.Options[x];
                                if (!string.IsNullOrWhiteSpace(option.GetAttribute("value")))
                                {
                                    indexes.Add(x);
                                }
                            }
                            Random rand = new Random();
                            select.SelectByIndex(rand.Next(indexes.Count));
                            succeededElements++;
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            failedElements++;
                        }
                    }
                }

                // no elements succeeded
                if (succeededElements == 0)
                {
                    Success = false;
                    PostActionMessage = lastException == null ?
                        "None of the elements for the current selector are <select> elements" :
                        String.Format("None of the elements could be selected (last exception: {0}", lastException.Message);
                    return;
                }

                // at least one element succeeded
                PostActionMessage = String.Format(
                        "Select random item succeeded for {0} element(s), failed for {1} elements(s)",
                        succeededElements,
                        failedElements);
                Success = true;
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Selects an option by value in the current select element
    /// </summary>
    public class SelectValue : BaseAction
    {
        public string Value;

        /// <summary>
        /// Selects the value in the current select element
        /// </summary>
        public SelectValue(object value)
        {
            Value = value == null ? string.Empty : value.ToString();
        }

        /// <summary>
        /// Selects the value in the elements matching the given selector
        /// </summary>
        public SelectValue(string selector, object value)
        {
            Selector = selector;
            Value = value == null ? string.Empty : value.ToString();
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Selecting value '{0}' the current element: <{1}>[{2}]", Value, Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null || !elements.Any())
                {
                    PostActionMessage = "There are no current elements";
                    Success = false;
                    return;
                }

                int succeededElements = 0;
                int failedElements = 0;
                Exception lastException = null;

                foreach (IWebElement element in elements)
                {
                    string tag = element.TagName.ToLower();
                    if (tag != "select")
                    {
                        failedElements++;
                    }
                    else
                    {
                        try
                        {
                            SelectElement select = new SelectElement(element);
                            select.SelectByValue(Value);
                            succeededElements++;
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            failedElements++;
                        }
                    }
                }

                // no elements succeeded
                if (succeededElements == 0)
                {
                    Success = false;
                    PostActionMessage = lastException == null ? 
                        "None of the elements for the current selector are <select> elements" : 
                        String.Format("None of the elements could be selected (last exception: {0}", lastException.Message);
                    return;
                }
                        
                // at least one element succeeded
                PostActionMessage = String.Format(
                        "Select value '{0}' succeeded for {1} element(s), failed for {2} elements(s)", Value,
                        succeededElements,
                        failedElements);
                Success = true;
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = true,
                    DefaultValue = null
                });
                parameters.Add(new ActionParameter
                {
                    Name = "value",
                    Type = typeof(object),
                    Description = "The value of the option to select",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Selects an option by text in the current select element
    /// </summary>
    public class SelectText : BaseAction
    {
        public string Text;

        /// <summary>
        /// Selects the text in the current select element
        /// </summary>
        public SelectText(object text)
        {
            Text = text == null ? string.Empty : text.ToString();
        }

        /// <summary>
        /// Selects the text in the elements matching the given selector
        /// </summary>
        public SelectText(string selector, object text)
        {
            Selector = selector;
            Text = text == null ? string.Empty : text.ToString();
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Selecting text '{0}' the current element: <{1}>[{2}]", Text, Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null || !elements.Any())
                {
                    PostActionMessage = "There are no current elements";
                    Success = false;
                    return;
                }

                int succeededElements = 0;
                int failedElements = 0;
                Exception lastException = null;

                foreach (IWebElement element in elements)
                {
                    string tag = element.TagName.ToLower();
                    if (tag != "select")
                    {
                        failedElements++;
                    }
                    else
                    {
                        try
                        {
                            SelectElement select = new SelectElement(element);
                            select.SelectByText(Text);
                            succeededElements++;
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            failedElements++;
                        }
                    }
                }

                // no elements succeeded
                if (succeededElements == 0)
                {
                    Success = false;
                    PostActionMessage = lastException == null ?
                        "None of the elements for the current selector are <select> elements" :
                        String.Format("None of the elements could be selected (last exception: {0}", lastException.Message);
                    return;
                }

                // at least one element succeeded
                PostActionMessage = String.Format(
                        "Select text '{0}' succeeded for {1} element(s), failed for {2} elements(s)", Text,
                        succeededElements,
                        failedElements);
                Success = true;
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = true,
                    DefaultValue = null
                });
                parameters.Add(new ActionParameter
                {
                    Name = "text",
                    Type = typeof(string),
                    Description = "The text of the option to select",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Selects the option at the given index in the current select element
    /// </summary>
    public class SelectIndex : BaseAction
    {
        public int Index;

        /// <summary>
        /// Selects the item at the given index in the current select element
        /// </summary>
        public SelectIndex(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Selects the text in the elements matching the given selector
        /// </summary>
        public SelectIndex(string selector, int index)
        {
            Selector = selector;
            Index = index;
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Selecting index '{0}' the current element: <{1}>[{2}]", Index, Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null || !elements.Any())
                {
                    PostActionMessage = "There are no current elements";
                    Success = false;
                    return;
                }

                int succeededElements = 0;
                int failedElements = 0;
                Exception lastException = null;

                foreach (IWebElement element in elements)
                {
                    string tag = element.TagName.ToLower();
                    if (tag != "select")
                    {
                        failedElements++;
                    }
                    else
                    {
                        try
                        {
                            SelectElement select = new SelectElement(element);
                            select.SelectByIndex(Index);
                            succeededElements++;
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            failedElements++;
                        }
                    }
                }

                // no elements succeeded
                if (succeededElements == 0)
                {
                    Success = false;
                    PostActionMessage = lastException == null ?
                        "None of the elements for the current selector are <select> elements" :
                        String.Format("None of the elements could be selected (last exception: {0}", lastException.Message);
                    return;
                }

                // at least one element succeeded
                PostActionMessage = String.Format(
                        "Select index '{0}' succeeded for {1} element(s), failed for {2} elements(s)", Index,
                        succeededElements,
                        failedElements);
                Success = true;
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = true,
                    DefaultValue = null
                });
                parameters.Add(new ActionParameter
                {
                    Name = "index",
                    Type = typeof(int),
                    Description = "The index of the option to select",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
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

        /// <summary>
        /// Types text into the elements matching the given selector
        /// </summary>
        public TypeText(string selector, string text)
        {
            Selector = selector;
            Text = text;
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Typing '{0}' into element <{1}>[{2}]", Text, Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null || !elements.Any())
                {
                    PostActionMessage = "The current element is not set, or is not a valid text input element";
                    Success = false;
                    return;
                }

                int succeededElements = 0;
                int failedElements = 0;
                Exception lastException = null;

                foreach (IWebElement element in elements)
                {
                    try
                    {
                        element.SendKeys(Text);
                        succeededElements++;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        failedElements++;
                    }
                        
                    PostActionMessage = String.Format("Typing '{0}' into element <{1}>[{2}]", Text,
                                                        Test.CurrentElements.ElementAt(Test.CurrentElementIndex)
                                                            .TagName, Test.CurrentElementIndex);
                }

                // no elements succeeded
                if (succeededElements == 0)
                {
                    Success = false;
                    PostActionMessage = lastException == null ? 
                        "None of the elements could be typed into" : 
                        String.Format("None of the elements could be typed into (last exception encountered: {0})", lastException.Message);
                    return;
                }

                // at least one element succeeded
                PostActionMessage = String.Format(
                        "Typing '{0}' succeeded for {1} element(s), failed for {2} elements(s)", Text,
                        succeededElements,
                        failedElements);
                Success = true;
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = true,
                    DefaultValue = null
                });
                parameters.Add(new ActionParameter
                {
                    Name = "text",
                    Type = typeof(string),
                    Description = "The text to type",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Types random alphabetical text into the current element
    /// </summary>
    public class RandomText : BaseAction
    {
        public int MinLength = 2;
        public int MaxLength = 10;
        public bool WithSpaces = false;

        /// <summary>
        /// Types random text of the given length into the current element
        /// </summary>
        public RandomText(int maxlength)
        {
            MaxLength = maxlength;
        }

        /// <summary>
        /// Types random text into the current element with the given minimum and maximum lengths
        /// </summary>
        public RandomText(int minlength, int maxlength)
        {
            MinLength = minlength;
            MaxLength = maxlength;
        }

        /// <summary>
        /// Types random text into the elements matching the given selector with the given maximum length
        /// </summary>
        public RandomText(string selector, int maxlength)
        {
            Selector = selector;
            MaxLength = maxlength;
        }

        /// <summary>
        /// Types random text into the elements matching the given selector with the given minimum and maximum lengths
        /// </summary>
        public RandomText(string selector, int minlength, int maxlength)
        {
            Selector = selector;
            MinLength = minlength;
            MaxLength = maxlength;
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Typing {0}-{1} random alphabetical characters long into element <{2}>[{3}]", MinLength, MaxLength, Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null || !elements.Any())
                {
                    PostActionMessage = "The current element is not set, or is not a valid text input element";
                    Success = false;
                    return;
                }

                int succeededElements = 0;
                int failedElements = 0;
                Exception lastException = null;

                foreach (IWebElement element in elements)
                {
                    try
                    {
                        // get random alphabetical text of the given length
                        Randomiser random = new Randomiser();
                        element.SendKeys(random.GetAlphabeticalText(MinLength, MaxLength, WithSpaces));
                        succeededElements++;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        failedElements++;
                    }

                    PostActionMessage = String.Format("Typing {0}-{1} random alphabetical characters into element <{2}>[{3}]", MinLength, MaxLength,
                                                        Test.CurrentElements.ElementAt(Test.CurrentElementIndex)
                                                            .TagName, Test.CurrentElementIndex);
                }

                // no elements succeeded
                if (succeededElements == 0)
                {
                    Success = false;
                    PostActionMessage = lastException == null ?
                        "None of the elements could be typed into" :
                        String.Format("None of the elements could be typed into (last exception encountered: {0})", lastException.Message);
                    return;
                }

                // at least one element succeeded
                PostActionMessage = String.Format(
                        "Typing {0}-{1} random alphabetical characters succeeded for {2} element(s), failed for {3} elements(s)", MinLength, MaxLength,
                        succeededElements,
                        failedElements);
                Success = true;
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = true,
                    DefaultValue = null
                });
                parameters.Add(new ActionParameter
                {
                    Name = "minlength",
                    Type = typeof(int),
                    Description = "The minimum length of the random string",
                    IsOptional = true,
                    DefaultValue = 2
                });
                parameters.Add(new ActionParameter
                {
                    Name = "maxlength",
                    Type = typeof(int),
                    Description = "The maximum length of the random string",
                    IsOptional = false,
                    DefaultValue = 10
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Types a random first name into the current element
    /// </summary>
    public class FirstName : BaseAction
    {
        /// <summary>
        /// Types a random first name into the current elements
        /// </summary>
        public FirstName()
        {
        }

        /// <summary>
        /// Types a random first name into the elements matching the given selector
        /// </summary>
        public FirstName(string selector)
        {
            Selector = selector;
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Typing a random first name into element <{0}>[{1}]", Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null || !elements.Any())
                {
                    PostActionMessage = "The current element is not set, or is not a valid text input element";
                    Success = false;
                    return;
                }

                int succeededElements = 0;
                int failedElements = 0;
                Exception lastException = null;

                foreach (IWebElement element in elements)
                {
                    try
                    {
                        // get random alphabetical text of the given length
                        Randomiser random = new Randomiser();
                        element.SendKeys(random.GetFirstName());
                        succeededElements++;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        failedElements++;
                    }

                    PostActionMessage = String.Format("Typing random first name into element <{0}>[{1}]",
                                                        Test.CurrentElements.ElementAt(Test.CurrentElementIndex)
                                                            .TagName, Test.CurrentElementIndex);
                }

                // no elements succeeded
                if (succeededElements == 0)
                {
                    Success = false;
                    PostActionMessage = lastException == null ?
                        "None of the elements could be typed into" :
                        String.Format("None of the elements could be typed into (last exception encountered: {0})", lastException.Message);
                    return;
                }

                // at least one element succeeded
                PostActionMessage = String.Format(
                        "Typing random first name succeeded for {0} element(s), failed for {1} elements(s)",
                        succeededElements,
                        failedElements);
                Success = true;
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = true,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Types a random last name into the current element
    /// </summary>
    public class LastName : BaseAction
    {
        /// <summary>
        /// Types a random last name into the current elements
        /// </summary>
        public LastName()
        {
        }

        /// <summary>
        /// Types a random last name into the elements matching the given selector
        /// </summary>
        public LastName(string selector)
        {
            Selector = selector;
        }

        public override void PreAction()
        {
            FindElements(Selector);
            PreActionMessage = String.Format("Typing a random last name into element <{0}>[{1}]", Test.CurrentElements.ElementAt(Test.CurrentElementIndex).TagName, Test.CurrentElementIndex);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                IEnumerable<IWebElement> elements = GetElements();
                if (elements == null || !elements.Any())
                {
                    PostActionMessage = "The current element is not set, or is not a valid text input element";
                    Success = false;
                    return;
                }

                int succeededElements = 0;
                int failedElements = 0;
                Exception lastException = null;

                foreach (IWebElement element in elements)
                {
                    try
                    {
                        // get random alphabetical text of the given length
                        Randomiser random = new Randomiser();
                        element.SendKeys(random.GetFirstName());
                        succeededElements++;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        failedElements++;
                    }

                    PostActionMessage = String.Format("Typing random last name into element <{0}>[{1}]",
                                                        Test.CurrentElements.ElementAt(Test.CurrentElementIndex)
                                                            .TagName, Test.CurrentElementIndex);
                }

                // no elements succeeded
                if (succeededElements == 0)
                {
                    Success = false;
                    PostActionMessage = lastException == null ?
                        "None of the elements could be typed into" :
                        String.Format("None of the elements could be typed into (last exception encountered: {0})", lastException.Message);
                    return;
                }

                // at least one element succeeded
                PostActionMessage = String.Format(
                        "Typing random last name succeeded for {0} element(s), failed for {1} elements(s)",
                        succeededElements,
                        failedElements);
                Success = true;
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
                    Name = "selector",
                    Type = typeof(string),
                    Description = "The selector to search for",
                    IsOptional = true,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }
}
