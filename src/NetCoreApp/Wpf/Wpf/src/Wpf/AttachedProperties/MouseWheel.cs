using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Teronis.Data.TreeColumn;
using Teronis.Windows.Media;

namespace Teronis.Wpf.AttachedProperties
{
    public static class MouseWheel
    {
        public static readonly DependencyProperty ShouldRouteBackwardProperty
            = DependencyProperty.RegisterAttached("ShouldRouteBackward", typeof(bool), typeof(MouseWheel), new PropertyMetadata(ShouldRouteBackwardChanged));

        private static void DefaultRouteBackwardPreviewMouseWheelHandler(object sender, MouseWheelEventArgs e)
        {
            if (sender is DependencyObject d && !e.Handled) {
                e.Handled = true;

                var args = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta) {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = e.Source,
                };

                var parent = VisualTreeHelperTools.GetParentObjectRecursive<UIElement>(d);
                parent.RaiseEvent(args);
            }
        }

        public static bool GetShouldRouteBackward(DependencyObject target)
            => (bool)target.GetValue(ShouldRouteBackwardProperty);

        public static void SetShouldRouteBackward(DependencyObject target, bool value)
            => target.SetValue(ShouldRouteBackwardProperty, value);

        public static void ShouldRouteBackwardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uiElement) {
                var shouldRouteBackward = GetShouldRouteBackward(uiElement);
                var defaultMouseWheelEventHandler = (MouseWheelEventHandler)DefaultRouteBackwardPreviewMouseWheelHandler;

                if (shouldRouteBackward) {
                    uiElement.RemoveHandler(UIElement.PreviewMouseWheelEvent, defaultMouseWheelEventHandler);
                    uiElement.AddHandler(UIElement.PreviewMouseWheelEvent, defaultMouseWheelEventHandler);
                } else if (!shouldRouteBackward) {
                    uiElement.RemoveHandler(UIElement.PreviewMouseWheelEvent, defaultMouseWheelEventHandler);
                }
            }
        }
    }
}
