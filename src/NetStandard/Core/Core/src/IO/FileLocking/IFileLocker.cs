namespace Teronis.IO
{
    public interface IFileLocker
    {
        FileLockUse WaitUntilAcquired();
    }
}
