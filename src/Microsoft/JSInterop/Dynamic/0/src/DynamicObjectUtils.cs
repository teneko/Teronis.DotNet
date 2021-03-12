using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq.Expressions;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    internal class DynamicObjectUtils
    {
        private static Func<InvokeMemberBinder, IList<Type>>? typeArgumentsDelegate;

        internal static bool TryGetInvokeMemberTypeArguments(InvokeMemberBinder binder, [MaybeNullWhen(false)] out IList<Type> typeArguments)
        {
            if (typeArgumentsDelegate is null) {
                var typeArgumentsProperty = binder.GetType().GetProperty("TypeArguments");

                if (!(typeArgumentsProperty is null)) {
                    var binderType = typeArgumentsProperty.DeclaringType ?? throw new InvalidOperationException();
                    var invokeMemberBinderExpressionParameter = Expression.Parameter(typeof(InvokeMemberBinder), nameof(binder));
                    var typeArgumentsDeclaringTypeCompatibleExpressionParameter = Expression.Convert(invokeMemberBinderExpressionParameter, binderType);

                    var typeArgumentsExpressionProperty = Expression.PropertyOrField(typeArgumentsDeclaringTypeCompatibleExpressionParameter, typeArgumentsProperty.Name);

                    var typeArgumentsGetterExpression = Expression.Lambda<Func<InvokeMemberBinder, IList<Type>>>(typeArgumentsExpressionProperty, invokeMemberBinderExpressionParameter);
                    typeArgumentsDelegate = typeArgumentsGetterExpression.Compile();
                }
            }

            if (!(typeArgumentsDelegate is null)) {
                typeArguments = typeArgumentsDelegate(binder);
                return !(typeArguments is null);
            }

            typeArguments = null;
            return false;
        }
    }
}
