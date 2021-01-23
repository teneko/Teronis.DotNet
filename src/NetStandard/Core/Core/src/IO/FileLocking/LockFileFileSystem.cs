using System.IO;

namespace Teronis.IO.FileLocking
{
    public class LockFileSystem : ILockFileSystem
    {
        public FileStream Open(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare) =>
            File.Open(filePath, fileMode, fileAccess, fileShare);
    }
}
