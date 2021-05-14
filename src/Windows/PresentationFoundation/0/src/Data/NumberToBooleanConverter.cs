// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Teronis.Windows.PresentationFoundation.Data
{
    /// <summary>
    /// Converts a number of an <see cref="int"/> or <see cref="GridLength"/> to <see cref="Visibility"/>.
    /// </summary>
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
