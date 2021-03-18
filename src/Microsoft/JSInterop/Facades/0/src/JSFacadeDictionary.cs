using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public sealed class JSFacadeDictionary : IReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>, IJSFacadeDictionary
    {
        public IEnumerable<Type> Keys =>
            dictionary.Keys;

        public IEnumerable<JSFacadeCreatorDelegate<IAsyncDisposable>?> Values =>
            dictionary.Values;

        public int Count =>
            dictionary.Count;

        private readonly IReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?> dictionary;

        public JSFacadeDictionary(IReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?> dictionary) =>
            this.dictionary = new Dictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>(dictionary) ?? throw new ArgumentNullException(nameof(dictionary));

        public JSFacadeCreatorDelegate<IAsyncDisposable>? this[Type key] =>
            dictionary[key];

        public bool ContainsKey(Type key) =>
            dictionary.ContainsKey(key);

        public bool TryGetValue(Type key, [MaybeNullWhen(false)] out JSFacadeCreatorDelegate<IAsyncDisposable>? value) =>
            dictionary.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>> GetEnumerator() =>
            dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
