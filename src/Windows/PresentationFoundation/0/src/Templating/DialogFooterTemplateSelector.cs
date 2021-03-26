// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using Teronis.Windows.PresentationFoundation.ViewModels;

namespace Teronis.Windows.PresentationFoundation.Templating
{
    public class DialogFooterTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OkTemplate { get; set; }
        public DataTemplate YesNoTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is DialogFooterOkViewModel) {
                return OkTemplate;
            }

            if (item is DialogFooterYesNoViewModel) {
                return YesNoTemplate;
            } else {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
