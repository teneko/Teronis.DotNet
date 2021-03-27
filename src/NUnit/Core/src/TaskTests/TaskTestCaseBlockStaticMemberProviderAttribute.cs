// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// Describes a class that it has a static member of type <see cref="ITaskTestCaseBlock"/>.
    /// This attribute is higly recommend to be used in conjunction with
    /// <see cref="AssemblyTaskTestCaseBlockStaticMemberCollector"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TaskTestCaseBlockStaticMemberProviderAttribute : Attribute
    {
        public string StaticMemberMemberName { get; }

        public TaskTestCaseBlockStaticMemberProviderAttribute(string staticMemberName) =>
            StaticMemberMemberName = staticMemberName;
    }
}
