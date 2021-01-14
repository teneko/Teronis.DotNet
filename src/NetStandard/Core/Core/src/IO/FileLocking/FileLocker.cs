using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("Teronis.NetStandard.Core.Test")]

namespace Teronis.IO.FileLocking
{
    /// <summary>
    /// Provides a file locker that is thread-safe and supports nesting.
    /// </summary>
    public sealed class FileLocker : IFileLocker
    {

        #region Trace Methods

#if TRACE
        internal const string TraceCategory = nameof(FileLocker);
        private static readonly Random random = new Random();

        private static string getFileStreamHasBeenLockedString(FileStream fileStream) =>
            "(locked=" + (fileStream != null && (fileStream.CanRead || fileStream.CanWrite)).ToString().ToLower() + ")";

        private static string getUnlockSourceString(bool decreaseToZero) =>
            $"{(decreaseToZero ? "(manual unlock)" : "(dispose unlock)")}";

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetCurrentThreadWithLockIdPrefixString(string lockId) =>
            $"Thread {Thread.CurrentThread.Name ?? "none"}: Lock {lockId}:";
#endif

        private static string getLockIdString()
        {
#if TRACE
            return random.Next(0, 999).ToString().PadLeft(3, '0');
#else
            return "none";
#endif
        }

        #endregion

        public string FilePath { get; }

        public FileStream? FileStream =>
            fileLockContext?.FileStream;

        /// <summary>
        /// If true, the lock attempts are going to throw the exception which occured in the lock before.
        /// This happens to all locks until the manual unlock within the lock in which the excpetion initially
        /// begun has been processed.
        /// </summary>
        public bool EnableConcurrentRethrow { get; set; }

        public int LocksInUse => locksInUse;

        public FileMode FileMode { get; }
        public FileAccess FileAccess { get; }
        public FileShare FileShare { get; }
        public int TimeoutInMilliseconds { get; }

        /// <summary>
        /// Zero represents the number where no lock is in place.
        /// </summary>
        private int locksInUse = 0;
        private FileLockContext? fileLockContext;
        private readonly object decreaseLockUseLocker;
        private readonly IFileStreamLocker fileStreamLocker;

        public FileLocker(IFileStreamLocker fileStreamLocker, string filePath, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess,
            FileShare fileShare = FileStreamLocker.DefaultFileShare)
        {
            decreaseLockUseLocker = new object();
            this.fileStreamLocker = fileStreamLocker;
            FilePath = filePath;
            FileMode = fileMode;
            FileAccess = fileAccess;
            FileShare = fileShare;
            TimeoutInMilliseconds = FileStreamLocker.DefaultTimeoutInMilliseconds;
        }

        public FileLocker(string filePath, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess,
           FileShare fileShare = FileStreamLocker.DefaultFileShare)
            : this(FileStreamLocker.Default, filePath, fileMode, fileAccess, fileShare)
        { }

        public FileLocker(string filePath, int timeoutInMilliseconds, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess,
            FileShare fileShare = FileStreamLocker.DefaultFileShare)
            : this(filePath, fileMode, fileAccess, fileShare)
        {
            TimeoutInMilliseconds = timeoutInMilliseconds;
        }

        public FileLocker(string filePath, TimeSpan timeout, FileMode fileMode = FileStreamLocker.DefaultFileMode, FileAccess fileAccess = FileStreamLocker.DefaultFileAccess,
            FileShare fileShare = FileStreamLocker.DefaultFileShare)
            : this(filePath, fileMode, fileAccess, fileShare)
        {
            TimeoutInMilliseconds = Convert.ToInt32(timeout.TotalMilliseconds);
        }

        /// <summary>
        /// Locks the file specified at location <see cref="FilePath"/>.
        /// </summary>
        /// <returns>The file lock use that can be revoked by disposing it.</returns>
        public FileLockUse WaitUntilAcquired()
        {
            var lockId = getLockIdString();
            Trace.WriteLine($"{GetCurrentThreadWithLockIdPrefixString(lockId)} Begin locking file {FilePath}.", TraceCategory);
            SpinWait spinWait = new SpinWait();

            while (true) {
                var currentLocksInUse = locksInUse;
                var desiredLocksInUse = currentLocksInUse + 1;
                var currentFileLockContext = fileLockContext;

                if (currentFileLockContext.IsErroneous()) {
                    if (EnableConcurrentRethrow) {
                        Trace.WriteLine($"{GetCurrentThreadWithLockIdPrefixString(lockId)} Error from previous lock will be rethrown.", TraceCategory);
                        throw currentFileLockContext!.Error!;
                    }

                    // Imagine stair steps where each stair step is Lock():
                    // Thread #0 Lock #0 -> Incremented to 1 -> Exception occured.
                    //  Thread #1 Lock #1 -> Incremented to 2. Recognozes exception in #0 because #0 not yet entered Unlock().
                    //   Thread #2 Lock #2 -> Incremented to 3. Recognizes excetion in #1 because #0 not yet entered Unlock().
                    // Thread #3 Lock #3 -> Incremented to 1. Lock was successful.
                    // We want Lock #1 and Lock #2 to retry their Lock():
                    //  Thread #1 Lock #1 -> Incremented to 2. Lock was successful.
                    //   Thread #2 Lock #2 -> Incremented to 3. Lock was successful.
                    currentFileLockContext!.ErrorUnlockDone!.WaitOne();
                    Trace.WriteLine($"{GetCurrentThreadWithLockIdPrefixString(lockId)} Retry lock due to previously failed lock.", TraceCategory);
                    continue;
                }
                // If it is the initial lock, then we expect file stream being null.
                // If it is not the initial lock, we expect the stream being not null.
                else if ((currentLocksInUse == 0 && currentFileLockContext != null) ||
                    (currentLocksInUse != 0 && currentFileLockContext == null)) {
                    spinWait.SpinOnce();
                    continue;
                } else {
                    if (currentLocksInUse != Interlocked.CompareExchange(ref locksInUse, desiredLocksInUse, currentLocksInUse)) {
                        continue;
                    }

                    // The above conditions met, so if it is the initial lock, then we want 
                    // to acquire the lock.
                    if (desiredLocksInUse == 1) {
                        try {
                            var fileStream = FileStreamLocker.Default.WaitUntilAcquired(FilePath, TimeoutInMilliseconds, fileMode: FileMode,
                                    fileAccess: FileAccess, fileShare: FileShare)!;

                            currentFileLockContext = new FileLockContext(this, decreaseLockUseLocker, fileStream);

                            fileLockContext = currentFileLockContext;
                            Trace.WriteLine($"{GetCurrentThreadWithLockIdPrefixString(lockId)} File {FilePath} locked by file locker.", TraceCategory);
                        } catch (Exception error) {
                            var errorUnlockDone = new ManualResetEvent(false);
                            currentFileLockContext = new FileLockContext(this, decreaseLockUseLocker, error, errorUnlockDone);
                            fileLockContext = currentFileLockContext;
                            Unlock(lockId);
                            // After we processed Unlock(), we can surpass these locks 
                            // who could be dependent on state assigment of this Lock().
                            currentFileLockContext.ErrorUnlockDone!.Set();
                            throw;
                        }
                    } else {
                        Trace.WriteLine($"{GetCurrentThreadWithLockIdPrefixString(lockId)} File {FilePath} locked {desiredLocksInUse} time(s) concurrently by file locker. {getFileStreamHasBeenLockedString(currentFileLockContext!.FileStream!)}", TraceCategory);
                    }
                }

                var fileLockContract = new FileLockUse(currentFileLockContext, lockId);
                return fileLockContract;
            }
        }

        /// <summary>
        /// Decreases the number of locks in use. If becoming zero, file gets unlocked.
        /// </summary>
        internal int DecreaseLockUse(bool decreaseToZero, string? lockId)
        {
            lockId ??= "none";
            SpinWait spinWait = new SpinWait();
            int desiredLocksInUse;

            do {
                var currentLocksInUse = locksInUse;

                if (0 >= currentLocksInUse) {
                    Trace.WriteLine($"{GetCurrentThreadWithLockIdPrefixString(lockId)} Number of lock remains at 0 because file has been unlocked before. {getUnlockSourceString(decreaseToZero)}", TraceCategory);
                    return 0;
                }

                if (decreaseToZero) {
                    desiredLocksInUse = 0;
                } else {
                    desiredLocksInUse = currentLocksInUse - 1;
                }

                var actualLocksInUse = Interlocked.CompareExchange(ref locksInUse, desiredLocksInUse, currentLocksInUse);

                if (currentLocksInUse == actualLocksInUse) {
                    break;
                }

                spinWait.SpinOnce();
            } while (true);

            string decreasedNumberOfLocksInUseMessage() =>
                $"{GetCurrentThreadWithLockIdPrefixString(lockId)} Number of lock uses is decreased to {desiredLocksInUse}. {getUnlockSourceString(decreaseToZero)}";

            // When no locks are registered, we have to ..
            if (0 == desiredLocksInUse) {
                // 1. wait for file stream assignment,
                FileLockContext? nullState = null;
                FileLockContext nonNullState = null!;

                while (true) {
                    nullState = Interlocked.CompareExchange(ref fileLockContext, null, nullState);

                    /* When class scoped file stream is null local file stream will be null too.
                     * => If so, spin once and continue loop.
                     * 
                     * When class scoped file stream is not null the local file stream will become
                     * not null too.
                     * => If so, assigned class scoped file streama to to local non null file stream
                     *    and continue loop.
                     *    
                     * When class scoped file stream is null and local non null file stream is not null
                     * => If so, break loop.
                     */
                    if (nullState == null && nonNullState is null) {
                        spinWait.SpinOnce();
                    } else if (nullState == null && !(nonNullState is null)) {
                        break;
                    } else {
                        nonNullState = nullState!;
                    }
                }

                // 2. invalidate the file stream.
                nonNullState.FileStream?.Close();
                nonNullState.FileStream?.Dispose();
                Trace.WriteLine($"{decreasedNumberOfLocksInUseMessage()}{Environment.NewLine}{GetCurrentThreadWithLockIdPrefixString(lockId)} File {FilePath} unlocked by file locker. {getUnlockSourceString(decreaseToZero)}", TraceCategory);
            } else {
                Trace.WriteLine($"{decreasedNumberOfLocksInUseMessage()}");
            }

            return desiredLocksInUse;
        }

        /// <summary>
        /// Unlocks the file specified at location <see cref="FilePath"/>.
        /// </summary>
        /// <param name="lockId">The lock id is for tracing purposes.</param>
        internal void Unlock(string lockId)
        {
            lock (decreaseLockUseLocker) {
                DecreaseLockUse(true, lockId);
            }
        }
    }
}
