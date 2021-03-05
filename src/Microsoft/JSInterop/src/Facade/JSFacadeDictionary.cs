using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.AddOn.Microsoft.JSInterop.Facade
{
    public sealed class JSFacadeDictionary : IReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>>, IJSFacadeDictionary
    {
        public IEnumerable<Type> Keys =>
            dictionary.Keys;

        public IEnumerable<JSFacadeCreatorDelegate<IAsyncDisposable>> Values =>
            dictionary.Values;

        public int Count =>
            dictionary.Count;

        private readonly IReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>> dictionary;

        public JSFacadeDictionary(IReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>> dictionary) =>
            this.dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));

        public JSFacadeCreatorDelegate<IAsyncDisposable> this[Type key] =>
            dictionary[key];

        public bool ContainsKey(Type key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool TryGetValue(Type key, [MaybeNullWhen(false)] out JSFacadeCreatorDelegate<IAsyncDisposable> value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<Type, JSFacadeCreatorDelegate<IAsyncDisposable>>> GetEnumerator() =>
            dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)dictionary).GetEnumerator();
    }
}
