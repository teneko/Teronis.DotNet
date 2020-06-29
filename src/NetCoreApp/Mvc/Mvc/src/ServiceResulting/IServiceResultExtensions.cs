using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Teronis.Mvc.ServiceResulting
{
    public static class IServiceResultExtensions
    {
        /// <summary>
        /// Check conditions for a succeeded result. You may ingnore error codes that could cause to a failed result.
        /// </summary>
        public static bool Success(this IServiceResult serviceResult, IEnumerable<string> ignoredErrorCodes)
        {
            serviceResult = serviceResult ?? throw new ArgumentNullException(nameof(serviceResult));
            var errors = serviceResult.Errors;

            if (serviceResult.Succeeded || errors == null || errors.Count == 0)
                return true;

            int notIgnoredErrorCodesCount;

            if (ignoredErrorCodes is null)
                notIgnoredErrorCodesCount = errors.Count;
            else
                notIgnoredErrorCodesCount = errors.Errors
               .Where(x => !ignoredErrorCodes.Contains(x.ErrorCode))
               .Count();

            if (notIgnoredErrorCodesCount == 0)
                return true;

            return false;
        }

        public static bool Success(this IServiceResult serviceResult, params string[] excludeErrorCodes) =>
            serviceResult.Success((IEnumerable<string>)excludeErrorCodes);

        public static void ThrowOnError(this IServiceResult serviceResult)
        {
            if (!serviceResult.Succeeded)
                throw new NotSucceededException(serviceResult, $"Service result did not succeed. {serviceResult.Errors.ToStringOrDefaultMessage()}");
        }
    }
}
