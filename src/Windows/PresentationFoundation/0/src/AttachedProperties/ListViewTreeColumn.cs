// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Data;

namespace Teronis.Windows.PresentationFoundation.AttachedProperties
{
    public class ListViewTreeColumn
    {
        public static readonly DependencyProperty BindingProperty
            = DependencyProperty.RegisterAttached("BindingProperty", typeof(Binding), typeof(ListViewTreeColumn), new PropertyMetadata());

        public static Binding GetBinding(DependencyObject target)
            => (Binding)target.GetValue(BindingProperty);

        public static void SetBinding(DependencyObject target, Binding binding)
            => target.SetValue(BindingProperty, binding);
    }
}
