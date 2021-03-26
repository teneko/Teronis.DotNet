// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInvocation : IJSFunctionalObjectInvocationBase<ValueTask>
    {
        internal abstract void Clone();
    }
}
