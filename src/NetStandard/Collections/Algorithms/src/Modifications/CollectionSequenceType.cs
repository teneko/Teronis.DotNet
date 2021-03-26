// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Algorithms.Modifications
{
    public enum CollectionSequenceType
    {
        /// <summary>
        /// The items are sorted ascended, e.g. index 0 is smaller than index 1
        /// and therefore comparable.
        /// </summary>
        Ascending,
        /// <summary>
        /// The items are sorted descended, e.g. index 0 is greater than index 1
        /// and therefore comparable.
        /// </summary>
        Descending,
        /// <summary>
        /// The items are in any order and therefore not comparable but equality
        /// comparable.
        /// </summary>
        Sequential
    }
}
