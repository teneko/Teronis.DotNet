

namespace Teronis.Identity.Presenters.Generic
{
    public class ServiceResultFactory<ServiceResultType, ServiceResultContentType> : IServiceResultFactory<ServiceResultType, ServiceResultContentType>
        where ServiceResultType : IServiceResult<ServiceResultContentType>
    {
        private readonly ServiceResultBase serviceResult;
        private readonly ServiceResultType readOnlyServiceResult;

        public ServiceResultFactory(ServiceResultBase serviceResult, ServiceResultType readOnlyServiceResult)
        {
            this.serviceResult = serviceResult;
            this.readOnlyServiceResult = readOnlyServiceResult;
        }

        public IServiceResultFactory WithStatusCode(int? statusCode)
        {
            serviceResult.StatusCode = statusCode;
            return this;
        }

        public ServiceResultType AsServiceResult() =>
            readOnlyServiceResult;
    }
}
