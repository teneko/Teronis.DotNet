using System.IO;
using System.Reflection;

namespace Teronis.GitVersionCache
{
    public static class ModuleInitializer
    {
        public static string ExecutingAssemblyDirectory { get; }
        public static string BaseDirectory { get; }
        public static string[] LoadAssemblies { get; }

        static ModuleInitializer()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            ExecutingAssemblyDirectory = Path.GetDirectoryName(executingAssembly.Location);

            if (ExecutingAssemblyDirectory.EndsWith(@"lib\netcoreapp2.1")) {
                BaseDirectory = ExecutingAssemblyDirectory + @"\..";
            } else {
                BaseDirectory = ExecutingAssemblyDirectory;
            }

            LoadAssemblies = new[] {
                //BaseDirectory + @"\tools\netstandard2.0\GitVersionCore.dll",
                BaseDirectory + @"\tools\netstandard2.0\GitVersionTask.MsBuild.dll",
                //BaseDirectory + @"\tools\netstandard2.0\GitVersionTask.dll",
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
