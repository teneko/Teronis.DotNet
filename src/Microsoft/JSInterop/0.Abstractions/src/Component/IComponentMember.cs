// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public interface IComponentMember : IDefinition
    {
        MemberInfo MemberInfo { get; }

        public void SetValue(object? value);
    }
}
