using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stillbreathing.co.uk.WTester
{
    /// <summary>
    /// Represents the details of an action type that can be performed
    /// </summary>
    public class ActionType
    {
        public ActionType(string functionName, string methodName)
        {
            Name = functionName;
            FunctionName = functionName;
            MethodName = methodName;
        }

        /// <summary>
        /// The name of this action
        /// </summary>
        public string Name;

        /// <summary>
        /// The name of the function that will call this action
        /// </summary>
        public string FunctionName;

        /// <summary>
        /// The fully-qualified method name that will be invoked for this action type
        /// </summary>
        public string MethodName;

        /// <summary>
        /// The description of the action type
        /// </summary>
        public string Description;

        /// <summary>
        /// The lost of parameters this action type takes
        /// </summary>
        public List<ActionParameter> Parameters;

        /// <summary>
        /// Gets the string of parameters with their types and names for display in the autocomplete list
        /// </summary>
        public string ParameterString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                if (Parameters != null && Parameters.Any())
                {
                    int x = 0;
                    foreach (ActionParameter parameter in Parameters)
                    {
                        if (x > 0)
                        {
                            sb.Append(", ");
                        }
                        sb.Append(parameter.Type.Name.ToString());
                        if (parameter.IsOptional)
                        {
                            sb.Append("?");
                        }
                        sb.Append(" ");
                        sb.Append(parameter.Name);
                        x++;
                    }
                }
                sb.Append(")");
                return sb.ToString();
            }
        }
    }
}
