// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Teronis.Collections.Generic
{
    public static class IOrderedKeysProviderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>The amount of keys.</returns>
        public static int ThrowWhenNoSuccessivKey(this IOrderedKeysProvider provider)
        {
            var orderedKeys = provider.GetOrderedKeys();
            int? foundLastNonNullKeyIndex = null;

            for (var index = orderedKeys.Count - 1; index >= 0; index--) {
                if (foundLastNonNullKeyIndex == null) {
                    if (orderedKeys[index] != null) {
                        foundLastNonNullKeyIndex = index;
                    }

                    continue;
                }

                if (orderedKeys[index] == null) {
                    throw new InvalidOperationException("Last key cannot be dependent on null key.");
                }
            }

            if (foundLastNonNullKeyIndex.HasValue) {
                return foundLastNonNullKeyIndex.Value + 1;
            }

            return 0;
        }

        public static string ToDependencyPath(this IOrderedKeysProvider provider)
        {
            var orderedKeys = provider.GetOrderedKeys();
            var keysLength = provider.KeysLength;
            var stringBuilder = new StringBuilder("<root>");

            for (var index = 0; index < keysLength; index++) {
                var key = orderedKeys[index];
                stringBuilder.Append(".");
                stringBuilder.Append(key?.ToString() ?? "<null>");
            }

            return stringBuilder.ToString();
        }
    }
}
