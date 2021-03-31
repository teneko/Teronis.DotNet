// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Dynamic.Reflection
{
    internal static class ParameterListExtensions
    {
        public static void ThrowParameterListExceptionWhenHavingErrors(this ParameterList parameterList)
        {
            if (parameterList.HasErrors) {
                throw new ParameterListException("The parameter list is invalid.", parameterList.Errors);
            }
        }
    }
}
