// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IComponentProperty : IMemberInfoAttributes
    {
        PropertyInfo PropertyInfo { get; }
        Type PropertyType { get; }
        IComponentPropertyType ComponentPropertyType { get; }
    }
}
