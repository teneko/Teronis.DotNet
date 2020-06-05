using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using CommandLine;
using System.Threading.Tasks;
using Teronis.DotNet.Build.CommandOptions;

using static Bullseye.Targets;
using static SimpleExec.Command;
using static Teronis.DotNet.Build.ICommandOptions;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Teronis.DotNet.Build
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<RestoreCommandOptions, BuildCommandOptions, PackCommandOptions, TestCommandOptions>(args)
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
            var dotNetArguments = $"--{ConfigurationLongName} {options.Configuration} --{VerbosityLongName} {options.Verbosity}";
            var rootDirectory = Utilities.GetRootDirectory() ?? throw new DirectoryNotFoundException("Root directory not found.");
            var sourceDirectory = Path.Combine(rootDirectory.FullName, "src");

            var allProjects = Directory.GetFiles(sourceDirectory, "*.csproj", SearchOption.AllDirectories)
                   .Select(x => new ProjectInfo(new FileInfo(x)));

            var matchTestProjects = @"(\.Test\.csproj|\\test\\)";

            var matchBuildProgramProjects = Regex.Escape(Path.Combine(sourceDirectory, "DotNet", "Build", @"Build\"));
            var matchBuildExcludedProjects = string.Format(@"({0}|{1})", matchTestProjects, matchBuildProgramProjects);

            var matchExampleProjects = @"(\.Example\.csproj|\\example\\)";
            var matchReferenceProjects = @"(\\Reference\.|\\ref\\)";
            var matchPackExcludedProjects = string.Format(@"({0}|{1}|{2})", matchBuildExcludedProjects, matchExampleProjects, matchReferenceProjects);

            IEnumerable<ProjectInfo> filteredProjects;

            switch (options.Command) {
                case RestoreCommandOptions.RestoreCommand:
                    filteredProjects = allProjects;
                    break;
                case BuildCommandOptions.BuildCommand:
                    filteredProjects = allProjects.Where(x => !Regex.IsMatch(x.Path, matchBuildExcludedProjects));
                    break;
                case PackCommandOptions.PackCommand:
                    filteredProjects = allProjects.Where(x => !Regex.IsMatch(x.Path, matchPackExcludedProjects));
                    break;
                case TestCommandOptions.TestCommand:
                    filteredProjects = allProjects.Where(x => Regex.IsMatch(x.Path, matchTestProjects));
                    break;
                default:
                    throw new ArgumentException();
            }

            Task RunDotNetProject(string command, ProjectInfo project, string? arguments = null)
            {
                //var dotNetArgumentsWithAdditionals = dotNetArguments + " " + arguments;
                var dotNetCommand = $"{command} \"{project.Path}\" {arguments}";

                if (options.DryRun) {
                    Console.WriteLine($"{project.Name}");
                    return Task.CompletedTask;
                } else {
                    return RunAsync(dotNetProgram, args: dotNetCommand);
                }
            }

            async Task RunDotNetProjects(string command, IEnumerable<ProjectInfo> projects, string? arguments = null)
            {
                if (options.DryRun) {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(dotNetProgram + " " + command);
                    Console.ResetColor();
                }

                foreach (var project in projects) {
                    await RunDotNetProject(command, project, arguments);
                }

                if (options.DryRun) {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(dotNetArguments);
                    Console.ResetColor();
                }
            }

            Target(RestoreCommandOptions.RestoreCommand, async () => {
                await RunDotNetProjects(RestoreCommandOptions.RestoreCommand, filteredProjects);
            });

            Target(BuildCommandOptions.BuildCommand, DependsOn(RestoreCommandOptions.RestoreCommand), async () => {
                await RunDotNetProjects(BuildCommandOptions.BuildCommand, filteredProjects, dotNetArguments);
            });

            Target(PackCommandOptions.PackCommand, DependsOn(BuildCommandOptions.BuildCommand), async () => {
                await RunDotNetProjects(PackCommandOptions.PackCommand, filteredProjects, dotNetArguments);
            });

            Target(TestCommandOptions.TestCommand, DependsOn(BuildCommandOptions.BuildCommand), async () => {
                await RunDotNetProjects(BuildCommandOptions.BuildCommand, filteredProjects, dotNetArguments);
            });

            await RunTargetsAndExitAsync(new string[] { options.Command });
            return 0;
        }
    }
}
