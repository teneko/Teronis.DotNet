using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Build.Utilities;
using Teronis.Extensions;
using Teronis.GitVersionCache.BuildTasks.Models;
using Teronis.IO.FileLocking;
using Teronis.Text.Json.Converters;
using Teronis.Text.Json.Serialization;
using Teronis.GitVersion.CommandLine;

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

        public FileInfo GitVersionYamlFileInfo { get; }
        public DirectoryInfo ParentOfGitDirectoryInfo { get; }

        public string CacheDirectoryName {
            get => cacheDirectoryName;

            set => cacheDirectoryName = value ??
                throw new ArgumentNullException(nameof(CacheDirectoryName));
        }

        public string CacheDirectory => getCacheDirectory(GitVersionYamlFileInfo.Directory);
        public string CacheFile => Path.Combine(CacheDirectory, cacheIdentification.CacheIdentifier + ".json");

        public readonly ICacheIdentification cacheIdentification;
        private string cacheDirectoryName;

        public BuildTaskExecutor(ICacheIdentification cacheIdentification)
        {
            string configFile;

            if (cacheIdentification.ConfigFile != null) {
                configFile = cacheIdentification.ConfigFile;

                if (!File.Exists(configFile)) {
                    throw new FileNotFoundException($"The config file {configFile} does not exist.");
                }
            } else {
                var parentOfGitVersionYamlDirectoryInfo = BuildTaskUtilities.GetParentDirectoryOfGitVersionYamlFile(cacheIdentification.ProjectDirectory) ??
                    throw new FileNotFoundException($"Could not find parent GitVersion.yml file upwards {cacheIdentification.ProjectDirectory}.");

                configFile = Path.Combine(parentOfGitVersionYamlDirectoryInfo.FullName, BuildTaskExecutorDefaults.GitVersionFileNameWithExtension);
            }

            GitVersionYamlFileInfo = new FileInfo(configFile);

            ParentOfGitDirectoryInfo = BuildTaskUtilities.GetParentDirectoryOfGitDirectory(cacheIdentification.ProjectDirectory) ??
                throw new FileNotFoundException($"Could not find parent .git directory upwards {cacheIdentification.ProjectDirectory}.");

            CacheDirectoryName = BuildTaskExecutorDefaults.CacheDirectoryName;
            this.cacheIdentification = cacheIdentification;
        }

        private string getCacheDirectory(DirectoryInfo baseDirectory) =>
            Path.Combine(baseDirectory.FullName, CacheDirectoryName);

        public void EnsureCacheDirectoryExistence()
        {
            var gitVersionFileScopedCacheDirectory = getCacheDirectory(GitVersionYamlFileInfo.Directory);
            ensureDirectoryExistence(gitVersionFileScopedCacheDirectory);
        }

        private IDisposable lockFile()
        {
            var gitFolderScopedCacheDirectory = getCacheDirectory(ParentOfGitDirectoryInfo);
            ensureDirectoryExistence(gitFolderScopedCacheDirectory);
            var lockFile = Path.Combine(gitFolderScopedCacheDirectory, "GitVersionCache.lock");
            return FileStreamLocker.Default.WaitUntilAcquired(lockFile);
        }

        /// <summary>
        /// Gets the cached, or if not existing, the new calculated git version variables.
        /// </summary>
        /// <returns>If true the cache could be retrieved.</returns>
        public bool LoadCacheOrGetVersion(GetVersionCacheTask buildTask)
        {
            using var fileLock = lockFile();
            string serializedGitVariables;
            bool isCache;

            if (!File.Exists(CacheFile)) {
                var gitDirectory = ParentOfGitDirectoryInfo.FullName.Replace("\\", "/");
                var configFile = GitVersionYamlFileInfo.FullName.Replace("\\", "/");
                var arguments = $"{gitDirectory} /config {configFile}";

                try {
                    serializedGitVariables = GitVersionCommandLineLibrary.ExecuteGitVersion(arguments);
                    isCache = false;
                } catch (Diagnostics.NonZeroExitCodeException error) {
                    var errorMessage = error.Message + $"(arguments: {arguments})" +
                        (error.InnerException is Exception ? " " + error.InnerException.Message : null);

                    throw new NonZeroExitCodeException(error.ExitCode, errorMessage);
                }
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
            var variablesInclusionJsonConverter = OnlyIncludedVariablesJsonConverter.CreateNonGeneric(buildTaskType, out var variablesHelper);
            variablesHelper.ConsiderVariable(typeof(string), nameof(GetVersionCacheTask.ProjectDirectory));

            foreach (var propertyMember in buildTask.GetType().GetPropertyMembers(typeof(Task))) {
                variablesHelper.ConsiderVariable(propertyMember.DeclaringType, propertyMember.Name);
            }

            var options = new JsonSerializerOptions() {
                WriteIndented = true
            };

            options.Converters.Add(variablesInclusionJsonConverter);
            var serializedData = JsonSerializer.Serialize(buildTask, buildTaskType, options);

            using var file = File.Open(CacheFile, FileMode.Create);
            var serializedDataBytes = Encoding.UTF8.GetBytes(serializedData);
            file.Write(serializedDataBytes, 0, serializedDataBytes.Length);
        }
    }
}
