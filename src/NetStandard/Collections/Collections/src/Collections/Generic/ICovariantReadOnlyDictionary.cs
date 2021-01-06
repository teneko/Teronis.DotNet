using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface ICovariantReadOnlyDictionary<TKey, out ValueType> : 
        IEnumerable<ICovariantKeyValuePair<TKey, ValueType>>, IEnumerable, IReadOnlyCollection<ICovariantKeyValuePair<TKey, ValueType>>
        where TKey : notnull
    {
        ValueType this[TKey key] { get; }
        IEnumerable<TKey> Keys { get; }
        IEnumerable<ValueType> Values { get; }
        bool ContainsKey(TKey key);
        /// <summary>
        /// Tries to finds a value by <paramref name="key"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>
        /// Tuple where <see cref="ICovariantTuple{T1, T2}.Item1" /> represents a boolean whether a value has been found
        /// and <see cref="ICovariantTuple{T1, T2}.Item2"/> represents the value that may have been found.
        /// </returns>
        ICovariantTuple<bool, ValueType> TryGetValue(TKey key);
    }
}
