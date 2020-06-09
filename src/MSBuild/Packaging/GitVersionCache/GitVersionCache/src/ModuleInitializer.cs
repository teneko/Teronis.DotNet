using System.IO;
using System.Reflection;

namespace Teronis.GitVersionCache
{
    public static class ModuleInitializer
    {
        public static string ExecutingAssemblyDirectory { get; }
        public static string ContainerRootDirectory { get; }
        public static string[] LoadAssemblies { get; }

        static ModuleInitializer()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            ExecutingAssemblyDirectory = Path.GetDirectoryName(executingAssembly.Location);

            if (ExecutingAssemblyDirectory.EndsWith(@"GitVersionCache\netstandard2.0")) {
                ContainerRootDirectory = ExecutingAssemblyDirectory + @"\..\..";
            } else {
                ContainerRootDirectory = ExecutingAssemblyDirectory;
            }

            LoadAssemblies = new[] {
                ContainerRootDirectory + @"\GitVersion\netstandard2.0\GitVersionCore.dll",
                ContainerRootDirectory + @"\GitVersion\netstandard2.0\GitVersionTask.MsBuild.dll",
                //ContainerRootDirectory + @"\tools\netstandard2.0\GitVersionTask.dll",
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
