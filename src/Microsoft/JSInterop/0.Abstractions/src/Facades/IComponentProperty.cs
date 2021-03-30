// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IComponentProperty : ICustomAttributes
    {
        PropertyInfo PropertyInfo { get; }
        Type OrignatingType { get; }
        IComponentPropertyType PropertyType { get; }
    }
}
