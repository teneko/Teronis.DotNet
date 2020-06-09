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

namespace Teronis.GitVersionCache.BuildTasks
{
    public class BuildTaskExecutor
    {
        public DirectoryInfo GitVersionYamlDirectory { get; }
        public string CacheDirectoryName { get; set; }
        public string CacheDirectory => Path.Combine(GitVersionYamlDirectory.FullName, CacheDirectoryName);
        public string CacheFile => Path.Combine(CacheDirectory, buildIdentification.BuildIdentifier + ".json");

        public readonly IBuildIdentification buildIdentification;

        public BuildTaskExecutor(IBuildIdentification buildIdentification)
        {
            GitVersionYamlDirectory = BuildTaskUtilities.GetGitVersionYamlDirectory() ??
                throw new FileNotFoundException("Could not find parent GitVersion.yml file.");

            CacheDirectoryName = BuildTaskExecutorDefaults.CacheDirectoryName;
            this.buildIdentification = buildIdentification;
        }

        protected void EnsureCacheDirectoryExistence()
        {
            if (Directory.Exists(CacheDirectory)) {
                return;
            }

            GitVersionYamlDirectory.CreateSubdirectory(CacheDirectoryName);
        }

        /// <summary>
        /// Gets the cached, or not existing, the new calculated git version variables.
        /// </summary>
        /// <returns>If true the cache could be retrieved.</returns>
        public bool LoadCacheOrGetVersion(GetVersionCacheTask buildTask)
        {
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
