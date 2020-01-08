using System.Windows;
using Teronis.Extensions.Wpf;
using WindowControl = System.Windows.Window;

namespace Teronis.AttachedProperties
{
    public static class Window
    {
        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var window = d as WindowControl;

            if (window != null && window.IsModal())
                window.DialogResult = e.NewValue as bool?;
        }

        public static readonly DependencyProperty DialogResultProperty
            = DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(Window),
                new PropertyMetadata(DialogResultChanged));

        public static void SetDialogResult(WindowControl target, bool? value)
            => target.SetValue(DialogResultProperty, value);
        //=> ;
    }
}
