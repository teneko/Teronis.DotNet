// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Mvc.ServiceResulting.Generic
{
    public class ServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> : IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType>
        where ServiceResultType : IServiceResult<ServiceResultContentType>
    {
        private readonly ServiceResult serviceResult;

        public ServiceResultPostConfiguration(ServiceResult serviceResult)
        {
            this.serviceResult = serviceResult;
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
