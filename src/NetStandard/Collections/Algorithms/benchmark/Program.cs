// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.NetStandard.Collections.Algorithms.Benchmark
{
    public class Program
    {
        static void Main()
        {
            var _ = BenchmarkRunner.Run<CollectionModificationsBenchmark>();
            Console.ReadKey();
        }
    }

    [CsvMeasurementsExporter]
    [RPlotExporter, MemoryDiagnoser, MedianColumn]
    public class CollectionModificationsBenchmark
    {
        public IEnumerable<object[]> Data()
        {
            var runs = new int[] { 500 };
            var random = new Random();
            var exclusiveMax = 2;

            foreach (var run in runs) {
                var leftItems = new List<int>(run);
                var rightItems = new List<int>(run);

                for (var index = 0; index < run; index++) {
                    leftItems.Add(random.Next(index * 10, (index * 10) + exclusiveMax));
                    rightItems.Add(random.Next(index * 10, (index * 10) + exclusiveMax));
                }

                yield return new object[] { new DataSource(leftItems), new DataSource(rightItems) };
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        [Obsolete]
        public List<CollectionModification<int, int>> YieldCollectionModificationsOld(DataSource leftItems, DataSource rightItems) =>
            AfterDeviationDeferredCollectionModifications.YieldCollectionModifications(leftItems.Items, rightItems.Items).ToList();

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public List<CollectionModification<int, int>> YieldCollectionModificationNew(DataSource leftItems, DataSource rightItems) =>
            EqualityTrailingCollectionModifications.YieldCollectionModifications(leftItems.Items, rightItems.Items).ToList();

        public class DataSource
        {
            public DataSource(List<int> items) =>
                Items = items;

            public List<int> Items { get; }

            public override string ToString() =>
                $"{Items.Count}";
        }
    }
}
