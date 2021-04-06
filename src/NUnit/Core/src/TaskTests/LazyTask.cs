// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// Class that inherits <see cref="SlimLazy{T}"/> where
    /// generic type is <see cref="Task"/>.
    /// </summary>
    public class LazyTask : SlimLazy<Task>
    {
        private readonly Func<CancellationToken, Task> taskProvider;

        /// <summary>
        /// Constructs the instance of <see cref="LazyTask"/>.
        /// </summary>
        /// <param name="taskProvider"></param>
        public LazyTask(Func<CancellationToken, Task> taskProvider)
            : base(() => taskProvider(CancellationToken.None)) =>
            this.taskProvider = taskProvider;

        /// <summary>
        /// Invokes the value factory with <paramref name="cancellationToken"/>
        /// if the value has not been created so far.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created task.</returns>
        public void SetValueIfNotCreated(CancellationToken cancellationToken) =>
            SetValueIfNotCreated(taskProvider(cancellationToken));

        public TaskAwaiter GetAwaiter() =>
            Value.GetAwaiter();
    }
}
