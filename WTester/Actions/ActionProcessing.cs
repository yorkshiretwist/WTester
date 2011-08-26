using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace stillbreathing.co.uk.WTester.Actions.ActionProcessing
{
    /// <summary>
    /// Pauses the processing of actions by a number of seconds
    /// </summary>
    public class Pause : BaseAction
    {
        public int Interval;

        public Pause(int interval)
        {
            this.Interval = interval;
        }

        public override void PreAction()
        {
            this.PreActionMessage = String.Format("Pausing for {0} seconds", this.Interval);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                this.Success = true;
                Thread.Sleep(this.Interval * 1000);
                this.PostActionMessage = String.Format("Paused for {0} seconds", this.Interval);
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }
}
