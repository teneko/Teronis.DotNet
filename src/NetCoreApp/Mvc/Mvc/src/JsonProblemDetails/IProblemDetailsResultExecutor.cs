using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Teronis.Mvc.JsonProblemDetails
{
    public class ProblemDetailsResultExecutor : IActionResultExecutor<ProblemDetailsResult>
    {
        private readonly IActionResultExecutor<ObjectResult> resultExecutor;

        public ProblemDetailsResultExecutor(IActionResultExecutor<ObjectResult> resultExecutor) =>
            this.resultExecutor = resultExecutor ?? throw new ArgumentNullException(nameof(resultExecutor));

        public Task ExecuteAsync(ActionContext context, ProblemDetailsResult result) =>
            resultExecutor.ExecuteAsync(context, result);
    }
}
