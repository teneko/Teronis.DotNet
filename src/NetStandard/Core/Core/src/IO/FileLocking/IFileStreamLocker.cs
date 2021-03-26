// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading;

namespace Teronis.IO.FileLocking
{
    public interface IFileStreamLocker
    {
        FileStream? TryAcquire(string filePath, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare);
        bool TryAcquire(string filePath, out FileStream? fileStream, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare);
        FileStream? WaitUntilAcquired(string filePath, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool noThrowOnTimeout = false, CancellationToken cancellationToken = default);
        FileStream? WaitUntilAcquired(string filePath, int timeoutInMilliseconds, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool noThrowOnTimeout = false, CancellationToken cancellationToken = default);
        bool WaitUntilAcquired(string filePath, int timeoutInMilliseconds, out FileStream? fileStream, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool throwOnTimeout = false, CancellationToken cancellationToken = default);
        bool WaitUntilAcquired(string filePath, out FileStream? fileStream, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool throwOnTimeout = false, CancellationToken cancellationToken = default);
        FileStream? WaitUntilAcquired(string filePath, TimeSpan timeout, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool noThrowOnTimeout = false, CancellationToken cancellationToken = default);
        bool WaitUntilAcquired(string filePath, TimeSpan timeout, out FileStream? fileStream, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool throwOnTimeout = false, CancellationToken cancellationToken = default);
    }
}
