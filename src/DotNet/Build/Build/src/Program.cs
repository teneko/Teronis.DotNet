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
            var matchBuildProjects = Regex.Escape(Path.Combine(sourceDirectory, "DotNet", "Build",  @"Build\"));
            var matchNonBuildProjects = string.Format(@"({0}|{1})", matchTestProjects, matchBuildProjects);

            Task RunDotNet(string command, string project, string additionalArguments = null) => 
                RunAsync($"dotnet.exe", $"{command} {project} --{ConfigurationLongName} {options.Configuration} --{VerbosityLongName} {options.Verbosity} {additionalArguments}");

            Target(BuildCommandOptions.COMMAND, async () => {
                var buildProjects = projects.Where(x => !Regex.IsMatch(x.FullName, matchNonBuildProjects));

                foreach (var buildProject in buildProjects) {
                    await RunDotNet("build", buildProject.FullName);
                }
            });

            Target(PackCommandOptions.COMMAND, async () => {
                var packProjects = projects.Where(x => !Regex.IsMatch(x.FullName, matchNonBuildProjects));

                foreach (var buildProject in packProjects) {
                    await RunDotNet("pack", buildProject.FullName);
                }
            });

            Target(TestCommandOptions.COMMAND, DependsOn("build", "pack"), async () => {
                var testProjects = projects.Where(x => Regex.IsMatch(x.FullName, matchTestProjects));

                foreach (var buildProject in testProjects) {
                    await RunDotNet("build", buildProject.FullName);
                }
            });

            await RunTargetsAndExitAsync(new string[] { options.Command });
            return 0;
        }
    }
}
