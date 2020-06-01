using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GitVersion.MSBuildTask;
using GitVersionTask.MsBuild;
using Microsoft.Build.Framework;
//using Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Teronis.IO;

namespace Teronis.DotNet.GitVersionCache.BuildTasks
{
    public static class BuildTaskUtilities
    {
        public static DirectoryInfo GetGitVersionYamlDirectory() =>
              DirectoryTools.GetDirectoryOfFileAbove("GitVersion.yml", ModuleInitializer.ExecutingAssemblyDirectory);

        /// <summary>
        /// Gets the GitVersionCore owned list of <see cref="ServiceDescriptor"/>s.
        /// </summary>
        public static IList<ServiceDescriptor> GetGitVersionCoreOwnedServiceDescriptors(GitVersionTaskBase buildTask)
        {
            var buildServiceProviderMethodInfo = typeof(GitVersionTasks).GetMethod("BuildServiceProvider", BindingFlags.Static | BindingFlags.NonPublic);
            var serviceProvider = (IServiceProvider)buildServiceProviderMethodInfo.Invoke(null, new[] { buildTask });

            var instanceBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            var engine = serviceProvider.GetType().GetField("_engine", instanceBindingFlags).GetValue(serviceProvider);
            var callSiteFactory = engine.GetType().GetProperty("CallSiteFactory", instanceBindingFlags).GetValue(engine);
            var serviceDescriptors = (List<ServiceDescriptor>)callSiteFactory.GetType().GetField("_descriptors", instanceBindingFlags).GetValue(callSiteFactory);
            return serviceDescriptors;
        }

        public static void SetUndefinedAsDefault(object instance, string propertyName, TaskLoggingHelper Log)
        {
            var instanceType = instance.GetType();
            var propertyInfo = instanceType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

            if (propertyInfo == null) {
                return;
            }

            var propertyValue = propertyInfo.GetValue(instance);

            if (propertyValue is string propertyString && propertyString == "*Undefined*") {
                propertyInfo.SetValue(instance, null);
            }
        }
    }
}
