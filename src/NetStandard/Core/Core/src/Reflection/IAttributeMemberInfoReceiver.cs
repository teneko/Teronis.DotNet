// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Reflection;

namespace Teronis
{
    public interface IAttributeMemberInfoReceiver<T> where T : Attribute
    {
        void ReceiveAttributeVariableInfo(AttributeMemberInfo<T> attrVarInfo);
    }
}
