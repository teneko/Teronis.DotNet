using System;
using System.Drawing;

namespace Teronis.NetStandard.Drawing.Tools
{
    public static class FontTools
    {
        public static Font GetAdjustedFont(Graphics graphicRef, string graphicString, Font originalFont, int containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail)
        {
            Font testFont = null;
            // We utilize MeasureString which we get via a control instance           
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--) {
                testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

                // Test the string with the new size
                var adjustedSizeNew = graphicRef.MeasureString(graphicString, testFont);

                if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width)) {
                    // Good font, return it
                    return testFont;
                }
            } 

            // If you get here there was no fontsize that worked
            // return minimumSize or original?
            if (smallestOnFail) {
                return testFont;
            } else {
                return originalFont;
            }
        }
    }
}
