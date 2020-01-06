using System;
using System.Globalization;
using System.Windows.Data;
using Teronis.Windows.Data;

namespace Teronis.Windows.Data
{
    public class CultureAwareBinding : Binding
    {
        public CultureAwareBinding()
            : base()
            => setConverterCulture();

        public CultureAwareBinding(Type propertyType)
            : base()
            => setConverterCulture(propertyType);

        public CultureAwareBinding(string path)
            : base(path)
            => setConverterCulture();

        public CultureAwareBinding(string path, Type propertyType)
            : base(path)
            => setConverterCulture(propertyType);

        private void setConverterCulture(Type propertyType)
        {
            ConverterCulture = CultureInfo.CurrentCulture;

            if (propertyType != null && (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)))
                Converter = new UtcToLocalDateTimeConverter();
        }

        private void setConverterCulture()
            => setConverterCulture(null);
    }
}
