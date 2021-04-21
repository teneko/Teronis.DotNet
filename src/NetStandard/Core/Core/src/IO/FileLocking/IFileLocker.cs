// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

namespace Teronis.IO.FileLocking
{
    public interface IFileLocker
    {
        /// <summary>
        /// Waits until file lock is acquired.
        /// </summary>
        /// <returns></returns>
        FileLockUse WaitUntilAcquired(CancellationToken cancellationToken = default);

        /// <summary>
        /// Tries once to acquire file lock.
        /// </summary>
        /// <returns></returns>
        FileLockUse TryAcquire();
    }
}
