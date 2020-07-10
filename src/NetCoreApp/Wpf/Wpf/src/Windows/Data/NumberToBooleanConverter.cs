using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Teronis.Windows.Data
{
    public class NumberToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number;

            if (value is int valueInteger) {
                number = valueInteger;
            } else if (value is GridLength gridLength) {
                number = (int)gridLength.Value;
            } else {
                number = 1;
            }

            Visibility visibility;

            if (number == 0) {
                visibility = Visibility.Collapsed;
            } else {
                visibility = Visibility.Visible;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
