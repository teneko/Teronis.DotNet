using System;
using System.IO;

namespace Teronis.IO.FileLocking
{
    public interface ILockFileApi
    {
        FileStream? TryAcquire(string filePath, FileMode fileMode = LockFileApi.DefaultFileMode, FileAccess fileAccess = LockFileApi.DefaultFileAccess, FileShare fileShare = LockFileApi.DefaultFileShare);
        bool TryAcquire(string filePath, out FileStream? fileStream, FileMode fileMode = LockFileApi.DefaultFileMode, FileAccess fileAccess = LockFileApi.DefaultFileAccess, FileShare fileShare = LockFileApi.DefaultFileShare);
        FileStream? WaitUntilAcquired(string filePath, FileMode fileMode = LockFileApi.DefaultFileMode, FileAccess fileAccess = LockFileApi.DefaultFileAccess, FileShare fileShare = LockFileApi.DefaultFileShare, bool noThrowOnTimeout = false);
        FileStream? WaitUntilAcquired(string filePath, int timeoutInMilliseconds, FileMode fileMode = LockFileApi.DefaultFileMode, FileAccess fileAccess = LockFileApi.DefaultFileAccess, FileShare fileShare = LockFileApi.DefaultFileShare, bool noThrowOnTimeout = false);
        bool WaitUntilAcquired(string filePath, int timeoutInMilliseconds, out FileStream? fileStream, FileMode fileMode = LockFileApi.DefaultFileMode, FileAccess fileAccess = LockFileApi.DefaultFileAccess, FileShare fileShare = LockFileApi.DefaultFileShare, bool throwOnTimeout = false);
        bool WaitUntilAcquired(string filePath, out FileStream? fileStream, FileMode fileMode = LockFileApi.DefaultFileMode, FileAccess fileAccess = LockFileApi.DefaultFileAccess, FileShare fileShare = LockFileApi.DefaultFileShare, bool throwOnTimeout = false);
        FileStream? WaitUntilAcquired(string filePath, TimeSpan timeout, FileMode fileMode = LockFileApi.DefaultFileMode, FileAccess fileAccess = LockFileApi.DefaultFileAccess, FileShare fileShare = LockFileApi.DefaultFileShare, bool noThrowOnTimeout = false);
        bool WaitUntilAcquired(string filePath, TimeSpan timeout, out FileStream? fileStream, FileMode fileMode = LockFileApi.DefaultFileMode, FileAccess fileAccess = LockFileApi.DefaultFileAccess, FileShare fileShare = LockFileApi.DefaultFileShare, bool throwOnTimeout = false);
    }
}
