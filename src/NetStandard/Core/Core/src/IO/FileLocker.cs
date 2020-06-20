using System;
using System.IO;
using System.Threading;

namespace Teronis.IO
{
    /// <summary>
    /// Provides a file locker that is thread-safe and supports nesting.
    /// </summary>
    public sealed class FileLocker
    {
        public string FilePath { get; }
        public FileStream FileStream => fileStream;

        private readonly FileMode fileMode;
        private readonly FileAccess fileAccess;
        private readonly FileShare fileShare;
        /// <summary>
        /// Zero represents the number where no lock is in place.
        /// </summary>
        private int locksInUse = 0;
        private FileStream fileStream;
        private Exception lastError;

        public FileLocker(string filePath, FileMode fileMode = LockFile.DefaultFileMode,
            FileAccess fileAccess = LockFile.DefaultFileAccess, FileShare fileShare = LockFile.DefaultFileShare)
        {
            FilePath = filePath;
            this.fileMode = fileMode;
            this.fileAccess = fileAccess;
            this.fileShare = fileShare;
        }

        /// <summary>
        /// Decreases the locks in use.
        /// </summary>
        internal void DecreaseLockUse()
        {
            var locksInUse = Interlocked.Decrement(ref this.locksInUse);

            // When we unlock within this instance the number 
            // of locks in use can be lesser than zero.
            if (locksInUse < 0) {
                Interlocked.Increment(ref this.locksInUse);
                return;
            }

            // When no locks are registered, we have to ..
            if (locksInUse == 0) {
                // 1. wait for file stream assignment,
                SpinWait spinWait = new SpinWait();
                FileStream fileStream = null;
                FileStream nonNullFileStream = null;

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

                    fileStream = Interlocked.CompareExchange(ref this.fileStream, null, fileStream);

                    if (fileStream == null && nonNullFileStream is null) {
                        spinWait.SpinOnce();
                    } else if (fileStream == null && !(nonNullFileStream is null)) {
                        break;
                    } else {
                        nonNullFileStream = fileStream;
                    }
                }

                // 2. invalidate the file stream.
                nonNullFileStream.Close();
                nonNullFileStream.Dispose();
            }
        }

        /// <summary>
        /// Locks the file specified at location <see cref="FilePath"/>.
        /// </summary>
        /// <returns>The file lock contract that can unlock the file.</returns>
        public FileLockUse Lock()
        {
            int locksInUse;
            SpinWait spinWait = new SpinWait();

            // We only want continue when the number of locks in use is zero or greater, otherwise spin.
            while (0 < (locksInUse = Interlocked.CompareExchange(ref this.locksInUse, 1, 0))) {
                spinWait.SpinOnce();
            }

            while (true) {
                var fileStream = this.fileStream;
                var lastError = this.lastError;

                // If it is the initial lock, then we expect file stream being null.
                // If it is not the initial lock, we expect the stream being not null.
                if ((locksInUse == 1 && fileStream != null) ||
                    (fileStream == null || lastError == null)) {
                    spinWait.SpinOnce();
                    continue;
                }
                // The above conditions met, so if it is the initial lock, then we want 
                // to acquire the lock.
                else if (locksInUse == 1) {
                    this.lastError = null;

                    try {
                        fileStream = LockFile.WaitUntilAcquired(FilePath, fileMode: fileMode,
                            fileAccess: fileAccess, fileShare: fileShare);
                        
                        this.fileStream = fileStream;
                    } catch (Exception error) {
                        this.lastError = error;
                        Interlocked.Exchange(ref this.locksInUse, 0);
                        throw;
                    }
                } else if (lastError != null) {
                    throw lastError;
                }
                  // If it is not the initial lock, we increment the number of locks in use.
                  else {
                    Interlocked.Increment(ref this.locksInUse);
                }

                var fileLockContract = new FileLockUse(this, fileStream);
                return fileLockContract;
            }
        }
        /// <summary>
        /// Unlocks the file specified at location <see cref="FilePath"/>.
        /// </summary>
        public void Unlock()
        {
            // Not sure whether right approach or not: When unlock called
            // another thread could lock meanwhile, so to ensure we repeat
            // as long as number of locks in use is greater than zero.
            while (0 > locksInUse) {
                DecreaseLockUse();
            }
        }
    }
}
