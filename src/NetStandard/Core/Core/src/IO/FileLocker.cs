using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Teronis.IO
{
    /// <summary>
    /// Provides a file locker that is thread-safe and supports nesting.
    /// </summary>
    public sealed class FileLocker
    {
#if DEBUG && TRACE
        public static string CurrentThreadPrefix() =>
            $"Thread {Thread.CurrentThread.Name ?? "unnamed"}:";

        public static string CurrentThreadWithLockIdPrefix(string lockId) =>
            $"{CurrentThreadPrefix()} Lock {lockId}:";

        private static string fileStreamHasBeenLockedString(FileStream fileStream) =>
            "(locked=" + (fileStream != null && (fileStream.CanRead || fileStream.CanWrite)).ToString().ToLower() + ")";

        private static string unlockOriginString(bool decreaseToZero) =>
            $"{(decreaseToZero ? "(manual unlock)" : "(dispose unlock)")}";
#endif

        public string FilePath { get; }

        public FileStream FileStream =>
            fileLockerState?.FileStream;

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
        private FileLockContext fileLockerState;
        private object decreaseLockUseLocker;

        public FileLocker(string filePath, FileMode fileMode = LockFile.DefaultFileMode, FileAccess fileAccess = LockFile.DefaultFileAccess,
            FileShare fileShare = LockFile.DefaultFileShare)
        {
            decreaseLockUseLocker = new object();
            FilePath = filePath;
            FileMode = fileMode;
            FileAccess = fileAccess;
            FileShare = fileShare;
            TimeoutInMilliseconds = LockFile.DefaultTimeoutInMilliseconds;
        }

        public FileLocker(string filePath, int timeoutInMilliseconds, FileMode fileMode = LockFile.DefaultFileMode, FileAccess fileAccess = LockFile.DefaultFileAccess,
            FileShare fileShare = LockFile.DefaultFileShare)
            : this(filePath, fileMode, fileAccess, fileShare)
        {
            TimeoutInMilliseconds = timeoutInMilliseconds;
        }

        public FileLocker(string filePath, TimeSpan timeout, FileMode fileMode = LockFile.DefaultFileMode, FileAccess fileAccess = LockFile.DefaultFileAccess,
            FileShare fileShare = LockFile.DefaultFileShare)
            : this(filePath, fileMode, fileAccess, fileShare)
        {
            TimeoutInMilliseconds = Convert.ToInt32(timeout.TotalMilliseconds);
        }

        /// <summary>
        /// Decreases the number of locks in use. If becoming zero, file gets unlocked.
        /// </summary>
#if DEBUG && TRACE
        internal int DecreaseLockUse(bool decreaseToZero, string lockId)
#else
        internal int DecreaseLockUse(bool decreaseToZero)
#endif
        {
            SpinWait spinWait = new SpinWait();
            int desiredLocksInUse;

            do {
                var currentLocksInUse = locksInUse;

                if (0 >= currentLocksInUse) {
#if DEBUG && TRACE
                    Trace.WriteLine($"{CurrentThreadWithLockIdPrefix(lockId)} Number of lock remains at 0 because file has been unlocked before. {unlockOriginString(decreaseToZero)}");
#endif
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

#if DEBUG && TRACE
            string numberOfLocksInUseDecreasedToMessage() =>
                $"{CurrentThreadWithLockIdPrefix(lockId)} Number of lock uses is decreased to {desiredLocksInUse}. {unlockOriginString(decreaseToZero)}";
#endif

            // When no locks are registered, we have to ..
            if (0 == desiredLocksInUse) {
                // 1. wait for file stream assignment,
                FileLockContext nullState = null;
                FileLockContext nonNullState = null;

                while (true) {
                    /* When class scoped file stream is null local file stream will be null too.
                     * => If so, spin once and continue loop.
                     * 
                     * When class scoped file stream is not null the local file stream will become
                     * not null too.
                     * => If so, assigned class scoped file streama to to local non null file stream
                     *    and continue loop.
                     *    
                     * When class scoped file stream is null and local non null file stream is not null
                     * We can break loop.
                     */

                    nullState = Interlocked.CompareExchange(ref fileLockerState, null, nullState);

                    if (nullState == null && nonNullState is null) {
                        spinWait.SpinOnce();
                    } else if (nullState == null && !(nonNullState is null)) {
                        break;
                    } else {
                        nonNullState = nullState;
                    }
                }

                // 2. invalidate the file stream.
                nonNullState.FileStream?.Close();
                nonNullState.FileStream?.Dispose();
#if DEBUG && TRACE
                Trace.WriteLine($"{numberOfLocksInUseDecreasedToMessage()}{Environment.NewLine}{CurrentThreadWithLockIdPrefix(lockId)} File {FilePath} unlocked by file locker. {unlockOriginString(decreaseToZero)}");
#endif
            }
#if DEBUG && TRACE
            else {
                Trace.WriteLine($"{numberOfLocksInUseDecreasedToMessage()}");
            }
#endif

            return desiredLocksInUse;
        }

        /// <summary>
        /// Locks the file specified at location <see cref="FilePath"/>.
        /// Those locks are going to fail who are called after a lock
        /// failed due to timeout/exception and before the locks in use
        /// are gotten zero.
        /// </summary>
        /// <returns>The file lock use is revoked if disposed.</returns>
        /// <exception cref="IOException">Thrown if file is already locked or otherwise inaccessible.</exception>
#if DEBUG && TRACE
        public FileLockUse Lock(string lockId)
#else
        public FileLockUse Lock()
#endif
        {
            SpinWait spinWait = new SpinWait();

            while (true) {
                var currentLocksInUse = locksInUse;
                var desiredLocksInUse = currentLocksInUse + 1;
                var currentFileLockerState = fileLockerState;

                if (currentFileLockerState.IsErroneous()) {
                    if (EnableConcurrentRethrow) {
#if DEBUG && TRACE
                        Trace.WriteLine($"{CurrentThreadWithLockIdPrefix(lockId)} Error from previous lock will be rethrown.");
#endif
                        throw currentFileLockerState.Error;
                    }

                    // Imagine stair steps where each stair step is Lock():
                    // Thread #0 Lock #0 -> Incremented to 1 -> Exception occured.
                    //  Thread #1 Lock #1 -> Incremented to 2. Recognozes exception in #0 because #0 not yet entered Unlock().
                    //   Thread #2 Lock #2 -> Incremented to 3. Recognizes excetion in #1 because #0 not yet entered Unlock().
                    // Thread #3 Lock #3 -> Incremented to 1. Lock was successful.
                    // We want Lock #1 and Lock #2 to retry their Lock():
                    //  Thread #1 Lock #1 -> Incremented to 2. Lock was successful.
                    //   Thread #2 Lock #2 -> Incremented to 3. Lock was successful.
                    currentFileLockerState.ErrorUnlockDone.WaitOne();
#if DEBUG && TRACE
                    Trace.WriteLine($"{CurrentThreadWithLockIdPrefix(lockId)} Retry lock due to previously failed lock.");
#endif
                    //spinWait.SpinOnce();
                    continue;
                }
                // If it is the initial lock, then we expect file stream being null.
                // If it is not the initial lock, we expect the stream being not null.
                else if ((currentLocksInUse == 0 && currentFileLockerState != null) ||
                    (currentLocksInUse != 0 && currentFileLockerState == null)) {
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
                            var fileStream = LockFile.WaitUntilAcquired(FilePath, TimeoutInMilliseconds, fileMode: FileMode,
                                    fileAccess: FileAccess, fileShare: FileShare);

                            currentFileLockerState = new FileLockContext(this, decreaseLockUseLocker) {
                                FileStream = fileStream
                            };

                            fileLockerState = currentFileLockerState;
#if DEBUG && TRACE
                            Trace.WriteLine($"{CurrentThreadWithLockIdPrefix(lockId)} File {FilePath} locked by file locker.");
#endif
                        } catch (Exception error) {
                            currentFileLockerState = new FileLockContext(this, decreaseLockUseLocker) {
                                Error = error,
                                ErrorUnlockDone = new ManualResetEvent(false)
                            };

                            fileLockerState = currentFileLockerState;
#if DEBUG && TRACE
                            Unlock(lockId);
#else
                            Unlock();
#endif
                            // After we processed Unlock(), we can surpass these locks 
                            // who could be dependent on state assigment of this Lock().
                            currentFileLockerState.ErrorUnlockDone.Set();
                            throw;
                        }
                    }
#if DEBUG && TRACE
                else {
                        Trace.WriteLine($"{CurrentThreadWithLockIdPrefix(lockId)} File {FilePath} locked {desiredLocksInUse} time(s) concurrently by file locker. {fileStreamHasBeenLockedString(currentFileLockerState.FileStream!)}");
                    }
#endif
                }

#if DEBUG && TRACE
                var fileLockContract = new FileLockUse(currentFileLockerState, lockId);
#else
                var fileLockContract = new FileLockUse(currentFileLockerState);
#endif
                return fileLockContract;
            }
        }



        /// <summary>
        /// Unlocks the file specified at location <see cref="FilePath"/>.
        /// </summary>
        /// <param name="scope">If specified it does not happen</param>
#if DEBUG && TRACE
        public void Unlock(string lockId)
#else
        public void Unlock()
#endif
        {
            lock (decreaseLockUseLocker) {
#if DEBUG && TRACE
                DecreaseLockUse(true, lockId);
#else
                DecreaseLockUse(true);
#endif
            }
        }
    }
}
