namespace Teronis.Microsoft.JSInterop.Dynamic
{
    internal static class ParameterListExtensions
    {
        public static void ThrowParameterListExceptionWhenHavingErrors(this ParameterList parameterList) {
            if (parameterList.HasErrors) {
                throw new ParameterListException("The parameter list is invalid.", parameterList.Errors);
            }
        }
    }
}
