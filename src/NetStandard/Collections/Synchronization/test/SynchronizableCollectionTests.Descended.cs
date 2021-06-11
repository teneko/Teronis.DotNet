// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Teronis.Collections.Algorithms.Modifications;
using Xunit;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SynchronizableCollectionTests
    {
        public class Descended : TestSuite<Number>
        {
            public override EqualityComparer<Number> EqualityComparer =>
                Number.ReferenceEqualityComparer.Default;

            public Descended() : base(
                new SynchronizableCollection<Number>(
                    new SynchronizableCollectionOptions<Number>()
                    .ConfigureItems(options => options
                        .SetItems(CollectionChangeHandler<Number>.CollectionItemReplaceBehaviour.Default))
                    .SetSortedSynchronizationMethod(Number.Comparer.Default, descended: true)))
            { }

            [Theory]
            [ClassData(typeof(DescendedGenerator))]
            public override void Direct_synchronization_by_modifications(Number[] leftItems, Number[] rightItems, Number[]? expected = null,
                CollectionModificationsYieldCapabilities? yieldCapabilities = null) =>
                base.Direct_synchronization_by_modifications(leftItems, rightItems, expected, yieldCapabilities);

            //[Theory]
            //[ClassData(typeof(DescendedGenerator))]
            //public override void Direct_synchronization_by_consumed_modifications(Number[] leftItems, Number[] rightItems, Number[]? expected = null,
            //    CollectionModificationsYieldCapabilities? yieldCapabilities = null) =>
            //    base.Direct_synchronization_by_consumed_modifications(leftItems, rightItems, expected, yieldCapabilities);

            //[Theory]
            //[ClassData(typeof(DescendedGenerator))]
            //public override void Relocated_synchronization_by_modifications(Number[] leftItems, Number[] rightItems, Number[]? expected = null,
            //    CollectionModificationsYieldCapabilities? yieldCapabilities = null) =>
            //    base.Relocated_synchronization_by_modifications(leftItems, rightItems, expected, yieldCapabilities);

            //[Theory]
            //[ClassData(typeof(DescendedGenerator))]
            //public override void Relocated_synchronization_by_consumed_modifications(Number[] leftItems, Number[] rightItems, Number[]? expected = null,
            //    CollectionModificationsYieldCapabilities? yieldCapabilities = null) =>
            //    base.Relocated_synchronization_by_consumed_modifications(leftItems, rightItems, expected, yieldCapabilities);

            public class DescendedGenerator : Ascended.Generator
            {
                protected override Number[] Values(params Number[] values) =>
                    values.Reverse().ToArray();
            }
        }
    }
}
