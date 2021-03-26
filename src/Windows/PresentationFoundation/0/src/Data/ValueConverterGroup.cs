// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace Teronis.Windows.PresentationFoundation.Data
{
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotImplementedException();

        #endregion
    }
}
