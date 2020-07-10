namespace Teronis.GitVersionCache.BuildTasks.Models
{
    public interface ICacheIdentification
    {
        string CacheIdentifier { get; }
        string ProjectDirectory { get; }
        string ConfigFile { get; }
    }
}
