// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Teronis.NUnit.TaskTests
{
    public class TaskTestCaseBlockMember
    {
        public MemberInfo MemberInfo { get; }
        /// <summary>
        /// If null it can be set by calling <see cref="SetInstance(ITaskTestCaseBlock)"/>.
        /// </summary>
        public ITaskTestCaseBlock? Instance { get; private set; }

        public TaskTestCaseBlockMember(MemberInfo memberInfo, ITaskTestCaseBlock? taskTestsInstance)
        {
            MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));

            if (memberInfo.MemberType != MemberTypes.Property
                && memberInfo.MemberType != MemberTypes.Field) {
                throw new ArgumentException("The member type of member info has to be property or field.");
            }

            Instance = taskTestsInstance;
        }

        /// <summary>
        /// Sets <paramref name="taskTestsInstance"/> as value of
        /// <see cref="MemberInfo"/>. After that it is stored to
        /// <see cref="Instance"/> too.
        /// </summary>
        /// <param name="taskTestsInstance">The new instance.</param>
        public void SetInstance(ITaskTestCaseBlock taskTestsInstance)
        {
            if (MemberInfo.MemberType == MemberTypes.Field) {
                ((FieldInfo)MemberInfo).SetValue(null, taskTestsInstance);
            } else {
                ((PropertyInfo)MemberInfo).SetValue(null, taskTestsInstance);
            }

            Instance = taskTestsInstance;
        }

        /// <summary>
        /// Gets the concrete instance type of <see cref="MemberInfo"/>.
        /// </summary>
        /// <returns></returns>
        public Type GetInstanceType() =>
            MemberInfo.MemberType switch {
                MemberTypes.Field => ((FieldInfo)MemberInfo).FieldType,
                _ => ((PropertyInfo)MemberInfo).PropertyType
            };
    }
}
