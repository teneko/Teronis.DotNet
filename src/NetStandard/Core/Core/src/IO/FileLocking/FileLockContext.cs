using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Teronis.IO
{
    internal class FileLockContext
    {
        public FileStream? FileStream { get; set; }
        public Exception? Error { get; set; }
        public ManualResetEvent ErrorUnlockDone { get; set; }

        private readonly FileLocker fileLocker;
        private object decreaseLockUseLocker;

        public FileLockContext(FileLocker fileLocker, object decreaseLockUseLocker)
        {
            this.fileLocker = fileLocker;
            this.decreaseLockUseLocker = decreaseLockUseLocker;
            this.decreaseLockUseLocker = decreaseLockUseLocker;
        }

        public void DecreaseLockUse(bool decreaseToZero, string lockId)
        {
            var decreaseLockUseLocker = this.decreaseLockUseLocker;

            if (decreaseLockUseLocker == null) {
                return;
            }

            // Why surround by lock?
            // There is a race condition, when number of file lock uses
            // is decrased to 0. It may not have invalidated the file
            // stream yet. Now it can happen that the number of file lock
            // uses is increased to 1 due to file lock, but right after another
            // file unlock is about to decrease the number again to 0.
            // There is the possiblity that the actual file lock gets released
            // two times accidentally.
            lock (decreaseLockUseLocker) {
                if (!(FileStream.CanRead || FileStream.CanWrite)) {
                    Trace.WriteLine($"{FileLocker.CurrentThreadWithLockIdPrefix(lockId)} Lock use has been invalidated before. Skip decreasing lock use.", FileLocker.TraceCategory);
                    return;
                }

                var locksInUse = fileLocker.DecreaseLockUse(decreaseToZero, lockId);

                if (0 == locksInUse) {
                    this.decreaseLockUseLocker = null;
                }
            }
        }
    }
}
