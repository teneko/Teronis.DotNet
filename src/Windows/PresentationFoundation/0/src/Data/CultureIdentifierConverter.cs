// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace Teronis.Windows.PresentationFoundation.Data
{
    /// <summary>
    /// A converter that converts either a predefined System.Globalization.CultureInfo identifier, System.Globalization.CultureInfo.LCID
    /// property of an existing System.Globalization.CultureInfo object, or Windows-only
    /// culture identifier, or a predefined System.Globalization.CultureInfo name, System.Globalization.CultureInfo.Name
    /// of an existing System.Globalization.CultureInfo, or Windows-only culture name to an instance of
    /// <see cref="CultureInfo"/>.
    /// </summary>
    public class CultureIdentifierConverter : IValueConverter
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
