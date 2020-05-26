using System.Diagnostics.CodeAnalysis;

namespace Teronis.Identity.Presenters.Generic
{
    public class ServiceResult<ContentType> : ServiceResultBase, IServiceResult<ContentType>
    {
        /// <summary>
        /// Creates a result that is marked as succeeded.
        /// </summary>
        public static ServiceResult<ContentType> SucceededWithContent(ContentType content) =>
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

        [MaybeNull, AllowNull]
        public ContentType Content {
            get => Value is ContentType value ? value : default;
            set => Value = value;
        }

        protected ServiceResult(bool succeeded, object? content = null, int? statusCode = null)
            : base(succeeded, content, statusCode) { }

        public ServiceResult(bool succeeded)
            : this(succeeded, default, default) { }

        public ServiceResult(ContentType content, int? statusCode = null)
            : base(true, content, statusCode) { }

        public ServiceResult(JsonErrors errors, int? statusCode = null)
            : base(false, errors, statusCode) { }
    }
}
