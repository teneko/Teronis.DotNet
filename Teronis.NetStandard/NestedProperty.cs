using System.Reflection;

namespace Teronis.NetStandard
{
    public class NestedProperty
    {
        public PropertyInfo PropertyInfo { get; set; }
        public object OriginObject { get; set; }
    }
}
