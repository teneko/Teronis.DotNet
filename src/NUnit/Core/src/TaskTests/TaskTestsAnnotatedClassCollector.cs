// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.NUnit.TaskTests
{
    public class TaskTestsAnnotatedClassCollector
    {
        private readonly Assembly assembly;

        public TaskTestsAnnotatedClassCollector(Assembly assembly) =>
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

        public IEnumerable<TaskTestsAnnotatedClassCollectorEntry> Collect()
        {
            var attributeType = typeof(TaskTestsAttribute);

            foreach (var type in assembly.GetTypes()) {
                if (!type.IsDefined(attributeType)) {
                    continue;
                }

                var attribute = type.GetCustomAttribute<TaskTestsAttribute>();

                if (attribute.InstanceMemberName is null) {
                    throw new InvalidOperationException($"The instance member name of attribute {typeof(TaskTestsAttribute)} is null.");
                }

                var instanceMemberName = attribute.InstanceMemberName;
                var members = type.GetMember(attribute.InstanceMemberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);


                if (members.Length != 1) {
                    throw new InvalidOperationException($"Cannot handle more than one member with name \"{instanceMemberName}\"");
                }

                var member = members[0];
                object? taskTestsInstanceObject;

                if (member.MemberType == MemberTypes.Field) {
                    taskTestsInstanceObject = ((FieldInfo)member).GetValue(null);
                } else if (member.MemberType == MemberTypes.Property) {
                    taskTestsInstanceObject = ((PropertyInfo)member).GetValue(null);
                } else {
                    throw new InvalidOperationException($"Member by name \"{instanceMemberName}\" is not field or property.");
                }

                var taskTestsInstance = taskTestsInstanceObject as ITaskTests;

                if (!(taskTestsInstanceObject is null) && taskTestsInstance is null) {
                    throw new InvalidOperationException($"Member by name \"{instanceMemberName}\" was already assigned but it is not of type {typeof(ITaskTests)}.");
                }

                yield return new TaskTestsAnnotatedClassCollectorEntry(member, taskTestsInstance);
            }
        }
    }
}
