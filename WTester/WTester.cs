using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using System.IO;
using stillbreathing.co.uk.WTester.Actions;

namespace stillbreathing.co.uk.WTester
{
    /// <summary>
    /// Instantiates a new WTest instance
    /// </summary>
    public class WTest
    {
        #region Public properties

        /// <summary>
        /// The list of possible action types that can be called
        /// </summary>
        public Dictionary<string, ActionType> ActionTypes = new Dictionary<string, ActionType>();

        /// <summary>
        /// The List of Actions that will be performed in this test
        /// </summary>
        public List<BaseAction> Actions = new List<BaseAction>();

        /// <summary>
        /// Returns the list of functions for registered actions
        /// </summary>
        public List<string> FunctionNames
        {
            get
            {
                return ActionTypes.Select(a => a.Key).ToList();
            }
        }

        /// <summary>
        /// The currently selected elements
        /// </summary>
        public IEnumerable<IWebElement> CurrentElements;

        /// <summary>
        /// The index of the currently selected element
        /// </summary>
        public int CurrentElementIndex;

        /// <summary>
        /// The currently loaded page
        /// </summary>
        public WTestPage CurrentPage;

        /// <summary>
        /// Gets the currenntly loaded Uri
        /// </summary>
        public Uri CurrentUri;

        /// <summary>
        /// The currently active browser instance
        /// </summary>
        public IWebDriver Browser;

        /// <summary>
        /// Gets the type of the current browser instance
        /// </summary>
        public BrowserType BrowserType;

        /// <summary>
        /// Stores the success or failure of the last action
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Stores the message from the last action
        /// </summary>
        public string Message { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// The lines in the current WTest document
        /// </summary>
        private List<string> Lines = new List<string>();

        /// <summary>
        /// The parser for this test
        /// </summary>
        private Parser Parser { get; set; }

        /// <summary>
        /// The total number of items to run
        /// </summary>
        private int _totalItemsToRun;

        /// <summary>
        /// The current item
        /// </summary>
        private int _currentItem;

        #endregion

        #region Contructors

        /// <summary>
        /// Create a new WTest instance
        /// </summary>
        public WTest()
        {
            Initialise();
        }

        /// <summary>
        /// Create a new WTest instance with the given string array of lines
        /// </summary>
        /// <param name="lines">A string array containing the lines of the current WTest document</param>
        public WTest(string[] lines)
        {
            this.Lines = lines.ToList();
            Initialise();
        }

        /// <summary>
        /// Create a new WTest instance for the given WTest document file
        /// </summary>
        /// <param name="fileName">The full file name of the WTest document to be loaded</param>
        public WTest(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("The specified WTest document could not be loaded", fileName);
            }
            else
            {
                this.Lines = File.ReadAllLines(fileName).ToList();
            }
            Initialise();
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Initialise this WTest instance
        /// </summary>
        private void Initialise()
        {
            CurrentUri = new Uri("http://www.google.com");
            RegisterStandardActions();
        }

        #endregion

        #region Events

        /// <summary>
        /// The event that is fired as the test progresses
        /// </summary>
        public event ProgressChangedEventHandler TestProgress;

        SendOrPostCallback _progressReporter;

        private void ProgressReporter(object arg)
        {
            OnProgressChanged((ProgressChangedEventArgs)arg);
        }

        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (TestProgress != null) TestProgress(this, e);
        }

        #endregion

        #region Action registration

        /// <summary>
        /// Register the standard actions
        /// </summary>
        private void RegisterStandardActions()
        {
            // action processing
            RegisterAction("pause", "stillbreathing.co.uk.WTester.Actions.ActionProcessing.Pause",
                "Pause",
                "Pauses test execution for a number of seconds",
                WTester.Actions.ActionProcessing.Pause.Parameters
                );

            // content
            RegisterAction("search", "stillbreathing.co.uk.WTester.Actions.Content.Search", 
                "Search",
                "Searches for text on the current page",
                WTester.Actions.Content.Search.Parameters);
            RegisterAction("title", "stillbreathing.co.uk.WTester.Actions.Content.Title",
                "Title",
                "Returns the title of the current page",
                WTester.Actions.Content.Title.Parameters);
            RegisterAction("highlight", "stillbreathing.co.uk.WTester.Actions.Content.Highlight",
                "Highlight",
                "Highlights the current element",
                WTester.Actions.Content.Highlight.Parameters);
            RegisterAction("html", "stillbreathing.co.uk.WTester.Actions.Content.InnerHtml",
                "InnerHtml",
                "Returns the inner HTML of the current element",
                WTester.Actions.Content.InnerHtml.Parameters);
            RegisterAction("outerhtml", "stillbreathing.co.uk.WTester.Actions.Content.OuterHtml",
                "OuterHtml",
                "Returns the outer HTML of the current element",
                WTester.Actions.Content.OuterHtml.Parameters);
            RegisterAction("text", "stillbreathing.co.uk.WTester.Actions.Content.Text",
                "Text",
                "Returns the text of the current element",
                WTester.Actions.Content.Text.Parameters);
            RegisterAction("waitfor", "stillbreathing.co.uk.WTester.Actions.Content.WaitFor",
                "WaitFor",
                "Waits until the given element is present on the page",
                WTester.Actions.Content.WaitFor.Parameters);

            // elements
            RegisterAction("find", "stillbreathing.co.uk.WTester.Actions.Elements.Find",
                "Find",
                "Finds elements on the current page",
                WTester.Actions.Elements.Find.Parameters);

            // forms
            RegisterAction("click", "stillbreathing.co.uk.WTester.Actions.Forms.Click",
                "Click",
                "Activates a click event on the current selected element",
                WTester.Actions.Forms.Click.Parameters);
            RegisterAction("clicklast", "stillbreathing.co.uk.WTester.Actions.Forms.ClickLast",
                "ClickLast",
                "Activates a click event on the last selected element",
                WTester.Actions.Forms.ClickLast.Parameters);
            RegisterAction("typetext", "stillbreathing.co.uk.WTester.Actions.Forms.TypeText",
                "TypeText",
                "Types text into the current element",
                WTester.Actions.Forms.TypeText.Parameters);
            RegisterAction("firstname", "stillbreathing.co.uk.WTester.Actions.Forms.FirstName",
                "FirstName",
                "Types a random first name into the current element",
                WTester.Actions.Forms.FirstName.Parameters);
            RegisterAction("lastname", "stillbreathing.co.uk.WTester.Actions.Forms.LastName",
                "LastName",
                "Types a random last name into the current element",
                WTester.Actions.Forms.LastName.Parameters);
            RegisterAction("selectvalue", "stillbreathing.co.uk.WTester.Actions.Forms.SelectValue",
                "SelectValue",
                "Selects an option by value in the current select element",
                WTester.Actions.Forms.SelectValue.Parameters);
            RegisterAction("selecttext", "stillbreathing.co.uk.WTester.Actions.Forms.SelectText",
                "SelectText",
                "Selects an option by text in the current select element",
                WTester.Actions.Forms.SelectText.Parameters);
            RegisterAction("selectindex", "stillbreathing.co.uk.WTester.Actions.Forms.SelectIndex",
                "SelectIndex",
                "Selects the option at the given index in the current select element",
                WTester.Actions.Forms.SelectIndex.Parameters);
            RegisterAction("selectrandom", "stillbreathing.co.uk.WTester.Actions.Forms.SelectRandom",
                "SelectRandom",
                "Selects a random option in the current select element",
                WTester.Actions.Forms.SelectRandom.Parameters);

            // javascript
            RegisterAction("eval", "stillbreathing.co.uk.WTester.Actions.JavaScript.Eval",
                "Eval",
                "Executes the given JavaScript",
                WTester.Actions.JavaScript.Eval.Parameters);

            // navigation
            RegisterAction("load", "stillbreathing.co.uk.WTester.Actions.Navigation.Load",
                "Load",
                "Loads the page at the given URI",
                WTester.Actions.Navigation.Load.Parameters);
            RegisterAction("open", "stillbreathing.co.uk.WTester.Actions.Navigation.Load",
                "Open",
                "Loads the page at the given URI",
                WTester.Actions.Navigation.Load.Parameters);
            RegisterAction("close", "stillbreathing.co.uk.WTester.Actions.Navigation.Close",
                "Close",
                "Closes the current browser instance",
                WTester.Actions.Navigation.Close.Parameters);
            RegisterAction("refresh", "stillbreathing.co.uk.WTester.Actions.Navigation.Refresh",
                "Refresh",
                "Refreshes the current page",
                WTester.Actions.Navigation.Refresh.Parameters);
            RegisterAction("back", "stillbreathing.co.uk.WTester.Actions.Navigation.Back",
                "Back",
                "Goes the the previous page",
                WTester.Actions.Navigation.Back.Parameters);
            RegisterAction("forward", "stillbreathing.co.uk.WTester.Actions.Navigation.Forward",
                "Forward",
                "Goes the the next page (after previously going back)",
                WTester.Actions.Navigation.Forward.Parameters);

            // output
            RegisterAction("screenshot", "stillbreathing.co.uk.WTester.Actions.Output.Screenshot",
                "Screenshot",
                "Saves a screenshot of the page to a file",
                WTester.Actions.Output.Screenshot.Parameters);
            RegisterAction("save", "stillbreathing.co.uk.WTester.Actions.Output.Save",
                "Save",
                "Saves the HTML of the page to a file",
                WTester.Actions.Output.Save.Parameters);

            // style
            RegisterAction("css", "stillbreathing.co.uk.WTester.Actions.Style.CSS",
                "CSS",
                "Applies the given styles to the current page",
                WTester.Actions.Style.CSS.Parameters);
            RegisterAction("zoomin", "stillbreathing.co.uk.WTester.Actions.Style.ZoomIn",
                "ZoomIn",
                "Zooms the page in",
                WTester.Actions.Style.ZoomIn.Parameters);
            RegisterAction("zoomout", "stillbreathing.co.uk.WTester.Actions.Style.ZoomOut",
                "ZoomOut",
                "Zooms the page out",
                WTester.Actions.Style.ZoomOut.Parameters);
            RegisterAction("resetzoom", "stillbreathing.co.uk.WTester.Actions.Style.ResetZoom",
                "ResetZoom",
                "Resets the zoom",
                WTester.Actions.Style.ResetZoom.Parameters);

            // window
            RegisterAction("resize", "stillbreathing.co.uk.WTester.Actions.Window.Resize",
                "Resize",
                "Resizes the browser window",
                WTester.Actions.Window.Resize.Parameters);
            RegisterAction("rotate", "stillbreathing.co.uk.WTester.Actions.Window.Rotate",
                "Rotate",
                "Rotates the window, switching between landscape and portrait",
                WTester.Actions.Window.Rotate.Parameters);
            RegisterAction("maximise", "stillbreathing.co.uk.WTester.Actions.Window.Maximise",
                "Maximise",
                "Maximises the browser window",
                WTester.Actions.Window.Maximise.Parameters);
            //RegisterAction("minimise", "stillbreathing.co.uk.WTester.Actions.Window.Minimise",
            //    "Minimise",
            //    "Minimises the browser window");
            //RegisterAction("reset", "stillbreathing.co.uk.WTester.Actions.Window.Reset",
            //    "Reset",
            //    "Resets the window size");
            RegisterAction("newtab", "stillbreathing.co.uk.WTester.Actions.Window.NewTab",
                "NewTab",
                "Opens a new tab",
                WTester.Actions.Window.NewTab.Parameters);
            RegisterAction("gototab", "stillbreathing.co.uk.WTester.Actions.Window.GoToTab",
                "GoToTab",
                "Goes to the tab with the given index",
                WTester.Actions.Window.GoToTab.Parameters);
            RegisterAction("closetab", "stillbreathing.co.uk.WTester.Actions.Window.CloseTab",
                "CloseTab",
                "Closes the current tab",
                WTester.Actions.Window.CloseTab.Parameters);
            RegisterAction("nexttab", "stillbreathing.co.uk.WTester.Actions.Window.NextTab",
                "NextTab",
                "Goes to the next tab",
                WTester.Actions.Window.NextTab.Parameters);
            RegisterAction("previoustab", "stillbreathing.co.uk.WTester.Actions.Window.PreviousTab",
                "PreviousTab",
                "Goes to the previous tab",
                WTester.Actions.Window.PreviousTab.Parameters);
            RegisterAction("keypress", "stillbreathing.co.uk.WTester.Actions.Window.KeyPress",
                "KeyPress",
                "Sends the given keys to the browser",
                WTester.Actions.Window.KeyPress.Parameters);

            // cookies
            RegisterAction("getcookie", "stillbreathing.co.uk.WTester.Actions.Cookies.GetCookie",
                "GetCookie",
                "Gets the value of a cookie");
            RegisterAction("setcookie", "stillbreathing.co.uk.WTester.Actions.Cookies.SetCookie",
                "SetCookie",
                "Sets the value of a cookie");
            RegisterAction("deletecookie", "stillbreathing.co.uk.WTester.Actions.Cookies.DeleteCookie",
                "DeleteCookie",
                "Deletes a cookie");
        }

        /// <summary>
        /// Register an action to be performed when a given function name is invoked
        /// </summary>
        /// <param name="functionName">The name of the function that will call this action</param>
        /// <param name="typeName">The type name of the action to be called</param>
        public void RegisterAction(string functionName, string typeName, string name = null, string description = null, List<ActionParameter> parameters = null)
        {
            if (!ActionTypes.ContainsKey(functionName))
            {
                ActionType actionType = new ActionType(functionName, typeName);
                if (description != null)
                {
                    actionType.Name = name;
                }
                if (description != null)
                {
                    actionType.Description = description;
                }
                if (parameters != null)
                {
                    actionType.Parameters = parameters;
                }
                ActionTypes.Add(functionName, actionType);
            }
        }

        #endregion

        #region Processing

        /// <summary>
        /// Runs a test with the current Actions
        /// </summary>
        public void RunTest(Delegate actionResultDelegate)
        {
            RunTest(null, actionResultDelegate);
        }
        public void RunTest(Delegate preActionDelegate, Delegate actionResultDelegate)
        {
            // create the new parser
            Parser = new Parser();

            // create the new reporter
            _progressReporter = new SendOrPostCallback(ProgressReporter);

            // report the loading of this browser
            var resultParameters = new List<object>
                    {
                        "test",
                        new List<object>(),
                        true,
                        "Starting test, please wait"
                    };
            actionResultDelegate.DynamicInvoke(resultParameters.ToArray());

            // create the list of browsers
            var browsers = new List<BrowserType>();

            // get the very first action, it may set the browser
            if (Lines.Any() && Lines.First().StartsWith("$.browser"))
            {
                // get the browser parameters
                List<object> parameters = Parser.GetParameters(Lines.First());

                // just one parameter? set the browser
                if (parameters.Count == 1)
                {
                    if (parameters[0].ToString().ToLower() == "firefox")
                    {
                        BrowserType = BrowserType.Firefox;
                    }
                    if (parameters[0].ToString().ToLower() == "chrome")
                    {
                        BrowserType = BrowserType.Chrome;
                    }
                    if (parameters[0].ToString().ToLower() == "safari")
                    {
                        BrowserType = BrowserType.Safari;
                    }
                    if (parameters[0].ToString().ToLower() == "ie")
                    {
                        BrowserType = BrowserType.IE;
                    }
                    
                    browsers.Add(BrowserType);
                }

                // more than one parameter? we are looping each browser type
                if (parameters.Count > 1)
                {
                    foreach (object o in parameters)
                    {
                        if (o.ToString().ToLower() == "firefox")
                        {
                            BrowserType = BrowserType.Firefox;
                        }
                        if (o.ToString().ToLower() == "chrome")
                        {
                            BrowserType = BrowserType.Chrome;
                        }
                        if (o.ToString().ToLower() == "safari")
                        {
                            BrowserType = BrowserType.Safari;
                        }
                        if (o.ToString().ToLower() == "ie")
                        {
                            BrowserType = BrowserType.IE;
                        }

                        browsers.Add(BrowserType);
                    }
                }

                // remove the first line as we've just run it
                Lines.RemoveAt(0);
            }

            // work out the total number of items to run
            _totalItemsToRun = browsers.Count * Lines.Count;

            // for each browser run the script
            foreach (BrowserType browserType in browsers)
            {
                // close the old browser if it is still open
                if (Browser != null)
                {
                    Browser.Close();
                    Browser = null;
                }

                // load the browser
                LoadBrowser(browserType);

                // report the loading of this browser
                resultParameters = new List<object>
                    {
                        "browser",
                        new List<object> {browserType.ToString()},
                        true,
                        "Loaded browser"
                    };
                actionResultDelegate.DynamicInvoke(resultParameters.ToArray());

                // process each line of the script
                ProcessLines(preActionDelegate, actionResultDelegate);
            }

            // report the test as completed
            _progressReporter(new ProgressChangedEventArgs(100, null));
        }

        /// <summary>
        /// Load the browser of the given type
        /// </summary>
        /// <param name="browserType"></param>
        void LoadBrowser(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Chrome:
                    Browser = new ChromeDriver();
                    break;
                case BrowserType.Firefox:
                    Browser = new FirefoxDriver();
                    break;
                case BrowserType.Safari:
                    Browser = new SafariDriver();
                    break;
                default:
                    Browser = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
                    break;
            }
        }

        /// <summary>
        /// Process the lines of the script
        /// </summary>
        /// <param name="preActionDelegate"></param>
        /// <param name="actionResultDelegate"></param>
        void ProcessLines(Delegate preActionDelegate, Delegate actionResultDelegate)
        {
            foreach (string line in Lines)
            {
                // check this line is valid
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // if the line starts with // it is a comment
                if (line.StartsWith("//"))
                {
                    // create the results parameters array
                    var resultParameters = new List<object> { "Comment:", new List<object>(), true, line };

                    // invoke the action result delegate
                    actionResultDelegate.DynamicInvoke(resultParameters.ToArray());

                    // go to the next line
                    continue;
                }

                // every line must start with '$.'
                if (line.StartsWith("$."))
                {
                    // get the actions, one on each line, perhap chained
                    List<string> actions = Parser.Split(line.Substring(2), ".", "\"", true);

                    foreach (string action in actions)
                    {
                        // get the function name for this action
                        string thisAction = action.Trim();
                        string functionName = Parser.GetFunctionName(thisAction);

                        // check the function name has been registered
                        if (functionName != null && ActionTypes.ContainsKey(functionName))
                        {
                            // get the parameters and index
                            List<object> parameters = Parser.GetParameters(thisAction);
                            int? index = Parser.GetIndex(thisAction);
                            if (index != null) CurrentElementIndex = index.Value;

                            if (Browser != null)
                            {
                                // create the IAction object
                                var invoker = new ActionInvoker();
                                IAction actionObject = invoker.Invoke(this, ActionTypes[functionName].MethodName, parameters);

                                // execute the PreAction() method of the IAction
                                actionObject = invoker.PreAction(this, actionObject);

                                // invoke the pre-action delegate, if it is set
                                if (preActionDelegate != null)
                                {
                                    // create the pre-action parameters array
                                    var preActionParameters = new List<object>
                                        {
                                            functionName,
                                            parameters,
                                            actionObject.PreActionMessage
                                        };

                                    // invoke the pre-action delegate
                                    preActionDelegate.DynamicInvoke(preActionParameters.ToArray());
                                }

                                // execute the Execute() method of the IAction
                                actionObject = invoker.Execute(this, actionObject);

                                // create the results parameters array
                                var resultParameters = new List<object> { functionName, parameters };
                                if (actionObject != null)
                                {
                                    resultParameters.Add(actionObject.Success);
                                    resultParameters.Add(actionObject.PostActionMessage);
                                }
                                else
                                {
                                    resultParameters.Add(Success);
                                    resultParameters.Add(Message);
                                }

                                // invoke the action result delegate
                                actionResultDelegate.DynamicInvoke(resultParameters.ToArray());
                            }
                        }
                        else
                        {
                            // create the error results parameters array
                            var resultParameters = new List<object>
                                {
                                    functionName,
                                    new List<object>(),
                                    false,
                                    string.Format("The function '{0}' was not recognised", functionName)
                                };

                            // invoke the action result delegate
                            actionResultDelegate.DynamicInvoke(resultParameters.ToArray());
                        }
                    }
                }
                else
                {
                    // create the error results parameters array
                    var resultParameters = new List<object>
                        {
                            "",
                            new List<object>(),
                            false,
                            "The line does not begin with '$.'"
                        };

                    // invoke the action result delegate
                    actionResultDelegate.DynamicInvoke(resultParameters.ToArray());
                }

                // report the test progress
                _currentItem++;
                int progress = (200 * _currentItem + 1) / (_totalItemsToRun * 2);
                _progressReporter(new ProgressChangedEventArgs(progress, null));
            }
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The delegate that may be called before each action is invoked
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="parameters"></param>
        /// <param name="message"></param>
        public delegate void PreActionDelegate(string functionName, List<object> parameters, string message);

        /// <summary>
        /// The delegate that must be called for each action
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="parameters"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public delegate void ActionResultDelegate(string functionName, List<object> parameters, bool success, string message);

        #endregion
    }
}
