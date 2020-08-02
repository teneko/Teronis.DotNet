using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using Teronis.Diagnostics;
using Teronis.DotNet.Build.CommandOptions;
using Microsoft.Build.Evaluation;
using static Bullseye.Targets;
using static Teronis.DotNet.Build.ICommandOptions;
using System.Diagnostics;

namespace Teronis.DotNet.Build
{
    class Program
    {
        private static void setMsBuildExePath()
        {
            try {
                var startInfo = new ProcessStartInfo("dotnet", "--list-sdks") {
                    RedirectStandardOutput = true
                };

                var process = Process.Start(startInfo);
                process.WaitForExit(1000);

                var output = process.StandardOutput.ReadToEnd();
                var sdkPaths = Regex.Matches(output, "([0-9]+.[0-9]+.[0-9]+) \\[(.*)\\]")
                    .OfType<Match>()
                    .Select(m => System.IO.Path.Combine(m.Groups[2].Value, m.Groups[1].Value, "MSBuild.dll"));

                var sdkPath = sdkPaths.Last();
                Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", sdkPath);
            } catch (Exception exception) {

            }
        }

        static async Task<int> Main(string[] args)
        {
            setMsBuildExePath();

            var options = Parser.Default.ParseArguments<RestoreCommandOptions, BuildCommandOptions, PackCommandOptions, TestCommandOptions, AzureCommandOptions>(args)
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
            var dotNetProgram = $"dotnet.exe";
            var gitVersionCacheIdentifier = Guid.NewGuid();
            var rootDirectory = TeronisBuildUtils.GetRootDirectory() ?? throw new DirectoryNotFoundException("Root directory not found.");
            var sourceDirectory = Path.Combine(rootDirectory.FullName, "src");

            var allProjects = Directory.GetFiles(sourceDirectory, "*.csproj", SearchOption.AllDirectories)
                   .Select(x => new ProjectInfo(new FileInfo(x)));

            var matchPublishablePackageProjects = @"(\\PublishablePackage.|PackagePublish.)";
            var matchGitVersionCacheProjects = @"(\\GitVersionCache.)";
            var matchSyntheticProjects = string.Format("({0}|{1})", matchPublishablePackageProjects, matchGitVersionCacheProjects);
            //var matchReferenceProjects = @"(\\Reference\.|\\ref\\)";
            //var matchReferenceProjects = string.Format("{0}|{1})", matchPublishablePackageProjects, matchGitVersionCacheProjects);

            var matchBuildProgramProjects = Regex.Escape(Path.Combine(sourceDirectory, "DotNet", "Build", @"Build\"));

            var matchTestProjects = @"(\.Test\.csproj|\\test\\)";

            var matchBuildExcludedProjects = string.Format(@"({0}|{1}|{2})",
                matchSyntheticProjects,
                matchTestProjects,
                matchBuildProgramProjects);

            var matchExampleProjects = @"(\\Example\.|\.Example\.csproj|\\example\\)";
            var matchPackExcludedProjects = string.Format(@"({0}|{1})", matchBuildExcludedProjects, matchExampleProjects);

            var matchAzureExcludedProjects = string.Format(@"({0}|{1})", matchSyntheticProjects, matchBuildProgramProjects);

            IEnumerable<ProjectInfo> restoreProjects = null!;
            IEnumerable<ProjectInfo> buildProjects = null!;
            IEnumerable<ProjectInfo> packProjects = null!;
            IEnumerable<ProjectInfo> testProjects = null!;
            IEnumerable<ProjectInfo> azureProjects = null!;

            IList<ProjectInfo> getPackProjects()
            {
                var validProjects = allProjects.Where(x => !Regex.IsMatch(x.Path, matchPackExcludedProjects)).ToList();
                var validProjectsLength = validProjects.Count;

                for (var index = validProjectsLength - 1; index >= 0; index--) {
                    var projectFile = validProjects[index].Path;

                    using var collection = new ProjectCollection();
                    var project = collection.LoadProject(projectFile);

                    var lastIsPackableElement = project
                        .GetProperty("IsPackable");

                    if (lastIsPackableElement != null && lastIsPackableElement.EvaluatedValue == "false") {
                        validProjects.RemoveAt(index);
                    }
                }

                return validProjects;
            }

            IEnumerable<ProjectInfo> getTestProjects() =>
                allProjects.Where(x => Regex.IsMatch(x.Path, matchTestProjects));

            if (options.Command == RestoreCommandOptions.RestoreCommand) {
                restoreProjects = allProjects;
            } else if (options.Command == BuildCommandOptions.BuildCommand) {
                buildProjects = allProjects.Where(x => !Regex.IsMatch(x.Path, matchBuildExcludedProjects));
                restoreProjects = buildProjects;
            } else if (options.Command == PackCommandOptions.PackCommand) {
                packProjects = getPackProjects();
                buildProjects = packProjects;
                restoreProjects = buildProjects;
            } else if (options.Command == TestCommandOptions.TestCommand) {
                testProjects = getTestProjects();
                buildProjects = testProjects;
                restoreProjects = testProjects;
            } else if (options.Command == AzureCommandOptions.AzureCommand) {
                azureProjects = allProjects.Where(x => !Regex.IsMatch(x.Path, matchAzureExcludedProjects));
                testProjects = getTestProjects();
                packProjects = getPackProjects();
                buildProjects = azureProjects;
                restoreProjects = azureProjects;
            } else {
                throw new ArgumentException();
            }

            Task runDotNetProject(BuildStyle buildStyle, string command, ProjectInfo project, string? additionalArguments = null)
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
                    return Task.CompletedTask;
                } else {
                    return SimpleProcess.RunAsync(dotNetProgram!, args: commandArgs, outputReceived: Console.Out.WriteLine, errorReceived: Console.Error.WriteLine);
                }
            }

            async Task runDotNetProjectsAsync(BuildStyle buildStyle, string command, IEnumerable<ProjectInfo> projects, bool enableParallelism = false)
            {
                options = options ?? throw new ArgumentNullException(nameof(options));
                projects = projects ?? throw new ArgumentNullException(nameof(projects));
                var additionalArgumentProperties = $"\"-p:GitVersionCacheIdentifier={gitVersionCacheIdentifier}\"";
                string additonalArguments;

                if (buildStyle == BuildStyle.DotNet) {
                    additonalArguments = $"--{ConfigurationLongName} {options.Configuration} --{VerbosityLongName} {options.Verbosity} {additionalArgumentProperties}";
                } else if (buildStyle == BuildStyle.MSBuild) {
                    additonalArguments = $"-p:{ConfigurationLongName}={options.Configuration} -p:{VerbosityLongName}={options.Verbosity} {additionalArgumentProperties}";
                } else {
                    throw new ArgumentException("Bad build style.");
                }

                if (options.DryRun) {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(dotNetProgram + " " + command);
                    Console.ResetColor();
                }

                async Task runDotNetProjectAsync(ProjectInfo project)
                {
                    await runDotNetProject(buildStyle, command, project, additonalArguments);

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

            string[] DependsOnIf(params string[] dependencies) =>
                options.SkipDependencies ? new string[] { } : dependencies;

            Target(RestoreCommandOptions.RestoreCommand, async () => {
                await runDotNetProjectsAsync(BuildStyle.MSBuild, RestoreCommandOptions.RestoreCommand, restoreProjects, enableParallelism: true);
            });

            Target(BuildCommandOptions.BuildCommand, DependsOnIf(RestoreCommandOptions.RestoreCommand), async () => {
                await runDotNetProjectsAsync(BuildStyle.MSBuild, BuildCommandOptions.BuildCommand, buildProjects);
            });

            Target(PackCommandOptions.PackCommand, DependsOnIf(BuildCommandOptions.BuildCommand), async () => {
                await runDotNetProjectsAsync(BuildStyle.MSBuild, PackCommandOptions.PackCommand, packProjects);
            });

            Target(TestCommandOptions.TestCommand, DependsOnIf(BuildCommandOptions.BuildCommand), async () => {
                await runDotNetProjectsAsync(BuildStyle.DotNet, TestCommandOptions.TestCommand, testProjects);
            });

            Target(AzureCommandOptions.AzureCommand, DependsOnIf(PackCommandOptions.PackCommand, TestCommandOptions.TestCommand), () => { });

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
