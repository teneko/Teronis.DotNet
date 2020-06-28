using System;
using System.ComponentModel;
using System.IO;

namespace Teronis.IO.FileLocking
{

    public struct FileLockUse : IDisposable
    {
        public FileStream FileStream => fileLockContext.FileStream!;

        private readonly FileLockContext fileLockContext;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly string LockId;

        internal FileLockUse(FileLockContext fileLockContext, string lockId)
        {
            this.fileLockContext = fileLockContext ?? throw new ArgumentNullException(nameof(fileLockContext));

            if (fileLockContext.FileStream is null) {
                throw new ArgumentException("File stream context has invalid file stream.");
            }

            LockId = lockId;
        }

        public void Dispose()
        {
            // When stream not closed, we can decrease lock use.
            fileLockContext.DecreaseLockUse(false, LockId);
        }
    }
}
