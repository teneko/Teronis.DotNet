using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Teronis.Mvc.ServiceResulting;

namespace Teronis.Identity.Extensions
{
    public static class IdentityResultExtensions
    {
        private static IEnumerable<IdentityError>? getIdentityResultErrorsOrThrow(IdentityResult? identityResult)
        {
            identityResult = identityResult ?? throw new ArgumentNullException(nameof(identityResult));

            if (identityResult.Succeeded) {
                throw new ArgumentException("There are no errors because the result is successful.");
            }

            return identityResult.Errors;
        }

        public static AggregateException ToAggregatedException(this IdentityResult identityResult, string? errorMessage = null)
        {
            var identityErrors = getIdentityResultErrorsOrThrow(identityResult);

            return new AggregateException(errorMessage, from error in identityResult.Errors
                                                        select new Exception($"{error.Description} ({error.Code})"));
        }

        public static JsonErrors ToJsonErrors(this IdentityResult identityResult)
        {
            var identityErrors = getIdentityResultErrorsOrThrow(identityResult);

            return new JsonErrors(from error in identityResult.Errors
                                  select new KeyValuePair<string, string>(error.Description, error.Code));
        }
    }
}
