using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public interface ICovariantReadOnlyDictionary<TKey, out ValueType> :
        IEnumerable<ICovariantKeyValuePair<TKey, ValueType>>, IEnumerable, IReadOnlyCollection<ICovariantKeyValuePair<TKey, ValueType>>
    {
        ValueType this[[AllowNull] TKey key] { get; }
        IEnumerable<TKey> Keys { get; }
        IEnumerable<ValueType> Values { get; }
        bool ContainsKey([AllowNull] TKey key);
        /// <summary>
        /// Tries to find a value by <paramref name="key"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>
        /// Tuple where <see cref="ICovariantTuple{T1, T2}.Item1" /> represents a boolean whether a value has been found
        /// and <see cref="ICovariantTuple{T1, T2}.Item2"/> represents the value that may have been found.
        /// </returns>
        ICovariantTuple<bool, ValueType> TryGetValue([AllowNull] TKey key);
    }
}
