// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.



namespace Teronis.Reflection.Caching
{
    public class PropertyCacheRemovedEventArgs<TProperty> : PropertyCacheEventArgs<TProperty>
    {
        public PropertyCacheRemovedEventArgs(string propertyName, TProperty property)
            : base(propertyName, property) { }
    }
}
