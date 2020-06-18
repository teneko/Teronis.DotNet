namespace Teronis.Mvc.ServiceResulting.Generic
{
    public class ServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> : IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType>
        where ServiceResultType : IServiceResult<ServiceResultContentType>
    {
        private readonly ServiceResult serviceResult;
        private readonly ServiceResultType readOnlyServiceResult;

        public ServiceResultPostConfiguration(ServiceResult serviceResult, ServiceResultType readOnlyServiceResult)
        {
            this.serviceResult = serviceResult;
            this.readOnlyServiceResult = readOnlyServiceResult;
        }

        public IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> WithStatusCode(int? statusCode)
        {
            serviceResult.StatusCode = statusCode;
            return this;
        }

        #region 

        IServiceResultPostConfiguration IServiceResultPostConfiguration.WithStatusCode(int? statusCode) =>
            WithStatusCode(statusCode);

        #endregion
    }
}
