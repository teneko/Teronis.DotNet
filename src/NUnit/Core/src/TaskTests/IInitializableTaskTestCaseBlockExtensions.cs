// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    public static class IInitializableTaskTestCaseBlockExtensions
    {
        /// <summary>
        /// Copies temporary, static and to be tested lazy tasks over to instance scoped 
        /// list. Must be called when you use <see cref="TaskTestCaseBlock.AddTest(Func{CancellationToken,Task}, string?)"/>
        /// .
        /// </summary>
        /// <returns></returns>
        public static T Initialize<T>(this T testCaseBlock)
            where T : TaskTestCaseBlock, IInitializableTaskTestCaseBlock
        {
            testCaseBlock.Initialize();
            return testCaseBlock;
        }
    }
}
