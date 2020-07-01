using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
//using GitVersion.MSBuildTask;
using Microsoft.Build.Utilities;
using Teronis.GitVersionCache.Utilities;
using Teronis.IO;

namespace Teronis.GitVersionCache.BuildTasks
{
    public static class BuildTaskUtilities
    {
        public static DirectoryInfo GetParentOfGitVersionYamlDirectory(string beginningDirectory) =>
              DirectoryTools.GetDirectoryOfFileAbove(BuildTaskExecutorDefaults.GitVersionFileNameWithExtension, beginningDirectory, includeBeginningDirectory: true);

        public static DirectoryInfo GetParentOfGitDirectory(string beginningDirectory) =>
              DirectoryTools.GetDirectoryOfDirectoryAbove(".git", beginningDirectory, includeBeginningDirectory: true);

        public static void SetUndefinedAsDefault(object instance, string propertyName, TaskLoggingHelper Log)
        {
            var instanceType = instance.GetType();
            var propertyInfo = instanceType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

            if (propertyInfo == null) {
                return;
            }

            var propertyValue = propertyInfo.GetValue(instance);

            if (propertyValue is string propertyString && propertyString == "*Undefined*") {
                propertyInfo.SetValue(instance, null);
            }
        }
    }
}
