using System;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    internal static class ParameterListExtensions
    {
        public static void ThrowAggregateExceptionWhenHavingErrors(this ParameterList parameterList) {
            if (parameterList.HasErrors) {
                throw new AggregateException("The parameter list is invalid.", parameterList.Errors);
            }
        }
    }
}
