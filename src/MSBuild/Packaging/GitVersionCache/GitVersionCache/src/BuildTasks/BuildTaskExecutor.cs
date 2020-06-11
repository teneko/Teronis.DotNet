using System.IO;
using System.Text;
using System.Text.Json;
using Teronis.GitVersionCache.BuildTasks.Models;
using System.Reflection;
using Teronis.Text.Json.Serialization;
using Teronis.Extensions;
using Teronis.Tools.GitVersion;
using Teronis.Text.Json.Converters;
using Microsoft.Build.Utilities;
using System;
using Teronis.IO;
using System.Threading;

namespace Teronis.GitVersionCache.BuildTasks
{
    public class BuildTaskExecutor
    {
        private static void ensureDirectoryExistence(string directory)
        {
            if (Directory.Exists(directory)) {
                return;
            }

            Directory.CreateDirectory(directory);
        }

        public DirectoryInfo ParentOfGitVersionYamlDirectoryInfo { get; }
        public DirectoryInfo ParentOfGitDirectoryInfo { get; }

        public string CacheDirectoryName {
            get => cacheDirectoryName;

            set => cacheDirectoryName = value ??
                throw new ArgumentNullException(nameof(CacheDirectoryName));
        }

        public string CacheDirectory => getCacheDirectory(ParentOfGitVersionYamlDirectoryInfo);
        public string CacheFile => Path.Combine(CacheDirectory, buildIdentification.CacheIdentifier + ".json");

        public readonly IBuildIdentification buildIdentification;
        private string cacheDirectoryName;

        public BuildTaskExecutor(IBuildIdentification buildIdentification)
        {
            ParentOfGitVersionYamlDirectoryInfo = BuildTaskUtilities.GetParentOfGitVersionYamlDirectory() ??
                throw new FileNotFoundException("Could not find parent GitVersion.yml file.");

            ParentOfGitDirectoryInfo = BuildTaskUtilities.GetParentOfGitDirectory() ??
                throw new FileNotFoundException("Could not find parent .git directory.");

            CacheDirectoryName = BuildTaskExecutorDefaults.CacheDirectoryName;
            this.buildIdentification = buildIdentification;
        }

        private string getCacheDirectory(DirectoryInfo baseDirectory) =>
            Path.Combine(baseDirectory.FullName, CacheDirectoryName);

        public void EnsureCacheDirectoryExistence()
        {
            var cacheDirectory = getCacheDirectory(ParentOfGitVersionYamlDirectoryInfo);
            ensureDirectoryExistence(cacheDirectory);
        }

        private IDisposable lockFile()
        {
            var cacheDirectory = getCacheDirectory(ParentOfGitDirectoryInfo);
            ensureDirectoryExistence(cacheDirectory);
            var lockFile = Path.Combine(cacheDirectory, "GitVersionCache.lock");
            return LockFile.WaitUntilAcquired(lockFile);
        }

        /// <summary>
        /// Gets the cached, or not existing, the new calculated git version variables.
        /// </summary>
        /// <returns>If true the cache could be retrieved.</returns>
        public bool LoadCacheOrGetVersion(GetVersionCacheTask buildTask)
        {
            using var fileLock = lockFile();
            string serializedGitVariables;
            bool isCache;

            if (!File.Exists(CacheFile)) {
                serializedGitVariables = GitVersionCommandLine.ExecuteGitVersion();
                isCache = false;
            } else {
                using var file = File.Open(CacheFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var fileReader = new StreamReader(file, Encoding.UTF8);
                serializedGitVariables = fileReader.ReadToEnd();
                isCache = true;
            }

            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new NumberStringificationJsonConverter());

            var getVersionData = (GetVersionCacheTask)JsonSerializer.Deserialize(serializedGitVariables, typeof(GetVersionCacheTask), options: jsonSerializerOptions);
            var getVersionProperties = typeof(GetVersionCacheTask).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);

            foreach (var getVersionProperty in getVersionProperties) {
                var value = getVersionProperty.GetValue(getVersionData);

                if (value is null) {
                    continue;
                }

                getVersionProperty.SetValue(buildTask, value);
            }

            return isCache;
        }

        public void SaveToFilesystem(GetVersionCacheTask buildTask)
        {
            using var fileLock = lockFile();
            EnsureCacheDirectoryExistence();


            var buildTaskType = buildTask.GetType();
            var variablesInclusionJsonConverter = JsonConverterFactory.CreateOnlyIncludedVariablesJsonConverter(buildTaskType, out var variablesHelper);
            variablesHelper.ConsiderVariable(typeof(string), nameof(GetVersionCacheTask.SolutionDirectory));

            foreach (var propertyMember in buildTask.GetType().GetPropertyMembers(typeof(Task))) {
                variablesHelper.ConsiderVariable(propertyMember.DeclaringType, propertyMember.Name);
            }

            var options = new JsonSerializerOptions() {
                WriteIndented = true
            };

            options.Converters.Add(variablesInclusionJsonConverter);
            var serializedData = JsonSerializer.Serialize(buildTask, buildTaskType, options);

            using (var file = File.Open(CacheFile, FileMode.Create)) {
                var serializedDataBytes = Encoding.UTF8.GetBytes(serializedData);
                file.Write(serializedDataBytes, 0, serializedDataBytes.Length);
            }
        }
    }
}
