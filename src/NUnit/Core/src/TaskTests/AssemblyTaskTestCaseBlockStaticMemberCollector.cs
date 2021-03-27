// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// A class that collects the member of each class that is annotated
    /// with <see cref="TaskTestCaseBlockStaticMemberProviderAttribute"/>.
    /// The results of <see cref="CollectMembers"/> you can use with
    /// <see cref="TaskTestCaseBlockMemberAssigner"/>.
    /// </summary>
    public class AssemblyTaskTestCaseBlockStaticMemberCollector : ITaskTestCaseBlockMemberCollector
    {
        private readonly Assembly assembly;

        public AssemblyTaskTestCaseBlockStaticMemberCollector(Assembly assembly) =>
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

        public IEnumerable<TaskTestCaseBlockMember> CollectMembers()
        {
            var attributeType = typeof(TaskTestCaseBlockStaticMemberProviderAttribute);

            foreach (var type in assembly.GetTypes()) {
                if (!type.IsDefined(attributeType)) {
                    continue;
                }

                var attribute = type.GetCustomAttribute<TaskTestCaseBlockStaticMemberProviderAttribute>();

                if (attribute.StaticMemberMemberName is null) {
                    throw new InvalidOperationException($"The instance member name of attribute {typeof(TaskTestCaseBlockStaticMemberProviderAttribute)} is null.");
                }

                var instanceMemberName = attribute.StaticMemberMemberName;
                var members = type.GetMember(attribute.StaticMemberMemberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);


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

                var taskTestsInstance = taskTestsInstanceObject as ITaskTestCaseBlock;

                if (!(taskTestsInstanceObject is null) && taskTestsInstance is null) {
                    throw new InvalidOperationException($"Member by name \"{instanceMemberName}\" was already assigned but it is not of type {typeof(ITaskTestCaseBlock)}.");
                }

                yield return new TaskTestCaseBlockMember(member, taskTestsInstance);
            }
        }
    }
}
