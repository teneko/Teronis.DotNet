using System;
using System.Diagnostics.CodeAnalysis;
using Teronis.Identity.Presenters.ObjectModel;

namespace Teronis.Identity.Presenters.Generic.ObjectModel
{
    public class ServiceResultDelegatedFactory<ContentType> : IServiceResultDelegatedFactory<IServiceResult<ContentType>, ContentType>
    {
        private readonly IServiceResultInjection<ContentType> serviceResultInjection;

        public ServiceResultDelegatedFactory(IServiceResultInjection<ContentType> serviceResultInjection) =>
            this.serviceResultInjection = serviceResultInjection ?? throw new ArgumentNullException(nameof(serviceResultInjection));

        private IServiceResultFactory<IServiceResult<ContentType>, ContentType> setServiceResult(ServiceResult<ContentType> serviceResult)
        {
            serviceResultInjection.SetResult(serviceResult);
            return new ServiceResultFactory<IServiceResult<ContentType>, ContentType>(serviceResult, serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToSuccess()
        {
            var serviceResult = new ServiceResult<ContentType>(true);
            return setServiceResult(serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToSuccess([AllowNull] ContentType content)
        {
            var serviceResult = ServiceResult<ContentType>.Success(content);
            return setServiceResult(serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToFailure()
        {
            var serviceResult = new ServiceResult<ContentType>(false);
            return setServiceResult(serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToFailure(IServiceResult? serviceResult)
        {
            var newServiceResult = ServiceResult<ContentType>.Failure(serviceResult);
            return setServiceResult(newServiceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToFailure(JsonError? error)
        {
            var serviceResult = ServiceResult<ContentType>.Failure(error);
            return setServiceResult(serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToFailure(Exception? error)
        {
            var serviceResult = ServiceResult<ContentType>.Failure(error);
            return setServiceResult(serviceResult);
        }

        public IServiceResultFactory<IServiceResult<ContentType>, ContentType> ToFailure(string? errorMessage)
        {
            var serviceResult = ServiceResult<ContentType>.Failure(errorMessage);
            return setServiceResult(serviceResult);
        }

        #region IServiceResultDelegatedFactory<IServiceResult<ContentType>, ContentType>

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToSuccess() =>
            ToSuccess();

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToSuccess([AllowNull] ContentType content) =>
            ToSuccess(content);

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToFailure() =>
            ToFailure();

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToFailure(IServiceResult? serviceResult) =>
            ToFailure(serviceResult);

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToFailure(JsonError? error) =>
            ToFailure(error);

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToFailure(Exception? error) =>
            ToFailure(error);

        IServiceResultFactory IServiceResultDelegatedFactory<ContentType>.ToFailure(string? errorMessage) =>
            ToFailure(errorMessage);

        #endregion
    }
}
