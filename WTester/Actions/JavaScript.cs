using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            this.Script = script;
        }

        public override void PreAction()
        {
            this.PreActionMessage = "Executing JavaScript";
        }

        public override void Execute()
        {
            try
            {
                string result = this.Test.Browser.Eval(this.Script);
                this.PostActionMessage = string.Format("Executed JavaScript. Result: {0}", result);
                this.Success = true;
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }
}
