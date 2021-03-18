using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IComponentPropertyTypeInfo : IMemberInfoAttributes
    {
        Type PropertyType { get; }
    }
}
