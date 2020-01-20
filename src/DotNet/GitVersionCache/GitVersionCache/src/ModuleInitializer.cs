using System;
using System.IO;
using System.Reflection;

namespace Teronis.DotNet.GitVersionCache
{
    public static class ModuleInitializer
    {
        public static string ExecutingAssemblyDirectory { get; }
        public static string[] LoadAssemblies { get; }

        static ModuleInitializer()
        {
            ExecutingAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            LoadAssemblies = new[] {
                ExecutingAssemblyDirectory + @"\tools\netstandard2.0\GitVersionCore.dll",
                ExecutingAssemblyDirectory + @"\tools\netstandard2.0\GitVersionTask.MsBuild.dll",
                ExecutingAssemblyDirectory + @"\tools\netstandard2.0\GitVersionTask.dll"
            };
        }

        public static void Initialize()
        {
            //try {
            foreach (var loadAssembly in LoadAssemblies) {
                Assembly.LoadFrom(loadAssembly);
            }
            //} catch {
            //    // We ignore them, because MSBuild can't resolve them.
            //}
        }
    }
}
