// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    internal sealed class InvocationDefinition : MemberDefinitionBase
    {
        public override string Name { get; }
        public override Type MemberType { get; }

        public InvocationDefinition(string name, Type taskArgumentType, ICustomAttributes invocationAttribtues, MemberTypeInfo taskArgumentTypeInfo)
            : base(invocationAttribtues, taskArgumentTypeInfo)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MemberType = taskArgumentType;
        }
    }
}
