using System;
using System.Reflection;

namespace Teronis.Tools
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

        #region Checked[Attribute]Variable

        /// <summary>
        /// Checks that <paramref name="memberInfo"/> is not null and is either field or property, otherwise an exception is been thrown.
        /// </summary>
        public static void CheckedVariable(MemberInfo memberInfo)
        {
            memberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));

            if (!IsFieldOrProperty(memberInfo))
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

        #region Imported from extensions

        public static bool IsFieldOrProperty(MemberInfo memberInfo)
            => memberInfo.MemberType == MemberTypes.Field || memberInfo.MemberType == MemberTypes.Property;

        public static Type GetVariableType(MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool IsVariable(MemberInfo memberInfo)
        {
            if (memberInfo == null || !IsFieldOrProperty(memberInfo))
                return false;

            return true;
        }

        private static bool isVariable(MemberInfo memberInfo, Type attributeType, bool getCustomAttributesInherit)
            => memberInfo.IsDefined(attributeType, getCustomAttributesInherit);

        public static bool IsVariable(MemberInfo memberInfo, Type attributeType, bool getCustomAttributesInherit)
            => IsVariable(memberInfo) && isVariable(memberInfo, attributeType, getCustomAttributesInherit);

        #endregion
    }
}
