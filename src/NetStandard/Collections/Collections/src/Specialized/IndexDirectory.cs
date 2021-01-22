using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.Tools;
using Teronis.Utils;

namespace Teronis.Collections.Specialized
{
    public class IndexDirectory
    {
        public int Count => virtualizableLength;
        public LastIndexDirectoryEntry LastIndexEntry {get;}

        private List<List<IndexDirectoryEntry>?> entryLists;
        private int virtualizableLength;

        public IndexDirectory()
        {
            entryLists = new List<List<IndexDirectoryEntry>?>();
            LastIndexEntry = new LastIndexDirectoryEntry(this);
        }

        public IndexDirectory(int capacity)
        {
            entryLists = new List<List<IndexDirectoryEntry>?>(capacity);
            LastIndexEntry = new LastIndexDirectoryEntry(this);
        }

        private List<IndexDirectoryEntry> prepareListAt(int index)
        {
            List<IndexDirectoryEntry>? entryList;

            if (index < entryLists.Count) {
                entryList = entryLists[index];
            } else {
                var lastIndex = entryLists.Count - 1;

                do {
                    entryList = null;
                    entryLists.Add(entryList);
                    lastIndex++;
                } while (lastIndex != index);
            }

            if (entryList is null) {
                entryList = new List<IndexDirectoryEntry>();
                entryLists[index] = entryList;
            }

            if (entryLists.Count > virtualizableLength) {
                virtualizableLength = entryLists.Count;
            }

            return entryList;
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
            var entryList = prepareListAt(indexEntry.Index);
            entryList.Add(indexEntry);
            return indexEntry;
        }

        /// <summary>
        /// Adds <paramref name="index"/> next to same indexes of <paramref name="index"/> and won't cause any index changes.
        /// </summary>
        /// <param name="index">The to be added index.</param>
        /// <returns>The index entry.</returns>
        public IndexDirectoryEntry Add(int index, IndexDirectoryEntryMode mode)
        {
            var entryList = prepareListAt(index);
            var indexEntry = new IndexDirectoryEntry(index, mode);
            entryList.Add(indexEntry);
            return indexEntry;
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
        /// <paramref name="index"/> by one except floating indexes.
        /// </summary>
        /// <param name="index">The to be inserted index.</param>
        /// <returns>The index entry.</returns>
        public IndexDirectoryEntry Insert(int index, IndexDirectoryEntryMode mode = IndexDirectoryEntryMode.Normal)
        {
            var entryListsCount = virtualizableLength;

            if (index >= entryListsCount) {
                return Add(index, mode);
            }

            var newLastIndex = entryListsCount;
            var indexEntry = new IndexDirectoryEntry(index, mode);
            entryLists.Insert(index, new List<IndexDirectoryEntry>() { indexEntry });
            virtualizableLength++;

            do {
                var entryList = entryLists[newLastIndex];

                if (!(entryList is null)) {
                    var entryListCount = entryList.Count;

                    for (var entryIndex = 0; entryIndex < entryListCount; entryIndex++) {
                        entryList[entryIndex].Index++;
                    }
                }
            } while (--newLastIndex > index);

            return indexEntry;
        }

        /// <summary>
        /// Removes index at <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is smaller than zero greater than <see cref="Count"/>.</exception>
        public void Remove(int index)
        {
            var entryListsCount = virtualizableLength;

            if (index < 0 || index >= virtualizableLength) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index >= entryLists.Count) {
                virtualizableLength--;
                return;
            }

            int nextIndex = index + 1;

            while (nextIndex < entryListsCount) {
                var entryList = entryLists[nextIndex];

                if (!(entryList is null)) {
                    var entryListCount = entryList.Count;

                    for (var entryIndex = 0; entryIndex < entryListCount; entryIndex++) {
                        entryList[entryIndex].Index--;
                    }
                }

                nextIndex++;
            }

            entryLists.RemoveAt(index);
            virtualizableLength--;
        }

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

            var entryList = entryLists[indexEntry.Index];

            if (entryList is null) {
                return false;
            }

            var success = entryList.Remove(indexEntry);

            if (entryList.Count == 0) {
                Remove(indexEntry.Index);
            }

            return success;
        }

        public void Move(int fromIndex, int toIndex)
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

            IEnumerator<(int Index, List<IndexDirectoryEntry>? Item)> entryEnumerator;
            var (minIndex, distance) = CollectionTools.GetMoveRange(fromIndex, toIndex, 1);
            bool enumeratesNonReversed;

            if (fromIndex < toIndex) {
                var enumeratorIndex = fromIndex;
                entryEnumerator = entryLists.Skip(minIndex).Take(distance).Select(item => (enumeratorIndex++, item)).GetEnumerator();
                enumeratesNonReversed = true;
            } else {
                entryEnumerator = IListGenericUtils.IndexedReverse(entryLists, minIndex, distance).GetEnumerator();
                enumeratesNonReversed = false;
            }

            entryEnumerator.MoveNext();
            var (firstEntryListIndex, firstEntryList) = entryEnumerator.Current;
            var newEntryListIndex = firstEntryListIndex;
            entryEnumerator.MoveNext();
            List<IndexDirectoryEntry>? floatingEntries;

            if (enumeratesNonReversed) {
                floatingEntries = new List<IndexDirectoryEntry>();
            } else {
                floatingEntries = null;
            }

            do {
                var (entryListIndex, entryList) = entryEnumerator.Current;

                if (!(entryList is null)) {
                    var entryListCount = entryList.Count;

                    if (enumeratesNonReversed && floatingEntries!.Count != 0) {
                        var floatingEntriesCount = floatingEntries.Count;

                        for (var index = 0; index < floatingEntriesCount; index++) {
                            var floatingEntry = floatingEntries[index];
                            // Set index because..
                            floatingEntry.Index = newEntryListIndex;
                            // added floating entries won't be iterated below.
                            entryList.Add(floatingEntry);
                        }

                        floatingEntries.Clear();
                    }

                    for (var index = entryListCount - 1; index >= 0; index--) {
                        var entry = entryList[index];
                        entry.Index = newEntryListIndex;

                        if (entry.Mode == IndexDirectoryEntryMode.Floating && enumeratesNonReversed) {
                            floatingEntries!.Add(entry);
                            entryList.RemoveAt(index);
                        }
                    }
                }

                newEntryListIndex = entryListIndex;
            } while (entryEnumerator.MoveNext());

            var requiresFloatingEntriesHandlingCausedByNonReversedEnumeration = enumeratesNonReversed && floatingEntries!.Count != 0;

            if (requiresFloatingEntriesHandlingCausedByNonReversedEnumeration && firstEntryList is null) {
                firstEntryList = new List<IndexDirectoryEntry>(floatingEntries!.Count);
            }

            if (requiresFloatingEntriesHandlingCausedByNonReversedEnumeration) {
                var floatingEntriesCount = floatingEntries!.Count;

                for (var floatingEntryIndex = 0; floatingEntryIndex < floatingEntriesCount; floatingEntryIndex++) {
                    var floatingEntry = floatingEntries[floatingEntryIndex];
                    floatingEntry.Index = toIndex;
                    firstEntryList!.Add(floatingEntry);
                }

                floatingEntries.Clear();
            }


            entryLists.RemoveAt(fromIndex);
            entryLists.Insert(toIndex, firstEntryList);

            if (!(firstEntryList is null)) {
                var firstEntryListCount = firstEntryList.Count;

                for (var index = firstEntryListCount - 1; index >= 0; index--) {
                    var entry = firstEntryList[index];

                    if (entry.Mode == IndexDirectoryEntryMode.Floating && !enumeratesNonReversed) {
                        firstEntryList.RemoveAt(index);
                        prepareListAt(entry.Index).Add(entry);
                    } else {
                        firstEntryList[index].Index = toIndex;
                    }
                }
            }
        }

        public void ReplaceEntry(IndexDirectoryEntry indexEntry, int newIndex)
        {
            if (indexEntry.Index >= 0) {
                entryLists[indexEntry.Index]!.Remove(indexEntry);
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
            var entryListsCount = entryLists.Count;
            var lastIndex = entryListsCount;

            for (var index = entryListsCount - 1; index >= 0; index--) {
                var entryList = entryLists[index];

                if (entryList is null || entryList.Count == 0) {
                    lastIndex = index;
                } else {
                    break;
                }
            }

            virtualizableLength = lastIndex;

            if (lastIndex >= entryListsCount) {
                return;
            }

            entryLists.RemoveRange(lastIndex, entryListsCount - lastIndex);
            entryLists.TrimExcess();
        }

        public void Clear() =>
            entryLists.Clear();
    }

    public sealed class IndexDirectoryEntry
    {
        public int Index { get; internal set; }
        public IndexDirectoryEntryMode Mode { get; }

        public IndexDirectoryEntry(int index, IndexDirectoryEntryMode mode)
        {
            Index = index;
            Mode = mode;
        }

        public static implicit operator int(IndexDirectoryEntry entry) =>
            entry.Index;
    }

    public sealed class LastIndexDirectoryEntry
    {
        public int Index => indexDirectory.Count - 1;

        private readonly IndexDirectory indexDirectory;

        internal LastIndexDirectoryEntry(IndexDirectory indexDirectory) => 
            this.indexDirectory = indexDirectory;

        public static implicit operator int(LastIndexDirectoryEntry entry) =>
            entry.Index;
    }

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
