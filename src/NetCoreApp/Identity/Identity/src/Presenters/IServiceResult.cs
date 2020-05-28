using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Teronis.Identity.Presenters
{
    public interface IServiceResult : IStatusCodeActionResult, IActionResult
    {
        bool Succeeded { get; }
        object? Value { get; }
        JsonErrors? Errors { get; }
        Type DeclaredType { get; }
        FormatterCollection<IOutputFormatter>? Formatters { get; }
        MediaTypeCollection? ContentTypes { get; }

        ServiceResultDatransject DeepCopy();
    }
}
