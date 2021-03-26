// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Reflection;
using Teronis.IO;

namespace Teronis.GitVersionCache.BuildTasks
{
    public static class BuildTaskUtilities
    {
        public static DirectoryInfo GetParentDirectoryOfGitVersionYamlFile(string beginningDirectory) =>
              DirectoryUtils.GetDirectoryOfFileAbove(BuildTaskExecutorDefaults.GitVersionFileNameWithExtension, beginningDirectory, includeBeginningDirectory: true);

        public static DirectoryInfo GetParentDirectoryOfGitDirectory(string beginningDirectory) =>
              DirectoryUtils.GetDirectoryOfDirectoryAbove(".git", beginningDirectory, includeBeginningDirectory: true);

        public static void SetUndefinedAsDefault(object instance, string propertyName)
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
