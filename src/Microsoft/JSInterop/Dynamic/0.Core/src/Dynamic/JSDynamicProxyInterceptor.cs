// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Concurrent.FastReflection.NetStandard;
using Teronis.Microsoft.JSInterop.Dynamic.Reflection;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    internal class JSDynamicProxyInterceptor : IInterceptor
    {
        private static readonly Dictionary<RuntimeMethodHandle, MethodCaller<object, object>> proxyMethodDelegates;
        private static readonly Dictionary<RuntimeMethodHandle, MemberGetter<object, object>> proxyMemberGetterDelegates;
        private static readonly Dictionary<RuntimeMethodHandle, MemberSetter<object, object>> proxyMemberSetterDelegates;

        static JSDynamicProxyInterceptor()
        {
            proxyMethodDelegates = new Dictionary<RuntimeMethodHandle, MethodCaller<object, object>>();
            proxyMemberGetterDelegates = new Dictionary<RuntimeMethodHandle, MemberGetter<object, object>>();
            proxyMemberSetterDelegates = new Dictionary<RuntimeMethodHandle, MemberSetter<object, object>>();
        }

        private readonly IJSObjectReferenceFacade jsDynamicObjectProxy;
        private readonly MethodDictionary methodDictionary;
        // This object walks through all interceptors.
        private readonly IJSObjectInterceptor jsObjectInterceptor;

        public JSDynamicProxyInterceptor(
            IJSObjectReferenceFacade jsDynamicObjectProxy,
            MethodDictionary methodDictionary,
            IJSObjectInterceptor jsObjectInterceptor)
        {
            this.jsDynamicObjectProxy = jsDynamicObjectProxy ?? throw new ArgumentNullException(nameof(jsDynamicObjectProxy));
            this.methodDictionary = methodDictionary ?? throw new ArgumentNullException(nameof(methodDictionary));
            this.jsObjectInterceptor = jsObjectInterceptor ?? throw new ArgumentNullException(nameof(jsObjectInterceptor));
        }

        public void Intercept(IInvocation invocation)
        {
            var arguments = invocation.Arguments ?? new object[0];
            var methodInfo = invocation.Method;

            if (proxyMethodDelegates.TryGetValue(methodInfo.MethodHandle, out var methodDelegate)) {
                goto callMethod;
            }

            if (proxyMemberGetterDelegates.TryGetValue(methodInfo.MethodHandle, out var memberGetterDelegate)) {
                goto getMember;
            }

            if (proxyMemberSetterDelegates.TryGetValue(methodInfo.MethodHandle, out var memberSetterDelegate)) {
                goto setMember;
            }

            var memberName = invocation.Method.Name;
            var genericParameterTypes = invocation.GenericArguments;
            var positionalArgumentNames = invocation.Method.GetParameters().Select(x => x.Name);

            // user interface methods
            if (methodDictionary.TryFindMethod(methodInfo.Name, positionalArgumentNames, out var method)) {
                var methodAttributes = new CustomAttributes(methodInfo);
                invocation.ReturnValue = method.GetReturnValue(jsObjectInterceptor, jsDynamicObjectProxy.JSObjectReference, genericParameterTypes, arguments, methodAttributes);
                return;
            }
            // proxy properties
            else if (invocation.Method.IsSpecialName) {
                var propertyName = memberName.Substring(4);
                var methodDeclaringType = methodInfo.DeclaringType ?? throw new InvalidOperationException();

                if (memberName.StartsWith("get_")) {
                    //invocation.ReturnValue = DynamiteyDynamic.InvokeGet(jsDynamicObjectProxy, propertyName);
                    memberGetterDelegate = methodDeclaringType.GetProperty(propertyName).DelegateForGet();
                    proxyMemberGetterDelegates.Add(methodInfo.MethodHandle, memberGetterDelegate);
                    goto getMember;
                } else if (memberName.StartsWith("set_") && arguments.Length == 1) {
                    //DynamiteyDynamic.InvokeSet(jsDynamicObjectProxy, propertyName, arguments[0]);
                    memberSetterDelegate = methodDeclaringType.GetProperty(propertyName).DelegateForSet();
                    proxyMemberSetterDelegates.Add(methodInfo.MethodHandle, memberSetterDelegate);
                    goto setMember;
                } else {
                    throw new NotImplementedException();
                }
            }
            // proxy methods
            else {
                methodDelegate = methodInfo.DelegateForCall();
                proxyMethodDelegates.Add(methodInfo.MethodHandle, methodDelegate);
                goto callMethod;
            }

            callMethod:
            invocation.ReturnValue = methodDelegate.Invoke(jsDynamicObjectProxy, arguments);
            return;

            getMember:
            invocation.ReturnValue = memberGetterDelegate.Invoke(jsDynamicObjectProxy);
            return;

            setMember:
            object jsDynamicObjectProxyObject = jsDynamicObjectProxy;
            memberSetterDelegate.Invoke(ref jsDynamicObjectProxyObject, arguments[0]);
            return;
        }
    }
}
