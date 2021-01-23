using System.Windows;
using System.Windows.Controls;
using Teronis.Windows.PresentationFoundation.ViewModels;

namespace Teronis.Windows.PresentationFoundation.Templating
{
    public class DialogTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HeaderFooterTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DialogFooterYesNoViewModel) {
                return HeaderFooterTemplate;
            } else {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
