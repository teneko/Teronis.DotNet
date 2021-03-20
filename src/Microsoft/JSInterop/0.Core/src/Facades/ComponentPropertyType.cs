using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class ComponentPropertyType : MemberInfoAttributeLookup, IComponentPropertyType
    {
        public Type PropertyType { get; }

        public ComponentPropertyType(Type propertyType)
            : base(propertyType) =>
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
    }
}
