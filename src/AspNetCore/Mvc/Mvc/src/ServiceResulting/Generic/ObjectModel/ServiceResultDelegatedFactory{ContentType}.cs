using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Mvc.ServiceResulting.Generic.ObjectModel
{
    public class ServiceResultDelegatedFactory<ContentType> : IServiceResultDelegatedFactory<IServiceResult<ContentType>, ContentType>
    {
        private readonly IServiceResultInjection<ContentType> serviceResultInjection;

        public ServiceResultDelegatedFactory(IServiceResultInjection<ContentType> serviceResultInjection) =>
            this.serviceResultInjection = serviceResultInjection ?? throw new ArgumentNullException(nameof(serviceResultInjection));

        private IServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType> setServiceResult(ServiceResult<ContentType> serviceResult)
        {
            serviceResultInjection.SetResult(serviceResult);
            return new ServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType>(serviceResult);
        }

        public IServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType> ToSuccess()
        {
            var serviceResult = new ServiceResult<ContentType>(true);
            return setServiceResult(serviceResult);
        }

        public IServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType> ToSuccess([AllowNull] ContentType content)
        {
            var serviceResult = ServiceResult<ContentType>.Success(content);
            return setServiceResult(serviceResult);
        }

        public IServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType> ToFailure()
        {
            var serviceResult = ServiceResult<ContentType>.Failure();
            return setServiceResult(serviceResult);
        }

        public IServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType> ToFailure(IServiceResult? serviceResult)
        {
            var newServiceResult = ServiceResult<ContentType>.Failure(serviceResult);
            return setServiceResult(newServiceResult);
        }

        public IServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType> ToFailure(JsonErrors? errors)
        {
            var serviceResult = ServiceResult<ContentType>.Failure(errors);
            return setServiceResult(serviceResult);
        }

        public IServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType> ToFailure(JsonError? error)
        {
            var serviceResult = ServiceResult<ContentType>.Failure(error);
            return setServiceResult(serviceResult);
        }

        public IServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType> ToFailure(Exception? error)
        {
            var serviceResult = ServiceResult<ContentType>.Failure(error);
            return setServiceResult(serviceResult);
        }

        public IServiceResultPostConfiguration<IServiceResult<ContentType>, ContentType> ToFailure(string? errorMessage)
        {
            var serviceResult = ServiceResult<ContentType>.Failure(errorMessage);
            return setServiceResult(serviceResult);
        }

        #region IServiceResultDelegatedFactory<IServiceResult<ContentType>, ContentType>

        IServiceResultPostConfiguration IServiceResultDelegatedFactory<ContentType>.ToSuccess() =>
            ToSuccess();

        IServiceResultPostConfiguration IServiceResultDelegatedFactory<ContentType>.ToSuccess([AllowNull] ContentType content) =>
            ToSuccess(content);

        IServiceResultPostConfiguration IServiceResultDelegatedFactory<ContentType>.ToFailure() =>
            ToFailure();

        IServiceResultPostConfiguration IServiceResultDelegatedFactory<ContentType>.ToFailure(IServiceResult? serviceResult) =>
            ToFailure(serviceResult);

        IServiceResultPostConfiguration IServiceResultDelegatedFactory<ContentType>.ToFailure(JsonErrors? errors) =>
            ToFailure(errors);

        IServiceResultPostConfiguration IServiceResultDelegatedFactory<ContentType>.ToFailure(JsonError? error) =>
            ToFailure(error);

        IServiceResultPostConfiguration IServiceResultDelegatedFactory<ContentType>.ToFailure(Exception? error) =>
            ToFailure(error);

        IServiceResultPostConfiguration IServiceResultDelegatedFactory<ContentType>.ToFailure(string? errorMessage) =>
            ToFailure(errorMessage);

        #endregion
    }
}
