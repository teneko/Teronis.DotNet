using System;
using System.ComponentModel;
using System.IO;

namespace Teronis.IO
{
    public struct FileLockUse : IDisposable
    {
        public FileStream FileStream => fileLockContext.FileStream;

        private readonly FileLockContext fileLockContext;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly string LockId;

        internal FileLockUse(FileLockContext fileLockContext, string LockId)
        {
            this.fileLockContext = fileLockContext;
            this.LockId = LockId;
        }

        public void Dispose()
        {
            // When stream not closed, we can decrease lock use.
            fileLockContext.DecreaseLockUse(false, LockId);
        }
    }
}
