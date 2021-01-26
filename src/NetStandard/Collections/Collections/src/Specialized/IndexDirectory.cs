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
        /// <paramref name="index"/> by one except floating indexes.
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
                    var entryListCount = entryList.NormalEntries.Count;

                    for (var entryIndex = 0; entryIndex < entryListCount; entryIndex++) {
                        entryList.NormalEntries[entryIndex].Index++;
                    }
                }
            } while (--newLastIndex > index);

            return indexEntry;
        }

        /// <summary>
        /// Inserts <paramref name="index"/> between <paramref name="index"/> - 1
        /// and <paramref name="index"/> and will cause to move existing indexes at
        /// <paramref name="index"/> by one except floating indexes.
        /// </summary>
        /// <param name="index">The to be inserted index.</param>
        /// <returns>The index entry.</returns>
        public IndexDirectoryEntry Insert(int index) =>
            Insert(index, IndexDirectoryEntryMode.Normal);

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

            if (index >= entriesList.Count) {
                virtualizableLength--;
                return;
            }

            int nextIndex = index + 1;

            while (nextIndex < entryListsCount) {
                var entryList = entriesList[nextIndex];

                if (!(entryList is null)) {
                    var entryListCount = entryList.NormalEntries.Count;

                    for (var entryIndex = 0; entryIndex < entryListCount; entryIndex++) {
                        entryList.NormalEntries[entryIndex].Index--;
                    }
                }

                nextIndex++;
            }

            entriesList.RemoveAt(index);
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

            IEnumerator<(int Index, Entries? Entries)> entryEnumerator;
            var (lowerIndex, distance) = CollectionTools.GetMoveRange(fromIndex, toIndex, count);
            bool enumeratesNonReversed;

            if (fromIndex < toIndex) {
                var enumeratorIndex = fromIndex;
                entryEnumerator = entriesList.Skip(lowerIndex).Take(distance).Select(item => (enumeratorIndex++, item)).GetEnumerator();
                enumeratesNonReversed = true;
            } else {
                entryEnumerator = IListGenericUtils.IndexedReverse(entriesList, lowerIndex, distance).GetEnumerator();
                enumeratesNonReversed = false;
            }

            entryEnumerator.MoveNext();
            var (entriesIndexFirst, entriesFirst) = entryEnumerator.Current;

            var entriesToBeMoved = new Entries?[count];
            entriesToBeMoved[0] = entriesFirst;

            {
                var toBeMovedIndex = 0;

                while (++toBeMovedIndex < count) {
                    entryEnumerator.MoveNext();
                    entriesToBeMoved[toBeMovedIndex] = entryEnumerator.Current.Entries;
                }
            }

            var entriesIndexCurrent = entriesIndexFirst;
            entryEnumerator.MoveNext();

            do {
                var (entryListIndex, entryList) = entryEnumerator.Current;

                if (!(entryList is null)) {
                    var entryListCount = entryList.NormalEntries.Count;

                    //if (enumeratesNonReversed && floatingEntries!.Count != 0) {
                    //    var floatingEntriesCount = floatingEntries.Count;

                    //    for (var index = 0; index < floatingEntriesCount; index++) {
                    //        var floatingEntry = floatingEntries[index];
                    //        // Set index because..
                    //        floatingEntry.Index = newEntryListIndex;
                    //        // added floating entries won't be iterated below.
                    //        entryList.Add(floatingEntry);
                    //    }

                    //    floatingEntries.Clear();
                    //}

                    for (var index = entryListCount - 1; index >= 0; index--) {
                        var entry = entryList.NormalEntries[index];
                        entry.Index = entriesIndexCurrent;

                        //if (entry.Mode == IndexDirectoryEntryMode.Floating && enumeratesNonReversed) {
                        //    floatingEntries!.Add(entry);
                        //    entryList.RemoveAt(index);
                        //}
                    }
                }

                entriesIndexCurrent = entryListIndex;
            } while (entryEnumerator.MoveNext());

            //var requiresFloatingEntriesHandlingCausedByNonReversedEnumeration = enumeratesNonReversed && floatingEntries!.Count != 0;

            //if (requiresFloatingEntriesHandlingCausedByNonReversedEnumeration) {
            //    if (firstEntryList is null) {
            //        firstEntryList = new List<IndexDirectoryEntry>(floatingEntries!.Count);
            //    }

            //    var floatingEntriesCount = floatingEntries!.Count;

            //    for (var floatingEntryIndex = 0; floatingEntryIndex < floatingEntriesCount; floatingEntryIndex++) {
            //        var floatingEntry = floatingEntries[floatingEntryIndex];
            //        floatingEntry.Index = toIndex;
            //        firstEntryList!.Add(floatingEntry);
            //    }

            //    floatingEntries.Clear();
            //}

            entriesList.RemoveAt(fromIndex);
            entriesList.InsertRange(toIndex, entriesToBeMoved);

            if (!(entriesFirst is null)) {
                var toIndexLast = toIndex + count;

                for (var entriesToBeMovedIndex = entriesIndexFirst; entriesToBeMovedIndex < toIndexLast; entriesToBeMovedIndex++) {
                    var entries = entriesToBeMoved[entriesToBeMovedIndex];

                    if (enumeratesNonReversed) {
                        for (var index = 0; index >= 0; index--) {
                            //var entries = entr
                            var entry = entriesFirst.NormalEntries[index];

                            //if (entry.Mode == IndexDirectoryEntryMode.Floating && !enumeratesNonReversed) {
                            //    firstEntryList.RemoveAt(index);
                            //    prepareListAt(entry.Index).Add(entry);
                            //} else {
                            entriesFirst.NormalEntries[index].Index = toIndex;
                            //}
                        }
                    }

                    //var floatingEntries = entries.FloatingEntries
                }

                var firstEntryListCount = entriesFirst.NormalEntries.Count;
            }
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
