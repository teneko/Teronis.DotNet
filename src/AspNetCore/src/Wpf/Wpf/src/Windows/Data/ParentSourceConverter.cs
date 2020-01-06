using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Teronis.Data;

namespace Teronis.Windows.Data
{
    public class ParentSourceConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IHaveParents havingParents && parameter is Type wantedParentType)
                return havingParents.GetParentsPicker().GetSingleParent(wantedParentType);
            else
                throw new Exception("Could not resolve parent");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
