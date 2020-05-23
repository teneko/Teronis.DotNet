

namespace Teronis.Reflection
{
    public abstract class PropertyEventArgsBase<PropertyType>
    {
        public string PropertyName { get; private set; }
        public abstract PropertyType CurrentPropertyValue { get; }

        public PropertyEventArgsBase(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
