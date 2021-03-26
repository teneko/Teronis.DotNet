// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

namespace Teronis.Reflection
{
    public static class AttributeMemberInfoExtensions
    {
        public static T FirstAttribute<T>(this AttributeMemberInfo<T> attrVarInfo)
            where T : Attribute
            => attrVarInfo.Attributes.First();
    }
}
