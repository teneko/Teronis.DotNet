// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator, IDisposable
        where TKey : notnull
    {
        public DictionaryEntry Entry {
            get {
                return new DictionaryEntry(enumerator.Current.Key,
                    enumerator.Current.Value);
            }
        }

        public object Key { get { return enumerator.Current.Key; } }
        public object? Value { get { return enumerator.Current.Value; } }
        public object Current { get { return Entry; } }

        readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

        public DictionaryEnumerator(IDictionary<TKey, TValue> value) =>
            enumerator = value.GetEnumerator() ?? throw new ArgumentException("An empty enumerator has been obtained.");

        public void Reset() =>
            enumerator.Reset();

        public bool MoveNext() =>
            enumerator.MoveNext();

        public void Dispose() =>
            enumerator.Dispose();
    }
}
