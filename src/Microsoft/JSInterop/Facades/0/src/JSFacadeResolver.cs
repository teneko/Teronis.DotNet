using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadeResolver : IJSFacadeResolver
    {
        private readonly IJSModuleActivator jsModuleActivator;
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;
        private readonly IServiceProvider serviceProvider;
        private readonly IJSFacadeDictionary jsFacadeDictionary;

        public JSFacadeResolver(
            IJSModuleActivator jsModuleActivator,
            IJSLocalObjectActivator jsLocalObjectActivator,
            IServiceProvider serviceProvider,
            JSFacadeResolverOptions options)
        {
            this.jsModuleActivator = jsModuleActivator ?? throw new ArgumentNullException(nameof(jsModuleActivator));
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (options is null) {
                throw new ArgumentNullException(nameof(options));
            }

            jsFacadeDictionary = options.JSFacadeDictionaryBuilder.Build();
        }

        protected virtual IAsyncDisposable CreateFacade(IJSObjectReferenceFacade facadeConstructorParameter, Type jsFacadeType)
        {
            if (facadeConstructorParameter is null) {
                throw new ArgumentNullException(nameof(facadeConstructorParameter));
            }

            if (!jsFacadeDictionary.TryGetValue(jsFacadeType, out var jsFacadeCreatorHandler)) {
                throw new NotSupportedException($"Type {jsFacadeType} is not supported.");
            }

            if (!(jsFacadeCreatorHandler is null)) {
                return jsFacadeCreatorHandler.Invoke(facadeConstructorParameter);
            }

            return (IAsyncDisposable)ActivatorUtilities.CreateInstance(serviceProvider, jsFacadeType, facadeConstructorParameter);
        }

        public async ValueTask<IAsyncDisposable> CreateModuleFacadeAsync(string moduleNameOrPath, Type jsFacadeType) =>
            CreateFacade(await jsModuleActivator.CreateInstanceAsync(moduleNameOrPath), jsFacadeType);

        public IAsyncDisposable CreateLocalObjectFacade(IJSObjectReference jsObjectReference, Type jsFacadeType) =>
            CreateFacade(jsLocalObjectActivator.CreateInstance(jsObjectReference), jsFacadeType);

        public async ValueTask<IAsyncDisposable> CreateLocalObjectFacadeAsync(string objectName, Type jsFacadeType) =>
            CreateFacade(await jsLocalObjectActivator.CreateInstanceAsync(objectName), jsFacadeType);

        public async ValueTask<IAsyncDisposable> CreateLocalObjectFacadeAsync(IJSObjectReference jsObjectReference, string objectName, Type jsFacadeType) =>
            CreateFacade(await jsLocalObjectActivator.CreateInstanceAsync(jsObjectReference, objectName), jsFacadeType);
    }
}
