using System;
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
            Interval = interval;
        }

        public override void PreAction()
        {
            PreActionMessage = String.Format("Pausing for {0} seconds", Interval);
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public override void Execute()
        {
            try
            {
                Success = true;
                Thread.Sleep(Interval * 1000);
                PostActionMessage = String.Format("Paused for {0} seconds", Interval);
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }
}
