using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using CommandLine;
using System.Threading.Tasks;
using CommandLine.Text;

using static Bullseye.Targets;
using static SimpleExec.Command;
using static Teronis.DotNet.Build.CommandLineOptions;

namespace Teronis.DotNet.Build
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CommandLineOptions>(args).MapResult(async (options) => {
                var rootDirectory = Utilities.GetRootDirectory() ?? throw new DirectoryNotFoundException("Root directory not found.");
                var sourceDirectory = Path.Combine(rootDirectory.FullName, "src");
                var projects = Directory.GetFiles(sourceDirectory, "*.csproj", SearchOption.AllDirectories)
                       .Select(x => new FileInfo(x));
                var matchTestProjects = @"(\.Test\.csproj|\\test\\)";
                var matchBuildProjects = Regex.Escape(Path.Combine(sourceDirectory, @"Build\"));
                var matchNonBuildProjects = string.Format(@"({0}|{1})", matchTestProjects, matchBuildProjects);

                Task RunDotNet(string command, string project, string additionalArguments = null) => RunAsync($"dotnet.exe", $"{command} {project} {ConfigurationName} {options.Configuration} {VerbosityName} {options.Verbosity} {additionalArguments}");

                Target("build", async () => {
                    var buildProjects = projects.Where(x => !Regex.IsMatch(x.FullName, matchNonBuildProjects));

                    foreach (var buildProject in buildProjects) {
                        await RunDotNet("build", buildProject.FullName);
                    }
                });

                Target("pack", async () => {
                    var buildProjects = projects.Where(x => !Regex.IsMatch(x.FullName, matchNonBuildProjects));

                    foreach (var buildProject in buildProjects) {
                        await RunDotNet("pack", buildProject.FullName);
                    }
                });

                Target("test", DependsOn("build", "pack"), async () => {
                    var testProjects = projects.Where(x => Regex.IsMatch(x.FullName, matchTestProjects));

                    foreach (var buildProject in testProjects) {
                        await RunDotNet("build", buildProject.FullName);
                    }
                });

                await RunTargetsAndExitAsync(args);
            }, (errors) => {
                foreach (var error in errors) {
                    Console.WriteLine(SentenceBuilder.Factory().FormatError(error));
                }

                return Task.CompletedTask;
            });
        }
    }
}
