using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using stillbreathing.co.uk.WTester.Actions;

namespace stillbreathing.co.uk.WTester
{
    /// <summary>
    /// Parses a WTest document
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Get the function name from an action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string GetFunctionName(string action)
        {
            if (action.Contains("(")) return action.Substring(0, action.IndexOf("(")).ToLower();
            return null;
        }

        /// <summary>
        /// Parses an action and returns an array of its parameters
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public List<object> GetParameters(string action)
        {
            int start = action.IndexOf('(');
            int end = action.Length;
            if (action.EndsWith("]")) end = action.LastIndexOf("[");
            if (start == end - 1) return new List<object>();
            List<string> p = Split(action.Substring(start + 1, end - (start + 2)), ",", "\"", true);
            List<object> output = new List<object>();
            foreach (string item in p)
            {
                string thisitem = item.Trim('(').Trim(')');
                if (thisitem.StartsWith("\"") && thisitem.EndsWith("\""))
                {
                    output.Add(thisitem.Trim('"'));
                }
                else
                {
                    int integer;
                    bool boolean;
                    if (Int32.TryParse(thisitem, out integer))
                    {
                        output.Add(integer);
                    }
                    else if (Boolean.TryParse(thisitem, out boolean))
                    {
                        output.Add(boolean);
                    }
                    else
                    {
                        output.Add(thisitem);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Parses an action and gets the requested element index
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public int? GetIndex(string action)
        {
            if (action.EndsWith("]"))
            {
                Match match = Regex.Match(action, @"\[([0-9]+)\]$");
                if (match.Success)
                {
                    return Int32.Parse(match.Value.Replace("[", "").Replace("]", ""));
                }
            }
            return null;
        }

        /// <summary>
        /// Split a string by a delimiter, respecting quoted text
        /// </summary>
        /// <remarks>
        /// From http://www.codeproject.com/KB/dotnet/TextQualifyingSplit.aspx
        /// </remarks>
        /// <param name="expression"></param>
        /// <param name="delimiter"></param>
        /// <param name="qualifier"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public List<string> Split(string expression, string delimiter,
            string qualifier, bool ignoreCase)
        {
            bool _QualifierState = false;
            int _StartIndex = 0;
            System.Collections.ArrayList _Values = new System.Collections.ArrayList();

            for (int _CharIndex = 0; _CharIndex < expression.Length - 1; _CharIndex++)
            {
                if ((qualifier != null)
                 & (string.Compare(expression.Substring
                (_CharIndex, qualifier.Length), qualifier, ignoreCase) == 0))
                {
                    _QualifierState = !(_QualifierState);
                }
                else if (!(_QualifierState) & (delimiter != null)
                      & (string.Compare(expression.Substring
                (_CharIndex, delimiter.Length), delimiter, ignoreCase) == 0))
                {
                    _Values.Add(expression.Substring
                (_StartIndex, _CharIndex - _StartIndex));
                    _StartIndex = _CharIndex + 1;
                }
            }

            if (_StartIndex < expression.Length)
                _Values.Add(expression.Substring
                (_StartIndex, expression.Length - _StartIndex));

            string[] _returnValues = new string[_Values.Count];
            _Values.CopyTo(_returnValues);
            List<string> list = _returnValues.ToList<string>();
            List<string> output = new List<string>();
            foreach (string item in list)
            {
                string thisitem = item.Trim();
                if (thisitem.StartsWith("\"") && thisitem.EndsWith("\""))
                {
                    output.Add(thisitem.Trim('"'));
                }
                else
                {
                    output.Add(thisitem);
                }
            }
            return output;
        }
    }
}
