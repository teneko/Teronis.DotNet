using Teronis.GitVersionCache.BuildTasks.Models;

namespace Teronis.GitVersionCache.BuildTasks
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
