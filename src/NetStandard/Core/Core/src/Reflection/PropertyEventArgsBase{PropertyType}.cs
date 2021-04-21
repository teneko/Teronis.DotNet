// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Reflection
{
    public abstract class PropertyEventArgsBase<PropertyType>
    {
        public string PropertyName { get; private set; }
        public abstract PropertyType CurrentPropertyValue { get; }

        public PropertyEventArgsBase(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
