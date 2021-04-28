// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using Teronis.Extensions;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails
{
    public class ProblemDetailsResultExecutor : IActionResultExecutor<ProblemDetailsResult>
    {
        private readonly IActionResultExecutor<ObjectResult> resultExecutor;

        public ProblemDetailsResultExecutor(IActionResultExecutor<ObjectResult> resultExecutor) =>
            this.resultExecutor = resultExecutor ?? throw new ArgumentNullException(nameof(resultExecutor));

        public async Task ExecuteAsync(ActionContext context, ProblemDetailsResult result)
        {
            var response = context.HttpContext.Response;
            response.Headers.AddOrUpdate(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
            response.Headers.AddOrUpdate(HeaderNames.Pragma, "no-cache");
            response.Headers.AddOrUpdate(HeaderNames.Expires, "0");

            if (result.StatusCode is int statusCode) {
                response.StatusCode = statusCode;
            }

            await resultExecutor.ExecuteAsync(context, result);
        }
    }
}
