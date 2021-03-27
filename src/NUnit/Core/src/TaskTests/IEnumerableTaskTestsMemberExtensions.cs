// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.NUnit.TaskTests
{
    public static class IEnumerableTaskTestsMemberExtensions
    {
        /// <summary>
        /// See <see cref="TaskTestCaseBlockMemberAssigner.AssignInstance(TaskTestCaseBlockMember, IServiceProvider?)"/>.
        /// </summary>
        /// <param name="taskTestsMember"></param>
        /// <param name="memberPreparer"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IEnumerable<ITaskTestCaseBlock> AssignInstances(
            this IEnumerable<TaskTestCaseBlockMember> taskTestsMember,
            TaskTestCaseBlockMemberAssigner memberPreparer,
            IServiceProvider? serviceProvider = null) =>
            memberPreparer.AssignInstances(taskTestsMember, serviceProvider: serviceProvider);
    }
}
