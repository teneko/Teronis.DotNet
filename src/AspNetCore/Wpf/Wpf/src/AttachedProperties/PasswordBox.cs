using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PasswordBoxControl = System.Windows.Controls.PasswordBox;

namespace Teronis.AttachedProperties
{
    public static class PasswordBox
    {
        public static readonly DependencyProperty IsPasswordUpdatingProperty =
            DependencyProperty.RegisterAttached("IsPasswordUpdating", typeof(bool), typeof(PasswordBox), new PropertyMetadata(false));

        public static bool GetIsPasswordUpdating(DependencyObject dependencyObject)
            => (bool)dependencyObject.GetValue(IsPasswordUpdatingProperty);

        public static void SetIsPasswordUpdating(DependencyObject dependencyObject, bool value)
            => dependencyObject.SetValue(IsPasswordUpdatingProperty, value);

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBoxControl)sender;
            SetIsPasswordUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsPasswordUpdating(passwordBox, false);
        }

        public static void PasswordChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var passwordBox = dependencyObject as PasswordBoxControl;

            if (passwordBox == null)
                return;

            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            string newPassword = (string)args.NewValue;

            if (!GetIsPasswordUpdating(dependencyObject))
                passwordBox.Password = newPassword;

            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(PasswordChanged)));

        public static string GetPassword(DependencyObject dependencyObject)
            => (string)dependencyObject.GetValue(PasswordProperty);

        public static void SetPassword(DependencyObject dependencyObject, string password)
            => dependencyObject.SetValue(PasswordProperty, password);

        public static void IsPasswordChangedObservedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) {
            var passwordBox = dependencyObject as PasswordBoxControl;

            if (passwordBox == null)
                return;

            if ((bool)args.OldValue)
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if ((bool)args.NewValue)
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        public static readonly DependencyProperty IsPasswordChangedObservedProperty =
            DependencyProperty.RegisterAttached("IsPasswordChangedObserved", typeof(bool), typeof(PasswordBox), new PropertyMetadata(false, IsPasswordChangedObservedChanged));

        public static bool GetIsPasswordChangedObserved(DependencyObject dependencyObject)
            => (bool)dependencyObject.GetValue(IsPasswordChangedObservedProperty);

        public static void SetIsPasswordChangedObserved(DependencyObject dependencyObject, bool value)
            => dependencyObject.SetValue(IsPasswordChangedObservedProperty, value);
    }
}
