using System.IO;

namespace Teronis.IO.FileLocking
{
    public interface ILockFileSystem
    {
        FileStream Open(string path, FileMode mode, FileAccess access, FileShare share);
    }
}
