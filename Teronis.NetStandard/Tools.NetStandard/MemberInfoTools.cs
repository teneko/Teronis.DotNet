using System;
using System.Reflection;
using Teronis.Extensions.NetStandard;

namespace Teronis.Tools.NetStandard
{
    public static class MemberInfoTools
    {
        public static object GetValue(MemberInfo memberInfo, object owner)
        {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(owner);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(owner);
                default:
                    throw new NotSupportedException();
            }
        }

        public static void SetValue(MemberInfo memberInfo, object owner, object value)
        {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    ((FieldInfo)memberInfo).SetValue(owner, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo)memberInfo).SetValue(owner, value);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        #region MemberInfo

        /// <summary>
        /// Checks that <paramref name="memberInfo"/> is not null and is either field or property, otherwise an exception is been thrown.
        /// </summary>
        public static void CheckedVariable(MemberInfo memberInfo)
        {
            memberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));

            if (!memberInfo.IsFieldOrProperty())
                throw new ArgumentException("The member info is not a field or a property");
        }

        /// <summary>
        /// Checks that <paramref name="memberInfo"/> is a valid variable and has an attribute of type <paramref name="attributeType"/> defined, otherwise an exception is been thrown.
        /// </summary>
        public static void CheckedAttributeVariable(MemberInfo memberInfo, Type attributeType)
        {
            CheckedVariable(memberInfo);

            if (!memberInfo.IsDefined(attributeType, false))
                throw new ArgumentException($"The member has not defined an attribute of type {attributeType}");
        }

        #endregion
    }
}
