// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails
{
    // Compare https://github.com/aspnet/Mvc/blob/master/src/Microsoft.AspNetCore.Mvc.Core/ObjectResult.cs
    public class ProblemDetailsResult : ObjectResult
    {
        /// <summary>
        /// The problem details that is about to be serialized.
        /// </summary>
        public new ProblemDetails Value {
            get => (ProblemDetails)base.Value ?? ProblemDetailsUtils.CreateDefault("A problem occured.");

            set {
                base.Value = value;
                onValueChanged();
            }
        }

        public ProblemDetailsResult(ProblemDetails? problemDetails)
            : base(problemDetails)
        {
            onValueChanged();
            // Not needed because the ObjectResultExecutor (used by 
            // ProblemDetailsResultExecutor) does this on execution.
            // Either way this should be implemented in action executor.
            // Compare https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.Core/src/Infrastructure/ObjectResultExecutor.cs
            //ContentTypes.Add(new MediaTypeHeaderValue("application/problem+json"));
            //ContentTypes.Add(new MediaTypeHeaderValue("application/problem+xml"));
        }

        private void onValueChanged()
        {
            var problemDetails = Value;
            StatusCode = problemDetails.Status;
            DeclaredType = problemDetails.GetType();
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<ProblemDetailsResult>>();
            return executor.ExecuteAsync(context, this);
        }
    }
}
