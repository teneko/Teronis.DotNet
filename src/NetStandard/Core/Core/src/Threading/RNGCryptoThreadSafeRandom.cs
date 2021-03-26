// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;
using System.Threading;

namespace Teronis.Threading
{
    public static class RNGCryptoThreadSafeRandom
    {
        private static readonly RNGCryptoServiceProvider global;
        private static readonly ThreadLocal<Random> local;

        static RNGCryptoThreadSafeRandom()
        {
            global = new RNGCryptoServiceProvider();

            local = new ThreadLocal<Random>(() => {
                byte[] buffer = new byte[4];
                global.GetBytes(buffer);
                return new Random(BitConverter.ToInt32(buffer, 0));
            });
        }

        public static int Next() => local.Value!.Next();
        public static int Next(int maxValue) => local.Value!.Next(maxValue);
        public static int Next(int minValue, int maxValue) => local.Value!.Next(minValue, maxValue);
    }
}
