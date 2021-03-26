// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Threading.Tasks;

namespace Teronis.Extensions
{
    public static class IHasAsyncableEventSequenceExtensions
    {
        public static bool IsAsyncEvent<KeyType>(this IHasAsyncableEventSequence<KeyType> asyncableEventSequenceContainer)
            where KeyType : notnull
            => !(asyncableEventSequenceContainer.AsyncEventSequence is null);
    }
}
