// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails.MappableObjectResolvers
{
    public interface IMappableObjectResolver
    {
        bool TryResolveObject(IActionResult result, [MaybeNullWhen(false)] out object resolvedObject);
    }
}
