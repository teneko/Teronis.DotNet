using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.Mvc.JsonProblemDetails.ObjectResolvers
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
