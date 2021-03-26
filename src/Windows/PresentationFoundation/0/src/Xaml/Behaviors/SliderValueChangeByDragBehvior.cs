// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;

namespace Teronis.Windows.PresentationFoundation.Xaml.Behaviors
{
    public class SliderValueChangeByDragBehavior : Behavior<Slider>
    {
        private bool hasDragStarted;

        /// <summary>
        /// On behavior attached.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.AddHandler(Thumb.DragStartedEvent, (DragStartedEventHandler)Slider_DragStarted);
            AssociatedObject.AddHandler(Thumb.DragCompletedEvent, (DragCompletedEventHandler)Slider_DragCompleted);
            AssociatedObject.ValueChanged += Slider_ValueChanged;

            base.OnAttached();
        }

        /// <summary>
        /// On behavior detaching.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.RemoveHandler(Thumb.DragStartedEvent, (DragStartedEventHandler)Slider_DragStarted);
            AssociatedObject.RemoveHandler(Thumb.DragCompletedEvent, (DragCompletedEventHandler)Slider_DragCompleted);
            AssociatedObject.ValueChanged -= Slider_ValueChanged;
        }

        private void updateValueBindingSource()
            => BindingOperations.GetBindingExpression(AssociatedObject, RangeBase.ValueProperty)?.UpdateSource();

        private void Slider_DragStarted(object sender, DragStartedEventArgs e)
            => hasDragStarted = true;

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            hasDragStarted = false;
            updateValueBindingSource();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!hasDragStarted) {
                updateValueBindingSource();
            }
        }
    }
}
