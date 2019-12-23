using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator, IDisposable
    {
        public DictionaryEntry Entry {
            get {
                var pair = enumerator.Current;
                return new DictionaryEntry(pair.Key, pair.Value);
            }
        }

        public object Key { get { return enumerator.Current.Key; } }
        public object Value { get { return enumerator.Current.Value; } }
        public object Current { get { return Entry; } }

        readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

        public DictionaryEnumerator(IDictionary<TKey, TValue> value) => enumerator = value.GetEnumerator();

        public void Reset() { enumerator.Reset(); }

        public bool MoveNext() { return enumerator.MoveNext(); }

        public void Dispose() { enumerator.Dispose(); }
    }
}
