// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInvocationBase<ReturnType> : IJSFunctionalObjectInvocationBase
        where ReturnType : struct
    {
        ReturnType GetResult();
        void SetAlternativeResult(ReturnType value);
    }
}
