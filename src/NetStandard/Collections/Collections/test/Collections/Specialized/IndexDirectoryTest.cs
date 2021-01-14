using System;
using Xunit;

namespace Teronis.Collections.Specialized
{
    public class IndexDirectoryTest : IDisposable
    {
        IndexDirectory indexDirectory;

        public IndexDirectoryTest() =>
            indexDirectory = new IndexDirectory();

        public void Dispose() =>
            indexDirectory.Clear();

        [Fact]
        public void Add()
        {
            var zero = indexDirectory.Add(0);
            var zero2 = indexDirectory.Add(0);
            Assert.Equal(0, zero);
            Assert.Equal(0, zero2);

            var one = indexDirectory.Add(1);
            Assert.Equal(1, one);
        }

        [Fact]
        public void Remove()
        {
            var zero = indexDirectory.Add(0);
            var zero2 = indexDirectory.Add(0);
            var two = indexDirectory.Add(2);

            indexDirectory.Remove(zero2);
            Assert.Equal(0, zero);
            Assert.Equal(2, two);

            indexDirectory.Remove(zero);
            Assert.Equal(0, zero);
            Assert.Equal(2, two);

            indexDirectory.Remove(0);
            Assert.Equal(1, two);
        }

        [Fact]
        public void Move_back_and_forth()
        {
            var two = indexDirectory.Add(2);
            var three = indexDirectory.Add(3);

            indexDirectory.Move(3, 0);
            Assert.Equal(0, three);
            Assert.Equal(3, two);

            indexDirectory.Move(0, 3);
            Assert.Equal(3, three);
            Assert.Equal(2, two);
        }

        [Fact]
        public void Insert()
        {
            var zero = indexDirectory.Add(0);
            var one = indexDirectory.Add(1);

            var newOne = indexDirectory.Insert(1);
            Assert.Equal(0, zero);
            Assert.Equal(1, newOne);
            Assert.Equal(2, one);
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
        }

        [Fact]
        public void Replace_index_of_entries()
        {
            var zero = indexDirectory.Add(0);
            var floatingZero = indexDirectory.Add(0, IndexDirectoryEntryMode.Floating);
            indexDirectory.Expand(3);

            indexDirectory.Replace(zero, 3);
            Assert.Equal(3, zero);

            indexDirectory.Replace(floatingZero, 3);
            Assert.Equal(3, floatingZero);
        }

        [Fact]
        public void Expand_and_trim_exceed()
        {
            indexDirectory.Expand(10);
            Assert.Equal(11, indexDirectory.Count);

            indexDirectory.TrimExcess();
            Assert.Equal(0, indexDirectory.Count);
        }
    }
}
