

namespace Teronis.Reflection.Caching
{
    public class PropertyCacheEventArgs<T>
    {
        public string PropertyName { get; private set; }
        public T PropertyValue { get; private set; }

        public PropertyCacheEventArgs(string propertyName, T propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}
