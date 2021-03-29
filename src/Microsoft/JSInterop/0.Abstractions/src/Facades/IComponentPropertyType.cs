// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IComponentPropertyType : ICustomAttributes
    {
        Type PropertyType { get; }
    }
}
