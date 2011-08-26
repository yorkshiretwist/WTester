using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using System.IO;
using System.Windows.Forms;
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
        public ElementCollection CurrentElements;

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
        /// The currently active WatiN browser instance
        /// </summary>
        public Browser Browser;

        /// <summary>
        /// Gets the type of the current WatiN browser instance
        /// </summary>
        public BrowserType BrowserType;

        #endregion

        #region Private properties

        /// <summary>
        /// The list of possible action types that can be called
        /// </summary>
        private Dictionary<string, string> ActionTypes = new Dictionary<string, string>();

        /// <summary>
        /// The lines in the current WTest document
        /// </summary>
        private List<string> Lines = new List<string>();

        /// <summary>
        /// Stores the success or failure of the last action
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Stores the message from the last action
        /// </summary>
        public string Message { get; set; }

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
            this.BrowserType = BrowserType.IE;
            this.CurrentUri = new Uri("http://www.google.com");
            RegisterStandardActions();
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

            // elements
            RegisterAction("find", "stillbreathing.co.uk.WTester.Actions.Elements.Find");

            // forms
            RegisterAction("click", "stillbreathing.co.uk.WTester.Actions.Forms.Click");
            RegisterAction("typetext", "stillbreathing.co.uk.WTester.Actions.Forms.TypeText");

            // javascript
            RegisterAction("eval", "stillbreathing.co.uk.WTester.Actions.JavaScript.Eval");

            // navigation
            RegisterAction("load", "stillbreathing.co.uk.WTester.Actions.Navigation.Load");
            RegisterAction("open", "stillbreathing.co.uk.WTester.Actions.Navigation.Load");
            RegisterAction("close", "stillbreathing.co.uk.WTester.Actions.Navigation.Close");
            RegisterAction("wait", "stillbreathing.co.uk.WTester.Actions.Navigation.Wait");
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
            RegisterAction("maximise", "stillbreathing.co.uk.WTester.Actions.Window.Maximise");
            RegisterAction("minimise", "stillbreathing.co.uk.WTester.Actions.Window.Minimise");
            RegisterAction("reset", "stillbreathing.co.uk.WTester.Actions.Window.Reset");
            RegisterAction("newtab", "stillbreathing.co.uk.WTester.Actions.Window.NewTab");
            RegisterAction("gototab", "stillbreathing.co.uk.WTester.Actions.Window.GoToTab");
            RegisterAction("closetab", "stillbreathing.co.uk.WTester.Actions.Window.CloseTab");
            RegisterAction("nexttab", "stillbreathing.co.uk.WTester.Actions.Window.NextTab");
            RegisterAction("previoustab", "stillbreathing.co.uk.WTester.Actions.Window.PreviousTab");
        }

        /// <summary>
        /// Register an action to be performed when a given function name is invoked
        /// </summary>
        /// <param name="functionName">The name of the function that will call this action</param>
        /// <param name="typeName">The type name of the action to be called</param>
        public void RegisterAction(string functionName, string typeName)
        {
            if (!ActionTypes.ContainsKey(functionName)) ActionTypes.Add(functionName, typeName);
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
            Parser parser = new Parser();
            foreach (string line in Lines)
            {
                // every line must start with '$.'
                if (line.StartsWith("$."))
                {
                    // get the actions, one on each line, perhap chained
                    List<string> actions = parser.Split(line.Substring(2), ".", "\"", true);
                    foreach (string action in actions)
                    {
                        // get the function name for this action
                        string thisAction = action.Trim();
                        string functionName = parser.GetFunctionName(thisAction);

                        // check the function name has been registered
                        if (functionName != null && ActionTypes.ContainsKey(functionName))
                        {
                            // get the parameters and index
                            List<object> parameters = parser.GetParameters(thisAction);
                            int? index = parser.GetIndex(thisAction);
                            if (index != null) this.CurrentElementIndex = index.Value;
                            
                            // make sure the first command is to load a URI
                            if (this.Browser == null && (functionName == "load" || functionName == "open"))
                            {
                                if (parameters.Count > 1)
                                {
                                    if (parameters[1].ToString().ToLower() == "firefox")
                                    {
                                        this.BrowserType = BrowserType.Firefox;
                                    }
                                }
                                if (this.BrowserType == BrowserType.IE) this.Browser = new IE();
                                if (this.BrowserType == BrowserType.Firefox) this.Browser = new FireFox();
                            }

                            if (this.Browser != null)
                            {
                                // create the IAction object
                                ActionInvoker invoker = new ActionInvoker();
                                IAction actionObject = invoker.Invoke(this, ActionTypes[functionName], parameters);

                                // execute the PreAction() method of the IAction
                                actionObject = invoker.PreAction(this, actionObject);

                                // invoke the pre-action delegate, if it is set
                                if (preActionDelegate != null)
                                {
                                    // create the pre-action parameters array
                                    List<object> preActionParameters = new List<object>();
                                    preActionParameters.Add(functionName);
                                    preActionParameters.Add(parameters);
                                    preActionParameters.Add(actionObject.PreActionMessage);

                                    // invoke the pre-action delegate
                                    preActionDelegate.DynamicInvoke(preActionParameters.ToArray());
                                }

                                // execute the Execute() method of the IAction
                                actionObject = invoker.Execute(this, actionObject);

                                // create the results paramters array
                                List<object> resultParameters = new List<object>();
                                resultParameters.Add(functionName);
                                resultParameters.Add(parameters);
                                if (actionObject != null)
                                {
                                    resultParameters.Add(actionObject.Success);
                                    resultParameters.Add(actionObject.PostActionMessage);
                                }
                                else
                                {
                                    resultParameters.Add(this.Success);
                                    resultParameters.Add(this.Message);
                                }

                                // invoke the action result delegate
                                actionResultDelegate.DynamicInvoke(resultParameters.ToArray());
                            }
                        }
                        else
                        {
                            // create the error results parameters array
                            List<object> resultParameters = new List<object>();
                            resultParameters.Add(functionName);
                            resultParameters.Add(new List<object>());
                            resultParameters.Add(false);
                            resultParameters.Add(string.Format("The function '{0}' was not recognised", functionName));

                            // invoke the action result delegate
                            actionResultDelegate.DynamicInvoke(resultParameters.ToArray());
                        }
                    }
                }
                else
                {
                    // create the error results parameters array
                    List<object> resultParameters = new List<object>();
                    resultParameters.Add("");
                    resultParameters.Add(new List<object>());
                    resultParameters.Add(false);
                    resultParameters.Add("The line does not begin with '$.'");

                    // invoke the action result delegate
                    actionResultDelegate.DynamicInvoke(resultParameters.ToArray());
                }
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
