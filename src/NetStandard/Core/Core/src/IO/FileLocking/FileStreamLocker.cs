// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace Teronis.IO.FileLocking
{
    /// <summary>
    /// This Windows and Linux compliant API offers to lock files via <see cref="FileStream"/>.
    /// </summary>
    public readonly struct FileStreamLocker : IFileStreamLocker
    {
        public static FileStreamLocker Default = new FileStreamLocker(new LockFileSystem());

        public const FileMode DefaultFileMode = FileMode.OpenOrCreate;
        public const FileAccess DefaultFileAccess = FileAccess.ReadWrite;
        public const FileShare DefaultFileShare = FileShare.None;
        public const int DefaultTimeoutInMilliseconds = Timeout.Infinite;

        private readonly ILockFileSystem fileSystem;

        public FileStreamLocker(ILockFileSystem fileSystem) =>
            this.fileSystem = fileSystem;

        /// <summary>
        /// Try to acquire lock on file but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that is tried to get locked..</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If true the lock acquirement was successful.</returns>
        public bool TryAcquire(string filePath, [MaybeNullWhen(false)] out FileStream fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

            try {
                fileStream = fileSystem.Open(filePath, fileMode, fileAccess, fileShare);
                // Add UNIX support (reference https://github.com/dotnet/coreclr/pull/8233).
                fileStream.Lock(0, 0);
                return true;
            }
            // The IOException does specify that the file could not been accessed because 
            // it was partially locked. All other exception have to be handled by consumer.
            // 
            // See references:
            // https://docs.microsoft.com/en-US/dotnet/api/system.io.file.open?view=netcore-3.1 (exceptions)
            // https://docs.microsoft.com/en-US/dotnet/api/system.io.filestream.lock?view=netcore-3.1#exceptions
            catch (Exception error) when (error.GetType() == typeof(IOException)) {
                fileStream = null;
                return false;
            }
        }

        /// <summary>
        /// Try to acquire lock on file but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that is tried to get locked..</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <returns>If not null the lock acquirement was successful.</returns>
        public FileStream? TryAcquire(string filePath, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare)
        {
            TryAcquire(filePath, out var fileStream, fileMode: fileMode,
                fileAccess: fileAccess, fileShare: fileShare);

            return fileStream;
        }

        /// <param name="timeoutInMilliseconds">The number of milliseconds to hold on, or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
        /// <exception cref="TimeoutException">When timeout is hit.</exception>
        /// <exception cref="OperationCanceledException">When cancellation was requested.</exception>
        private bool waitUntilAcquired(string filePath, [MaybeNullWhen(false)] out FileStream fileStream, FileMode fileMode,
            FileAccess fileAccess, FileShare fileShare, int timeoutInMilliseconds, bool throwOnTimeout, 
            CancellationToken cancellationToken)
        {
            /// Needs to be done, because we use 
            /// method of <see cref="TryAcquire(string, out FileStream?, FileMode, FileAccess, FileShare)"/> 
            /// inside lambda expression below.
            var fileStreamLocker = this;
            FileStream? spinningFileStream = null;

            var spinHasBeenFinishedSuccessfully = SpinWait.SpinUntil(() => {
                if (cancellationToken.IsCancellationRequested) {
                    return false;
                }

                return fileStreamLocker.TryAcquire(filePath, out spinningFileStream, fileMode: fileMode, fileAccess: fileAccess, fileShare: fileShare);
            }, timeoutInMilliseconds);

            if (spinHasBeenFinishedSuccessfully) {
                // File stream should never be null.
                fileStream = spinningFileStream!;
                return true;
            } else {
                if (throwOnTimeout) {
                    if (cancellationToken.IsCancellationRequested) {
                        cancellationToken.ThrowIfCancellationRequested();
                    } else {
                        throw new TimeoutException($"Acquiring file lock failed due to timeout.");
                    }
                }

                fileStream = null;
                return false;
            }
        }

        /// <param name="timeoutInMilliseconds">The number of milliseconds to hold on, or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
        /// <exception cref="TimeoutException">When timeout is hit.</exception>
        /// <exception cref="OperationCanceledException">When cancellation was requested.</exception>
        private FileStream? waitUntilAcquired(string filePath, FileMode fileMode,
            FileAccess fileAccess, FileShare fileShare, int timeoutInMilliseconds, 
            bool noThrowOnTimeout, CancellationToken cancellationToken)
        {
            waitUntilAcquired(filePath, out var fileStream, fileMode, fileAccess, fileShare, timeoutInMilliseconds, !noThrowOnTimeout, cancellationToken);
            return fileStream;
        }

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that is tried to get locked..</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <param name="throwOnTimeout">Enable throw when exception occured due due to timeout.</param>
        /// <returns>If true the lock acquirement was successful.</returns>
        /// <exception cref="OperationCanceledException">When cancellation was requested.</exception>
        public bool WaitUntilAcquired(string filePath, [MaybeNullWhen(false)] out FileStream fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare, bool throwOnTimeout = false,
            CancellationToken cancellationToken = default)
        {
            var timeoutInMilliseconds = DefaultTimeoutInMilliseconds;
            return waitUntilAcquired(filePath, out fileStream, fileMode, fileAccess, fileShare, timeoutInMilliseconds, throwOnTimeout, cancellationToken);
        }

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that is tried to get locked..</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <param name="noThrowOnTimeout">Prevents throwing exception when lock fails.</param>
        /// <returns>If not null the lock acquirement was successful.</returns>
        /// <exception cref="OperationCanceledException">When cancellation was requested.</exception>
        public FileStream? WaitUntilAcquired(string filePath, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare, bool noThrowOnTimeout = false,
            CancellationToken cancellationToken = default)
        {
            var timeoutInMilliseconds = DefaultTimeoutInMilliseconds;
            return waitUntilAcquired(filePath, fileMode, fileAccess, fileShare, timeoutInMilliseconds, noThrowOnTimeout, cancellationToken);
        }

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that is tried to get locked..</param>
        /// <param name="timeoutInMilliseconds">The number of milliseconds to hold on, or System.Threading.Timeout.Infinite (-1) to hold on indefinitely.</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <param name="throwOnTimeout">Enable throw when exception occured due due to timeout.</param>
        /// <returns>If true the lock acquirement was successful.</returns>
        /// <exception cref="TimeoutException">When timeout is hit.</exception>
        /// <exception cref="OperationCanceledException">When cancellation was requested.</exception>
        public bool WaitUntilAcquired(string filePath, int timeoutInMilliseconds,[MaybeNullWhen(false)] out FileStream fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare, bool throwOnTimeout = false,
            CancellationToken cancellationToken = default) =>
            waitUntilAcquired(filePath, out fileStream, fileMode, fileAccess, fileShare, timeoutInMilliseconds, throwOnTimeout, cancellationToken);

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that is tried to get locked..</param>
        /// <param name="timeoutInMilliseconds">The number of milliseconds to hold on, or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <param name="noThrowOnTimeout">Prevents throwing exception when lock fails.</param>
        /// <returns>If not null the lock acquirement was successful.</returns>
        /// <exception cref="TimeoutException">When timeout is hit.</exception>
        /// <exception cref="OperationCanceledException">When cancellation was requested.</exception>
        public FileStream? WaitUntilAcquired(string filePath, int timeoutInMilliseconds, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare, bool noThrowOnTimeout = false,
            CancellationToken cancellationToken = default) =>
            waitUntilAcquired(filePath, fileMode, fileAccess, fileShare, timeoutInMilliseconds, noThrowOnTimeout, cancellationToken);

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that is tried to get locked..</param>
        /// <param name="timeout">The timeout specified as <see cref="TimeSpan"/>.</param>
        /// <param name="fileStream">The locked file as file stream.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <param name="throwOnTimeout">Enable throw when exception occured due due to timeout.</param>
        /// <returns>If true the lock acquirement was successful.</returns>
        /// <exception cref="TimeoutException">When timeout is hit.</exception>
        /// <exception cref="OperationCanceledException">When cancellation was requested.</exception>
        public bool WaitUntilAcquired(string filePath, TimeSpan timeout,[MaybeNullWhen(false)] out FileStream fileStream, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare, bool throwOnTimeout = false,
            CancellationToken cancellationToken = default)
        {
            var timeoutInMilliseconds = Convert.ToInt32(timeout.TotalMilliseconds);
            return waitUntilAcquired(filePath, out fileStream, fileMode, fileAccess, fileShare, timeoutInMilliseconds, throwOnTimeout, cancellationToken);
        }

        /// <summary>
        /// Wait until file gets acquired lock but only as long the file stream is opened.
        /// </summary>
        /// <param name="filePath">The path to file that is tried to get locked..</param>
        /// <param name="timeout">The timeout specified as <see cref="TimeSpan"/>.</param>
        /// <param name="fileMode">The file mode when opening file.</param>
        /// <param name="fileAccess">The file access when opening file.</param>
        /// <param name="fileShare">The file share when opening file</param>
        /// <param name="noThrowOnTimeout">Prevents throwing exception when lock fails.</param>
        /// <returns>If not null the lock acquirement was successful.</returns>
        /// <exception cref="TimeoutException">When timeout is hit.</exception>
        /// <exception cref="OperationCanceledException">When cancellation was requested.</exception>
        public FileStream? WaitUntilAcquired(string filePath, TimeSpan timeout, FileMode fileMode = DefaultFileMode,
            FileAccess fileAccess = DefaultFileAccess, FileShare fileShare = DefaultFileShare, bool noThrowOnTimeout = false,
            CancellationToken cancellationToken = default)
        {
            var timeoutInMilliseconds = Convert.ToInt32(timeout.TotalMilliseconds);
            return waitUntilAcquired(filePath, fileMode, fileAccess, fileShare, timeoutInMilliseconds, noThrowOnTimeout, cancellationToken);
        }
    }
}
