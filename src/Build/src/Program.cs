using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using Teronis.Diagnostics;
using Teronis.Build.CommandOptions;
using static Bullseye.Targets;
using static Teronis.Build.ICommandOptions;
using System.Xml.Linq;

namespace Teronis.Build
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<RestoreCommandOptions, BuildSyntheticCommandOptions, BuildCommandOptions, PackCommandOptions, TestCommandOptions, AzureCommandOptions>(args)
                .MapResult<ICommandOptions, ICommandOptions>((options) => options, (errors) => {
#if DEBUG
                    if (errors != null) {
                        foreach (var error in errors) {
                            Console.WriteLine(error.ToString());
                        }
                    }
#endif

                    return null!;
                });

            if (options is null) {
                return 1;
            }

            // Marker file represents root directory
            var dotNetExecutableNameWithExtension = $"dotnet.exe";
            var gitVersionCacheRandomIdentifier = Guid.NewGuid();
            var teronisProjectRootDirectory = TeronisBuildUtils.GetRootDirectory() ?? throw new DirectoryNotFoundException("Root directory not found.");
            var rootSourceDirectory = Path.Combine(teronisProjectRootDirectory.FullName, "src");
            var buildDirectory = Path.Combine(rootSourceDirectory, @"Build\");

            if (!Directory.Exists(buildDirectory)) {
                throw new InvalidOperationException("Build directory does not exist.");
            }

            var allCSharpProjects = Directory.GetFiles(rootSourceDirectory, "*.csproj", SearchOption.AllDirectories)
                .Select(x => new ProjectInfo(new FileInfo(x)))
                .ToList();

            var matchPatternOfSyntheticPublishProjects = @"\\[a-zA-Z.]*~(ExecutablePublish|DependencyPublish)\\?";

            var matchPatternOfBuildProgramProjects = Regex.Escape(buildDirectory);

            var matchPatternOfTestProjects = @"(\.Test\.csproj|\\test\\)";

            var matchPatternOfBuildExcludedProjects = string.Format(@"({0}|{1}|{2})",
                matchPatternOfSyntheticPublishProjects,
                matchPatternOfTestProjects,
                matchPatternOfBuildProgramProjects);

            var matchPatternOfExampleProjects = @"(\\Example\.|\.Example\.csproj|\\example\\)";
            var matchPatternOfBenchmarkProjects = @"(\\Benchmark\.|\.Benchmark\.csproj|\\benchmark\\)";

            /* Localization projects have to be explicitly excluded by 
             * setting <IsPackable>false</IsPackable> in their respective
             * projects, because maybe they are wanted to be published.
             */

            var matchPatternOfPackExcludedProjects = string.Format(@"({0}|{1}|{2})",
                matchPatternOfBuildExcludedProjects,
                matchPatternOfExampleProjects,
                matchPatternOfBenchmarkProjects);

            var matchPatternOfAzureExcludedProjects = string.Format(@"({0}|{1})",
                matchPatternOfSyntheticPublishProjects,
                matchPatternOfBuildProgramProjects);

            IReadOnlyList<ProjectInfo> restoreProjects = null!;
            IReadOnlyList<ProjectInfo> buildProjects = null!;
            IReadOnlyList<ProjectInfo> buildSyntheticProjects = null!;
            IReadOnlyList<ProjectInfo> packProjects = null!;
            IReadOnlyList<ProjectInfo> testProjects = null!;
            IReadOnlyList<ProjectInfo> azureProjects = null!;

            IReadOnlyList<ProjectInfo> getPackProjects()
            {
                var validProjects = allCSharpProjects.Where(x => !Regex.IsMatch(x.Path, matchPatternOfPackExcludedProjects)).ToList();
                var validProjectsLength = validProjects.Count;

                for (var index = validProjectsLength - 1; index >= 0; index--) {
                    var projectFile = validProjects[index].Path;
                    var projectDocument = XDocument.Load(projectFile);

                    var lastIsPackableElement = projectDocument.Root
                        .Elements("PropertyGroup")
                        .Elements("IsPackable")
                        .LastOrDefault();

                    if (lastIsPackableElement != null && lastIsPackableElement.Value == "false") {
                        validProjects.RemoveAt(index);
                    }
                }

                return validProjects.ToList();
            }

            IReadOnlyList<ProjectInfo> getTestProjects() =>
                allCSharpProjects.Where(x => Regex.IsMatch(x.Path, matchPatternOfTestProjects)).ToList();

            void prepareProjectLists()
            {
                restoreProjects = allCSharpProjects;

                if (options.IsRestoreCommand()) {
                    return;
                }

                buildSyntheticProjects = allCSharpProjects.Where(x => Regex.IsMatch(x.Path, matchPatternOfSyntheticPublishProjects)).ToList();

                if (options.IsBuildSyntheticCommand()) {
                    return;
                }

                if (options.IsBuildCommand()) {
                    buildProjects = allCSharpProjects.Where(x => !Regex.IsMatch(x.Path, matchPatternOfBuildExcludedProjects)).ToList();
                } else if (options.IsPackCommand()) {
                    packProjects = getPackProjects();
                    buildProjects = packProjects;
                } else if (options.IsTestCommand()) {
                    testProjects = getTestProjects();
                    buildProjects = testProjects;
                } else if (options.IsAzureCommand()) {
                    azureProjects = allCSharpProjects.Where(x => !Regex.IsMatch(x.Path, matchPatternOfAzureExcludedProjects)).ToList();
                    buildProjects = azureProjects;
                    packProjects = getPackProjects();
                    testProjects = getTestProjects();
                } else {
                    throw new ArgumentException();
                }
            }

            prepareProjectLists();

            async Task runDotNetProject(BuildStyle buildStyle, string command, ProjectInfo project, string? additionalArguments = null, int retries = 0)
            {
                string commandArgs;

                if (buildStyle == BuildStyle.DotNet) {
                    commandArgs = $"{command} \"{project.Path}\" {additionalArguments}";
                } else if (buildStyle == BuildStyle.MSBuild) {
                    commandArgs = $"msbuild -t:{command} \"{project.Path}\" {additionalArguments}";
                } else {
                    throw new ArgumentException("Bad build style.");
                }

                if (options.DryRun) {
                    Console.WriteLine($"{project.Name}");
                    await Task.CompletedTask;
                } else {
                    retry:

                    try {
                        Console.WriteLine($"\u001b[35;1m{dotNetExecutableNameWithExtension} {commandArgs}\u001b[0m");
                        await SimpleProcess.RunAsync(dotNetExecutableNameWithExtension!, args: commandArgs, outputReceived: Console.Out.WriteLine, errorReceived: Console.Error.WriteLine);
                    } catch {
                        if (retries <= 0) {
                            throw;
                        }

                        retries--;
                        goto retry;
                    }
                }
            }

            async Task runDotNetProjectsAsync(BuildStyle buildStyle, string command, IReadOnlyList<ProjectInfo> projects, bool enableParallelism = false, int retries = 0)
            {
                options = options ?? throw new ArgumentNullException(nameof(options));
                projects = projects ?? throw new ArgumentNullException(nameof(projects));
                var additionalArgumentProperties = $"\"-p:GitVersionCacheIdentifier={gitVersionCacheRandomIdentifier}\"";
                string additonalArguments;

                if (buildStyle == BuildStyle.DotNet) {
                    additonalArguments = $"--{ConfigurationLongName} {options.Configuration} --{VerbosityLongName} {options.Verbosity} {additionalArgumentProperties}";
                } else if (buildStyle == BuildStyle.MSBuild) {
                    additonalArguments = $"-p:{ConfigurationLongName}={options.Configuration} -{VerbosityLongName}:{options.Verbosity} {additionalArgumentProperties}";
                } else {
                    throw new ArgumentException("Bad build style.");
                }

                if (options.DryRun) {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"(Total Projects = {projects.Count}): " + dotNetExecutableNameWithExtension + " " + command);
                    Console.ResetColor();
                }

                async Task runDotNetProjectAsync(ProjectInfo project)
                {
                    await runDotNetProject(buildStyle, command, project, additionalArguments: additonalArguments, retries: retries);

                    if (!options!.DryRun) {
                        Console.WriteLine(new string(Enumerable.Range(0, 80).Select(x => '_').ToArray()));
                    }
                }

                if (enableParallelism) {
                    var projectTasks = projects.Select(x => runDotNetProjectAsync(x));
                    await Task.WhenAll(projectTasks);
                } else {
                    foreach (var project in projects) {
                        await runDotNetProjectAsync(project);
                    }
                }

                if (options.DryRun) {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(additonalArguments);
                    Console.ResetColor();
                }
            }

            string[] DependsOnWhenNotSkippingDependencies(params string[] dependencies) =>
                options.SkipDependencies ? new string[] { } : dependencies;

            Target(Target.Restore, async () => {
                await runDotNetProjectsAsync(BuildStyle.MSBuild, RestoreCommandOptions.RestoreCommand, restoreProjects, retries: 3);
            });

            Target(Target.RestoreAndBuildSynthetic, DependsOnWhenNotSkippingDependencies(Target.Restore), async () => {
                await runDotNetProjectsAsync(BuildStyle.MSBuild, BuildCommandOptions.BuildCommand, buildSyntheticProjects);
            });

            Target(Target.RestoreAndBuildSyntheticAndNonSynthethic, DependsOnWhenNotSkippingDependencies(Target.RestoreAndBuildSynthetic), async () => {
                await runDotNetProjectsAsync(BuildStyle.MSBuild, BuildCommandOptions.BuildCommand, buildProjects);
            });

            Target(Target.RestoreAndBuildAndPack, DependsOnWhenNotSkippingDependencies(Target.RestoreAndBuildSyntheticAndNonSynthethic), async () => {
                await runDotNetProjectsAsync(BuildStyle.MSBuild, PackCommandOptions.PackCommand, packProjects);
            });

            Target(Target.RestoreAndBuildAndTest, DependsOnWhenNotSkippingDependencies(Target.RestoreAndBuildSyntheticAndNonSynthethic), async () => {
                await runDotNetProjectsAsync(BuildStyle.DotNet, TestCommandOptions.TestCommand, testProjects);
            });

            Target(Target.RestoreAndBuildAndPackAndTest, DependsOnWhenNotSkippingDependencies(Target.RestoreAndBuildAndPack, Target.RestoreAndBuildAndTest), () => { });

            await RunTargetsAndExitAsync(new string[] { options.Command });
            return 0;
        }

        private enum BuildStyle
        {
            DotNet,
            MSBuild
        }
    }
}
