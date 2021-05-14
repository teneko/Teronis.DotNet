// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Data;

namespace Teronis.Windows.PresentationFoundation.Templating
{
    public class ControlTemplate<FrameworkElementType> : DataTemplate
        where FrameworkElementType : FrameworkElement
    {
        public Binding DataContextBinding {
            set => VisualTree.SetBinding(FrameworkElement.DataContextProperty, value);
        }

        public ControlTemplate() =>
            VisualTree = new FrameworkElementFactory(typeof(FrameworkElementType));
    }
}
