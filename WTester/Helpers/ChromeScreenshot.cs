using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace stillbreathing.co.uk.WTester.Helpers
{
    public class ChromeScreenshot
    {
        private IWebDriver _driver;

        /// <summary>
        /// Gets a screenshot of the entire page for a Chrome browser window
        /// </summary>
        /// <remarks>
        /// Code from http://dev.flauschig.ch/wordpress/?p=341
        /// </remarks>
        /// <param name="test"></param>
        /// <returns></returns>
        public Bitmap GetScreenshot(WTest test)
        {
            if (test.BrowserType != BrowserType.Chrome)
                return null;

            _driver = test.Browser;

            // Get the Total Size of the Document
            int totalWidth = (int)EvalScript<long>("return Math.max(document.body.scrollWidth,document.documentElement.scrollWidth,document.body.offsetWidth,document.documentElement.offsetWidth,document.body.clientWidth,document.documentElement.clientWidth);");
            int totalHeight = (int)EvalScript<long>("return Math.max(document.body.scrollHeight,document.documentElement.scrollHeight,document.body.offsetHeight,document.documentElement.offsetHeight,document.body.clientHeight,document.documentElement.clientHeight);");

            // Get the Size of the Viewport
            int viewportWidth = (int)EvalScript<long>("return Math.max(document.body.clientWidth,document.documentElement.clientWidth);");
            int viewportHeight = (int)EvalScript<long>("return Math.max(document.body.clientHeight,document.documentElement.clientHeight);");

            // Split the Screen in multiple Rectangles
            List<Rectangle> rectangles = new List<Rectangle>();
            // Loop until the Total Height is reached
            for (int i = 0; i < totalHeight; i += viewportHeight)
            {
                int newHeight = viewportHeight;
                // Fix if the Height of the Element is too big
                if (i + viewportHeight > totalHeight)
                {
                    newHeight = totalHeight - i;
                }
                // Loop until the Total Width is reached
                for (int ii = 0; ii < totalWidth; ii += viewportWidth)
                {
                    int newWidth = viewportWidth;
                    // Fix if the Width of the Element is too big
                    if (ii + viewportWidth > totalWidth)
                    {
                        newWidth = totalWidth - ii;
                    }

                    // Create and add the Rectangle
                    Rectangle currRect = new Rectangle(ii, i, newWidth, newHeight);
                    rectangles.Add(currRect);
                }
            }

            // Build the Image
            var stitchedImage = new Bitmap(totalWidth, totalHeight);
            // Get all Screenshots and stitch them together
            Rectangle previous = Rectangle.Empty;
            foreach (var rectangle in rectangles)
            {
                // Calculate the Scrolling (if needed)
                if (previous != Rectangle.Empty)
                {
                    int xDiff = rectangle.Right - previous.Right;
                    int yDiff = rectangle.Bottom - previous.Bottom;
                    // Scroll
                    EvalScript<object>(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                    System.Threading.Thread.Sleep(200);
                }

                // Take Screenshot
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();

                // Build an Image out of the Screenshot
                Image screenshotImage;
                using (MemoryStream memStream = new MemoryStream(screenshot.AsByteArray))
                {
                    screenshotImage = Image.FromStream(memStream);
                }

                // Calculate the Source Rectangle
                Rectangle sourceRectangle = new Rectangle(viewportWidth - rectangle.Width, viewportHeight - rectangle.Height, rectangle.Width, rectangle.Height);

                // Copy the Image
                using (Graphics g = Graphics.FromImage(stitchedImage))
                {
                    g.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                }

                // Set the Previous Rectangle
                previous = rectangle;
            }
            // The full Screenshot is now in the Variable "stitchedImage"
            return stitchedImage;
        }

        private T EvalScript<T>(string script)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            return (T)js.ExecuteScript(script);
        }
    }
}
