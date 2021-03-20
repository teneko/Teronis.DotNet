using System;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IComponentProperty : IMemberInfoAttributes
    {
        PropertyInfo PropertyInfo { get; }
        Type PropertyType { get; }
        IComponentPropertyType ComponentPropertyType { get; }
    }
}
