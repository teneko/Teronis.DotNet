using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Teronis.DotNet.GitVersionCache.BuildTasks.Models;
//using Teronis.DotNet.GitVersionCache.Core.ContextIsolation2;

namespace Teronis.DotNet.GitVersionCache.BuildTasks
{
    public class SaveGetVersionTask : SaveGetVersionTaskBase
    {
        protected override bool OnExecute()
        {
            Log.LogMessage(MessageImportance.High, "######################## IT");
            Log.LogMessage(MessageImportance.High, "########################    DOES");
            Log.LogMessage(MessageImportance.High, "########################         WORK");
            Log.LogMessage(MessageImportance.High, $"########################         {ModuleInitializer.ExecutingAssemblyDirectory}");


            new BuildTaskCacheContext(this, this).SaveGetVersionToDisk();

            return true;
        }
    }
}
