using System;
using System.Globalization;
using System.Windows.Data;

namespace Teronis.Windows.PresentationFoundation.Data
{
    class UtcToLocalDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) {
                return null;
            } else if (value is DateTime || value is DateTime?) {
                var dateTime = (DateTime)value;
                return dateTime.ToLocalTime();
            } else {
                return DateTime.SpecifyKind(DateTime.Parse(value.ToString()), DateTimeKind.Utc).ToLocalTime();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
