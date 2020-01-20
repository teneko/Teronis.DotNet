using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GitVersion;
using GitVersion.Extensions;
using GitVersion.MSBuildTask;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Teronis.IO;

namespace Teronis.DotNet.GitVersionCache
{
    public class Utilities
    {
        public static int GetTaskMaxDepth<T>(T derivedTask)
        where T : Task
        {
            var derivedTaskType = derivedTask.GetType();
            var objectType = typeof(object);
            var taskType = typeof(Task);
            var depth = 0;

            while (derivedTaskType != taskType) {
                derivedTaskType = derivedTaskType.BaseType;
                depth++;

                if (derivedTaskType == objectType) {
                    return 0;
                }
            }

            return depth;
        }
    }
}
