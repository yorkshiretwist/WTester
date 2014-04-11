using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace stillbreathing.co.uk.WTester.Actions.Cookies
{
    /// <summary>
    /// Gets the value of a cookie
    /// </summary>
    public class GetCookie : BaseAction
    {
        public string CookieName;

        /// <summary>
        /// Gets the value of a cookie
        /// </summary>
        /// <returns></returns>
        public GetCookie(string cookieName)
        {
            CookieName = cookieName.Trim().Trim('\'');
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Getting the value from cookie: {0}", CookieName);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Cookie cookie = Test.Browser.Manage().Cookies.GetCookieNamed(CookieName);
                if (cookie != null)
                {
                    Success = true;
                    PostActionMessage = String.Format("Cookie '{0}' value: '{1}'", CookieName, cookie.Value);
                }
                else
                {
                    Success = false;
                    PostActionMessage = String.Format("Cookie '{0}' not found", CookieName);
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
                    Name = "cookieName",
                    Type = typeof(string),
                    Description = "The name of the cookie to get",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Sets the value of a cookie
    /// </summary>
    public class SetCookie : BaseAction
    {
        public string CookieName;
        public string Value;

        /// <summary>
        /// Gets the value of a cookie
        /// </summary>
        /// <returns></returns>
        public SetCookie(string cookieName, string value)
        {
            CookieName = cookieName.Trim().Trim('\'');
            Value = value.Trim().Trim('\'');
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Setting the value of cookie '{0}' to '{1}'", CookieName, Value);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Success = true;
                Cookie cookie = Test.Browser.Manage().Cookies.GetCookieNamed(CookieName);
                if (cookie != null)
                {
                    PostActionMessage = String.Format("Updated cookie '{0}' with value: '{1}'", CookieName, Value);
                }
                else
                {
                    Test.Browser.Manage().Cookies.AddCookie(new Cookie(CookieName, Value));
                    PostActionMessage = String.Format("Set cookie '{0}' with value '{1}'", CookieName, Value);
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
                    Name = "cookieName",
                    Type = typeof(string),
                    Description = "The name of the cookie to set",
                    IsOptional = false,
                    DefaultValue = null
                });
                parameters.Add(new ActionParameter
                {
                    Name = "value",
                    Type = typeof(string),
                    Description = "The value of the cookie",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }

    /// <summary>
    /// Deletes a cookie
    /// </summary>
    public class DeleteCookie : BaseAction
    {
        public string CookieName;

        /// <summary>
        /// Gets the value of a cookie
        /// </summary>
        /// <returns></returns>
        public DeleteCookie(string cookieName)
        {
            CookieName = cookieName.Trim().Trim('\'');
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Deleting cookie: {0}", CookieName);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Success = true;
                Cookie cookie = Test.Browser.Manage().Cookies.GetCookieNamed(CookieName);
                if (cookie != null)
                {
                    Test.Browser.Manage().Cookies.DeleteCookieNamed(CookieName);
                    PostActionMessage = String.Format("Cookie '{0}' deleted'", CookieName, cookie.Value);
                }
                else
                {
                    PostActionMessage = String.Format("Cookie '{0}' not found", CookieName);
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
                    Name = "cookieName",
                    Type = typeof(string),
                    Description = "The name of the cookie to delete",
                    IsOptional = false,
                    DefaultValue = null
                });
                return parameters;
            }
        }
    }
}
