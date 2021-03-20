using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IComponentPropertyType : IMemberInfoAttributes
    {
        Type PropertyType { get; }
    }
}
