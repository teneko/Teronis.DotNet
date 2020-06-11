using System;
using System.IO;
using System.Threading;

namespace Teronis.IO
{
    public static class LockFile
    {
        public const FileMode DefaultFileMode = FileMode.OpenOrCreate;
        public const FileAccess DefaultFileAccess = FileAccess.ReadWrite;
        public const FileShare DefaultFileShare = FileShare.None;

        public static bool TryAcquire(string filePath, out FileStream? fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

            try {
                fileStream = File.Open(filePath, fileMode, fileAccess, fileShare);
                return true;
            } catch (Exception error) when (error.GetType() == typeof(IOException)) {
                fileStream = null;
                return false;
            }
        }

        public static FileStream? TryAcquire(string filePath, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            TryAcquire(filePath, out var fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare);

            return fileStream;
        }

        public static bool WaitUntilAcquired(string filePath, out FileStream? fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare, int timeoutInMilliseconds = Timeout.Infinite)
        {
            FileStream spinningFileStream = null;

            var spinHasBeenFinished = SpinWait.SpinUntil(() => {
                return TryAcquire(filePath, out spinningFileStream, fileMode: fileMode, fileAccess: fileAccess, fileShare: fileShare);
            }, timeoutInMilliseconds);

            if (spinHasBeenFinished) {
                fileStream = spinningFileStream ?? throw new ArgumentNullException(nameof(spinningFileStream));
                return true;
            } else {
                fileStream = null;
                return false;
            }
        }

        public static FileStream? WaitUntilAcquired(string filePath, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare, int timeoutInMilliseconds = Timeout.Infinite)
        {
            WaitUntilAcquired(filePath, out var fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare, timeoutInMilliseconds: timeoutInMilliseconds);

            return fileStream;
        }

        public static bool WaitUntilAcquired(string filePath, int timeoutInMilliseconds, out FileStream? fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            return WaitUntilAcquired(filePath, out fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare, timeoutInMilliseconds: timeoutInMilliseconds);
        }

        public static FileStream? WaitUntilAcquired(string filePath, int timeoutInMilliseconds, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            WaitUntilAcquired(filePath, timeoutInMilliseconds, out var fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare);

            return fileStream;
        }

        public static bool WaitUntilAcquired(string filePath, TimeSpan timeout, out FileStream? fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            var timeoutInMilliseconds = Convert.ToInt32(timeout.TotalMilliseconds);

            return WaitUntilAcquired(filePath, timeoutInMilliseconds, out fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare);
        }

        public static FileStream? WaitUntilAcquired(string filePath, TimeSpan timeout, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            WaitUntilAcquired(filePath, timeout, out var fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare);

            return fileStream;
        }
    }
}
