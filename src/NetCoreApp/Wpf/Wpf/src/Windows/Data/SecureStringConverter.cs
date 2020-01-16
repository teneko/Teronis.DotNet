using System;
using System.Globalization;
using Teronis.Extensions;
using System.Windows.Data;
using System.Security;

namespace Teronis.Windows.Data
{
    public class SecureStringToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            else if (value is SecureString securedString)
                return securedString.ToUnsecureString();
            else
                throw new ArgumentException($"Value is not of type {typeof(SecureString)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            else if (value is string unsecuredString)
                return unsecuredString.ToSecureString();
            else
                throw new ArgumentException($"Value is not of type {typeof(string)}");
        }
    }
}
