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

namespace Teronis.DotNet.Build
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<BuildCommandOptions, PackCommandOptions, TestCommandOptions>(args)
                .MapResult<ICommandOptions, ICommandOptions>((options) => options, (errors) => {
#if DEBUG
                    if (errors != null) {
                        foreach (var error in errors) {
                            Console.WriteLine(error.ToString());
                        }
                    }
#endif

                    return null;
                });

            if (options == null) {
                return 1;
            }

            // Marker file represents root directory
            var rootDirectory = Utilities.GetRootDirectory() ?? throw new DirectoryNotFoundException("Root directory not found.");
            var sourceDirectory = Path.Combine(rootDirectory.FullName, "src");

            var projects = Directory.GetFiles(sourceDirectory, "*.csproj", SearchOption.AllDirectories)
                   .Select(x => new FileInfo(x));

            var matchTestProjects = @"(\.Test\.csproj|\\test\\)";

            var matchBuildProgramProjects = Regex.Escape(Path.Combine(sourceDirectory, "DotNet", "Build", @"Build\"));
            var matchBuildExcludedProjects = string.Format(@"({0}|{1})", matchTestProjects, matchBuildProgramProjects);

            var matchExampleProjects = @"\.Example\.csproj|\\example\\";
            var matchPackExcludedProjects = string.Format(@"({0}|{1})", matchBuildExcludedProjects, matchExampleProjects);

            Task RunDotNet(string command, string project, string additionalArguments = null)
            {
                var dotNetProgram = $"dotnet.exe";
                var dotNetCommandArgs = $"{command} \"{project}\" --{ConfigurationLongName} {options.Configuration} --{VerbosityLongName} {options.Verbosity} {additionalArguments}";

                if (options.DryRun) {
                    Console.WriteLine($"{dotNetProgram} {dotNetCommandArgs}");
                    return Task.CompletedTask;
                } else {
                    return RunAsync(dotNetProgram, args: dotNetCommandArgs);
                }
            }

            Target(BuildCommandOptions.BuildCommand, async () => {
                var buildProjects = projects.Where(x => !Regex.IsMatch(x.FullName, matchBuildExcludedProjects));

                foreach (var buildProject in buildProjects) {
                    await RunDotNet(BuildCommandOptions.BuildCommand, buildProject.FullName);
                }
            });

            Target(PackCommandOptions.PackCommand, DependsOn(), async () => {

                var packProjects = projects.Where(x => !Regex.IsMatch(x.FullName, matchPackExcludedProjects));

                foreach (var buildProject in packProjects) {
                    await RunDotNet("pack", buildProject.FullName);
                }
            });

            Target(TestCommandOptions.TestCommand, DependsOn(BuildCommandOptions.BuildCommand, PackCommandOptions.PackCommand), async () => {
                var testProjects = projects.Where(x => Regex.IsMatch(x.FullName, matchTestProjects));

                foreach (var buildProject in testProjects) {
                    await RunDotNet(BuildCommandOptions.BuildCommand, buildProject.FullName);
                }
            });

            await RunTargetsAndExitAsync(new string[] { options.Command });
            return 0;
        }
    }
}
