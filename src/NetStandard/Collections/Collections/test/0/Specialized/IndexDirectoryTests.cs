// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Xunit;

namespace Teronis.Collections.Specialized
{
    public class IndexDirectoryTests : IDisposable
    {
        IndexDirectory indexDirectory;

        public IndexDirectoryTests() =>
            indexDirectory = new IndexDirectory();

        public void Dispose() =>
            indexDirectory.Clear();

        [Fact]
        public void Add()
        {
            var zero = indexDirectory.Add(0);
            var zero2 = indexDirectory.Add(0);
            Assert.Equal(1, indexDirectory.Count);
            Assert.Equal(0, zero);
            Assert.Equal(0, zero2);

            var one = indexDirectory.Add(1);
            Assert.Equal(2, indexDirectory.Count);
            Assert.Equal(1, one);
        }

        [Fact]
        public void Remove()
        {
            var zero = indexDirectory.Add(0);
            var zero2 = indexDirectory.Add(0);
            var two = indexDirectory.Add(2);

            indexDirectory.RemoveEntry(zero2);
            Assert.Equal(3, indexDirectory.Count);
            Assert.Equal(0, zero);
            Assert.Equal(2, two);

            indexDirectory.RemoveEntry(zero);
            Assert.Equal(2, indexDirectory.Count);
            Assert.Equal(0, zero);
            Assert.Equal(1, two);

            indexDirectory.Remove(0);
            Assert.Equal(1, indexDirectory.Count);
            Assert.Equal(0, two);
        }

        [Fact]
        public void Remove_last_from_entries_list()
        {
            var _ = indexDirectory.Add(5);
            indexDirectory.Remove(5);
            Assert.Equal(5, indexDirectory.Count);
        }

        [Fact]
        public void Remove_last_from_virtualized_length()
        {
            indexDirectory.Expand(5);
            indexDirectory.Remove(5);
            Assert.Equal(5, indexDirectory.Count);
        }

        [Fact]
        public void Remove_in_between_entries_list_and_virtualized_length()
        {
            var _ = indexDirectory.Add(5);
            indexDirectory.Expand(10);
            Assert.Equal(11, indexDirectory.Count);

            indexDirectory.Remove(5, 2);
            Assert.Equal(9, indexDirectory.Count);
        }

        [Fact]
        public void Remove_in_between_entries_list_length()
        {
            var _ = indexDirectory.Add(5);
            indexDirectory.Expand(10);
            Assert.Equal(11, indexDirectory.Count);

            indexDirectory.Remove(4, 2);
            Assert.Equal(9, indexDirectory.Count);
        }

        [Fact]
        public void Remove_in_between_virtualized_length()
        {
            var _ = indexDirectory.Add(5);
            indexDirectory.Expand(10);
            Assert.Equal(11, indexDirectory.Count);

            indexDirectory.Remove(6, 2);
            Assert.Equal(9, indexDirectory.Count);
        }

        [Fact]
        public void Remove_range() {
            _ = indexDirectory.Add(5);
            _ = indexDirectory.Add(7);

            indexDirectory.Remove(5, 3);

            Assert.Equal(5, indexDirectory.Count);
        }

        [Fact]
        public void Insert()
        {
            var one = indexDirectory.Add(1);
            var two = indexDirectory.Add(2);

            // Insert between.
            var newOne = indexDirectory.Insert(1);
            Assert.Equal(4, indexDirectory.Count);
            Assert.Equal(1, newOne);
            Assert.Equal(2, one);
            Assert.Equal(3, two);

            // Insert at the end.
            var four = indexDirectory.Insert(4);
            Assert.Equal(5, indexDirectory.Count);
            Assert.Equal(3, two);
            Assert.Equal(4, four);
        }

        [Fact]
        public void Move_index_forward_with_floating_indexes()
        {
            var zero = indexDirectory.Add(0);
            var floatingZero = indexDirectory.Add(0, IndexDirectoryEntryMode.Floating);
            var one = indexDirectory.Add(1);
            var floatingOne = indexDirectory.Add(1, IndexDirectoryEntryMode.Floating);
            var two = indexDirectory.Add(2);
            var floatingTwo = indexDirectory.Add(2, IndexDirectoryEntryMode.Floating);
            var three = indexDirectory.Add(3);
            var floatingThree = indexDirectory.Add(3, IndexDirectoryEntryMode.Floating);

            indexDirectory.Move(0, 3);
            Assert.Equal(3, zero);
            Assert.Equal(3, floatingZero);
            Assert.Equal(0, one);
            Assert.Equal(1, floatingOne);
            Assert.Equal(1, two);
            Assert.Equal(2, floatingTwo);
            Assert.Equal(2, three);
            Assert.Equal(3, floatingThree);

            indexDirectory.Move(3, 0);
            Assert.Equal(0, zero);
            Assert.Equal(3, floatingZero);
            Assert.Equal(1, one);
            Assert.Equal(2, floatingOne);
            Assert.Equal(2, two);
            Assert.Equal(3, floatingTwo);
            Assert.Equal(3, three);
            Assert.Equal(3, floatingThree);
        }

        [Fact]
        public void Move_two_indexes_forward_with_floating_indexes()
        {
            var zero = indexDirectory.Add(0);
            var floatingZero = indexDirectory.Add(0, IndexDirectoryEntryMode.Floating);
            var one = indexDirectory.Add(1);
            var floatingOne = indexDirectory.Add(1, IndexDirectoryEntryMode.Floating);
            var two = indexDirectory.Add(2);
            var floatingTwo = indexDirectory.Add(2, IndexDirectoryEntryMode.Floating);
            var three = indexDirectory.Add(3);
            var floatingThree = indexDirectory.Add(3, IndexDirectoryEntryMode.Floating);
            var four = indexDirectory.Add(4);
            var floatingFour = indexDirectory.Add(4, IndexDirectoryEntryMode.Floating);

            indexDirectory.Move(0, 3, 2);
            Assert.Equal(3, zero);
            Assert.Equal(3, floatingZero);
            Assert.Equal(4, one);
            Assert.Equal(4, floatingOne);
            Assert.Equal(0, two);
            Assert.Equal(2, floatingTwo);
            Assert.Equal(1, three);
            Assert.Equal(3, floatingThree);
            Assert.Equal(2, four);
            Assert.Equal(4, floatingFour);
        }

        [Fact]
        public void Move_more_indexes_forth_and_back_than_skipping_indexes()
        {
            var zero = indexDirectory.Add(0);
            var floatingZero = indexDirectory.Add(0, IndexDirectoryEntryMode.Floating);
            var one = indexDirectory.Add(1);
            var floatingOne = indexDirectory.Add(1, IndexDirectoryEntryMode.Floating);
            var two = indexDirectory.Add(2);
            var floatingTwo = indexDirectory.Add(2, IndexDirectoryEntryMode.Floating);
            var three = indexDirectory.Add(3);
            var floatingThree = indexDirectory.Add(3, IndexDirectoryEntryMode.Floating);
            var four = indexDirectory.Add(4);
            var floatingFour = indexDirectory.Add(4, IndexDirectoryEntryMode.Floating);

            indexDirectory.Move(0, 1, 4);
            Assert.Equal(1, zero);
            Assert.Equal(1, floatingZero);
            Assert.Equal(2, one);
            Assert.Equal(2, floatingOne);
            Assert.Equal(3, two);
            Assert.Equal(3, floatingTwo);
            Assert.Equal(4, three);
            Assert.Equal(4, floatingThree);
            Assert.Equal(0, four);
            Assert.Equal(4, floatingFour);

            indexDirectory.Move(1, 0, 4);
            Assert.Equal(0, zero);
            Assert.Equal(1, floatingZero);
            Assert.Equal(1, one);
            Assert.Equal(2, floatingOne);
            Assert.Equal(2, two);
            Assert.Equal(3, floatingTwo);
            Assert.Equal(3, three);
            Assert.Equal(4, floatingThree);
            Assert.Equal(4, four);
            Assert.Equal(4, floatingFour);
        }

        [Fact]
        public void Move_index_backward_with_floating_indexes()
        {
            var zero = indexDirectory.Add(0);
            var floatingZero = indexDirectory.Add(0, IndexDirectoryEntryMode.Floating);
            var one = indexDirectory.Add(1);
            var floatingOne = indexDirectory.Add(1, IndexDirectoryEntryMode.Floating);
            var two = indexDirectory.Add(2);
            var floatingTwo = indexDirectory.Add(2, IndexDirectoryEntryMode.Floating);
            var three = indexDirectory.Add(3);
            var floatingThree = indexDirectory.Add(3, IndexDirectoryEntryMode.Floating);

            indexDirectory.Move(3, 0);
            Assert.Equal(1, zero);
            Assert.Equal(1, floatingZero);
            Assert.Equal(2, one);
            Assert.Equal(2, floatingOne);
            Assert.Equal(3, two);
            Assert.Equal(3, floatingTwo);
            Assert.Equal(0, three);
            Assert.Equal(3, floatingThree);

            indexDirectory.Move(0, 3);
            Assert.Equal(0, zero);
            Assert.Equal(1, floatingZero);
            Assert.Equal(1, one);
            Assert.Equal(2, floatingOne);
            Assert.Equal(2, two);
            Assert.Equal(3, floatingTwo);
            Assert.Equal(3, three);
            Assert.Equal(3, floatingThree);
        }

        [Fact]
        public void Move_two_indexes_backward_with_floating_indexes()
        {
            var zero = indexDirectory.Add(0);
            var floatingZero = indexDirectory.Add(0, IndexDirectoryEntryMode.Floating);
            var one = indexDirectory.Add(1);
            var floatingOne = indexDirectory.Add(1, IndexDirectoryEntryMode.Floating);
            var two = indexDirectory.Add(2);
            var floatingTwo = indexDirectory.Add(2, IndexDirectoryEntryMode.Floating);
            var three = indexDirectory.Add(3);
            var floatingThree = indexDirectory.Add(3, IndexDirectoryEntryMode.Floating);
            var four = indexDirectory.Add(4);
            var floatingFour = indexDirectory.Add(4, IndexDirectoryEntryMode.Floating);

            indexDirectory.Move(3, 0, 2);
            Assert.Equal(2, zero);
            Assert.Equal(2, floatingZero);
            Assert.Equal(3, one);
            Assert.Equal(3, floatingOne);
            Assert.Equal(4, two);
            Assert.Equal(4, floatingTwo);
            Assert.Equal(0, three);
            Assert.Equal(3, floatingThree);
            Assert.Equal(1, four);
            Assert.Equal(4, floatingFour);
        }

        [Fact]
        public void Move_more_indexes_back_and_forth_than_skipping_indexes()
        {
            var zero = indexDirectory.Add(0);
            var floatingZero = indexDirectory.Add(0, IndexDirectoryEntryMode.Floating);
            var one = indexDirectory.Add(1);
            var floatingOne = indexDirectory.Add(1, IndexDirectoryEntryMode.Floating);
            var two = indexDirectory.Add(2);
            var floatingTwo = indexDirectory.Add(2, IndexDirectoryEntryMode.Floating);
            var three = indexDirectory.Add(3);
            var floatingThree = indexDirectory.Add(3, IndexDirectoryEntryMode.Floating);
            var four = indexDirectory.Add(4);
            var floatingFour = indexDirectory.Add(4, IndexDirectoryEntryMode.Floating);

            indexDirectory.Move(1, 0, 4);
            Assert.Equal(4, zero);
            Assert.Equal(4, floatingZero);
            Assert.Equal(0, one);
            Assert.Equal(1, floatingOne);
            Assert.Equal(1, two);
            Assert.Equal(2, floatingTwo);
            Assert.Equal(2, three);
            Assert.Equal(3, floatingThree);
            Assert.Equal(3, four);
            Assert.Equal(4, floatingFour);

            indexDirectory.Move(0, 1, 4);
            Assert.Equal(0, zero);
            Assert.Equal(4, floatingZero);
            Assert.Equal(1, one);
            Assert.Equal(2, floatingOne);
            Assert.Equal(2, two);
            Assert.Equal(3, floatingTwo);
            Assert.Equal(3, three);
            Assert.Equal(4, floatingThree);
            Assert.Equal(4, four);
            Assert.Equal(4, floatingFour);
        }

        [Fact]
        public void Replace_index_of_entries()
        {
            var zero = indexDirectory.Add(0);
            var floatingZero = indexDirectory.Add(0, IndexDirectoryEntryMode.Floating);
            indexDirectory.Expand(3);

            indexDirectory.ReplaceEntry(zero, 3);
            Assert.Equal(3, zero);

            indexDirectory.ReplaceEntry(floatingZero, 3);
            Assert.Equal(3, floatingZero);
        }

        [Fact]
        public void Expand_and_trim_exceed()
        {
            indexDirectory.Expand(10);
            Assert.Equal(11, indexDirectory.Count);

            indexDirectory.TrimEnd();
            Assert.Equal(0, indexDirectory.Count);
        }
    }
}
