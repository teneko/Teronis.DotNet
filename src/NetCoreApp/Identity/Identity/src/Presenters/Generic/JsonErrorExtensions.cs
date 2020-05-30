namespace Teronis.Identity.Presenters.Generic
{
    public static class JsonErrorExtensions
    {
        /// <summary>
        /// Creates a servce result from provided <paramref name="error"/> that failed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ServiceResult<T> ToServiceResult<T>(this JsonError error) =>
               ServiceResult<T>.Failure(error);

        /// <summary>
        /// Creates a service result factory for creating a customized service result.
        /// </summary>
        /// <typeparam name="ContentType"></typeparam>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IServiceResultFactory<ServiceResult<ContentType>, ContentType> ToServiceResultFactory<ContentType>(this JsonError error)
        {
            var serviceResult = ServiceResult<ContentType>.Failure(error);
            return new ServiceResultFactory<ServiceResult<ContentType>, ContentType>(serviceResult, serviceResult);
        }
    }
}
