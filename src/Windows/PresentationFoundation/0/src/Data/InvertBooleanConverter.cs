// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace Teronis.Windows.PresentationFoundation.Data
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool original = (bool)value;
            return !original;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool original = (bool)value;
            return !original;
        }
    }
}
