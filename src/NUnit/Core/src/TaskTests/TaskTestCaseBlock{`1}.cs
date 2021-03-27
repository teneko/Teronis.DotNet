// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// <para>
    /// This class is inteded to be inherited. In its derived class it is intended to implement a
    /// static member of such derived class. There are three ways to initialize the static member.
    /// </para>
    /// <para>
    /// The manual way is only achievable with <see cref="TaskTestCaseBlock"/>. See coments there.
    /// </para>
    /// <para>
    /// The more automatic way is to use <see cref="TaskTestCaseBlock{TDerived}"/>. It passes a
    /// <see cref="TaskTestCaseList"/> to its base type <see cref="TaskTestCaseBlock{TDerived}"/>. Because you
    /// can't to call <see cref="TaskTestCaseBlock.AddTest(TaskTestCaseList, Func{CancellationToken,Task}, string?)"/> but
    /// <see cref="AddTest(Func{CancellationToken,Task}, string?)"/>, you have to <see cref="Initialize"/> after
    /// the creation of derived class.
    /// </para>
    /// <para>
    /// Last but not least you can you just skip the instantiation of the static member. You then
    /// can leave the instantiaton to <see cref="AssemblyTaskTestCaseBlockStaticMemberCollector"/> in
    /// conjunction with <see cref="TaskTestCaseBlockStaticMemberProviderAttribute"/> and
    /// <see cref="TaskTestCaseBlockMemberAssigner"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TDerived"></typeparam>
    public abstract class TaskTestCaseBlock<TDerived> : TaskTestCaseBlock
        where TDerived : TaskTestCaseBlock<TDerived>
    {
        protected TaskTestCaseBlock()
            : base(new TaskTestCaseList()) { }

        /// <summary>
        /// Adds <paramref name="assertableTaskFactory"/> to an internal static task factory list.
        /// You must call <see cref="Initialize"/> when creating <see cref="TaskTestCaseBlock{TDerived}"/>
        /// manually.
        /// </summary>
        /// <param name="assertableTaskFactory"></param>
        /// <param name="testName">Inferred by <see cref="CallerMemberNameAttribute"/>.</param>
        /// <returns></returns>
        public static TaskTestCase AddTest(Func<CancellationToken, Task> assertableTaskFactory, [CallerMemberName] string? testName = null) =>
            AddTest(TemporaryTaskTestCaseList, assertableTaskFactory, testName: testName);

        /// <summary>
        /// Copies temporary, static and to be tested lazy tasks over to instance scoped 
        /// list. Must be called when you use <see cref="AddTest(Func{CancellationToken,Task}, string?)"/>
        /// .
        /// </summary>
        /// <returns></returns>
        public TDerived Initialize()
        {
            InitializeInstance();
            return (TDerived)this;
        }
    }
}
