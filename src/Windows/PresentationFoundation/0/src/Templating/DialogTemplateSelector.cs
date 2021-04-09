// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using Teronis.Windows.PresentationFoundation.ViewModels;

namespace Teronis.Windows.PresentationFoundation.Templating
{
    public class DialogTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? HeaderFooterTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DialogFooterYesNoViewModel && !(HeaderFooterTemplate is null)) {
                return HeaderFooterTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
