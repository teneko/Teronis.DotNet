using System;
using System.Globalization;
using System.Windows.Data;

namespace Teronis.Windows.Data
{
    public class CultureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string cultureName) {
                return new CultureInfo(cultureName);
            }

            if (value != null && value is int cultureNumber) {
                return new CultureInfo(cultureNumber);
            } else {
                throw new ArgumentException($"The content '{value}' cannot be converted to an instance of {nameof(CultureInfo)}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CultureInfo cultureInfo) {
                return cultureInfo.Name;
            } else {
                throw new ArgumentException($"The content '{value}' is not an instance of {nameof(CultureInfo)}");
            }
        }
    }
}
