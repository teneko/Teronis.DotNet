// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSObjectInvocationBase<ReturnType> : IJSObjectInvocationBase
        where ReturnType : struct
    {
        new ReturnType GetDeterminedResult();
        void SetDeterminedResult(ReturnType value);
    }
}
