using System;
using System.IO;

namespace Teronis.IO
{
    public struct FileLockUse : IDisposable
    {
        public FileStream FileStream => fileLockContext.FileStream;

        private readonly FileLockContext fileLockContext;
#if DEBUG && TRACE
        private readonly string lockId;
#endif

        internal FileLockUse(FileLockContext fileLockContext)
        {
#if DEBUG && TRACE
            lockId = null;
#endif
            this.fileLockContext = fileLockContext;
        }

#if DEBUG && TRACE
        internal FileLockUse(FileLockContext fileLockContext, string lockId
            )
        {
            this.fileLockContext = fileLockContext;
            this.lockId = lockId;
        }
#endif

        public void Dispose()
        {
            // When stream not closed, we can decrease lock use.
#if DEBUG && TRACE
            fileLockContext.DecreaseLockUse(false, lockId);
#else
            fileLockContext.DecreaseLockUse(false);
#endif
        }
    }
}
