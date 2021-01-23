using System;

namespace Teronis.Mvc.ServiceResulting.Generic
{
    public static class IServiceResultContentExtensions
    {
        /// <summary>
        /// If you are sure that content is not null it returns the content as not null.
        /// </summary>
        public static ContentType Content<ContentType>(this IServiceResult<ContentType> result) =>
            result.Content!;

        /// <summary>
        /// If you are sure that content is not null it returns the content as not null.
        /// If it is null an excpetion will be thrown.
        /// </summary>
        public static ContentType ContentOrException<ContentType>(this IServiceResult<ContentType> result) =>
            result.Content ?? throw new ArgumentException($"{nameof(IServiceResult<ContentType>)}.{nameof(IServiceResult<ContentType>.Content)}");

        private static ServiceResult<TargetContentType> copy<SourceContentType, TargetContentType>(this IServiceResult<SourceContentType> result)
        {
            result = result ?? throw new ArgumentNullException(nameof(result));
            var resultDeepCopyDatransject = result.DeepCopy();
            return new ServiceResult<TargetContentType>(resultDeepCopyDatransject);
        }

        public static ServiceResult<TargetContentType> CopyButSucceededWithContent<SourceContentType, TargetContentType>(this IServiceResult<SourceContentType> result, TargetContentType content)
        {
            var copiedResult = result.copy<SourceContentType, TargetContentType>();
            IMutableServiceResult mutableServiceResult = copiedResult;
            mutableServiceResult.Succeeded = true;
            mutableServiceResult.Content = content;
            return copiedResult;
        }

        public static ServiceResult<TargetContentType> CopyButSucceededWithContentIfSucceeded<SourceContentType, TargetContentType>(this IServiceResult<SourceContentType> result, TargetContentType content)
        {
            result = result ?? throw new ArgumentNullException(nameof(result));

            if (result.Succeeded) {
                return result.CopyButSucceededWithContent(content);
            }

            return result.copy<SourceContentType, TargetContentType>();
        }

        public static ServiceResult<TargetContentType> CopyButFailed<SourceContentType, TargetContentType>(this IServiceResult<SourceContentType> result, JsonErrors? errors = null)
        {
            var copiedResult = result.copy<SourceContentType, TargetContentType>();
            IMutableServiceResult mutableServiceResult = copiedResult;
            mutableServiceResult.Succeeded = false;

            if (errors != null) {
                mutableServiceResult.Errors = errors;
            }

            return copiedResult;
        }

        public static ServiceResult<TargetContentType> CopyButFailedIfFailed<SourceContentType, TargetContentType>(this IServiceResult<SourceContentType> result, JsonErrors? errors = null)
        {
            result = result ?? throw new ArgumentNullException(nameof(result));

            if (!result.Succeeded) {
                return result.CopyButFailed<SourceContentType, TargetContentType>(errors);
            }

            return result.copy<SourceContentType, TargetContentType>();
        }
    }
}
