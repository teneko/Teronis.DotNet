using System;
using System.Linq;
using System.Threading.Tasks;
using Teronis.Reflection;
using Teronis.Extensions.NetStandard;
using System.Reflection;

namespace Teronis.Tools.NetStandard
{
    public static class GenericTools
    {
        public static bool ReturnNonDefault<T>(T inValue, out T outValue, Func<T> getNonDefaultIfDefault = null)
            => !Tools.CompareEquality(outValue = inValue, default) || (FuncGenericTools.ReturnIsInvocable(getNonDefaultIfDefault, out outValue) && !Tools.CompareEquality(outValue, default));

        public static I ReturnInValue<I>(I inValue, out I outInValue)
        {
            outInValue = inValue;
            return inValue;
        }

        public static I ReturnInValue<I>(I inValue, Action<I> modifyInValue)
        {
            modifyInValue(inValue);
            return inValue;
        }

        public static I ReturnInValue<I>(I inValue, Func<I, I> modifyInValue)
            => modifyInValue(inValue);

        public static I ReturnInValue<I>(I inValue, Action doSomething)
        {
            doSomething();
            return inValue;
        }

        public static async Task<I> ReturnInValue<I>(I inValue, Task task)
        {
            await task;
            return inValue;
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, V value)
        {
            outInValue = inValue;
            return value;
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, Func<V> getValue)
        {
            outInValue = inValue;
            return getValue();
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, Func<I, V> getValue)
        {
            outInValue = inValue;
            return getValue(inValue);
        }

        public static V ReturnValue<I, V>(I inValue, Func<I, V> getValue)
            => getValue(inValue);

        public static TCloningObject ShallowCopy<TCloningObject, TCopyingObject>(TCopyingObject copyingObject)
        {
            var flags = VariableInfoSettings.DefaultFlags | BindingFlags.NonPublic;

            var cloningObjectMembersSettings = new VariableInfoSettings() {
                Flags = flags,
                IncludeIfWritable = true,
            };

            var declaredType = typeof(TCloningObject);

            var cloningObjectMembersByNameList = declaredType
                .GetVariableMembers(cloningObjectMembersSettings)
                .ToDictionary(x => x.Name);

            var copyingObjectMembersSettings = new VariableInfoSettings() {
                Flags = flags,
                IncludeIfReadable = true,
            };

            var copyingObjectMembersByNameList = typeof(TCopyingObject)
                .GetVariableMembers(copyingObjectMembersSettings)
                .ToDictionary(x => x.Name);

            var clonedObejct = (TCloningObject)declaredType.InstantiateUninitializedObject();

            foreach (var nameAndCloningObjectMembersPair in cloningObjectMembersByNameList) {
                var cloningObjectMembersKey = nameAndCloningObjectMembersPair.Key;

                if (copyingObjectMembersByNameList.ContainsKey(cloningObjectMembersKey)) {
                    var cloningObjectMember = nameAndCloningObjectMembersPair.Value;
                    var copyingObjectMember = copyingObjectMembersByNameList[cloningObjectMembersKey];

                    if (!cloningObjectMember.GetVariableType().IsAssignableFrom(copyingObjectMember.GetVariableType()))
                        continue;

                    var copyingObjectVariableValue = copyingObjectMember.GetValue(copyingObject);
                    cloningObjectMember.SetValue(clonedObejct, copyingObjectVariableValue);
                }
            }

            return clonedObejct;
        }

        public static TCloningAndCopyingObject ShallowCopy<TCloningAndCopyingObject>(TCloningAndCopyingObject copyingObject)
            => ShallowCopy<TCloningAndCopyingObject, TCloningAndCopyingObject>(copyingObject);
    }
}