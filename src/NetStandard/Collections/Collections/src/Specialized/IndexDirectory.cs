// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.Utils;

namespace Teronis.Collections.Specialized
{
    public class IndexDirectory
    {
        public int Count => virtualizableLength;

        private List<Entries?> entriesList;
        private int virtualizableLength;

        public IndexDirectory() =>
            entriesList = new List<Entries?>();

        public IndexDirectory(int capacity) =>
            entriesList = new List<Entries?>(capacity);

        private Entries prepareEntriesAt(int index)
        {
            Entries? entries;

            if (index < entriesList.Count) {
                entries = entriesList[index];
            } else {
                var lastIndex = entriesList.Count - 1;

                do {
                    entries = null;
                    entriesList.Add(entries);
                    lastIndex++;
                } while (lastIndex != index);
            }

            if (entries is null) {
                entries = new Entries();
                entriesList[index] = entries;
            }

            if (entriesList.Count > virtualizableLength) {
                virtualizableLength = entriesList.Count;
            }

            return entries;
        }

        /// <summary>
        /// Expands the index directory.
        /// </summary>
        /// <param name="toIndex">The index to be used to expand index directory.</param>
        public void Expand(int toIndex)
        {
            if (toIndex >= virtualizableLength) {
                virtualizableLength = toIndex + 1;
            }
        }

        /// <summary>
        /// Adds <paramref name="index"/> next to same indexes of <paramref name="index"/> and won't cause any index changes.
        /// </summary>
        /// <param name="index">The to be added index.</param>
        /// <returns>The index entry.</returns>
        public IndexDirectoryEntry AddEntry(IndexDirectoryEntry indexEntry)
        {
            var entries = prepareEntriesAt(indexEntry.Index);
            entries.Add(indexEntry);
            return indexEntry;
        }

        /// <summary>
        /// Adds <paramref name="index"/> next to same indexes of <paramref name="index"/> and won't cause any index changes.
        /// </summary>
        /// <param name="index">The to be added index.</param>
        /// <returns>The index entry.</returns>
        public IndexDirectoryEntry Add(int index, IndexDirectoryEntryMode mode)
        {
            var indexEntry = new IndexDirectoryEntry(index, mode);
            return AddEntry(indexEntry);
        }

        /// <summary>
        /// Adds <paramref name="index"/> next to same indexes of <paramref name="index"/> and won't cause any index changes.
        /// </summary>
        /// <param name="index">The to be added index.</param>
        /// <returns>The index entry.</returns>
        public IndexDirectoryEntry Add(int index) =>
            Add(index, IndexDirectoryEntryMode.Normal);

        public IndexDirectoryEntry Add(IndexDirectoryEntryMode mode) =>
            Add(virtualizableLength, mode);

        public IndexDirectoryEntry Add() =>
            Add(IndexDirectoryEntryMode.Normal);

        /// <summary>
        /// Inserts <paramref name="index"/> between <paramref name="index"/> - 1
        /// and <paramref name="index"/> and will cause to move existing indexes at
        /// <paramref name="index"/> by one.
        /// </summary>
        /// <param name="index">The to be inserted index.</param>
        /// <param name="mode">The mode for the index.</param>
        /// <returns>The index entry.</returns>
        public IndexDirectoryEntry Insert(int index, IndexDirectoryEntryMode mode)
        {
            var entryListsCount = virtualizableLength;

            if (index >= entryListsCount) {
                return Add(index, mode);
            }

            var newLastIndex = entryListsCount;
            var indexEntry = new IndexDirectoryEntry(index, mode);
            entriesList.Insert(index, new Entries(new List<IndexDirectoryEntry>() { indexEntry }));
            virtualizableLength++;

            do {
                var entryList = entriesList[newLastIndex];

                if (!(entryList is null)) {
                    static void increaseEntryIndexesByOne(List<IndexDirectoryEntry> entries)
                    {
                        var entriesCount = entries.Count;

                        for (var entryIndex = 0; entryIndex < entriesCount; entryIndex++) {
                            entries[entryIndex].Index++;
                        }
                    }

                    increaseEntryIndexesByOne(entryList.NormalEntries);
                    increaseEntryIndexesByOne(entryList.FloatingEntries);
                }
            } while (--newLastIndex > index);

            return indexEntry;
        }

        /// <summary>
        /// Inserts <paramref name="index"/> between <paramref name="index"/> - 1
        /// and <paramref name="index"/> and will cause to move existing indexes at
        /// <paramref name="index"/> by one.
        /// </summary>
        /// <param name="index">The to be inserted index.</param>
        /// <returns>The index entry.</returns>
        public IndexDirectoryEntry Insert(int index) =>
            Insert(index, IndexDirectoryEntryMode.Normal);

        /// <summary>
        /// Removes index entries that are in range of <paramref name="index"/> and <paramref name="index"/> + <paramref name="count"/> - 1.
        /// </summary>
        /// <param name="index">The beginning index.</param>
        /// <param name="count">
        /// The number of entries from beginning of <paramref name="index"/> that are intended to be removed. Smaller than zero leads to an
        /// <see cref="ArgumentOutOfRangeException"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is smaller than zero greater than <see cref="Count"/>.</exception>
        public void Remove(int index, int count)
        {
            var indexCount = index + count;

            if (index < 0 || indexCount > virtualizableLength) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            virtualizableLength -= count;

            var entriesListCount = entriesList.Count;

            if (index >= entriesListCount) {
                return;
            }

            var borderedIndexCount = indexCount > entriesListCount
                ? entriesListCount
                : indexCount;

            var removeRange = borderedIndexCount - index;
            int nextIndex = index;

            while (nextIndex < entriesListCount) {
                var entryList = entriesList[nextIndex];

                if (!(entryList is null)) {
                    static void decreaseEntryIndexesBy(List<IndexDirectoryEntry> entries, int amount)
                    {
                        var entriesCount = entries.Count;

                        for (var entryIndex = 0; entryIndex < entriesCount; entryIndex++) {
                            entries[entryIndex].Index -= amount;
                        }
                    }

                    decreaseEntryIndexesBy(entryList.NormalEntries, removeRange);
                    decreaseEntryIndexesBy(entryList.FloatingEntries, removeRange);
                }

                nextIndex++;
            }

            entriesList.RemoveRange(index, removeRange);
        }

        /// <summary>
        /// Removes index entry at <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index) =>
            Remove(index, 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexEntry"></param>
        /// <returns></returns>
        public bool RemoveEntry(IndexDirectoryEntry indexEntry)
        {
            if (indexEntry.Index >= virtualizableLength) {
                throw new ArgumentOutOfRangeException(nameof(indexEntry));
            }

            var entryList = entriesList[indexEntry.Index];

            if (entryList is null) {
                return false;
            }

            var success = entryList.NormalEntries.Remove(indexEntry);

            if (entryList.NormalEntries.Count == 0) {
                Remove(indexEntry.Index);
            }

            return success;
        }

        public void Move(int fromIndex, int toIndex, int count)
        {
            if (fromIndex == toIndex) {
                return;
            }

            if (fromIndex < 0 || fromIndex >= virtualizableLength) {
                new ArgumentOutOfRangeException(nameof(fromIndex));
            }

            if (toIndex < 0 || toIndex >= virtualizableLength) {
                new ArgumentOutOfRangeException(nameof(toIndex));
            }

            // The two enumerators are intended to be iterated parallelly.
            IEnumerator<(int Index, Entries? Entries)> enumerator;
            // The future enumerator represents the "future". That means
            // that we pretend it to be the result of the move operation.
            // This we do, because first we manipulate the index values
            // of the entries before we remove and insert the actual
            // items to its destination index.
            IEnumerator<(int Index, Entries? Entries)> futureEnumerator;

            int nonEqualNormalEntryIndexAddend;
            int equalNormalEntryIndexAddend;

            var (lowerIndex, distance) = CollectionTools.GetMoveRange(fromIndex, toIndex, count);
            // Used for creating future enumerator.
            var conserveDistance = distance - count;

            // Used for creating future enumerator.
            var overlapDistance = conserveDistance > count
                ? count
                : conserveDistance;

            if (fromIndex < toIndex) {
                enumerator = Enumerable.Concat(
                        IListGenericUtils.YieldIndexedReverse(entriesList, fromIndex, count),
                        IListGenericUtils.YieldIndexedReverse(entriesList, fromIndex + count, conserveDistance))
                    .GetEnumerator();

                futureEnumerator = Enumerable.Concat(
                        Enumerable.Concat(
                            IListGenericUtils.YieldIndexedReverse(entriesList, fromIndex, count),
                            IListGenericUtils.YieldIndexedReverse(entriesList, fromIndex + count - overlapDistance, overlapDistance)),
                        IListGenericUtils.YieldIndexedReverse(entriesList, fromIndex + count + overlapDistance, conserveDistance - overlapDistance))
                    .GetEnumerator();

                nonEqualNormalEntryIndexAddend = -count;
                equalNormalEntryIndexAddend = conserveDistance;
            } else {
                enumerator = Enumerable.Concat(
                        IListGenericUtils.YieldIndexedReverse(entriesList, lowerIndex, conserveDistance),
                        IListGenericUtils.YieldIndexedReverse(entriesList, fromIndex, count))
                    .GetEnumerator();

                var refillRequiredNonSpareCount = count - overlapDistance;
                // The count of indexes from the back of conserve distance.
                var nonRefillRequiredNonSpareCount = count - refillRequiredNonSpareCount;

                futureEnumerator = Enumerable.Concat(
                        Enumerable.Concat(
                            IListGenericUtils.YieldIndexedReverse(entriesList, lowerIndex, conserveDistance),
                            IListGenericUtils.YieldIndexedReverse(entriesList, lowerIndex + conserveDistance - nonRefillRequiredNonSpareCount, nonRefillRequiredNonSpareCount)),
                        IListGenericUtils.YieldIndexedReverse(entriesList, fromIndex + count - refillRequiredNonSpareCount, refillRequiredNonSpareCount))
                    .GetEnumerator();

                nonEqualNormalEntryIndexAddend = -conserveDistance;
                equalNormalEntryIndexAddend = count;
            }

            while (enumerator.MoveNext() && futureEnumerator.MoveNext()) {
                var (entriesIndex, entries) = enumerator.Current;
                var (futureEntriesIndex, futureEntries) = futureEnumerator.Current;

                if (!(entries is null)) {
                    if (entries.FloatingEntries.Count != 0) {
                        if (entriesIndex != futureEntriesIndex) {
                            if (futureEntries is null) {
                                futureEntries = prepareEntriesAt(futureEntriesIndex);
                            }

                            futureEntries.FloatingEntries.AddRange(entries.FloatingEntries);
                            entries.FloatingEntries.Clear();
                        } else {
                            var floatingEntriesCount = entries.FloatingEntries.Count;

                            while (--floatingEntriesCount >= 0) {
                                entries.FloatingEntries[floatingEntriesCount].Index = entries.FloatingEntries[floatingEntriesCount].Index + equalNormalEntryIndexAddend;
                            }
                        }
                    }

                    var entryListCount = entries.NormalEntries.Count;

                    for (var index = entryListCount - 1; index >= 0; index--) {
                        var entry = entries.NormalEntries[index];

                        if (entriesIndex != futureEntriesIndex) {
                            entry.Index += nonEqualNormalEntryIndexAddend;
                        } else {
                            entry.Index += equalNormalEntryIndexAddend;
                        }
                    }
                }
            }

            var entriesToBeMoved = new Entries[count];
            entriesList.CopyTo(fromIndex, entriesToBeMoved, 0, count);
            entriesList.RemoveRange(fromIndex, count);
            entriesList.InsertRange(toIndex, entriesToBeMoved);
        }

        public void Move(int fromIndex, int toIndex) =>
            Move(fromIndex, toIndex, 1);

        public void ReplaceEntry(IndexDirectoryEntry indexEntry, int newIndex)
        {
            if (indexEntry.Index >= 0) {
                entriesList[indexEntry.Index]!.NormalEntries.Remove(indexEntry);
            }

            indexEntry.Index = newIndex;
            AddEntry(indexEntry);
        }

        /// <summary>
        /// Removes all null and empty lists at the end of
        /// this index directory.
        /// </summary>
        public void TrimEnd()
        {
            var entryListsCount = entriesList.Count;
            var lastIndex = entryListsCount;

            for (var index = entryListsCount - 1; index >= 0; index--) {
                var entries = entriesList[index];

                if (entries is null || (entries.NormalEntries.Count == 0 && entries.FloatingEntries.Count == 0)) {
                    lastIndex = index;
                } else {
                    break;
                }
            }

            virtualizableLength = lastIndex;

            if (lastIndex >= entryListsCount) {
                return;
            }

            entriesList.RemoveRange(lastIndex, entryListsCount - lastIndex);
            entriesList.TrimExcess();
        }

        public void Clear() =>
            entriesList.Clear();

        private class Entries
        {
            public List<IndexDirectoryEntry> NormalEntries {
                get {
                    if (normalEntries is null) {
                        var entryList = new List<IndexDirectoryEntry>();
                        normalEntries = entryList;
                        return entryList;
                    }

                    return normalEntries;
                }
            }

            public List<IndexDirectoryEntry> FloatingEntries {
                get {
                    if (floatingEntries is null) {
                        var entryList = new List<IndexDirectoryEntry>();
                        floatingEntries = entryList;
                        return entryList;
                    }

                    return floatingEntries;
                }
            }

            private List<IndexDirectoryEntry>? normalEntries;
            private List<IndexDirectoryEntry>? floatingEntries;

            public Entries(List<IndexDirectoryEntry>? normalEntries, List<IndexDirectoryEntry>? floatingEntries)
            {
                this.normalEntries = normalEntries;
                this.floatingEntries = floatingEntries;
            }

            public Entries(List<IndexDirectoryEntry> normalEntries)
                : this(normalEntries, null) { }

            public Entries()
                : this(null, null) { }

            public void Add(IndexDirectoryEntry entry)
            {
                if (entry.Mode == IndexDirectoryEntryMode.Normal) {
                    NormalEntries.Add(entry);
                } else if (entry.Mode == IndexDirectoryEntryMode.Floating) {
                    FloatingEntries.Add(entry);
                } else {
                    throw new ArgumentException("Index entry has bad mode.");
                }
            }
        }
    }
}
