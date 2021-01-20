using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.Tools;
using Teronis.Utils;

namespace Teronis.Collections.Specialized
{
    public class IndexDirectory
    {
        public int Count => entryLists.Count;

        private List<List<IndexDirectoryEntry>?> entryLists;

        public IndexDirectory() =>
            entryLists = new List<List<IndexDirectoryEntry>?>();

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

            return entryList;
        }

        public void Expand(int toIndex) {
            var newCount = entryLists.Count + toIndex;

            for (int index = entryLists.Count; index <= newCount; index++) {
                entryLists.Add(null);
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

        /// <summary>
        /// Inserts <paramref name="index"/> between <paramref name="index"/> - 1
        /// and <paramref name="index"/> and will cause to move existing indexes at
        /// <paramref name="index"/> by one except floating indexes.
        /// </summary>
        /// <param name="index">The to be inserted index.</param>
        /// <returns>The index entry.</returns>
        public IndexDirectoryEntry Insert(int index, IndexDirectoryEntryMode mode = IndexDirectoryEntryMode.Normal)
        {
            var entryListsCount = entryLists.Count;

            if (index >= entryListsCount) {
                return Add(index, mode);
            }

            var newLastIndex = entryListsCount;
            var indexEntry = new IndexDirectoryEntry(index, mode);
            entryLists.Insert(index, new List<IndexDirectoryEntry>() { indexEntry });

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
        /// 
        /// </summary>
        /// <param name="indexEntry"></param>
        /// <returns></returns>
        public bool RemoveEntry(IndexDirectoryEntry indexEntry)
        {
            if (indexEntry.Index >= entryLists.Count) {
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

        public void Remove(int index)
        {
            var entryListsCount = entryLists.Count;

            if (index >= entryListsCount) {
                throw new ArgumentOutOfRangeException(nameof(index));
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
        }

        public void Move(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex) {
                return;
            }

            if (fromIndex < 0 || fromIndex >= entryLists.Count) {
                new ArgumentOutOfRangeException(nameof(fromIndex));
            }

            if (toIndex < 0 || toIndex >= entryLists.Count) {
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
        /// Removes all null and empty lists that are containing 
        /// the <see cref="IndexDirectoryEntry"/> at the end of
        /// this directory.
        /// </summary>
        public void TrimExcess()
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

        public static implicit operator int(IndexDirectoryEntry indexEntry) =>
            indexEntry.Index;
    }

    public enum IndexDirectoryEntryMode
    {
        Normal,
        /// <summary>
        /// Adding and removing other indexes affects own index as expected 
        /// but other moving indexes increases own index always by one only
        /// if they cross own index.
        /// </summary>
        Floating,
    }
}
