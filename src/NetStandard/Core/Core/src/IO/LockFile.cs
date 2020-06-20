using System;
using System.IO;
using System.Threading;

namespace Teronis.IO
{
    /// <summary>
    /// This helper class can lock files.
    /// </summary>
    public static class LockFile
    {
        public const FileMode DefaultFileMode = FileMode.OpenOrCreate;
        public const FileAccess DefaultFileAccess = FileAccess.ReadWrite;
        public const FileShare DefaultFileShare = FileShare.None;

        /// <summary>
        /// Try to acquire lock on file but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that get locked.</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If true the acquire was successful.</returns>
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

        /// <summary>
        /// Try to acquire lock on file but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that get locked.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If not null the acquire was successful.</returns>
        public static FileStream? TryAcquire(string filePath, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            TryAcquire(filePath, out var fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare);

            return fileStream;
        }

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that get locked.</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If true the acquire was successful.</returns>
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

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that get locked.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If not null the acquire was successful.</returns>
        public static FileStream? WaitUntilAcquired(string filePath, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare, int timeoutInMilliseconds = Timeout.Infinite)
        {
            WaitUntilAcquired(filePath, out var fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare, timeoutInMilliseconds: timeoutInMilliseconds);

            return fileStream;
        }

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that get locked.</param>
        /// <param name="timeoutInMilliseconds">The timeout in milliseconds.</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If true the acquire was successful.</returns>
        public static bool WaitUntilAcquired(string filePath, int timeoutInMilliseconds, out FileStream? fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            return WaitUntilAcquired(filePath, out fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare, timeoutInMilliseconds: timeoutInMilliseconds);
        }

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that get locked.</param>
        /// <param name="timeoutInMilliseconds">The timeout in milliseconds.</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If not null the acquire was successful.</returns>
        public static FileStream? WaitUntilAcquired(string filePath, int timeoutInMilliseconds, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            WaitUntilAcquired(filePath, timeoutInMilliseconds, out var fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare);

            return fileStream;
        }

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that get locked.</param>
        /// <param name="timeout">The timeout specified as <see cref="TimeSpan"/>.</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If true the lock acquirement was successful.</returns>
        public static bool WaitUntilAcquired(string filePath, TimeSpan timeout, out FileStream? fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            var timeoutInMilliseconds = Convert.ToInt32(timeout.TotalMilliseconds);

            return WaitUntilAcquired(filePath, timeoutInMilliseconds, out fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare);
        }

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that get locked.</param>
        /// <param name="timeout">The timeout specified as <see cref="TimeSpan"/>.</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If ont null lock acquirement was successful.</returns>
        public static FileStream? WaitUntilAcquired(string filePath, TimeSpan timeout, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            WaitUntilAcquired(filePath, timeout, out var fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare);

            return fileStream;
        }
    }
}
