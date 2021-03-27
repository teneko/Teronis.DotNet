// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.NUnit.TaskTests
{
    public interface ITaskTestCaseBlockMemberAssigner
    {
        /// <summary>
        /// Sets <see cref="TaskTestCaseBlockMember.Instance"/> via <see cref="ActivatorUtilities"/> and
        /// <paramref name="serviceProvider"/>.
        /// </summary>
        /// <param name="collectorEntry">The collector its instance you want to assign.</param>
        /// <param name="serviceProvider">
        /// The service provider is used to get or create instances if
        /// <see cref="TaskTestCaseBlockMember.Instance"/>
        /// is null. If null <see cref="EmptyServiceProvider.Instance"/>
        /// is taken in exchange. The consequence is that the class cannot have
        /// parameterized constructors unless <see cref="ActivatorUtilitiesConstructorAttribute"/>
        /// is used.
        /// </param>
        /// <returns>The assigned instance.</returns>
        ITaskTestCaseBlock AssignInstance(TaskTestCaseBlockMember collectorEntry, IServiceProvider? serviceProvider);
    }
}
