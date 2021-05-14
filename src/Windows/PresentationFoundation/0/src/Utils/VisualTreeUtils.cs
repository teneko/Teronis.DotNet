// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace Teronis.Windows.PresentationFoundation.Utils
{
    public class VisualTreeUtils
    {
        public static bool IsMouseOverTarget(Visual? target, GetPositionFromInputElementDelegate getPosition)
        {
            // It can happen that target is null
            if (target is null) {
                return false;
            }

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
        /// <param name="dependencyObject">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise null.</returns>
        public static DependencyObject? GetParentObject(DependencyObject? dependencyObject)
        {
            if (dependencyObject is null) {
                return null;
            }

            if (dependencyObject is ContentElement contentElement) {
                var parent = ContentOperations.GetParent(contentElement);

                if (!(parent is null)) {
                    return parent;
                }

                var frameworkContentElement = contentElement as FrameworkContentElement;
                return frameworkContentElement?.Parent;
            }

            // If it's not a ContentElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(dependencyObject);
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="dependencyObject">
        /// A direct or indirect child of the queried item.
        /// </param>
        /// <returns>
        /// The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null reference is being returned.
        /// </returns>
        [return: MaybeNull]
        public static T GetParentObjectRecursive<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            // Get parent item
            var parentObject = GetParentObject(dependencyObject);

            // We've reached the end of the tree
            if (parentObject == null) {
                return null;
            }

            // Check if the parent matches the type we're looking for
            if (parentObject is T parent) {
                return parent;
            }

            // Use recursion to proceed with next level
            return GetParentObjectRecursive<T>(parentObject);
        }
    }
}
