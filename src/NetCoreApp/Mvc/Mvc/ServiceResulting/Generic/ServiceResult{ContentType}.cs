using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Mvc.ServiceResulting.Generic
{
    public class ServiceResult<ContentType> : ServiceResult, IServiceResult<ContentType>, IMutableServiceResult
    {
        /// <summary>
        /// Creates a result that is marked as succeeded.
        /// </summary>
        public static ServiceResult<ContentType> Success([AllowNull] ContentType content) =>
            new ServiceResult<ContentType>(content);

        /// <summary>
        /// Creates a result that is marked as succeeded but without content.
        /// </summary>
        public static ServiceResult<ContentType> Success() =>
            Success(default);

        /// <summary>
        /// Creates a result that is marked as failed.
        /// </summary>
        public static ServiceResult<ContentType> Failure() =>
            new ServiceResult<ContentType>(false);

        /// <summary>
        /// Creates a result that is marked as failed with json errors as content.
        /// </summary>
        public static ServiceResult<ContentType> Failure(JsonErrors? errors) =>
            new ServiceResult<ContentType>(false, content: errors);

        /// <summary>
        /// Creates a result that is marked as failed with json error as content.
        /// </summary>
        public static ServiceResult<ContentType> Failure(JsonError? error) =>
            new ServiceResult<ContentType>(false, content: error is null ? null : new JsonErrors(error));

        /// <summary>
        /// Creates a result that is marked as failed with json error as content.
        /// </summary>
        public static ServiceResult<ContentType> Failure(Exception? error) =>
            new ServiceResult<ContentType>(false, content: error is null ? null : new JsonErrors(error));

        /// <summary>
        /// Creates a result that is marked as failed with json converted error message as content.
        /// </summary>
        public static ServiceResult<ContentType> Failure(string? errorMessage) =>
            Failure((JsonError)errorMessage);

        /// <summary>
        /// Creates a copy of <paramref name="result"/> but failed.
        /// <returns>
        public static ServiceResult<ContentType> Failure(IServiceResult? result) =>
            new ServiceResult<ContentType>(false, result?.Errors?.DeepCopy(), result?.StatusCode);

        protected override object? ContentOrDefault => Succeeded ? Value : default(ContentType);

        [MaybeNull]
        public new ContentType Content {
            get => (ContentType)ContentOrDefault;
        }

        internal ServiceResult(in ServiceResultDatransject datransject)
            : base(datransject) { }

        protected ServiceResult(bool succeeded, object? content = null, int? statusCode = null)
            : base(succeeded, content, statusCode) { }

        public ServiceResult(bool succeeded)
            : this(succeeded, default, default) { }

        public ServiceResult([AllowNull] ContentType content, int? statusCode = null)
            : base(true, content, statusCode) { }

        public ServiceResult(JsonErrors? errors, int? statusCode = null)
            : base(false, errors, statusCode) { }
    }
}
