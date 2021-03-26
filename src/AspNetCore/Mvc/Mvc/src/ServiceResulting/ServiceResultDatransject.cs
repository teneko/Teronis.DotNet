// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Teronis.Mvc.ServiceResulting
{
    public readonly ref struct ServiceResultDatransject
    {
        public readonly bool Succeeded { get; }
        public readonly object? Content { get; }
        public readonly JsonErrors? Errors { get; }
        public readonly Type DeclaredType { get; }
        public readonly FormatterCollection<IOutputFormatter>? Formatters { get; }
        public readonly MediaTypeCollection? ContentTypes { get; }
        public readonly int? StatusCode { get; }

        public ServiceResultDatransject(bool succeeded, object? content, JsonErrors? errors, Type declaredType,
            FormatterCollection<IOutputFormatter>? formatters, MediaTypeCollection? contentTypes, int? statusCode)
        {
            Succeeded = succeeded;
            Content = content;
            Errors = errors;
            DeclaredType = declaredType;
            Formatters = formatters;
            ContentTypes = contentTypes;
            StatusCode = statusCode;
        }
    }
}
