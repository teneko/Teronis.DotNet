// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;
using Xunit;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SynchronizableCollectionTests
    {
        public class Sequential : TestSuite<Number>
        {
            public override EqualityComparer<Number> EqualityComparer =>
                Number.ReferenceEqualityComparer.Default;

            private static Number zero = 0;
            private static Number zero2 = 0;
            private static Number zero3 = 0;
            private static Number zero4 = 0;
            private static Number one = 1;
            private static Number one2 = 1;
            private static Number t_one = new Number(1) { CompareValueWhenComparingReference = true };
            private static Number t_one2 = new Number(1) { CompareValueWhenComparingReference = true };
            private static Number two = 2;
            private static Number three = 3;
            private static Number four = 3;
            private static Number four2 = 3;
            private static Number five = 5;
            private static Number six = 6;
            private static Number seven = 7;
            private static Number eight = 8;
            private static Number nine = 9;
            private static Number nine2 = 9;

            public Sequential() : base(
                new SynchronizableCollection<Number>(
                    new SynchronizableCollectionOptions<Number>()
                        .ConfigureItems(options => options
                            .SetItems(CollectionChangeHandler<Number>.CollectionItemReplaceBehaviour.Default))
                        .SetSequentialSynchronizationMethod(Number.ReferenceOrValueEqualityComparer.Default)))
            { }

            [Theory]
            [ClassData(typeof(Generator))]
            public override void Direct_synchronization_by_modifications(
                Number[] leftItems,
                Number[] rightItems,
                Number[]? expected = null,
                CollectionModificationsYieldCapabilities? yieldCapabilities = null) =>
                base.Direct_synchronization_by_modifications(leftItems, rightItems, expected, yieldCapabilities);

            [Theory]
            [ClassData(typeof(Generator))]
            public override void Direct_synchronization_by_batched_modifications(
                Number[] leftItems,
                Number[] rightItems,
                Number[]? expected = null,
                CollectionModificationsYieldCapabilities? yieldCapabilities = null) =>
                base.Direct_synchronization_by_batched_modifications(leftItems, rightItems, expected, yieldCapabilities);

            [Theory]
            [ClassData(typeof(Generator))]
            public override void Relocated_synchronization_by_modifications(
                Number[] leftItems,
                Number[] rightItems,
                Number[]? expected = null,
                CollectionModificationsYieldCapabilities? yieldCapabilities = null) =>
                base.Relocated_synchronization_by_modifications(leftItems, rightItems, expected, yieldCapabilities);

            [Theory]
            [ClassData(typeof(Generator))]
            public override void Relocated_synchronization_by_batched_modifications(
                Number[] leftItems,
                Number[] rightItems,
                Number[]? expected = null,
                CollectionModificationsYieldCapabilities? yieldCapabilities = null) =>
                base.Relocated_synchronization_by_batched_modifications(leftItems, rightItems, expected, yieldCapabilities);

            public class Generator : GeneratorBase<Number>
            {
                public override IEnumerator<object[]> GetEnumerator()
                {
                    yield return List(Values(nine), Values(nine));
                    yield return List(Values(nine, nine2), Values(1, nine));
                    yield return List(Values(nine, six), Values(six, nine));
                    yield return List(Values(four, four2, nine), Values(nine, nine2, four));
                    yield return List(Values(four, nine, five), Values(five, nine, four));
                    yield return List(Values(four, nine, five), Values(five, nine, four));
                    yield return List(Values(three, four, nine, five), Values(five, six, nine, four));
                    yield return List(Values(four, nine, five, six), Values(nine, four, three));
                    yield return List(Values(nine, six), Values(six, nine, four, five));
                    yield return List(Values(nine, zero, six), Values(six, nine));
                    yield return List(Values(nine, five, six), Values(six, nine, four));
                    yield return List(Values(nine, five, six), Values(six, nine, four, three));
                    yield return List(Values(four, nine, five, six), Values(six, nine, four, three));
                    yield return List(Values(five, four, three, two, one), Values(three, four, five, six, seven, eight, nine));
                    yield return List(Values(three, four, nine, five, six), Values(five, six, nine, three, four));
                    yield return List(Values(zero, zero2, zero3, zero4, one), Values(one, zero, zero2, zero3, one2));
                    yield return List(Values(zero, zero2, one), Values(one, zero, one2));

                    yield return List(Values(zero, zero2, one, zero3, one2), Values(one), Values(one), CollectionModificationsYieldCapabilities.Remove);
                    yield return List(Values(zero, zero2, one, zero3, one2), Values(one2), Values(one2), CollectionModificationsYieldCapabilities.Remove);

                    yield return List(Values(zero, t_one, t_one2, zero2), Values(zero, t_one2, t_one, zero2), Values(zero, t_one, t_one2, zero2), CollectionModificationsYieldCapabilities.InsertRemove);
                    yield return List(Values(zero, t_one, t_one2, zero2), Values(zero, t_one2, t_one, zero2), Values(zero, t_one2, t_one, zero2), CollectionModificationsYieldCapabilities.All);

                    yield return List(Values(five, three), Values(five, zero), Values(five, three, zero), CollectionModificationsYieldCapabilities.Insert);
                }
            }
        }
    }
}
