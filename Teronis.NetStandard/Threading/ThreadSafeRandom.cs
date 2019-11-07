using System;
using System.Threading;

namespace Teronis.Threading
{
    public static class ThreadSafeRandom
    {
        private static readonly Random global;
        private static readonly ThreadLocal<Random> local;

        static ThreadSafeRandom()
        {
            global = new Random();

            local = new ThreadLocal<Random>(() => {
                int seed;

                lock (global)
                    seed = global.Next();

                return new Random(seed);
            });
        }

        public static int Next() => local.Value.Next();

        /// <summary>
        /// The <paramref name="maxValue"/> for the upper-bound in the <see cref="Next"/> method is exclusive - the range includes <paramref name="maxValue"/>-1.
        /// </summary>
        public static int Next(int maxValue) => local.Value.Next(maxValue);

        /// <summary>
        /// The <paramref name="maxValue"/> for the upper-bound in the <see cref="Next"/> method is exclusive - the range includes <paramref name="minValue"/>, 
        /// <paramref name="maxValue"/>-1 and all number in between.
        /// </summary>
        public static int Next(int minValue, int maxValue) => local.Value.Next(minValue, maxValue);
    }
}
