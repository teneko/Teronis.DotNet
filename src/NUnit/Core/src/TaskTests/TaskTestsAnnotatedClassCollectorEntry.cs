using System;
using System.Reflection;

namespace Teronis.NUnit.TaskTests
{
    public class TaskTestsAnnotatedClassCollectorEntry
    {
        public MemberInfo MemberInfo { get; }
        /// <summary>
        /// Not null if the value of <see cref="MemberInfo"/> was not null at time of collection.
        /// </summary>
        public ITaskTests? Instance { get; private set; }

        public TaskTestsAnnotatedClassCollectorEntry(MemberInfo memberInfo, ITaskTests? taskTestsInstance)
        {
            MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));

            if (memberInfo.MemberType != MemberTypes.Property
                && memberInfo.MemberType != MemberTypes.Field) {
                throw new ArgumentException("The member type of member info has to be property or field.");
            }

            Instance = taskTestsInstance;
        }

        public void SetInstance(ITaskTests taskTestsInstance)
        {
            if (MemberInfo.MemberType == MemberTypes.Field) {
                ((FieldInfo)MemberInfo).SetValue(null, taskTestsInstance);
            } else {
                ((PropertyInfo)MemberInfo).SetValue(null, taskTestsInstance);
            }

            Instance = taskTestsInstance;
        }

        public Type GetInstanceType() =>
            MemberInfo.MemberType switch {
                MemberTypes.Field => ((FieldInfo)MemberInfo).FieldType,
                _ => ((PropertyInfo)MemberInfo).PropertyType
            };
    }
}
