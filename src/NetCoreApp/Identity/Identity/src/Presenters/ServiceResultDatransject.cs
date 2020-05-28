using System;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Teronis.Identity.Presenters
{
    public readonly ref struct ServiceResultDatransject
    {
        public readonly bool Succeeded { get; }
        public readonly object Value { get; }
        public readonly JsonErrors? Errors { get; }
        public readonly Type DeclaredType { get; }
        public readonly FormatterCollection<IOutputFormatter>? Formatters { get; }
        public readonly MediaTypeCollection? ContentTypes { get; }
        public readonly int? StatusCode { get; }

        public ServiceResultDatransject(bool succeeded, object value, JsonErrors? errors, Type declaredType,
            FormatterCollection<IOutputFormatter>? formatters, MediaTypeCollection? contentTypes, int? statusCode)
        {
            Succeeded = succeeded;
            Value = value;
            Errors = errors;
            DeclaredType = declaredType;
            Formatters = formatters;
            ContentTypes = contentTypes;
            StatusCode = statusCode;
        }
    }
}
