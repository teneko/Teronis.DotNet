using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Teronis.Identity.Presenters.Generic
{
    public class ServiceResult<ContentType> : ServiceResult, IServiceResult<ContentType>, IMutableServiceResult
    {
        /// <summary>
        /// Creates a result that is marked as succeeded.
        /// </summary>
        public static ServiceResult<ContentType> SucceededWithContent([AllowNull] ContentType content) =>
            new ServiceResult<ContentType>(content);

        /// <summary>
        /// Creates a result that is marked as failed with json error as content.
        /// </summary>
        public static ServiceResult<ContentType> FailedWithJsonError(JsonError error) =>
            new ServiceResult<ContentType>(false, content: new JsonErrors(error));

        /// <summary>
        /// Creates a result that is marked as failed with json converted error message as content.
        /// </summary>
        public static ServiceResult<ContentType> FailedWithErrorMessage(string errorMessage) =>
            ServiceResult<ContentType>.FailedWithJsonError(errorMessage);

        /// <summary>
        /// Creates a copy of <paramref name="result"/> but failed.
        /// <returns>
        public static ServiceResult<ContentType> Failed(IServiceResult result) =>
            new ServiceResult<ContentType>(false, JsonErrors.FromJsonErrors(result.Errors), result.StatusCode);

        [MaybeNull]
        public ContentType Content {
            get => Value is ContentType value ? value : default;
        }

        internal ServiceResult(in ServiceResultDatransject datransject)
            : base(datransject) { }

        protected ServiceResult(bool succeeded, object? content = null, int? statusCode = null)
            : base(succeeded, content, statusCode) { }

        public ServiceResult(bool succeeded)
            : this(succeeded, default, default) { }

        public ServiceResult([AllowNull] ContentType content, int? statusCode = null)
            : base(true, content, statusCode) { }

        public ServiceResult([AllowNull] JsonErrors errors, int? statusCode = null)
            : base(false, errors, statusCode) { }
    }
}
