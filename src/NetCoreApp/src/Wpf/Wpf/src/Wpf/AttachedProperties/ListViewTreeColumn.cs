using System.Windows;
using System.Windows.Data;

namespace Teronis.Wpf.AttachedProperties
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
