using System.IO;
using System.Text;
using System.Text.Json;
using GitVersion.MSBuildTask.Tasks;
using Teronis.GitVersionCache.BuildTasks.Models;
using System.Reflection;

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

        //protected void CleanTemporaryDirectory() {
        //    if

        //    var  Directory.GetFiles(TemporaryDirectory,"*",SearchOption.TopDirectoryOnly);
        //}

        public void LoadCacheOrGetVersion(GetVersion buildTask)
        {
            if (!File.Exists(CacheFile)) {
                //var serviceCollection = BuildTaskUtilities.GetGitVersionCoreOwnedServiceProvider(buildTask);
                //var gitVersionTaskExecutor = serviceCollection.GetService<IGitVersionTaskExecutor>();
                //gitVersionTaskExecutor.GetVersion(buildTask);
                buildTask.Log.LogError("# 5 3 HALLO ES FUNKTTKJDFJ");
            } else {
                using var file = File.Open(CacheFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var fileReader = new StreamReader(file, Encoding.UTF8);
                var json = fileReader.ReadToEnd();
                var getVersionData = (GetVersion)JsonSerializer.Deserialize(json, typeof(GetVersion));
                var getVersionProperties = typeof(GetVersion).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);

                foreach (var getVersionProperty in getVersionProperties) {
                    var value = getVersionProperty.GetValue(getVersionData);
                    getVersionProperty.SetValue(buildTask, value);
                }
            }
        }

        public void SaveToFilesystem(GetVersion buildTask)
        {
            EnsureCacheDirectoryExistence();
            var buildTaskType = buildTask.GetType();
            //var variablesInclusionJsonConverter = JsonConverterFactory.CreateOnlyIncludedVariablesJsonConverter(buildTaskType, out var variablesHelper);

            //foreach (var propertyMember in buildTask.GetType().GetPropertyMembers(typeof(GitVersionTaskBase))) {
            //    variablesHelper.ConsiderVariable(propertyMember.DeclaringType, propertyMember.Name);
            //}

            var options = new JsonSerializerOptions() {
                WriteIndented = true
            };

            //options.Converters.Add(variablesInclusionJsonConverter);
            var serializedData = JsonSerializer.Serialize(buildTask, buildTaskType, options);

            using (var file = File.Open(CacheFile, FileMode.Create)) {
                var serializedDataBytes = Encoding.UTF8.GetBytes(serializedData);
                file.Write(serializedDataBytes, 0, serializedDataBytes.Length);
            }
        }
    }
}
