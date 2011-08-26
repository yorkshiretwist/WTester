using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace stillbreathing.co.uk.WTester
{
    /// <summary>
    /// Invokes the right Action for a function
    /// </summary>
    public class ActionInvoker
    {
        public ActionInvoker() { }
        
        /// <summary>
        /// Invoke a new instance of the type for this action
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IAction Invoke(WTest wTestObject, string typeName, List<object> parameters)
        {
            if (typeName != null)
            {
                Type type = Type.GetType(typeName);
                if (type != null)
                {
                    try
                    {
                        // create the IAction object
                        IAction action = type.InvokeMember("", BindingFlags.CreateInstance, null, this, parameters.ToArray()) as IAction;

                        // pass the test to the action
                        action.Test = wTestObject;
                        return action;
                    }
                    catch (Exception ex)
                    {
                        wTestObject.Success = false;
                        wTestObject.Message = ex.Message;
                        if (ex.InnerException != null)
                        {
                            wTestObject.Message += ": " + ex.InnerException.Message;
                        }
                        return null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Run the PreAction() method of the IAction
        /// </summary>
        /// <param name="?"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public IAction PreAction(WTest wTestObject, IAction action)
        {
            try
            {
                action.PreAction();
            } 
            catch (Exception ex)
            {
                wTestObject.Success = false;
                wTestObject.Message = ex.Message;
                if (ex.InnerException != null)
                {
                    wTestObject.Message += ": " + ex.InnerException.Message;
                }
            }
            return action;
        }

        /// <summary>
        /// Run the Execute() method of the IAction
        /// </summary>
        /// <param name="wTestObject"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public IAction Execute(WTest wTestObject, IAction action)
        {
            try
            {
                // execute the action
                action.Execute();

                // get the results from the action
                wTestObject.Success = action.Success;
                wTestObject.Message = action.PostActionMessage;
            }
            catch (Exception ex)
            {
                wTestObject.Success = false;
                wTestObject.Message = ex.Message;
                if (ex.InnerException != null)
                {
                    wTestObject.Message += ": " + ex.InnerException.Message;
                }
            }
            return action;
        }
    }
}
