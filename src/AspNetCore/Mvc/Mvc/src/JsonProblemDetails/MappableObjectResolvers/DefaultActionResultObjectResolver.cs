// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails.MappableObjectResolvers
{
    public class DefaultActionResultObjectResolver : IMappableObjectResolver
    {
        public bool TryResolveObject(IActionResult result, [MaybeNullWhen(false)] out object resolvedObject)
        {
            if (result is ObjectResult objectResult) {
                resolvedObject = objectResult.Value;
                return true;
            }

            resolvedObject = null;
            return false;
        }
    }
}
