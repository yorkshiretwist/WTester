using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WatiN.Core;

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
            this.Styles = styles;
        }

        public override void PreAction()
        {
            this.PreActionMessage = "Applying CSS styles";
        }

        public override void Execute()
        {
            try
            {
                string styleScript = "var head = document.getElementsByTagName('head')[0].innerHTML; head = head + '<style type=\"text/css\" class=\"wtesterstyles\">{0}</style>';";
                styleScript = string.Format(styleScript, this.Styles);
                string result = this.Test.Browser.Eval(styleScript);
                this.PostActionMessage = "Applied CSS styles";
                this.Success = true;
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Zooms the page in
    /// </summary>
    public class ZoomIn : BaseAction
    {
        public ZoomIn() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Zooming in";
        }

        public override void Execute()
        {
            try
            {
                this.Success = true;
                SendKeys.SendWait("^{+}");
                this.PostActionMessage = "Zoomed in";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Zooms the page out
    /// </summary>
    public class ZoomOut : BaseAction
    {
        public ZoomOut() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Zooming out";
        }

        public override void Execute()
        {
            try
            {
                this.Success = true;
                SendKeys.SendWait("^{-}");
                this.PostActionMessage = "Zoomed out";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }

    /// <summary>
    /// Resets the zoom
    /// </summary>
    public class ResetZoom : BaseAction
    {
        public ResetZoom() { }

        public override void PreAction()
        {
            this.PreActionMessage = "Resetting zoom";
        }

        public override void Execute()
        {
            try
            {
                this.Success = true;
                SendKeys.SendWait("^{0}");
                this.PostActionMessage = "Reset zoom";
            }
            catch (Exception ex)
            {
                this.PostActionMessage = ex.Message;
                this.Success = false;
            }
        }
    }
}
