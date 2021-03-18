using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public sealed class JSFacadeDictionaryBuilder : IJSFacadeDictionaryConfiguration
    {
        public IEnumerable<Type> Keys =>
            facadeByTypeDictionary.Keys;

        public IEnumerable<JSFacadeCreatorDelegate<IAsyncDisposable>?> Values =>
            facadeByTypeDictionary.Values;

        public int Count =>
            facadeByTypeDictionary.Count;

        private readonly Dictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?> facadeByTypeDictionary;

        public JSFacadeDictionaryBuilder() =>
            facadeByTypeDictionary = new Dictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>();

        public JSFacadeCreatorDelegate<IAsyncDisposable>? this[Type key] =>
            facadeByTypeDictionary[key];

        public bool ContainsKey(Type key) =>
            facadeByTypeDictionary.ContainsKey(key);

        public bool TryGetValue(Type key, [MaybeNullWhen(false)] out JSFacadeCreatorDelegate<IAsyncDisposable>? value) =>
            facadeByTypeDictionary.TryGetValue(key, out value);

        public JSFacadeDictionaryBuilder AddDefault()
        {
            ((IJSFacadeDictionaryConfiguration)this).AddDefault();
            return this;
        }

        public JSFacadeDictionaryBuilder Add(Type jsFacadeType, JSFacadeCreatorDelegate<IAsyncDisposable>? jsFacadeCreatorDelegate = null)
        {
            facadeByTypeDictionary.Add(jsFacadeType, jsFacadeCreatorDelegate);
            return this;
        }

        public JSFacadeDictionaryBuilder Add<T>(JSFacadeCreatorDelegate<T>? jsFacadeCreatorDelegate = null)
            where T : class, IAsyncDisposable
        {
            facadeByTypeDictionary.Add(typeof(T), jsFacadeCreatorDelegate);
            return this;
        }

        public JSFacadeDictionaryBuilder Remove(Type jsFacadeType)
        {
            facadeByTypeDictionary.Remove(jsFacadeType);
            return this;
        }

        public JSFacadeDictionaryBuilder Remove<T>()
            where T : IAsyncDisposable
        {
            facadeByTypeDictionary.Remove(typeof(T));
            return this;
        }

        public JSFacadeDictionaryBuilder Clear()
        {
            facadeByTypeDictionary.Clear();
            return this;
        }

        public IEnumerator<KeyValuePair<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>> GetEnumerator() =>
            facadeByTypeDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public JSFacadeDictionary Build() =>
            new JSFacadeDictionary(new ReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>(facadeByTypeDictionary));

        #region IJSFacadeDictionaryBuilder

        IJSFacadeDictionaryConfiguration IJSFacadeDictionaryConfiguration.Add(Type jsFacadeType, JSFacadeCreatorDelegate<IAsyncDisposable>? jsFacadeCreatorDelegate) =>
            Add(jsFacadeType, jsFacadeCreatorDelegate);

        IJSFacadeDictionaryConfiguration IJSFacadeDictionaryConfiguration.Add<T>(JSFacadeCreatorDelegate<T>? jsFacadeCreatorHandler) =>
            Add(jsFacadeCreatorHandler);

        IJSFacadeDictionaryConfiguration IJSFacadeDictionaryConfiguration.Remove(Type jsFacadeType) =>
            Remove(jsFacadeType);

        IJSFacadeDictionaryConfiguration IJSFacadeDictionaryConfiguration.Remove<T>() =>
            Remove<T>();

        IJSFacadeDictionaryConfiguration IJSFacadeDictionaryConfiguration.Clear() =>
            Clear();

        #endregion
    }
}
