using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Dynamitey;
using Microsoft.JSInterop;
using DynamiteyDynamic = Dynamitey.Dynamic;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectInterceptor : IInterceptor
    {
        private readonly JSDynamicObjectProxy jsDynamicObjectProxy;
        private readonly MethodDictionary methodDictionary;
        private readonly IJSFunctionalObject jsFunctionalObject;

        internal JSDynamicObjectInterceptor(JSDynamicObjectProxy jsDynamicObjectProxy, MethodDictionary methodDictionary, IJSFunctionalObject jsFunctionalObject)
        {
            this.jsDynamicObjectProxy = jsDynamicObjectProxy ?? throw new ArgumentNullException(nameof(jsDynamicObjectProxy));
            this.methodDictionary = methodDictionary ?? throw new ArgumentNullException(nameof(methodDictionary));
            this.jsFunctionalObject = jsFunctionalObject;
        }

        private static string?[] GetPositionalArgumentNames(int numberOfArguments, IReadOnlyList<string> argumentNames)
        {
            var numberOfArgumentNames = argumentNames.Count;

            var positionalArgumentNames = new string?[numberOfArguments];
            var currentArgumentPosition = 0;

            for (; currentArgumentPosition < numberOfArguments - numberOfArgumentNames; currentArgumentPosition++) {
                positionalArgumentNames[currentArgumentPosition] = null;
            }

            foreach (var argumentName in argumentNames) {
                positionalArgumentNames[currentArgumentPosition] = argumentName;
                currentArgumentPosition++;
            }

            return positionalArgumentNames;
        }

        public void Intercept(IInvocation invocation) {
            var name = invocation.Method.Name;
            var arguments = invocation.Arguments ?? new object[0];
            var genericParameterTypes = invocation.GenericArguments;
            var positionalArgumentNames = invocation.Method.GetParameters().Select(x => x.Name);

            if (methodDictionary.TryFindMethod(invocation.Method.Name, positionalArgumentNames, out var method)) {
                invocation.ReturnValue = method.Invoke(jsFunctionalObject, jsDynamicObjectProxy.JSObjectReference, genericParameterTypes, arguments);
                return; // We have our return value set.
            }

            invocation.ReturnValue = DynamiteyDynamic.InvokeMember(jsDynamicObjectProxy, InvokeMemberName.Create(name, genericParameterTypes), arguments);
        }
    }
}
