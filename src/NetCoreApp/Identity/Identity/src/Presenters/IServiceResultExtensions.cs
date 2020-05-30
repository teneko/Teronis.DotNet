using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Teronis.Identity.Presenters;

namespace Teronis.Identity.Extensions
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

            if (serviceResult.Succeeded || errors == null || errors.Count == 0) {
                return true;
            }

            ignoredErrorCodes ??= ignoredErrorCodes ?? Enumerable.Empty<string>();

            var filteredErrorCodes = errors.Errors.Keys
                .Except(ignoredErrorCodes)
                .Count();

            if (filteredErrorCodes == 0) {
                return true;
            }

            return false;
        }

        public static bool Success(this IServiceResult serviceResult, params string[] excludeErrorCodes) =>
            Success(serviceResult, (IEnumerable<string>)excludeErrorCodes);
    }
}
