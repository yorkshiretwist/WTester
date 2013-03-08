using System;
using System.Windows.Forms;
using OpenQA.Selenium;

namespace stillbreathing.co.uk.WTester.Actions.Style
{
    /// <summary>
    /// Applies the given styles to the current page
    /// </summary>
    public class CSS : BaseAction
    {
        private string Styles;

        public CSS(string styles)
        {
            Styles = styles;
        }

        public override void PreAction()
        {
            PreActionMessage = "Applying CSS styles";
        }

        public override void Execute()
        {
            try
            {
                var js = Test.Browser as IJavaScriptExecutor;
                string styleScript = "var head = document.getElementsByTagName('head')[0].innerHTML; head = head + '<style type=\"text/css\" class=\"wtesterstyles\">{0}</style>';";
                styleScript = string.Format(styleScript, Styles);
                js.ExecuteScript(styleScript, null);
                PostActionMessage = "Applied CSS styles";
                Success = true;
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    /// <summary>
    /// Zooms the page in
    /// </summary>
    public class ZoomIn : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Zooming in";
        }

        public override void Execute()
        {
            try
            {
                Success = true;
                SendKeys.SendWait("^{+}");
                PostActionMessage = "Zoomed in";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    /// <summary>
    /// Zooms the page out
    /// </summary>
    public class ZoomOut : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Zooming out";
        }

        public override void Execute()
        {
            try
            {
                Success = true;
                SendKeys.SendWait("^{-}");
                PostActionMessage = "Zoomed out";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }

    /// <summary>
    /// Resets the zoom
    /// </summary>
    public class ResetZoom : BaseAction
    {
        public override void PreAction()
        {
            PreActionMessage = "Resetting zoom";
        }

        public override void Execute()
        {
            try
            {
                Success = true;
                SendKeys.SendWait("^{0}");
                PostActionMessage = "Reset zoom";
            }
            catch (Exception ex)
            {
                PostActionMessage = ex.Message;
                Success = false;
            }
        }
    }
}
