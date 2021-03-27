// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// Provides <see cref="ITaskTestCaseBlock"/> service collector.
    /// </summary>
    public class TaskTestCaseBlockServiceCollector
    {
        /// <summary>
        /// Collects services from <paramref name="serviceProvider"/> that registered as
        /// <see cref="ITaskTestCaseBlock"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public virtual IEnumerable<ITaskTestCaseBlock> CollectTaskTests(IServiceProvider serviceProvider) =>
            (IEnumerable<ITaskTestCaseBlock>)serviceProvider.GetService(typeof(IEnumerable<ITaskTestCaseBlock>));
    }
}
