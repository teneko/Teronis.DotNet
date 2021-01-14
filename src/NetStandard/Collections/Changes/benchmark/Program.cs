using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Teronis.Collections.Changes;

namespace Teronis.NetStandard.Collections.Changes.Benchmark
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
            var runs = new int[] { 1000 };
            var random = new Random();

            foreach (var run in runs) {
                var leftItems = new List<int>(run);
                var rightItems = new List<int>(run);

                for (var index = 0; index < run; index++) {
                    leftItems.Add(random.Next(index * 10, (index * 10) + 10));
                    rightItems.Add(random.Next(index * 10, (index * 10) + 10));
                }

                yield return new object[] { new DataSource(leftItems), new DataSource(rightItems) };
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public List<CollectionModification<int, int>> YieldCollectionModificationsOld(DataSource leftItems, DataSource rightItems) =>
            CollectionModificationsObsolete.YieldCollectionModifications(leftItems.Items, rightItems.Items).ToList();

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public List<CollectionModification<int, int>> YieldCollectionModificationNew(DataSource leftItems, DataSource rightItems) =>
            CollectionModifications.YieldCollectionModifications(leftItems.Items, rightItems.Items).ToList();

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
