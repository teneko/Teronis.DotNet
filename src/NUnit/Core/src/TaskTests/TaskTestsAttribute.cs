// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.NUnit.TaskTests
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TaskTestsAttribute : Attribute
    {
        public string InstanceMemberName { get; }

        public TaskTestsAttribute(string instanceMemberName) =>
            InstanceMemberName = instanceMemberName;
    }
}
