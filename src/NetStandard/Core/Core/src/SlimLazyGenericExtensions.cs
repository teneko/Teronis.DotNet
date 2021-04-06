// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Teronis
{
    public static class SlimLazyGenericExtensions
    {
        public static TaskAwaiter<T> GetAwaiter<T>(this SlimLazy<Task<T>> lazyTask) =>
             lazyTask.Value.GetAwaiter();

        public static TaskAwaiter GetAwaiter(this SlimLazy<Task> lazyTask) =>
             lazyTask.Value.GetAwaiter();

        public static ValueTaskAwaiter GetAwaiter(this SlimLazy<ValueTask> lazyTask) =>
             lazyTask.Value.GetAwaiter();

        public static ValueTaskAwaiter<T> GetAwaiter<T>(this SlimLazy<ValueTask<T>> lazyTask) =>
             lazyTask.Value.GetAwaiter();
    }
}
