using System.Windows;
using Teronis.Extensions;
using WindowControl = System.Windows.Window;

namespace Teronis.Wpf.AttachedProperties
{
    public static class Window
    {
        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowControl window && window.IsModal()) {
                window.DialogResult = e.NewValue as bool?;
            }
        }

        public static readonly DependencyProperty DialogResultProperty
            = DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(Window),
                new PropertyMetadata(DialogResultChanged));

        public static void SetDialogResult(WindowControl target, bool? value)
            => target.SetValue(DialogResultProperty, value);
    }
}
