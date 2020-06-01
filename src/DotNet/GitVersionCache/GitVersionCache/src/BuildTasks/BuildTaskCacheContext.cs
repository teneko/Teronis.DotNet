using System;
using System.IO;
using System.Text;
using System.Text.Json;
using GitVersion.MSBuildTask;
using Teronis.DotNet.GitVersionCache.BuildTasks.Models;
using Teronis.Extensions;
using Microsoft.Build.Utilities;
using Teronis.Text.Json.Serialization;
using Microsoft.Build.Framework;

namespace Teronis.DotNet.GitVersionCache.BuildTasks
{
    public class BuildTaskCacheContext
    {
        public DirectoryInfo GitVersionYamlDirectory { get; }
        public string CacheDirectoryName { get; set; }
        public string CacheDirectory => Path.Combine(GitVersionYamlDirectory.FullName, CacheDirectoryName);
        public string CacheFile => Path.Combine(CacheDirectory, buildIdentification.BuildIdentifier + ".json");

        private readonly IBuildIdentification buildIdentification;
        private readonly GitVersionTaskBase buildTask;

        public BuildTaskCacheContext(IBuildIdentification buildIdentification, GitVersionTaskBase buildTask)
        {
            GitVersionYamlDirectory = BuildTaskUtilities.GetGitVersionYamlDirectory() ??
                throw new FileNotFoundException("Could not find parent GitVersion.yml file.");

            CacheDirectoryName = BuildTaskCacheContextDefaults.CacheDirectoryName;
            this.buildIdentification = buildIdentification;
            this.buildTask = buildTask;
        }

        protected void EnsureCacheDirectoryExistence()
        {
            if (Directory.Exists(CacheDirectory)) {
                return;
            }

            GitVersionYamlDirectory.CreateSubdirectory(CacheDirectoryName);
        }

        //protected void CleanTemporaryDirectory() {
        //    if

        //    var  Directory.GetFiles(TemporaryDirectory,"*",SearchOption.TopDirectoryOnly);
        //}

        public void SaveGetVersionToDisk()
        {
            dynamic test = buildTask;
            buildTask.Log.LogMessage(MessageImportance.High, test.Major);

            EnsureCacheDirectoryExistence();
            var buildTaskType = buildTask.GetType();
            var variablesInclusionJsonConverter = JsonConverterFactory.CreateOnlyIncludedVariablesJsonConverter(buildTaskType, out var variablesHelper);

            foreach (var propertyMember in buildTask.GetType().GetPropertyMembers(typeof(GitVersionTaskBase))) {
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
