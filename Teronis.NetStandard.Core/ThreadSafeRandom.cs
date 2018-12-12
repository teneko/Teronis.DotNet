using System;
using System.Threading;

namespace Teronis.NetStandard
{
    public static class ThreadSafeRandom
    {
        private static readonly Random global;
        private static readonly ThreadLocal<Random> local;

        static ThreadSafeRandom()
        {
            global = new Random();

            local = new ThreadLocal<Random>(() =>
            {
                int seed;

                lock (global)
                    seed = global.Next();

                return new Random(seed);
            });
        }

        public static int Next() => local.Value.Next();
        public static int Next(int maxValue) => local.Value.Next(maxValue);
        public static int Next(int minValue, int maxValue) => local.Value.Next(minValue, maxValue);
    }
}
