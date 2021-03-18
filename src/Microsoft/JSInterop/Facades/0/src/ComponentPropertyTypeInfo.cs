using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class ComponentPropertyTypeInfo : MemberInfoAttributeLookup, IComponentPropertyTypeInfo
    {
        public Type PropertyType { get; }

        public ComponentPropertyTypeInfo(Type propertyType)
            : base(propertyType) =>
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
    }
}
