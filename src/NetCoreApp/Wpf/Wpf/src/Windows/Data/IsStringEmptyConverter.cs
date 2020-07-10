using System;
using System.Globalization;
using System.Windows.Data;

namespace Teronis.Windows.Data
{
    public class IsStringEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isStringNullOrEmpty = string.IsNullOrEmpty(value as string);

            if (parameter is bool invert && invert) {
                return !isStringNullOrEmpty;
            } else {
                return isStringNullOrEmpty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
