using Teronis.DotNet.GitVersionCache.BuildTasks.Models;

namespace Teronis.DotNet.GitVersionCache.BuildTasks
{
    public class GetVersionCacheTask : GetVersionCacheTaskBase, IBuildIdentification
    {
        protected override bool OnExecute()
        {
            var taskTaskExecutor = new BuildTaskExecutor(this);
            taskTaskExecutor.LoadCacheOrGetVersion(this);
            taskTaskExecutor.SaveToFilesystem(this);
            return true;
        }
    }
}
