// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NET5_0
using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource;
#endif

namespace Teronis.Threading.Tasks
{
    /// <summary>
    /// This class can coordinate the invocation order of async events.
    /// <para/>
    /// In begin of the event method you may use one of the following methods:
    /// [<see cref="RegisterDependency(KeyType)"/> or <see cref="RegisterDependency(KeyType, out TaskCompletionSource)"/>], and [<see cref="TryAwaitDependency(KeyType[])"/>].
    /// <para/>
    /// After the event handler invocation:
    /// [<see cref="FinishDependenciesAsync"/>].
    /// </summary>
    /// <typeparam name="KeyType"></typeparam>
    public class AsyncEventSequence : AsyncEventSequence<Singleton>
    {
        public TaskCompletionSource RegisterDependency()
            => RegisterDependency(Singleton.Default);
    }
}
