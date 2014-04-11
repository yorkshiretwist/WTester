using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stillbreathing.co.uk.WTester
{
    /// <summary>
    /// Represents the details of a parameter that can be passed to an action type
    /// </summary>
    public class ActionParameter
    {
        /// <summary>
        /// The name of this parameter
        /// </summary>
        public string Name;

        /// <summary>
        /// The description of this parameter
        /// </summary>
        public string Description;

        /// <summary>
        /// The type of this parameter
        /// </summary>
        public Type Type;

        /// <summary>
        /// Whether this parameter is optional
        /// </summary>
        public bool IsOptional;

        /// <summary>
        /// The default value of this parameter
        /// </summary>
        public object DefaultValue;
    }
}
