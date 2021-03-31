// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    internal sealed class InvocationDefinition : ManagedDefinitionBase
    {
        public override Type MemberType { get; }

        public InvocationDefinition(Type taskArgumentType, ICustomAttributes definitionAttributes, ManagedMemberType managedMemberType)
            : base(definitionAttributes, managedMemberType) =>
            MemberType = taskArgumentType;
    }
}
