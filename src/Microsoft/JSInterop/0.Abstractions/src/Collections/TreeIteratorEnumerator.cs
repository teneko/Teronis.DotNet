// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections;

namespace Teronis.Microsoft.JSInterop.Collections
{
    public class TreeIteratorEnumerator<TEntry> : IEnumerator<TEntry>
        where TEntry : class, ITreeIteratorEntry
    {
        public int CurrentIndex =>
            currentIndex;

        private TEntry? currentInterceptor;
        private readonly IReadOnlyList<TEntry> entries;
        private int currentIndex;

        public TreeIteratorEnumerator(IReadOnlyList<TEntry> entries, int initialIndex = -1)
        {
            this.entries = entries;
            currentIndex = initialIndex;
            currentInterceptor = null;
        }

        public TEntry Current =>
            currentInterceptor ?? throw new InvalidOperationException("Current interceptor not available.");

        public bool MoveNext()
        {
            var newCurrentIndex = currentIndex;
            TEntry? newCurrentInterceptor;

            do {
                var canMoveNext = ++newCurrentIndex < entries.Count;

                if (canMoveNext) {
                    newCurrentInterceptor = entries[newCurrentIndex];
                } else {
                    currentInterceptor = null;
                    currentIndex = newCurrentIndex;
                    return false;

                }
            } while (newCurrentInterceptor.Skippable);

            currentIndex = newCurrentIndex;
            currentInterceptor = newCurrentInterceptor;
            return true;
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public void Reset() =>
            throw new NotSupportedException();

        object IEnumerator.Current =>
            Current;

        public void Dispose()
        { }
    }
}
