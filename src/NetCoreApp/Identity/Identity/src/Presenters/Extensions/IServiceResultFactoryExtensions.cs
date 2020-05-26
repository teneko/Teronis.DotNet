using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Teronis.Identity.Presenters.Extensions
{
    public static class IServiceResultFactoryExtensions
    {
        [return: NotNullIfNotNull("serviceResultFactory")]
        public static IServiceResultFactory? WithHttpStatusCode(this IServiceResultFactory serviceResultFactory, HttpStatusCode statusCode)
        {
            if (!ReferenceEquals(serviceResultFactory, null)) {
                serviceResultFactory.WithStatusCode((int)statusCode);
            }

            return serviceResultFactory;
        }
    }
}
