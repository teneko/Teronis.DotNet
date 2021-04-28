// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails
{
    public static class ProblemDetailsUtils
    {
        public static ProblemDetails CreateDefault(
            string title,
            string? detail = null,
            string? instance = null,
            int? statusCode = null,
            string? type = null)
        {
            statusCode ??= StatusCodes.Status500InternalServerError;

            return new ProblemDetails() {
                Title = title,
                Detail = detail,
                Instance = instance,
                Status = statusCode,
                Type = type
            };
        }

        public static ProblemDetails CreateMissingMapper(Type exampleMapperType,
            string? detail = null,
            string? instance = null,
            int? statusCode = null)
        {
            exampleMapperType = exampleMapperType ?? throw new ArgumentNullException(nameof(exampleMapperType));
            var type = "https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Mvc/Mvc/src/JsonProblemDetails#missing-mapper";
            var title = $"You specified a problem details provider but your forgot to add the belonging mapper (e.g. {exampleMapperType.FullName}).";

            return CreateDefault(title,
                detail: detail,
                instance: instance,
                statusCode: statusCode,
                type: type);
        }
    }
}
