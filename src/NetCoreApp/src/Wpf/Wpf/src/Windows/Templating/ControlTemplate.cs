using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Teronis.Windows.Templating
{
    public class ControlTemplate<FrameworkElementType> : DataTemplate
        where FrameworkElementType : FrameworkElement
    {
        public Binding DataContextBinding {
            set => VisualTree.SetBinding(FrameworkElement.DataContextProperty, value);
        }

        public ControlTemplate()
            => VisualTree = new FrameworkElementFactory(typeof(FrameworkElementType));
    }
}
