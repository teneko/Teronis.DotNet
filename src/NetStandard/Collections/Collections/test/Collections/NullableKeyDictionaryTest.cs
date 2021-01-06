using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.Collections.Generic;
using Xunit;

namespace Test.NetStandard.Collections
{
    public class NullableKeyDictionaryTest
    {
        [Fact]
        public void Add_and_remove_test()
        {
            // Assign.
            var dictionary = new NullableKeyDictionary<string, string>();
            IDictionary<string, string> nonNullableDictionary = dictionary;
            INullableKeyDictionary<string, string> nullableDictionary = dictionary;

            // Assert.
            dictionary.Add("value");
            /// Assert.Empty does cast to IEnumerable, but our implementation of IEnumerable 
            /// returns an enumerator of type <see cref="KeyValuePair{StillNullable, TValue}"/>.
            /// So we test on correct enumerator implementation wether it can move or not.
            Assert.False(nonNullableDictionary.GetEnumerator().MoveNext());
            Assert.NotEmpty(nullableDictionary);
            Assert.Throws<ArgumentException>(() => dictionary.Add("value"));

            Assert.True(dictionary.Remove());
            Assert.Empty(nullableDictionary);

            dictionary.Add("key", "value");
            Assert.True(nonNullableDictionary.GetEnumerator().MoveNext());
            Assert.NotEmpty(nullableDictionary);
            Assert.Throws<ArgumentException>(() => dictionary.Add("key", "value"));

            dictionary.Add("value");
            Assert.Equal(1, nonNullableDictionary.Count);
            Assert.Equal(2, nullableDictionary.Count);
        }
    }
}
