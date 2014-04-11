using System;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace stillbreathing.co.uk.WTester.Actions.JavaScript
{
    /// <summary>
    /// Executes the given JavaScript
    /// </summary>
    public class Eval : BaseAction
    {
        private string Script;

        public Eval(string script)
        {
            if (!script.EndsWith(";")) script = script + ";";
            Script = script;
        }

        public override void PreAction()
        {
            PreActionMessage = "Executing JavaScript";
        }

        public override void Execute()
        {
            try
            {
                var js = Test.Browser as IJavaScriptExecutor;
                var result = js.ExecuteScript(Script, null) as string;
                PostActionMessage = string.Format("Executed JavaScript. Result: {0}", result);
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
                    Name = "script",
                    Type = typeof(string),
                    Description = "The JavaScript to execute",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }
}
