//using System;
//using System.Text;
//using Microsoft.Build.Framework;
//using Teronis.MSBuild.Extensions;
//using Teronis.MSBuild.Tools;
//using BuildTask = Microsoft.Build.Utilities.Task;

//namespace Teronis.MSBuild
//{

//    public class RestoreCleanBuildPack : BuildTask
//    {
//        [Required]
//        public string Project { get; set; } = null!;
//        public string? Configuration { get; set; }

//        private void Run(string name, string args) =>
//            ProcessTools.Run(name, args, Log.LogHighImportantMessage, Log.LogHighImportantError);

//        public override bool Execute()
//        {
//            Log.LogMessage(MessageImportance.High, $"[{nameof(RestoreCleanBuildPack)}] Project '{Project}' is about to restore build and pack.");
//            var argsBuilder = new StringBuilder();

//            if (Configuration != null) {
//                argsBuilder.Append(" --configuration " + Configuration);
//            }

//            var args = argsBuilder.ToString();

//            try {
//                Run("dotnet", $"msbuild {Project} -t:restore -t:clean -t:build -t:pack" + args);
//            } catch (Exception error) {
//                Log.LogError(error.ToString());
//                return false;
//            }

//            return true;
//        }
//    }
//}
