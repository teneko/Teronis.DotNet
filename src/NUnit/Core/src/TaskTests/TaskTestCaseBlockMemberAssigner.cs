// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.NUnit.TaskTests
{
    public class TaskTestCaseBlockMemberAssigner : ITaskTestCaseBlockMemberAssigner
    {
        /// <summary>
        /// <inheritdoc/>
        /// If the activated instance is of type <see cref="IInitializableTaskTestCaseBlock"/>
        /// its method <see cref="IInitializableTaskTestCaseBlock.Initialize"/> method will be called.
        /// </summary>
        /// <param name="collectorEntry"><inheritdoc/></param>
        /// <param name="serviceProvider"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual ITaskTestCaseBlock AssignInstance(TaskTestCaseBlockMember collectorEntry, IServiceProvider? serviceProvider)
        {
            serviceProvider ??= EmptyServiceProvider.Instance;
            ITaskTestCaseBlock? instance = collectorEntry.Instance;

            if (instance is null) {
                var instanceType = collectorEntry.GetInstanceType();
                instance = (ITaskTestCaseBlock)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, instanceType);

                if (instance is IInitializableTaskTestCaseBlock initializableInstance) {
                    initializableInstance.Initialize();
                }

                collectorEntry.SetInstance(instance);
            }

            return instance;
        }
    }
}
