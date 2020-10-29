using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Teronis.Identity.Bearer
{
    public class CustomExceptionFilter : IExceptionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnException(ExceptionContext context)
        {
            var error = context.Exception;

            context.Result = new ContentResult() {
                StatusCode = (int)(error is ArgumentException ? HttpStatusCode.UnprocessableEntity : HttpStatusCode.InternalServerError),
                Content = error.Message,
                ContentType = "text/plain"
            };

            context.ExceptionHandled = true;
        }
    }
}
