using System;
using System.Linq;
using System.IO;
using System.Reflection;
//using System.Runtime.Loader;
using Microsoft.Build.Framework;
using System.Threading;
using Microsoft.Build.Utilities;

namespace Teronis.DotNet.GitVersionCache
{
    ///// <summary>
    ///// A composition class to use for an MSBuild Task that needs to supply its own dependencies
    ///// independently of the assemblies that the hosting build engine may be willing to supply.
    ///// </summary>
    //public class ContextIsolatedTaskExecutor
    //{
    //    private readonly IContextIsolatedTask nonContextIsolatedTask;

    //    /// <summary>
    //    /// The context the inner task is loaded within.
    //    /// </summary>
    //    private AssemblyLoadContext assemblyLoadContext;

    //    /// <summary>
    //    /// The source of the <see cref="CancellationToken" /> that is canceled when
    //    /// <see cref="ICancelableTask.Cancel" /> is invoked.
    //    /// </summary>
    //    private readonly CancellationTokenSource cancellationTokenSource;

    //    /// <summary>
    //    /// Logs errors and warnings to build engine. Can be null.
    //    /// </summary>
    //    public TaskLoggingHelper Logger { get; set; }

    //    /// <summary>Gets a token that is canceled when MSBuild is requesting that we abort.</summary>
    //    public CancellationToken CancellationToken => cancellationTokenSource.Token;

    //    /// <summary>Gets the path to the directory containing managed dependencies.</summary>
    //    protected virtual string ManagedDllDirectory =>
    //        Path.GetDirectoryName(new Uri(GetType().GetTypeInfo().Assembly.CodeBase).LocalPath);

    //    /// <summary>
    //    /// Gets the path to the directory containing native dependencies.
    //    /// May be null if no native dependencies are required.
    //    /// </summary>
    //    public string UnmanagedDllDirectory { get; set; }

    //    public ContextIsolatedTaskExecutor(IContextIsolatedTask nonContextIsolatedTask)
    //    {
    //        assemblyLoadContext = new ContextIsolatedTaskAssemblyLoader(this, nonContextIsolatedTask);
    //        cancellationTokenSource = new CancellationTokenSource();
    //        this.nonContextIsolatedTask = nonContextIsolatedTask;
    //    }

    //    /// <inheritdoc/>
    //    public void Cancel() => cancellationTokenSource.Cancel();

    //    /// <summary>
    //    /// Loads the assembly at the specified path within the isolated context.
    //    /// </summary>
    //    /// <param name="assemblyPath">The path to the assembly to be loaded.</param>
    //    /// <returns>The loaded assembly.</returns>
    //    public Assembly LoadAssemblyByPath(string assemblyPath)
    //    {
    //        assemblyLoadContext = assemblyLoadContext ?? throw new ArgumentNullException(nameof(assemblyLoadContext));
    //        return assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
    //    }

    //    /// <summary>
    //    /// Loads an assembly with a given name.
    //    /// </summary>
    //    /// <param name="assemblyName">The name of the assembly to load.</param>
    //    /// <returns>The loaded assembly, if one could be found; otherwise <c>null</c>.</returns>
    //    /// <remarks>
    //    /// The default implementation searches the <see cref="ManagedDllDirectory"/> folder for
    //    /// any assembly with a matching simple name.
    //    /// Derived types may use <see cref="LoadAssemblyByPath(string)"/> to load an assembly
    //    /// from a given path once some path is found.
    //    /// </remarks>
    //    public virtual Assembly LoadAssemblyByName(AssemblyName assemblyName)
    //    {
    //        if (assemblyName.Name.StartsWith("Microsoft.Build", StringComparison.OrdinalIgnoreCase) ||
    //            assemblyName.Name.StartsWith("System.", StringComparison.OrdinalIgnoreCase))                 // MSBuild and System.* make up our exchange types. So don't load them in this LoadContext.
    //            // We need to inherit them from the default load context.
    //            return null;

    //        var assemblyPath = Path.Combine(ManagedDllDirectory, assemblyName.Name) + ".dll";

    //        if (File.Exists(assemblyPath))
    //            return LoadAssemblyByPath(assemblyPath);

    //        return null;
    //    }

    //    private bool executeTaskInIsolatedContext(IContextIsolatedTask contxtIsolatedTask)
    //    {
    //        var contextIsolatedTaskType = contxtIsolatedTask.GetType();

    //        var outerProperties = nonContextIsolatedTask.GetType()
    //            .GetRuntimeProperties()
    //            .ToDictionary(i => i.Name);

    //        var innerProperties = contextIsolatedTaskType.GetRuntimeProperties()
    //            .ToDictionary(i => i.Name);

    //        var propertiesDiscovery = from outerProperty in outerProperties.Values
    //                                  where outerProperty.SetMethod != null && outerProperty.GetMethod != null
    //                                  let innerProperty = innerProperties[outerProperty.Name]
    //                                  select new { outerProperty, innerProperty };

    //        var propertiesMap = propertiesDiscovery.ToArray();

    //        var outputPropertiesMap = propertiesMap.Where(pair => {
    //            return pair.outerProperty.GetCustomAttribute<OutputAttribute>() != null;
    //        }).ToArray();

    //        foreach (var propertyPair in propertiesMap) {
    //            var outerPropertyValue = propertyPair.outerProperty.GetValue(this);
    //            propertyPair.innerProperty.SetValue(contxtIsolatedTask, outerPropertyValue);
    //        }

    //        // Forward any cancellation requests
    //        var innerCancelMethod = contextIsolatedTaskType.GetMethod(nameof(Cancel));

    //        using (CancellationToken.Register(() => innerCancelMethod.Invoke(contxtIsolatedTask, new object[0]))) {
    //            CancellationToken.ThrowIfCancellationRequested();

    //            // Execute the inner task.
    //            var executeInnerMethod = contextIsolatedTaskType.GetMethod(nameof(IContextIsolatedTask.ExecuteIsolated), BindingFlags.Instance | BindingFlags.NonPublic);
    //            var result = (bool)executeInnerMethod.Invoke(contxtIsolatedTask, new object[0]);

    //            // Retrieve any output properties.
    //            foreach (var propertyPair in outputPropertiesMap)
    //                propertyPair.outerProperty.SetValue(this, propertyPair.innerProperty.GetValue(contxtIsolatedTask));

    //            return result;
    //        }
    //    }

    //    /// <inheritdoc />
    //    public bool Execute()
    //    {
    //        try {
    //            var taskAssemblyPath = new Uri(GetType().GetTypeInfo().Assembly.CodeBase).LocalPath;
    //            var inContextAssembly = assemblyLoadContext.LoadFromAssemblyPath(taskAssemblyPath);
    //            var nonContextIsolatedTaskType = inContextAssembly.GetType(nonContextIsolatedTask.GetType().FullName);
    //            var contextIsolatedTask = (IContextIsolatedTask)Activator.CreateInstance(nonContextIsolatedTaskType);
    //            return executeTaskInIsolatedContext(contextIsolatedTask);
    //        } catch (OperationCanceledException) {
    //            Logger?.LogMessage(MessageImportance.High, "Task has been canceled.");
    //            return false;
    //        }
    //    }
    //}
}
