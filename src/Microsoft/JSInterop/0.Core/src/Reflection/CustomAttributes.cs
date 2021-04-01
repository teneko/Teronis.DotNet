// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    public class CustomAttributes : ICustomAttributes
    {
        public readonly static CustomAttributes Empty = new CustomAttributes(EnumerableCustomAttributes.Empty);

        public static CustomAttributes FromEnumerable(IEnumerable<Attribute> attributes) =>
            new CustomAttributes(new EnumerableCustomAttributes(attributes));

        public static CustomAttributes FromEnumerable(params Attribute[] attributes) =>
            FromEnumerable((IEnumerable<Attribute>)attributes);

        public ILookup<Type, Attribute> Attributes =>
            customAttributes.Attributes;

        private ICustomAttributes customAttributes;

        internal CustomAttributes(ICustomAttributes customAttributes) =>
            this.customAttributes = customAttributes;

        public CustomAttributes(ICustomAttributeProvider customAttributeProvider) =>
            customAttributes = new CustomProvidingAttributes(customAttributeProvider);

        public bool IsAttributeDefined(Type attributeType) =>
            customAttributes.IsAttributeDefined(attributeType);

        public bool IsAttributeDefined<T>() =>
            customAttributes.IsAttributeDefined<T>();

        public bool TryGetAttribute<T>([MaybeNullWhen(false)] out T attribute)
            where T : Attribute =>
            customAttributes.TryGetAttribute(out attribute);

        private class CustomProvidingAttributes : ICustomAttributes {
            public ILookup<Type, Attribute> Attributes {
                get {
                    if (attributes is null) {
                        attributes = GetAttributes()
                            .Cast<Attribute>()
                            .ToLookup(x => x.GetType());
                    }

                    return attributes;
                }
            }

            private ILookup<Type, Attribute>? attributes;
            private readonly ICustomAttributeProvider customAttributeProvider;

            public CustomProvidingAttributes(ICustomAttributeProvider customAttributeProvider) =>
                this.customAttributeProvider = customAttributeProvider ?? throw new ArgumentNullException(nameof(customAttributeProvider));

            protected IEnumerable<Attribute> GetAttributes() =>
                customAttributeProvider
                    .GetCustomAttributes(inherit: true)
                    .Cast<Attribute>();

            protected bool IsDefined(Type attributeType) =>
                customAttributeProvider.IsDefined(attributeType, inherit: true);

            public bool IsAttributeDefined(Type attributeType) =>
                attributes?.Contains(attributeType) ?? IsDefined(attributeType);

            public bool IsAttributeDefined<T>() =>
                IsAttributeDefined(typeof(T));

            public bool TryGetAttribute<T>([MaybeNullWhen(false)] out T attribute)
                where T : Attribute
            {
                var attributeType = typeof(T);

                if (!IsAttributeDefined(attributeType)) {
                    attribute = null;
                    return false;
                }

                attribute = (T)Attributes[attributeType].First();
                return true;
            }
        }

        private class EnumerableCustomAttributes : ICustomAttributes {
            public static EnumerableCustomAttributes Empty = new EnumerableCustomAttributes(Enumerable.Empty<Attribute>());

            public ILookup<Type, Attribute> Attributes { get; }

            public EnumerableCustomAttributes(IEnumerable<Attribute> attributes) {
                if (attributes is null) {
                    throw new ArgumentNullException(nameof(attributes));
                }

                Attributes = attributes.ToLookup(x => default(Type)!);
            }

            public bool IsAttributeDefined(Type attributeType) =>
                false;

            public bool IsAttributeDefined<T>() =>
                false;

            public bool TryGetAttribute<T>([MaybeNullWhen(false)] out T attribute)
                where T : Attribute
            {
                attribute = null;
                return false;
            }
        }
    }
}
