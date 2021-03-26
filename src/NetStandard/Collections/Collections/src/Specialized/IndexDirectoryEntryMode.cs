// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Specialized
{
    public enum IndexDirectoryEntryMode
    {
        Normal,
        /// <summary>
        /// Adding and removing other indexes affects own index as expected 
        /// but other moving indexes increases own index always by one only
        /// if they cross own index.
        /// </summary>
        Floating
    }
}
