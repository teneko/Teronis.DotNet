using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Teronis.Identity.Presenters.Generic
{
    public static class IServiceResultFactoryGenericExtensions
    {
        [return: NotNullIfNotNull("serviceResultFactory")]
        public static IServiceResultFactory<ServiceResultType, ServiceResultContentType>? WithHttpStatusCode<ServiceResultType, ServiceResultContentType>(this IServiceResultFactory<ServiceResultType, ServiceResultContentType> serviceResultFactory, HttpStatusCode statusCode)
            where ServiceResultType : IServiceResult<ServiceResultContentType>
        {
            if (!(serviceResultFactory is null))
                serviceResultFactory.WithStatusCode((int)statusCode);

            return serviceResultFactory;
        }
    }
}
