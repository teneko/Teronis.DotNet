// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Teronis.Extensions;
using Teronis.Utils;

namespace Teronis.Reflection
{
    public static class TeronisReflectionUtils
    {
        // Property <see cref="VariableInfoSettings.ExcludeByAttributeTypes"/> contains <see cref="IgnoreEntityVariableAttribute"/>.
        /// <summary>
        /// Property <see cref="VariableInfoSettings.IncludeIfReadable"/> is true.
        /// Property <see cref="VariableInfoSettings.IncludeIfWritable"/> is true.
        /// </summary>
        private static VariableMemberDescriptor createDefaultVariableInfoSettings()
        {
            return new VariableMemberDescriptor() {
                IncludeIfReadable = true,
                IncludeIfWritable = true,
            };
        }

        #region Update Entity Variables

        private static void updateEntityVariables<T>(T leftEntity, T rightEntity, IEnumerable<MemberInfo> variableInfos)
            where T : notnull
        {
            foreach (var variable in variableInfos) {
                object? value = null;

                try {
                    value = variable.GetValue(rightEntity);
                } catch (Exception error) {
                    throw new Exception($"A value cannot be retrieved from variable '{variable.Name}'", error);
                }

                try {
                    variable.SetValue(leftEntity, value);
                } catch (Exception error) {
                    throw new Exception($"The value '{value}' cannot be assigned to variable '{variable.Name}'", error);
                }
            }
        }

        private static void ensureNonNullEntityTypes<LeftEntityType, RightEntityType>(LeftEntityType leftEntity, RightEntityType rightEntity, out Type leftEntityType, out Type rightEntityType)
            where LeftEntityType : notnull
            where RightEntityType : notnull
        {
            leftEntityType = leftEntity?.GetType()!;
            rightEntityType = rightEntity?.GetType()!;

            if (leftEntityType == null) {
                throw new ArgumentException("Left entity is null.");
            } else if (rightEntityType == null) {
                throw new ArgumentException("Right entity is null.");
            }
        }

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, IEnumerable<MemberInfo> variableInfos)
            where T : notnull
        {
            ensureNonNullEntityTypes(leftEntity, rightEntity, out _, out _);
            updateEntityVariables(leftEntity, rightEntity, variableInfos);
        }

        // // BASE-TYPE-LOOP

        // NON-ATTRIBUTE

        public static void UpdateEntityVariables<T>(T leftEntity, T rightEntity, VariableMemberDescriptor? leftVariablesDescriptor = null, VariableMemberDescriptor? rightVariablesDescriptor = null)
            where T : notnull
        {
            ensureNonNullEntityTypes(leftEntity, rightEntity, out var leftEntityType, out var rightEntityType);
            leftVariablesDescriptor ??= createDefaultVariableInfoSettings();
            var leftEntityVariableMembers = leftEntityType.GetVariableMembers(descriptor: leftVariablesDescriptor);
            var intersectedEntityVariableMembers = leftEntityVariableMembers.Intersect(rightEntityType, rightVariablesDescriptor);
            updateEntityVariables(leftEntity, rightEntity, intersectedEntityVariableMembers);
        }

        // ATTRIBUTE

        public static void UpdateEntityVariablesByAttribute<T>(T leftEntity, T rightEntity, Type attributeType, VariableMemberDescriptor? variableInfoSettings = null)
            where T : notnull
        {
            ensureNonNullEntityTypes(leftEntity, rightEntity, out var leftEntityType, out _);
            variableInfoSettings ??= createDefaultVariableInfoSettings();

            var leftEntityVariableMembers = leftEntityType.GetAttributeVariableMembers(attributeType, descriptor: variableInfoSettings)
                .Select(x => x.MemberInfo);

            UpdateEntityVariables(leftEntity, rightEntity, leftEntityVariableMembers);
        }

        #endregion

        #region Shallow Copy

        public static TargetType ShallowCopy<SourceType, TargetType>(SourceType source)
            where TargetType : notnull
            where SourceType : notnull
        {
            var flags = VariableMemberDescriptor.DefaultFlags | BindingFlags.NonPublic;

            var cloningObjectMembersSettings = new VariableMemberDescriptor() {
                Flags = flags,
                IncludeIfWritable = true,
            };

            var declaredType = typeof(TargetType);

            var cloningObjectMembersByNameList = declaredType
                .GetVariableMembers(descriptor: cloningObjectMembersSettings)
                .ToDictionary(x => x.Name);

            var copyingObjectMembersSettings = new VariableMemberDescriptor() {
                Flags = flags,
                IncludeIfReadable = true,
            };

            var copyingObjectMembersByNameList = typeof(SourceType)
                .GetVariableMembers(descriptor: copyingObjectMembersSettings)
                .ToDictionary(x => x.Name);

            var clonedObejct = Instantiator.Instantiate<TargetType>(declaredType);

            foreach (var nameAndCloningObjectMembersPair in cloningObjectMembersByNameList) {
                var cloningObjectMembersKey = nameAndCloningObjectMembersPair.Key;

                if (copyingObjectMembersByNameList.ContainsKey(cloningObjectMembersKey)) {
                    var cloningObjectMember = nameAndCloningObjectMembersPair.Value;
                    var copyingObjectMember = copyingObjectMembersByNameList[cloningObjectMembersKey];

                    if (!cloningObjectMember.GetVariableType().IsAssignableFrom(copyingObjectMember.GetVariableType())) {
                        continue;
                    }

                    var copyingObjectVariableValue = copyingObjectMember.GetValue(source);
                    cloningObjectMember.SetValue(clonedObejct, copyingObjectVariableValue);
                }
            }

            return clonedObejct;
        }

        public static T ShallowCopy<T>(T entity)
            where T : notnull
            => ShallowCopy<T, T>(entity);

        #endregion

        #region Members

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableMemberDescriptor, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type? interruptingBaseType = null, VariableMemberDescriptor? variableInfoDescriptor = null)
        {
            if (beginningType is null) {
                yield break;
            }

            variableInfoDescriptor = variableInfoDescriptor.DefaultIfNull(true);

            if (variableInfoDescriptor.Flags.HasFlag(BindingFlags.DeclaredOnly)) {
                interruptingBaseType = beginningType.BaseType;
            } else {
                variableInfoDescriptor = variableInfoDescriptor.ShallowCopy();
                variableInfoDescriptor.Flags |= BindingFlags.DeclaredOnly;
                variableInfoDescriptor.Seal();
                interruptingBaseType ??= typeof(object);
            }

            var basesTypes = TypeUtils.GetTypeThenBaseTypes(beginningType, interruptingBaseType: interruptingBaseType);

            foreach (var type in basesTypes) {
                foreach (var varInfo in getMembers(type, variableInfoDescriptor)) {
                    yield return varInfo;
                }
            }
        }

        #endregion

        #region Attribute Members 

        // TYPED

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeMembers<TAttribute>(Func<Type, VariableMemberDescriptor, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type? interruptingBaseType = null, VariableMemberDescriptor? variableInfoDescriptor = null, bool? getCustomAttributesInherit = null)
            where TAttribute : Attribute
        {
            foreach (var type in TypeUtils.GetTypeThenBaseTypes(beginningType, interruptingBaseType: interruptingBaseType)) {
                foreach (var propertyInfo in GetMembers(getMembers, beginningType, interruptingBaseType, variableInfoDescriptor)) {
                    if (propertyInfo.TryGetAttributeVariableMember(out AttributeMemberInfo<TAttribute>? varAttrInfo, getCustomAttributesInherit) && !(varAttrInfo is null)) {
                        yield return varAttrInfo;
                    }
                }
            }
        }

        // NON-TYPED

        public static IEnumerable<AttributeMemberInfo> GetAttributeMembers(Type attributeType, Func<Type, VariableMemberDescriptor, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type? interruptingBaseType = null, VariableMemberDescriptor? variableInfoDescriptor = null, bool? getCustomAttributesInherit = null)
        {
            foreach (var type in TypeUtils.GetTypeThenBaseTypes(beginningType, interruptingBaseType: interruptingBaseType)) {
                foreach (var propertyInfo in GetMembers(getMembers, beginningType, interruptingBaseType, variableInfoDescriptor)) {
                    if (propertyInfo.TryGetAttributeVariableMember(attributeType, out var varAttrInfo, getCustomAttributesInherit) && !(varAttrInfo is null)) {
                        yield return varAttrInfo;
                    }
                }
            }
        }

        #endregion
    }
}
