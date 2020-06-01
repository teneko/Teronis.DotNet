using System;
using System.Collections.Generic;
using System.Text;
using GitVersion.MSBuildTask.Tasks;
using Microsoft.Build.Utilities;
using Teronis.DotNet.GitVersionCache.BuildTasks.Models;

namespace Teronis.DotNet.GitVersionCache.BuildTasks
{
    public class GetVersionCacheTask : GetVersionCacheTaskBase, IBuildIdentification
    {
        protected override bool OnExecute()
        {
            //new BuildTaskCacheContext(this, this).SaveGetVersionToDisk;
            return true;
        }
    }
}
