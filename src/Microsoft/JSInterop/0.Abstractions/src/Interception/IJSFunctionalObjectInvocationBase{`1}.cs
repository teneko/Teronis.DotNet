namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInvocationBase<ReturnType> : IJSFunctionalObjectInvocationBase
        where ReturnType : struct
    {
        ReturnType GetResult();
        void SetAlternativeResult(ReturnType value);
    }
}
