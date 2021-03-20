using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public sealed class JSCustomFacadeDictionaryBuilder : IJSCustomFacadeDictionaryBuilder
    {
        public IEnumerable<Type> Keys =>
            facadeByTypeDictionary.Keys;

        public IEnumerable<JSCustomFacadeFactoryDelegate<IAsyncDisposable>?> Values =>
            facadeByTypeDictionary.Values;

        public int Count =>
            facadeByTypeDictionary.Count;

        private readonly Dictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?> facadeByTypeDictionary;

        public JSCustomFacadeDictionaryBuilder() =>
            facadeByTypeDictionary = new Dictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>();

        public JSCustomFacadeFactoryDelegate<IAsyncDisposable>? this[Type key] =>
            facadeByTypeDictionary[key];

        public bool ContainsKey(Type key) =>
            facadeByTypeDictionary.ContainsKey(key);

        public bool TryGetValue(Type key, [MaybeNullWhen(false)] out JSCustomFacadeFactoryDelegate<IAsyncDisposable>? value) =>
            facadeByTypeDictionary.TryGetValue(key, out value);

        public JSCustomFacadeDictionaryBuilder AddDefault()
        {
            ((IJSCustomFacadeDictionaryBuilder)this).AddDefault();
            return this;
        }

        public JSCustomFacadeDictionaryBuilder Add(Type jsFacadeType, JSCustomFacadeFactoryDelegate<IAsyncDisposable>? jsFacadeCreatorDelegate = null)
        {
            facadeByTypeDictionary.Add(jsFacadeType, jsFacadeCreatorDelegate);
            return this;
        }

        public JSCustomFacadeDictionaryBuilder Add<T>(JSCustomFacadeFactoryDelegate<T>? jsFacadeCreatorDelegate = null)
            where T : class, IAsyncDisposable
        {
            facadeByTypeDictionary.Add(typeof(T), jsFacadeCreatorDelegate);
            return this;
        }

        public JSCustomFacadeDictionaryBuilder Remove(Type jsFacadeType)
        {
            facadeByTypeDictionary.Remove(jsFacadeType);
            return this;
        }

        public JSCustomFacadeDictionaryBuilder Remove<T>()
            where T : IAsyncDisposable
        {
            facadeByTypeDictionary.Remove(typeof(T));
            return this;
        }

        public JSCustomFacadeDictionaryBuilder Clear()
        {
            facadeByTypeDictionary.Clear();
            return this;
        }

        public IEnumerator<KeyValuePair<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>> GetEnumerator() =>
            facadeByTypeDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public JSCustomFacadeDictionary Build() =>
            new JSCustomFacadeDictionary(new ReadOnlyDictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>(facadeByTypeDictionary));

        #region IJSFacadeDictionaryBuilder

        IJSCustomFacadeDictionaryBuilder IJSCustomFacadeDictionaryBuilder.Add(Type jsFacadeType, JSCustomFacadeFactoryDelegate<IAsyncDisposable>? jsFacadeCreatorDelegate) =>
            Add(jsFacadeType, jsFacadeCreatorDelegate);

        IJSCustomFacadeDictionaryBuilder IJSCustomFacadeDictionaryBuilder.Add<T>(JSCustomFacadeFactoryDelegate<T>? jsFacadeCreatorHandler) =>
            Add(jsFacadeCreatorHandler);

        IJSCustomFacadeDictionaryBuilder IJSCustomFacadeDictionaryBuilder.Remove(Type jsFacadeType) =>
            Remove(jsFacadeType);

        IJSCustomFacadeDictionaryBuilder IJSCustomFacadeDictionaryBuilder.Remove<T>() =>
            Remove<T>();

        IJSCustomFacadeDictionaryBuilder IJSCustomFacadeDictionaryBuilder.Clear() =>
            Clear();

        #endregion
    }
}
