using System;
using Microsoft.Extensions.Logging;
using Teronis.Identity.Extensions;
using Teronis.Mvc.ServiceResulting;
using Teronis.Mvc.ServiceResulting.Generic;

namespace Teronis.Identity
{
    /// <summary>
    /// A class that logs sensitive errors and only provides detailed 
    /// errors if <see cref="getIncludeErrorDetails"/> returns true.
    /// </summary>
    internal class ErrorDetailsProvider
    {
        private readonly Func<bool> getIncludeErrorDetails;
        private readonly ILogger? logger;

        public ErrorDetailsProvider(Func<bool> getIncludeErrorDetails, ILogger? logger)
        {
            this.getIncludeErrorDetails = getIncludeErrorDetails;
            this.logger = logger;
        }

        #region appropiate error

        public ServiceResult<ContentType> BuildAppropiateErrorResult<ContentType>(JsonErrors? sensitiveJsonErrors, JsonErrors? insensitiveJsonErrors = null)
        {
            if (sensitiveJsonErrors is null && insensitiveJsonErrors is null) {
                return ServiceResult<ContentType>.Failure();
            }

            if (!ReferenceEquals(sensitiveJsonErrors, null) && getIncludeErrorDetails()) {
                if (!ReferenceEquals(insensitiveJsonErrors, null)) {
                    var insensitiveJsonErrorsCount = insensitiveJsonErrors.Count;

                    for (var index = 0; index < insensitiveJsonErrorsCount; index++) {
                        var jsonError = insensitiveJsonErrors[index];
                        sensitiveJsonErrors.Insert(index, jsonError);
                    }
                }

                return ServiceResult<ContentType>.Failure(sensitiveJsonErrors);
            }

            return ServiceResult<ContentType>.Failure(insensitiveJsonErrors);
        }

        public ServiceResult<ContentType> LogThenBuildAppropiateError<ContentType>(LogLevel logLevel, JsonErrors? sensitiveJsonErrors, JsonErrors? insensitiveJsonErrors = null)
        {
            var appropiateErrorResult = BuildAppropiateErrorResult<ContentType>(sensitiveJsonErrors, insensitiveJsonErrors);

            if (!ReferenceEquals(sensitiveJsonErrors, null) || !ReferenceEquals(insensitiveJsonErrors, null)) {
                logger?.Log(logLevel, sensitiveJsonErrors.ToSingleOrAggregatedOrDefaultException(), insensitiveJsonErrors.ToStringOrDefaultMessage());
            }

            return appropiateErrorResult;
        }

        public ServiceResult<ContentType> LogErrorThenBuildAppropiateError<ContentType>(JsonErrors? sensitiveJsonErrors, JsonErrors? insensitiveJsonErrors = null) =>
            LogThenBuildAppropiateError<ContentType>(LogLevel.Error, sensitiveJsonErrors, insensitiveJsonErrors);

        public ServiceResult<ContentType> LogErrorThenBuildAppropiateError<ContentType>(JsonErrors? sensitiveJsonErrors, string? insensitiveErrorMessage = null)
        {
            var insensitiveJsonErrors = insensitiveErrorMessage.ToJsonErrors();
            return LogErrorThenBuildAppropiateError<ContentType>(sensitiveJsonErrors, insensitiveJsonErrors);
        }

        public ServiceResult<ContentType> LogErrorThenBuildAppropiateError<ContentType>(Exception? sensitiveError, string? insensitiveErrorMessage = null)
        {
            var sensitiveJsonErrors = sensitiveError.ToJsonErrors();
            var insensitiveJsonErrors = insensitiveErrorMessage.ToJsonErrors();
            return LogErrorThenBuildAppropiateError<ContentType>(sensitiveJsonErrors, insensitiveJsonErrors);
        }

        public ServiceResult<ContentType> LogCriticalThenBuildAppropiateError<ContentType>(JsonErrors? sensitiveJsonErrors, JsonErrors? insensitiveJsonErrors = null) =>
            LogThenBuildAppropiateError<ContentType>(LogLevel.Critical, sensitiveJsonErrors, insensitiveJsonErrors);

        public ServiceResult<ContentType> LogCriticalThenBuildAppropiateError<ContentType>(JsonErrors? sensitiveJsonErrors, string? insensitiveErrorMessage = null)
        {
            var insensitiveJsonErrors = insensitiveErrorMessage.ToJsonErrors();
            return LogCriticalThenBuildAppropiateError<ContentType>(sensitiveJsonErrors, insensitiveJsonErrors);
        }

        public ServiceResult<ContentType> LogCriticalThenBuildAppropiateError<ContentType>(Exception? sensitiveError, string? insensitiveErrorMessage = null)
        {
            var sensitiveJsonErrors = sensitiveError.ToJsonErrors();
            var insensitiveJsonErrors = insensitiveErrorMessage.ToJsonErrors();
            return LogCriticalThenBuildAppropiateError<ContentType>(sensitiveJsonErrors, insensitiveJsonErrors);
        }

        #endregion
    }
}
