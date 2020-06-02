using Teronis.DotNet.GitVersionCache.BuildTasks.Models;

namespace Teronis.DotNet.GitVersionCache.BuildTasks
{
    public class SaveGetVersionTask : SaveGetVersionTaskBase
    {
        protected override bool OnExecute()
        {
            var buildTaskExecutor = new BuildTaskExecutor(this);
            buildTaskExecutor.SaveToFilesystem(this);
            return true;
        }
    }
}
