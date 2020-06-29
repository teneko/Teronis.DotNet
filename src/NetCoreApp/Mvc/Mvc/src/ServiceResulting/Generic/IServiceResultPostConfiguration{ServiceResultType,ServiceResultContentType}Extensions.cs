using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Teronis.Mvc.ServiceResulting.Generic
{
    public static class IServiceResultPostConfigurationGenericExtensions
    {
        [return: NotNullIfNotNull("serviceResultFactory")]
        public static IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType>? WithHttpStatusCode<ServiceResultType, ServiceResultContentType>(this IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> serviceResultFactory, HttpStatusCode statusCode)
            where ServiceResultType : IServiceResult<ServiceResultContentType>
        {
            if (!(serviceResultFactory is null))
                serviceResultFactory.WithStatusCode((int)statusCode);

            return serviceResultFactory;
        }
    }
}
