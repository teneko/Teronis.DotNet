using System.Collections.Generic;
using GitVersion.MSBuildTask;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DotNet.GitVersionCache.BuildTasks
{
    public class BuildTaskServiceCollectionBuilder<GitVersionTaskExecutorType>
        where GitVersionTaskExecutorType : class, IGitVersionTaskExecutor
    {
        private readonly GitVersionTaskBase buildTask;

        public BuildTaskServiceCollectionBuilder(GitVersionTaskBase buildTask) =>
            this.buildTask = buildTask;

        protected virtual IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            var serviceDescriptors = BuildTaskUtilities.GetGitVersionCoreOwnedServiceDescriptors(buildTask);
            var serviceColletionImplementationView = (IList<ServiceDescriptor>)serviceCollection;

            /// Add services but remove existing <see cref="IGitVersionTaskExecutor"/>.
            foreach (var descriptor in serviceDescriptors) {
                if (descriptor.ServiceType != typeof(IGitVersionTaskExecutor)) {
                    serviceColletionImplementationView.Add(descriptor);
                }
            }

            /// Add replacement of <see cref="IGitVersionTaskExecutor"/>.
            serviceCollection.AddSingleton<IGitVersionTaskExecutor, GitVersionTaskExecutorType>();

            return serviceCollection;
        }
    }
}
