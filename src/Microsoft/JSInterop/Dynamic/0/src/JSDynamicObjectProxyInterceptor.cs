using System;
using System.Linq;
using Castle.DynamicProxy;
using Dynamitey;
using DynamiteyDynamic = Dynamitey.Dynamic;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectProxyInterceptor : IInterceptor
    {
        private readonly JSDynamicObjectProxy jsDynamicObjectProxy;
        private readonly MethodDictionary methodDictionary;
        private readonly IJSFunctionalObject jsFunctionalObject;

        internal JSDynamicObjectProxyInterceptor(JSDynamicObjectProxy jsDynamicObjectProxy, MethodDictionary methodDictionary, IJSFunctionalObject jsFunctionalObject)
        {
            this.jsDynamicObjectProxy = jsDynamicObjectProxy ?? throw new ArgumentNullException(nameof(jsDynamicObjectProxy));
            this.methodDictionary = methodDictionary ?? throw new ArgumentNullException(nameof(methodDictionary));
            this.jsFunctionalObject = jsFunctionalObject;
        }

        public void Intercept(IInvocation invocation)
        {
            var name = invocation.Method.Name;
            var arguments = invocation.Arguments ?? new object[0];
            var genericParameterTypes = invocation.GenericArguments;
            var positionalArgumentNames = invocation.Method.GetParameters().Select(x => x.Name);

            // user interface methods
            if (methodDictionary.TryFindMethod(invocation.Method.Name, positionalArgumentNames, out var method)) {
                invocation.ReturnValue = method.Invoke(jsFunctionalObject, jsDynamicObjectProxy.JSObjectReference, genericParameterTypes, arguments);
            }
            // proxy properties
            else if (invocation.Method.IsSpecialName) {
                var propertyName = name.Substring(4);

                if (name.StartsWith("get_")) {
                    invocation.ReturnValue = DynamiteyDynamic.InvokeGet(jsDynamicObjectProxy, propertyName);
                } else if (name.StartsWith("set_") && arguments.Length == 1) {
                    DynamiteyDynamic.InvokeSet(jsDynamicObjectProxy, propertyName, arguments[0]);
                } else {
                    throw new NotImplementedException();
                }
            }
            // proxy methods
            else {
                invocation.ReturnValue = DynamiteyDynamic.InvokeMember(jsDynamicObjectProxy, InvokeMemberName.Create(name, genericParameterTypes), arguments);
            }
        }
    }
}
