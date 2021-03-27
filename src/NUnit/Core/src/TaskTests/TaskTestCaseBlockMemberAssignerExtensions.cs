using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.NUnit.TaskTests
{
    public static class TaskTestCaseBlockMemberAssignerExtensions
    {
        /// <summary>
        /// Sets <see cref="TaskTestCaseBlockMember.Instance"/> via <see cref="ActivatorUtilities"/> and
        /// <paramref name="serviceProvider"/> of each item.
        /// </summary>
        /// <param name="memberAssigner"></param>
        /// <param name="collectorEntries"></param>
        /// <param name="serviceProvider">
        /// The service provider is used to get or create instances if
        /// <see cref="TaskTestCaseBlockMember.Instance"/>
        /// is null. If null <see cref="EmptyServiceProvider.Instance"/>
        /// is taken in exchange. The consequence is that the class cannot have
        /// parameterized constructors unless <see cref="ActivatorUtilitiesConstructorAttribute"/>
        /// is used.
        /// </param>
        /// <returns>The assigned instance.</returns>
        public static IEnumerable<ITaskTestCaseBlock> AssignInstances(
            this ITaskTestCaseBlockMemberAssigner memberAssigner,
            IEnumerable<TaskTestCaseBlockMember> collectorEntries,
            IServiceProvider? serviceProvider = null)
        {
            foreach (var collectorEntry in collectorEntries) {
                yield return memberAssigner.AssignInstance(collectorEntry, serviceProvider);
            }
        }
    }
}
