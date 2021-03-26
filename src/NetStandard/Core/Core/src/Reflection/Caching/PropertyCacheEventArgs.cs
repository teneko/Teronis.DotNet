// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis.Reflection.Caching
{
    public class PropertyCacheEventArgs<T>
    {
        public string PropertyName { get; private set; }
        [MaybeNull, AllowNull]
        public T PropertyValue { get; private set; }

        public PropertyCacheEventArgs(string propertyName, [AllowNull] T propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}
