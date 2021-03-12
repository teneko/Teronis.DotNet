using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public sealed class ComponentProperty
    {
        public PropertyInfo PropertyInfo => propertyInfo;

        public string JSModuleIdentifier =>
            jsModuleIdentifier ?? throw new InvalidOperationException();

        private readonly PropertyInfo propertyInfo;
        private string? jsModuleIdentifier;

        public IReadOnlyList<Attribute> Attributes {
            get => attributes ??= propertyInfo.GetCustomAttributes().ToArray();
        }

        private Attribute[]? attributes;

        public ComponentProperty(PropertyInfo propertyInfo) =>
            this.propertyInfo = propertyInfo;

        public bool IsAttributeDefined(Type attributeType) =>
             propertyInfo.IsDefined(attributeType);

        public bool IsAttributeDefined(Type attributeType, bool inherit) =>
            propertyInfo.IsDefined(attributeType, inherit);

        public bool IsAttributeDefined<T>() =>
            IsAttributeDefined(typeof(T));

        public bool IsAttributeDefined<T>(bool inherit) =>
            IsAttributeDefined(typeof(T), inherit);
    }
}
