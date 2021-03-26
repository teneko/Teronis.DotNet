// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.EntityFrameworkCore.Query
{
    /// <summary>
    /// Controls the behaviour of the collection of comparison items.
    /// </summary>
    public enum ComparisonItemsBehaviourFlags
    {
        /// <summary>
        /// If the collection of the comparison items is null or empty, 
        /// the boolean expression branch for each item won't be created.
        /// </summary>
        NullOrEmptyLeadsToSkip = 0,
        /// <summary>
        /// If the collection of the comparison items is null, the boolean 
        /// expression branch for each each item won't be created. Instead 
        /// an expression is used that leads to false.
        /// </summary>
        NullLeadsToFalse = 1,
        /// <summary>
        /// If the collection of the comparison items is empty, the boolean
        /// expression branch for each each item won't be created. Instead 
        /// an expression is used that leads to false.
        /// </summary>
        EmptyLeadsToFalse = 2,
        /// <summary>
        /// If the collection of the comparison items is null or empty,
        /// the boolean expression branch for each each item won't be
        /// created. Instead an expression is used that leads to false.
        /// </summary>
        NullOrEmptyLeadsToFalse = NullLeadsToFalse | EmptyLeadsToFalse
    }
}
