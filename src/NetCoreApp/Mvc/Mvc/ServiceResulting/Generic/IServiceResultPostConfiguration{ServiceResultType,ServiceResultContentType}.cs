namespace Teronis.Mvc.ServiceResulting.Generic
{
    public interface IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> : IServiceResultPostConfiguration
        where ServiceResultType : IServiceResult<ServiceResultContentType>
    {
        new IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> WithStatusCode(int? statusCode);
    }
}
