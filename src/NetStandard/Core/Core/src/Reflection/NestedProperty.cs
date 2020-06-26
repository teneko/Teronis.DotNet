using System.Reflection;

namespace Teronis.Reflection
{
    public class NestedProperty
    {
        public PropertyInfo? PropertyInfo { get; set; }
        public object? PropertyHolderObject { get; set; }
    }
}
