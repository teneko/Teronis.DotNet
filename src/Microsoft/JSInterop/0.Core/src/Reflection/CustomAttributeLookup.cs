// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    public class CustomAttributeLookup : ICustomAttributes
    {
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

        public CustomAttributeLookup(ICustomAttributeProvider customAttributeProvider) =>
            this.customAttributeProvider = customAttributeProvider;

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
}
