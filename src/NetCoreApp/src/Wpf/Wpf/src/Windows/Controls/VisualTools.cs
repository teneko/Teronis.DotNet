using System.Windows;
using System.Windows.Media;

namespace Teronis.Windows.Controls
{
    public class VisualTools
    {
        public static bool IsMouseOverTarget(Visual target, GetPositionFromInputElementDelegate getPosition)
        {
            // It can happen that target is null
            if (target == null)
                return false;

            var bounds = VisualTreeHelper.GetDescendantBounds(target);
            var mousePos = getPosition((IInputElement)target);
            return bounds.Contains(mousePos);
        }
    }
}
