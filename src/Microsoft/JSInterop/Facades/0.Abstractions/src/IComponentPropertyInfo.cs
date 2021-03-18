using System;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IComponentPropertyInfo : IMemberInfoAttributes
    {
        PropertyInfo PropertyInfo { get; }
        Type PropertyType { get; }
        IComponentPropertyTypeInfo ComponentPropertyTypeInfo { get; }
    }
}
