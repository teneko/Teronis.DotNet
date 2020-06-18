using System;
using System.Linq;
using Teronis.Identity;
using Teronis.Mvc.ServiceResulting.Generic;

namespace Teronis.Mvc.ServiceResulting
{
    public static class JsonErrorsExtensions
    {
        public static Exception? ToSingleOrAggregatedException(this JsonErrors? jsonErrors)
        {
            if (jsonErrors is null)
                return null;

            var errors = jsonErrors.Errors;
            var errorsCount = errors.Count;

            if (errorsCount == 0)
                return null;
            else if (errorsCount == 1)
                return errors[0].Error;

            return new AggregateException(StringResources.MoreThanOneExcpetionOccuredMessage, errors.Select(x => x.Error)
                .ToArray());
        }

        public static Exception ToSingleOrAggregatedOrDefaultException(this JsonErrors? jsonErrors) =>
            jsonErrors.ToSingleOrAggregatedException() ?? new Exception(StringResources.DefaultErrorMessage);

        public static string ToStringOrDefaultMessage(this JsonErrors? jsonErrors) =>
            jsonErrors?.ToString() ?? StringResources.DefaultErrorMessage;

        /// <summary>
        /// Makes a copy of <see cref="jsonErrors"/>.
        /// </summary>
        public static JsonErrors DeepCopy(this JsonErrors? jsonErrors)
        {
            var newJsonErrors = new JsonErrors();

            if (jsonErrors?.Error is null)
                return newJsonErrors;

            foreach (var error in jsonErrors.Errors)
                newJsonErrors.Errors.Add(new JsonError(error.Error, error.ErrorCode));

            return newJsonErrors;
        }

        /// <summary>
        /// Creates a servce result from provided <paramref name="errors"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ServiceResult ToServiceResult(this JsonErrors? errors) =>
            ServiceResult<object>.Failure(errors);

        /// <summary>
        /// Creates a servce result from provided <paramref name="error"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ServiceResult<T> ToServiceResult<T>(this JsonErrors? errors) =>
            ServiceResult<T>.Failure(errors);

        /// <summary>
        /// Creates a service result factory for creating a customized service result.
        /// </summary>
        /// <typeparam name="ContentType"></typeparam>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IServiceResultPostConfiguration<ServiceResult<ContentType>, ContentType> ToServiceResultFactory<ContentType>(this JsonErrors? errors)
        {
            var serviceResult = ServiceResult<ContentType>.Failure(errors);
            return new ServiceResultPostConfiguration<ServiceResult<ContentType>, ContentType>(serviceResult, serviceResult);
        }
    }
}
