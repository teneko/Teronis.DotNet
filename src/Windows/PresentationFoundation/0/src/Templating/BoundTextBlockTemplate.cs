// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Teronis.Windows.PresentationFoundation.Templating
{
    public class BoundTextBlockTemplate : DataTemplate
    {
        public Binding TextPropertyBinding {
            set => VisualTree.SetBinding(TextBlock.TextProperty, value);
        }

        public BoundTextBlockTemplate() =>
            VisualTree = new FrameworkElementFactory(typeof(TextBlock));
    }
}
