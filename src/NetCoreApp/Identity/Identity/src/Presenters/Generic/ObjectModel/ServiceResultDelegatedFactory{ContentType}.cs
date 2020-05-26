using System;
using System.Diagnostics.CodeAnalysis;
using Teronis.Identity.Presenters.Generic;
using Teronis.Identity.Presenters.ObjectModel;

namespace Teronis.Identity.Presenters.Generic.ObjectModel
{
    public class ServiceResultDelegatedFactory<ContentType> : IServiceResultDelegatedFactory<IServiceResult<ContentType>, ContentType>
    {
        private readonly IServiceResultInjection<ContentType> serviceResultInjection;

        public ServiceResultDelegatedFactory([DisallowNull] IServiceResultInjection<ContentType> serviceResultInjection) =>
            this.serviceResultInjection = serviceResultInjection ?? throw new ArgumentNullException(nameof(serviceResultInjection));

        private IServiceResultFactory<IServiceResult<ContentType>, ContentType> setServiceResult(ServiceResult<ContentType> serviceResult)
        {
            serviceResultInjection.SetResult(serviceResult);
            return new ServiceResultFactory<IServiceResult<ContentType>, ContentType>(serviceResult, serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToSucceeded()
        {
            var serviceResult = new ServiceResult<ContentType>(true);
            return setServiceResult(serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToSucceededWithContent(ContentType content)
        {
            var serviceResult = ServiceResult<ContentType>.SucceededWithContent(content);
            return setServiceResult(serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToFailed()
        {
            var serviceResult = new ServiceResult<ContentType>(false);
            return setServiceResult(serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToFailed(IServiceResult serviceResult)
        {
            var newServiceResult = ServiceResult<ContentType>.Failed(serviceResult);
            return setServiceResult(newServiceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToFailedWithJsonError(JsonError error)
        {
            var serviceResult = ServiceResult<ContentType>.FailedWithJsonError(error);
            return setServiceResult(serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToFailedWithErrorMessage(string errorMessage)
        {
            var serviceResult = ServiceResult<ContentType>.FailedWithErrorMessage(errorMessage);
            return setServiceResult(serviceResult);
        }

        #region IServiceResultDelegatedFactory<IServiceResult<ContentType>, ContentType>

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToSucceeded() =>
            ToSucceeded();

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToSucceededWithContent(ContentType content) =>
            ToSucceededWithContent(content);

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToFailed() =>
            ToFailed();

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToFailed(IServiceResult serviceResult) =>
            ToFailed(serviceResult);

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToFailedWithJsonError(JsonError error) =>
            ToFailedWithJsonError(error);

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToFailedWithErrorMessage(string errorMessage) =>
            ToFailedWithErrorMessage(errorMessage);

        #endregion
    }
}
