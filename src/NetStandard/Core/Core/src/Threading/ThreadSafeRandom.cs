// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace Teronis.Threading
{
    public static class ThreadSafeRandom
    {
        private static readonly Random global;
        private static readonly ThreadLocal<Random> local;
        private static Random localRandom => local.Value!;

        static ThreadSafeRandom()
        {
            global = new Random();

            local = new ThreadLocal<Random>(() => {
                int seed;

                lock (global) {
                    seed = global.Next();
                }

                return new Random(seed);
            });
        }

        public static int Next() => localRandom.Next();

        /// <summary>
        /// The <paramref name="maxValue"/> for the upper-bound in the <see cref="Next"/> method is exclusive - the range includes <paramref name="maxValue"/>-1.
        /// </summary>
        public static int Next(int maxValue) => localRandom.Next(maxValue);

        /// <summary>
        /// The <paramref name="maxValue"/> for the upper-bound in the <see cref="Next"/> method is exclusive - the range includes <paramref name="minValue"/>, 
        /// <paramref name="maxValue"/>-1 and all number in between.
        /// </summary>
        public static int Next(int minValue, int maxValue) =>
            localRandom.Next(minValue, maxValue);
    }
}
