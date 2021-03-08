using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public sealed class JSFacadeDictionaryBuilder : IJSFacadeDictionaryBuilder
    {
        private readonly Dictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>> moduleWrapperByTypeDictionary;

        public JSFacadeDictionaryBuilder() =>
            moduleWrapperByTypeDictionary = new Dictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>>();

        public IJSFacadeDictionaryBuilder AddModuleWrapper<T>(JSFacadeCreatorDelegate<T> moduleWrapperCreatorHandler)
            where T : class, IAsyncDisposable
        {
            moduleWrapperByTypeDictionary.Add(typeof(T), moduleWrapperCreatorHandler);
            return this;
        }

        public JSFacadeDictionaryBuilder AddDefault() {
            ((IJSFacadeDictionaryBuilder)this).AddDefault();
            return this;
        }

        public JSFacadeDictionary Build() =>
            new JSFacadeDictionary(new ReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>>(moduleWrapperByTypeDictionary));
    }
}
