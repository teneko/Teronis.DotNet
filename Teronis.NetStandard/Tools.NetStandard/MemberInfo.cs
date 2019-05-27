using System;
using System.Reflection;
using Teronis.Extensions.NetStandard;
using Teronis.Reflection;

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
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static MemberInfo GetCheckedVariable(MemberInfo memberInfo)
        {
            memberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));

            if (!memberInfo.IsFieldOrProperty())
                throw new ArgumentException("The member info is not a field or a property");

            return memberInfo;
        }

        #endregion
    }
}
