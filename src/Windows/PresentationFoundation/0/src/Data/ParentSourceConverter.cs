using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using Teronis.ObjectModel.Parenting;

namespace Teronis.Windows.PresentationFoundation.Data
{
    public class ParentSourceConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IHaveParents havingParents && parameter is Type wantedParentType) {
                return havingParents.CreateParentsCollector().CollectParents(wantedParentType).Single();
            } else {
                throw new Exception("Could not resolve parent");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
