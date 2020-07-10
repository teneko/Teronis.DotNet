using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Teronis.Windows.Templating
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
