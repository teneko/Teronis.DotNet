using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.Mvc.JsonProblemDetails.ObjectResolvers
{
    public interface IMappableObjectResolver
    {
        bool TryResolveObject(IActionResult result, [MaybeNullWhen(false)] out object resolvedObject);
    }
}
