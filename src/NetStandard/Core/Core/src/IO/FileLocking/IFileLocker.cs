

namespace Teronis.IO.FileLocking
{
    public interface IFileLocker
    {
        FileLockUse WaitUntilAcquired();
    }
}
