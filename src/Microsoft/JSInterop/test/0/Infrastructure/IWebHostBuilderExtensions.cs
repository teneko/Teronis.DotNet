using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Build.Construction;
using Microsoft.Extensions.FileSystemGlobbing;
using Teronis.IO;

namespace Teronis.Microsoft.JSInterop.Test.Infrastructure
{
    public static class IWebHostBuilderExtensions
    {
        public const string SolutionPattern = "*.sln";

        private static bool IsDirectoryOfPathAbove(DirectoryInfo dirInfo, string solutionPattern, [MaybeNullWhen(false)] out string solutionPath)
        {
            var matcher = new Matcher();
            matcher.AddInclude(solutionPattern);
            solutionPath = matcher.GetResultsInFullPath(dirInfo.FullName).SingleOrDefault();
            return solutionPath != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName">The project name. Most of the times it is the assembly name.</param>
        /// <returns></returns>
        public static IWebHostBuilder UseSolutionProjectContentRoot(this IWebHostBuilder webHostBuilder, string projectName, string solutionPattern = SolutionPattern)
        {
            string? solutionPath = null;

            _ = DirectoryUtils.GetDirectoryOfPathAbove(dirInfo => {
                return IsDirectoryOfPathAbove(dirInfo, solutionPattern, out solutionPath);
            }, new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));

            if (solutionPath != null) {
                var solution = SolutionFile.Parse(solutionPath);
                var solutionProjects = solution.ProjectsInOrder;

                foreach (var solutionProject in solutionProjects) {
                    if (solutionProject.ProjectName != projectName) {
                        continue;
                    }

                    var absoluteProjectDirectory = Path.GetDirectoryName(solutionProject.AbsolutePath) ??
                        throw new InvalidOperationException();

                    webHostBuilder.UseContentRoot(absoluteProjectDirectory);
                    break;
                }
            }

            return webHostBuilder;
        }
    }
}
