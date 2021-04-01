// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Collections
{
    public class TreeIteratorEntry<T> : ITreeIteratorEntry
    {
        public T Item { get; }
        public bool Skippable { get; private set; }

        public TreeIteratorEntry(T item) =>
            Item = item ?? throw new ArgumentNullException(nameof(item));

        public void SetSkippable() =>
            Skippable = true;
    }
}
