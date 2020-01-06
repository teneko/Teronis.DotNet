using System.Windows;
using System.Windows.Controls;
using Teronis.ViewModels.Wpf;

namespace Teronis.Windows.Templates
{
    public class DialogHeaderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DialogHeaderMessageTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DialogHeaderMessageViewModel)
                return DialogHeaderMessageTemplate;
            else
                return base.SelectTemplate(item, container);
        }
    }
}
