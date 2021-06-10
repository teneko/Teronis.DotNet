// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Specialized
{
    public enum IndexDirectoryEntryMode
    {
        Normal,
        /// <summary>
        /// Adding and removing normal index entries affects floating index the same
        /// as normal index entries.
        /// Only moving normal index entries increases only the floating index by one 
        /// if they cross the normal and therefore the floating index entry index.
        /// </summary>
        Floating
    }
}
