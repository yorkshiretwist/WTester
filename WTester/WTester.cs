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
        /// The List of Actions that will be performed in this test
        /// </summary>
        public List<BaseAction> Actions = new List<BaseAction>();

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
        /// The list of possible action types that can be called
        /// </summary>
        private Dictionary<string, string> ActionTypes = new Dictionary<string, string>();

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
            RegisterAction("pause", "stillbreathing.co.uk.WTester.Actions.ActionProcessing.Pause");

            // content
            RegisterAction("search", "stillbreathing.co.uk.WTester.Actions.Content.Search");
            RegisterAction("title", "stillbreathing.co.uk.WTester.Actions.Content.Title");
            RegisterAction("highlight", "stillbreathing.co.uk.WTester.Actions.Content.Highlight");
            RegisterAction("html", "stillbreathing.co.uk.WTester.Actions.Content.InnerHtml");
            RegisterAction("outerhtml", "stillbreathing.co.uk.WTester.Actions.Content.OuterHtml");
            RegisterAction("text", "stillbreathing.co.uk.WTester.Actions.Content.Text");
            RegisterAction("waitfor", "stillbreathing.co.uk.WTester.Actions.Content.WaitFor");

            // elements
            RegisterAction("find", "stillbreathing.co.uk.WTester.Actions.Elements.Find");

            // forms
            RegisterAction("click", "stillbreathing.co.uk.WTester.Actions.Forms.Click");
            RegisterAction("clicklast", "stillbreathing.co.uk.WTester.Actions.Forms.ClickLast");
            RegisterAction("typetext", "stillbreathing.co.uk.WTester.Actions.Forms.TypeText");
            RegisterAction("firstname", "stillbreathing.co.uk.WTester.Actions.Forms.FirstName");
            RegisterAction("lastname", "stillbreathing.co.uk.WTester.Actions.Forms.LastName");
            RegisterAction("selectvalue", "stillbreathing.co.uk.WTester.Actions.Forms.SelectValue");
            RegisterAction("selecttext", "stillbreathing.co.uk.WTester.Actions.Forms.SelectText");
            RegisterAction("selectindex", "stillbreathing.co.uk.WTester.Actions.Forms.SelectIndex");
            RegisterAction("selectrandom", "stillbreathing.co.uk.WTester.Actions.Forms.SelectRandom");

            // javascript
            RegisterAction("eval", "stillbreathing.co.uk.WTester.Actions.JavaScript.Eval");

            // navigation
            RegisterAction("load", "stillbreathing.co.uk.WTester.Actions.Navigation.Load");
            RegisterAction("open", "stillbreathing.co.uk.WTester.Actions.Navigation.Load");
            RegisterAction("close", "stillbreathing.co.uk.WTester.Actions.Navigation.Close");
            RegisterAction("refresh", "stillbreathing.co.uk.WTester.Actions.Navigation.Refresh");
            RegisterAction("back", "stillbreathing.co.uk.WTester.Actions.Navigation.Back");
            RegisterAction("forward", "stillbreathing.co.uk.WTester.Actions.Navigation.Forward");

            // output
            RegisterAction("screenshot", "stillbreathing.co.uk.WTester.Actions.Output.Screenshot");
            RegisterAction("save", "stillbreathing.co.uk.WTester.Actions.Output.Save");

            // style
            RegisterAction("css", "stillbreathing.co.uk.WTester.Actions.Style.CSS");
            RegisterAction("zoomin", "stillbreathing.co.uk.WTester.Actions.Style.ZoomIn");
            RegisterAction("zoomout", "stillbreathing.co.uk.WTester.Actions.Style.ZoomOut");
            RegisterAction("resetzoom", "stillbreathing.co.uk.WTester.Actions.Style.ResetZoom");

            // window
            RegisterAction("resize", "stillbreathing.co.uk.WTester.Actions.Window.Resize");
            RegisterAction("rotate", "stillbreathing.co.uk.WTester.Actions.Window.Rotate");
            RegisterAction("maximise", "stillbreathing.co.uk.WTester.Actions.Window.Maximise");
            RegisterAction("minimise", "stillbreathing.co.uk.WTester.Actions.Window.Minimise");
            RegisterAction("reset", "stillbreathing.co.uk.WTester.Actions.Window.Reset");
            RegisterAction("newtab", "stillbreathing.co.uk.WTester.Actions.Window.NewTab");
            RegisterAction("gototab", "stillbreathing.co.uk.WTester.Actions.Window.GoToTab");
            RegisterAction("closetab", "stillbreathing.co.uk.WTester.Actions.Window.CloseTab");
            RegisterAction("nexttab", "stillbreathing.co.uk.WTester.Actions.Window.NextTab");
            RegisterAction("previoustab", "stillbreathing.co.uk.WTester.Actions.Window.PreviousTab");
            RegisterAction("keypress", "stillbreathing.co.uk.WTester.Actions.Window.KeyPress");

            // cookies
            RegisterAction("getcookie", "stillbreathing.co.uk.WTester.Actions.Cookies.GetCookie");
            RegisterAction("setcookie", "stillbreathing.co.uk.WTester.Actions.Cookies.SetCookie");
            RegisterAction("deletecookie", "stillbreathing.co.uk.WTester.Actions.Cookies.DeleteCookie");
        }

        /// <summary>
        /// Register an action to be performed when a given function name is invoked
        /// </summary>
        /// <param name="functionName">The name of the function that will call this action</param>
        /// <param name="typeName">The type name of the action to be called</param>
        public void RegisterAction(string functionName, string typeName)
        {
            if (!ActionTypes.ContainsKey(functionName)) 
                ActionTypes.Add(functionName, typeName);
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
                var resultParameters = new List<object>
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
                                IAction actionObject = invoker.Invoke(this, ActionTypes[functionName], parameters);

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
