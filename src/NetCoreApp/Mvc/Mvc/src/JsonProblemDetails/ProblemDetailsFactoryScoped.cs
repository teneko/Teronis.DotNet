using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Teronis.Extensions;
using Teronis.Mvc.JsonProblemDetails.Mappers;

namespace Teronis.Mvc.JsonProblemDetails
{
    /// <summary>
    /// Wraps the original <see cref="ProblemDetailsFactory"/> and
    /// extends it.
    /// It is now <see cref="HttpContext"/> dependent on creation.
    /// Accessible through <see cref="IMapperContext.ProblemDetailsFactory"/>.
    /// </summary>
    public class ProblemDetailsFactoryScoped
    {
        private readonly ProblemDetailsFactory problemDetailsFactory;
        private readonly HttpContext httpContext;

        public ProblemDetailsFactoryScoped(ProblemDetailsFactory problemDetailsFactory, HttpContext httpContext)
        {
            this.problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
            this.httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }

        /// <summary>
        /// Gets the alternative instance string value.
        /// </summary>
        /// <returns>The alternative instance string.</returns>
        protected virtual string? GetAlternativeInstance()
        {
            var request = httpContext.Request;
            var pathAndQuery = request.GetEncodedPathAndQuery();

            if (!string.IsNullOrEmpty(pathAndQuery)) {
                return $"{request.PathBase}{request.GetEncodedPathAndQuery()}";
            }

            return null;
        }

        protected virtual int? GetAlternativeExcpetionStatusCode(Exception? exception) =>
            exception is ArgumentException ? (int?)StatusCodes.Status422UnprocessableEntity : null;

        protected virtual void ApplyAlternativeProblemDetails(ProblemDetails problemDetails, ProblemDetails alternativeProblemDetails)
        {
            problemDetails.Status ??= alternativeProblemDetails.Status;
            problemDetails.Title ??= alternativeProblemDetails.Title;
            problemDetails.Type ??= alternativeProblemDetails.Type;
            problemDetails.Detail ??= alternativeProblemDetails.Detail;
            problemDetails.Instance ??= alternativeProblemDetails.Instance;
        }

        public virtual ProblemDetails CreateProblemDetails(
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null) =>
            problemDetailsFactory.CreateProblemDetails(httpContext,
                statusCode: statusCode,
                title: title,
                type: type,
                detail: detail,
                instance: instance ?? GetAlternativeInstance());

        protected virtual ValidationProblemDetails CreateValidationProblemDetails(
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null) =>
            problemDetailsFactory.CreateValidationProblemDetails(httpContext, new ModelStateDictionary(),
                statusCode: statusCode,
                title: title,
                type: type,
                detail: detail,
                instance: instance ?? GetAlternativeInstance());

        public virtual ValidationProblemDetails CreateValidationProblemDetails(
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            var validationProblemDetails = new ValidationProblemDetails(modelStateDictionary);

            var alternativeProblemDetails = CreateValidationProblemDetails(
                statusCode: statusCode,
                title: title,
                type: type,
                detail: detail,
                instance: instance);

            ApplyAlternativeProblemDetails(validationProblemDetails, alternativeProblemDetails);
            return validationProblemDetails;
        }

        public virtual ValidationProblemDetails CreateValidationProblemDetails(
            IDictionary<string, string[]> errors,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            var validationProblemDetails = new ValidationProblemDetails(errors);

            var alternativeProblemDetails = CreateValidationProblemDetails(
                statusCode: statusCode,
                title: title,
                type: type,
                detail: detail,
                instance: instance);

            ApplyAlternativeProblemDetails(validationProblemDetails, alternativeProblemDetails);
            return validationProblemDetails;
        }

        protected virtual void ApplyExceptionDetails(ProblemDetails problemDetails, Exception exception)
        {
            exception = exception ?? throw new ArgumentNullException(nameof(exception));
            var exceptionType = exception.GetType();
            var typeNameOfException = nameof(Exception);
            var newType = exceptionType.Name.TrimEnd(typeNameOfException);

            if (newType == string.Empty) {
                newType = typeNameOfException;
            }

            var newTitle = exception.Message;

            // Assign
            problemDetails.Type = newType;
            problemDetails.Title = newTitle;
        }

        public virtual ProblemDetails CreateProblemDetails(Exception exception,
            int? statusCode = null)
        {
            var problemDetails = CreateProblemDetails(
                statusCode: statusCode ?? GetAlternativeExcpetionStatusCode(exception));

            ApplyExceptionDetails(problemDetails, exception);
            return problemDetails;
        }

        public virtual ValidationProblemDetails CreateValidationProblemDetails(AggregateException aggregateException,
            int? statusCode = null)
        {
            aggregateException = aggregateException ?? throw new ArgumentNullException(nameof(aggregateException));
            var errors = new Dictionary<string, string[]> { { "default", aggregateException.InnerExceptions.Select(x => x.Message).ToArray() } };

            var validationDetails = CreateValidationProblemDetails(errors,
                statusCode: statusCode ?? GetAlternativeExcpetionStatusCode(aggregateException));

            ApplyExceptionDetails(validationDetails, aggregateException);
            return validationDetails;
        }

        public virtual ValidationProblemDetails CreateValidationProblemDetails(KeyedAggregateException keyedAggregateException,
            int? statusCode = null)
        {
            keyedAggregateException = keyedAggregateException ?? throw new ArgumentNullException(nameof(keyedAggregateException));
            var errors = keyedAggregateException.InnerExceptionsWithKeys.ToDictionary(x => x.Key, x => new[] { x.Value.Message });

            var validationDetails = CreateValidationProblemDetails(errors,
                statusCode: statusCode ?? GetAlternativeExcpetionStatusCode(keyedAggregateException));

            ApplyExceptionDetails(validationDetails, keyedAggregateException);
            return validationDetails;
        }
    }
}
