using System;
using System.Linq;
using System.IO;
using System.Reflection;
//using System.Runtime.Loader;

namespace Teronis.GitVersionCache
{
    //public class ContextIsolatedTaskAssemblyLoader : AssemblyLoadContext
    //{
    //    private readonly IContextIsolatedTask contextIsolatedTask;
    //    private readonly ContextIsolatedTaskExecutor contextIsolatedTaskExecutor;

    //    public ContextIsolatedTaskAssemblyLoader(ContextIsolatedTaskExecutor contextIsolatedTaskExecutor, IContextIsolatedTask contextIsolatedTask)
    //    {
    //        this.contextIsolatedTask = contextIsolatedTask;
    //        this.contextIsolatedTaskExecutor = contextIsolatedTaskExecutor;
    //    }

    //    protected override Assembly Load(AssemblyName assemblyName)
    //    {
    //        return contextIsolatedTaskExecutor.LoadAssemblyByName(assemblyName)
    //            ?? Default.LoadFromAssemblyName(assemblyName);
    //    }

    //    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    //    {
    //        var unmanagedDllPath = Directory.EnumerateFiles(contextIsolatedTaskExecutor.UnmanagedDllDirectory, $"{unmanagedDllName}.*")
    //            .Concat(Directory.EnumerateFiles(contextIsolatedTaskExecutor.UnmanagedDllDirectory, $"lib{unmanagedDllName}.*"))
    //            .FirstOrDefault();

    //        if (unmanagedDllPath != null)
    //            return LoadUnmanagedDllFromPath(unmanagedDllPath);

    //        return base.LoadUnmanagedDll(unmanagedDllName);
    //    }
    //}
}
