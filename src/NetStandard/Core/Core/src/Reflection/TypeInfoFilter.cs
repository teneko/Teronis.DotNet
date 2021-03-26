// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;

namespace Teronis.Reflection
{
    public class TypeInfoFilter : ITypeInfoFilter
    {
        private TypeInfo[]? typeInfoAllowList;
        private TypeInfo[]? typeInfoBlockList;

        public TypeInfoFilter(TypeInfo[]? typeInfoAllowList, params TypeInfo[]? typeInfoBlockList)
        {
            this.typeInfoAllowList = typeInfoAllowList;
            this.typeInfoBlockList = typeInfoBlockList;
        }

        public TypeInfoFilter(params TypeInfo[] typeInfoAllowList)
            : this(typeInfoAllowList, default) { }

        public virtual bool IsAllowed(TypeInfo? typeInfo) =>
            (typeInfoAllowList == null || typeInfoAllowList.Contains(typeInfo))
            && (typeInfoBlockList == null || !typeInfoBlockList.Contains(typeInfo));
    }
}
