// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public sealed class JSCustomFacadeDictionary : IReadOnlyDictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>, IJSCustomFacadeDictionary
    {
        public IEnumerable<Type> Keys =>
            dictionary.Keys;

        public IEnumerable<JSCustomFacadeFactoryDelegate<IAsyncDisposable>?> Values =>
            dictionary.Values;

        public int Count =>
            dictionary.Count;

        private readonly IReadOnlyDictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?> dictionary;

        public JSCustomFacadeDictionary(IReadOnlyDictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?> dictionary) =>
            this.dictionary = new Dictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>(dictionary) ?? throw new ArgumentNullException(nameof(dictionary));

        public JSCustomFacadeFactoryDelegate<IAsyncDisposable>? this[Type key] =>
            dictionary[key];

        public bool ContainsKey(Type key) =>
            dictionary.ContainsKey(key);

        public bool TryGetValue(Type key, [MaybeNullWhen(false)] out JSCustomFacadeFactoryDelegate<IAsyncDisposable>? value) =>
            dictionary.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>> GetEnumerator() =>
            dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
