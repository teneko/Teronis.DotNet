using System;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public sealed class ComponentPropertyInfo : MemberInfoAttributeLookup, IComponentPropertyInfo
    {
        public PropertyInfo PropertyInfo =>
            propertyInfo;

        public Type PropertyType =>
            PropertyInfo.PropertyType;

        public ComponentPropertyTypeInfo ComponentPropertyTypeInfo {
            get {
                if (propertyType is null) {
                    propertyType = new ComponentPropertyTypeInfo(propertyInfo.PropertyType);
                }

                return propertyType;
            }
        }

        private readonly PropertyInfo propertyInfo;
        private ComponentPropertyTypeInfo? propertyType;

        public ComponentPropertyInfo(PropertyInfo propertyInfo)
            : base(propertyInfo) =>
            this.propertyInfo = propertyInfo;

        IComponentPropertyTypeInfo IComponentPropertyInfo.ComponentPropertyTypeInfo =>
            ComponentPropertyTypeInfo;
    }
}
