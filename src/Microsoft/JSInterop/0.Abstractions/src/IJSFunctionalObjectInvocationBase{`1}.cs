namespace Teronis.Microsoft.JSInterop
{
    public interface IJSFunctionalObjectInvocationBase<ReturnType> : IJSFunctionalObjectInvocationBase
        where ReturnType : struct
    {
        ReturnType GetResult();
        void SetAlternativeResult(ReturnType value);
    }
}
