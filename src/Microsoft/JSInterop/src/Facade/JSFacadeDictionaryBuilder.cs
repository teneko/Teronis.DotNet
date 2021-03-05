using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Teronis.AddOn.Microsoft.JSInterop.Facade
{
    public sealed class JSFacadeDictionaryBuilder
    {
        private readonly Dictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>> moduleWrapperByTypeDictionary;

        public JSFacadeDictionaryBuilder() =>
            moduleWrapperByTypeDictionary = new Dictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>>();

        public JSFacadeDictionaryBuilder AddModuleWrapper<T>(JSFacadeCreatorDelegate<T> moduleWrapperCreatorHandler)
            where T : class, IAsyncDisposable
        {
            moduleWrapperByTypeDictionary.Add(typeof(T), moduleWrapperCreatorHandler);
            return this;
        }

        public JSFacadeDictionary Build() =>
            new JSFacadeDictionary(new ReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>>(moduleWrapperByTypeDictionary));
    }
}
