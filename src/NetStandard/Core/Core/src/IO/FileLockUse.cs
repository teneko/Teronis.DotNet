using System;
using System.IO;

namespace Teronis.IO
{
    public struct FileLockUse : IDisposable
    {
        public FileStream FileStream => fileLocker.FileStream;

        private readonly FileLocker fileLocker;
        private readonly FileStream fileStream;

        internal FileLockUse(FileLocker fileLocker, FileStream fileStream)
        {
            this.fileLocker = fileLocker;
            this.fileStream = fileStream;
        }

        public void Dispose()
        {
            // When stream not closed, we can decrease lock use.
            if (fileStream.CanRead || fileStream.CanWrite) {
                fileLocker.DecreaseLockUse();
            }
        }
    }
}
