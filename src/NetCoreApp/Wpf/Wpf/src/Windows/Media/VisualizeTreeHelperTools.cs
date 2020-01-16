using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Teronis.Windows.Media
{
    public class VisualTreeHelperTools
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

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Do note, that for content element,
        /// this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise null.</returns>
        public static DependencyObject GetParentObject(DependencyObject child)
        {
            if (child == null)
                return null;

            if (child is ContentElement contentElement) {
                var parent = ContentOperations.GetParent(contentElement);

                if (parent != null)
                    return parent;

                var fce = contentElement as FrameworkContentElement;

                return fce != null ? fce.Parent : null;
            }

            // If it's not a ContentElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">
        /// A direct or indirect child of the queried item.
        /// </param>
        /// <returns>
        /// The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null reference is being returned.
        /// </returns>
        public static T GetParentObjectRecursive<T>(DependencyObject child)
            where T : DependencyObject
        {
            // Get parent item
            var parentObject = GetParentObject(child);

            // We've reached the end of the tree
            if (parentObject == null)
                return null;

            // Check if the parent matches the type we're looking for
            if (parentObject is T parent)
                return parent;
            else                 // Use recursion to proceed with next level
                return GetParentObjectRecursive<T>(parentObject);
        }
    }
}
