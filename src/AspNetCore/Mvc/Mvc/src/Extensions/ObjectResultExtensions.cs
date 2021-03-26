// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.AspNetCore.Identity.Extensions
{
    public static class ObjectResultExtensions
    {
        [return: NotNullIfNotNull("objectResult")]
        public static T WithHttpStatusCode<T>(this T objectResult, HttpStatusCode statusCode)
            where T : ObjectResult?
        {
            if (!ReferenceEquals(objectResult, null)) {
                objectResult.StatusCode = (int)statusCode;
            }

            return objectResult;
        }
    }
}
