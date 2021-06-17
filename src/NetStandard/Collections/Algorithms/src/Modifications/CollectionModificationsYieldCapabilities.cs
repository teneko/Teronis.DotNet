// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Algorithms.Modifications
{
    public enum CollectionModificationYieldCapabilities
    {
        /// <summary>
        /// Modifications for inserting items are considered to be yielded.
        /// </summary>
        Insert = 1,
        /// <summary>
        /// Modifications for removing items are considered to be yielded.
        /// </summary>
        Remove = 2,
        /// <summary>
        /// Modifications for replacing items are considered to be yielded.
        /// </summary>
        Replace = 4,
        /// <summary>
        /// Modifications for inserting or removing items are considered to be yielded.
        /// </summary>
        InsertRemove = Insert | Remove,
        /// <summary>
        /// Modifications for inserting or replacing items are considered to be yielded.
        /// </summary>
        InsertReplace = Insert | Replace,
        /// <summary>
        /// Modifications for inserting, removing or replacing items are considered to be yielded.
        /// </summary>
        All = Insert | Remove | Replace,
    }
}

