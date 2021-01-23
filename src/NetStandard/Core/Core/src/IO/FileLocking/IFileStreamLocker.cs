using System;
using System.IO;

namespace Teronis.IO.FileLocking
{
    public interface IFileStreamLocker
    {
        FileStream? TryAcquire(string filePath, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare);
        bool TryAcquire(string filePath, out FileStream? fileStream, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare);
        FileStream? WaitUntilAcquired(string filePath, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool noThrowOnTimeout = false);
        FileStream? WaitUntilAcquired(string filePath, int timeoutInMilliseconds, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool noThrowOnTimeout = false);
        bool WaitUntilAcquired(string filePath, int timeoutInMilliseconds, out FileStream? fileStream, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool throwOnTimeout = false);
        bool WaitUntilAcquired(string filePath, out FileStream? fileStream, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool throwOnTimeout = false);
        FileStream? WaitUntilAcquired(string filePath, TimeSpan timeout, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool noThrowOnTimeout = false);
        bool WaitUntilAcquired(string filePath, TimeSpan timeout, out FileStream? fileStream, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess, FileShare fileShare = FileStreamLocker.DefaultFileShare, bool throwOnTimeout = false);
    }
}
