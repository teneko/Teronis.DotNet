

namespace Teronis.Data
{
    public class PropertyCacheEventArgs<T>
    {
        public string PropertyName { get; private set; }
        public T Property { get; private set; }

        public PropertyCacheEventArgs(string propertyName, T property)
        {
            PropertyName = propertyName;
            Property = property;
        }
    }
}
