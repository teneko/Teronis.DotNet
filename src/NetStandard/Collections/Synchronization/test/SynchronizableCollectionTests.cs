using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;
using Xunit;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SynchronizableCollectionTests
    {
        public abstract class TestSuite<T>
            where T : notnull
        {
            public SynchronizableCollection<T> Collection { get; }
            public abstract EqualityComparer<T> EqualityComparer { get; }

            protected TestSuite(SynchronizableCollection<T> collection) =>
                Collection = collection;

            public virtual void Direct_synchronization_by_modifications(
                T[] leftItems,
                T[] rightItems,
                T[]? expected = null,
                CollectionModificationsYieldCapabilities? yieldCapabilities = null)
            {
                expected = expected ?? rightItems;
                yieldCapabilities = yieldCapabilities ?? CollectionModificationsYieldCapabilities.All;

                Collection.SynchronizeCollection(leftItems);
                Assert.Equal(leftItems, Collection, EqualityComparer);

                Collection.SynchronizeCollection(rightItems, yieldCapabilities.Value);
                Assert.Equal(expected, Collection, EqualityComparer);
            }

            public virtual void Direct_synchronization_by_consumed_modifications(
                T[] leftItems,
                T[] rightItems,
                T[]? expected = null,
                CollectionModificationsYieldCapabilities? yieldCapabilities = null)
            {
                expected = expected ?? rightItems;
                yieldCapabilities = yieldCapabilities ?? CollectionModificationsYieldCapabilities.All;

                Collection.SynchronizeCollection(leftItems, consumeModifications: true);
                Assert.Equal(leftItems, Collection, EqualityComparer);

                Collection.SynchronizeCollection(rightItems, yieldCapabilities.Value, consumeModifications: true);
                Assert.Equal(expected, Collection, EqualityComparer);
            }

            private IEnumerable<T> ToEnumerable(IEnumerable<T> items)
            {
                foreach (var item in items) {
                    yield return item;
                }
            }

            public virtual void Relocated_synchronization_by_modifications(
                T[] leftItems,
                T[] rightItems,
                T[]? expected = null,
                CollectionModificationsYieldCapabilities? yieldCapabilities = null)
            {
                expected = expected ?? rightItems;
                yieldCapabilities = yieldCapabilities ?? CollectionModificationsYieldCapabilities.All;

                Collection.SynchronizeCollection(leftItems, consumeModifications: true);
                Assert.Equal(leftItems, Collection, EqualityComparer);

                var collection = ToEnumerable(Collection);
                Collection.SynchronizeCollection(collection, rightItems, yieldCapabilities.Value, consumeModifications: true);
                Assert.Equal(expected, Collection, EqualityComparer);
            }

            public virtual void Relocated_synchronization_by_consumed_modifications(
                T[] leftItems,
                T[] rightItems,
                T[]? expected = null,
                CollectionModificationsYieldCapabilities? yieldCapabilities = null)
            {
                expected = expected ?? rightItems;
                yieldCapabilities = yieldCapabilities ?? CollectionModificationsYieldCapabilities.All;

                Collection.SynchronizeCollection(leftItems, consumeModifications: true);
                Assert.Equal(leftItems, Collection, EqualityComparer);

                var collection = ToEnumerable(Collection);
                Collection.SynchronizeCollection(collection, rightItems, yieldCapabilities.Value, consumeModifications: true);
                Assert.Equal(expected, Collection, EqualityComparer);
            }
        }
    }
}
