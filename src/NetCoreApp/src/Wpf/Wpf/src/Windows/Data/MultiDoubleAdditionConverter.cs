using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;

namespace Teronis.Windows.Data
{
    public class MultiDoubleAdditionConverter : IMultiValueConverter
    {
        public object Convert(object[] targets, Type targetType, object parameter, CultureInfo culture)
        {
            if (targets == null)
                return null;

            double result = 0;

            foreach (var target in targets)
            {
                if (double.TryParse(target?.ToString(), out var targetSummand))
                    result += targetSummand;
                else
                    return null;
            }

            if (double.TryParse(parameter?.ToString(), out var parameterSummand))
                result += parameterSummand;

            return result;
        }

        public object[] ConvertBack(object values, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
