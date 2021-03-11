using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public sealed class JSFacadeDictionaryBuilder : IJSFacadeDictionaryBuilder
    {
        private readonly Dictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?> facadeByTypeDictionary;

        public JSFacadeDictionaryBuilder() =>
            facadeByTypeDictionary = new Dictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>();

        public JSFacadeDictionaryBuilder AddDefault()
        {
            ((IJSFacadeDictionaryBuilder)this).AddDefault();
            return this;
        }

        public JSFacadeDictionaryBuilder AddFacade<T>(JSFacadeCreatorDelegate<T> facadeCreatorHandler)
            where T : class, IAsyncDisposable
        {
            if (facadeCreatorHandler is null) {
                throw new ArgumentNullException(nameof(facadeCreatorHandler));
            }

            facadeByTypeDictionary.Add(typeof(T), facadeCreatorHandler);
            return this;
        }

        public JSFacadeDictionaryBuilder AddFacade<T>()
            where T : class
        {
            facadeByTypeDictionary.Add(typeof(T), null);
            return this;
        }

        public JSFacadeDictionary Build() =>
            new JSFacadeDictionary(new ReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>(facadeByTypeDictionary));

        #region IJSFacadeDictionaryBuilder

        IJSFacadeDictionaryBuilder IJSFacadeDictionaryBuilder.AddFacade<T>(JSFacadeCreatorDelegate<T> jsFacadeCreatorHandler) =>
            AddFacade(jsFacadeCreatorHandler);

        IJSFacadeDictionaryBuilder IJSFacadeDictionaryBuilder.AddFacade<T>() =>
            AddFacade<T>();

        #endregion
    }
}
