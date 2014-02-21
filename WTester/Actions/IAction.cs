using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stillbreathing.co.uk.WTester
{
    /// <summary>
    /// The base Action from which all WTest Actions must derive
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// The currently executing test
        /// </summary>
        WTest Test { get; set; }

        /// <summary>
        /// The message written just before the action is invoked
        /// </summary>
        string PreActionMessage { get; set; }

        /// <summary>
        /// The message written just after the action is invoked
        /// </summary>
        string PostActionMessage { get; set; }

        /// <summary>
        /// A value indicating whether the action has been successful
        /// </summary>
        bool Success { get; set; }

        /// <summary>
        /// The method which is called just before the action is executed
        /// </summary>
        void PreAction();

        /// <summary>
        /// The method that executes the action
        /// </summary>
        void Execute();
    }
}
